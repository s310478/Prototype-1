using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnVehicles : MonoBehaviour
{
    [System.Serializable]
    public class CarPrefab
    {
        public GameObject prefab;
    }

    [SerializeField] List<CarPrefab> carPrefabs;
    [SerializeField] GameObject planePrefab;
    private Vector3 initialSpawnAreaMin = new Vector3(-7f, 0f, 50f);
    private Vector3 initialSpawnAreaMax = new Vector3(7f, 0f, 440f);
    private Vector3 continuousSpawnMax = new Vector3(-7f, 0f, 440f);
    private Vector3 continuousSpawnMin = new Vector3(7f, 0f, 440f);
    [SerializeField] int numberOfInitialSpawns;
    [SerializeField] Transform continuousSpawnPoint;
    [SerializeField] float spawnInterval = 2.0f;
    [SerializeField] float planeSpawnDelay = 5.0f;

    private float spawnTimer;
    private float planeSpawntimer;
    private List<GameObject> spawnedVehicles = new List<GameObject>(); // List to keep track of spawned vehicles

    void Start()
    {
        RespawnVehicles();
    }

    // Update is called once per frame
    void Update()
    {

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnContinuousCarsAtPoint();
            spawnTimer = spawnInterval;
        }

        // Check if it's time to spawn the plane
        planeSpawntimer -= Time.deltaTime;
        if (planeSpawntimer <= 0f)
        {
            SpawnPlane();
            planeSpawntimer = float.MaxValue; // Prevents the plane from spawning again
        }
    }

    public void SetDifficulty(int difficultyLevel)
    {
        switch (difficultyLevel)
        {
            case 0: // Easy
                numberOfInitialSpawns = 10;
                spawnInterval = 5.0f;
                break;
            case 1: // Medium
                numberOfInitialSpawns = 25;
                spawnInterval = 2.0f;
                break;
            case 2: // Hard
                numberOfInitialSpawns = 50;
                spawnInterval = 1.0f;
                break;
            default:
                Debug.LogError("Invalid difficulty level");
                break;
        }
        RespawnVehicles();
    }

    public void RespawnVehicles()
    {
        DespawnVehicles();
        InitialSpawn();
        spawnTimer = spawnInterval;
        planeSpawntimer = planeSpawnDelay;
    }

    void DespawnVehicles()
    {
        foreach (var vehicle in spawnedVehicles)
        {
            if (vehicle != null)
            {
                Destroy(vehicle);
            }
        }
        spawnedVehicles.Clear(); // Clears list
    }

    void InitialSpawn()
    {
        for (int i = 0; i < numberOfInitialSpawns; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(initialSpawnAreaMin.x, initialSpawnAreaMax.x),
                Random.Range(initialSpawnAreaMin.y, initialSpawnAreaMax.y),
                Random.Range(initialSpawnAreaMin.z, initialSpawnAreaMax.z)
                );

            GameObject spawnedVehicle = InstantiateRandomCar(position);
            spawnedVehicles.Add(spawnedVehicle);
        }
    }

    void SpawnContinuousCarsAtPoint()
    {
        Vector3 position = new Vector3(
            Random.Range(continuousSpawnMin.x, continuousSpawnMax.x),
            continuousSpawnPoint.position.y, // Use a fixed y-coordinate
            continuousSpawnPoint.position.z  // Use a fixed z-coordinate
        );

        GameObject spawnedVehicle = InstantiateRandomCar(position);
        spawnedVehicles.Add(spawnedVehicle);
    }

    GameObject InstantiateRandomCar(Vector3 position)
    {
        int index = Random.Range(0, carPrefabs.Count);
        GameObject carPrefab = carPrefabs[index].prefab;
        Quaternion rotation = Quaternion.Euler(0, 180, 0);
        return Instantiate(carPrefab, position, rotation);
    }

    void SpawnPlane()
    {
        Vector3 planePosition = new Vector3(0, 200, 410);
        Quaternion planeRotation = Quaternion.Euler(0, 180, 0);
        GameObject spawnedPlane = Instantiate(planePrefab, planePosition, planeRotation);
        spawnedVehicles.Add(spawnedPlane);
    }
}
