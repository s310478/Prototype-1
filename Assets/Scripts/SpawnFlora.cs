using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlora : MonoBehaviour
{
    [System.Serializable]
    public class Spawnable
    {
        public GameObject prefab;
        public float spawnProbability;
    }

    [System.Serializable]
    public class SpawnArea
    {
        public Vector3 areaMin;
        public Vector3 areaMax;
    }

    [SerializeField] List<Spawnable> spawnables;
    [SerializeField] List<SpawnArea> spawnAreas;
    [SerializeField] int numberOfSpawns;
    private List<GameObject> spawnedObjects = new List<GameObject>(); // keep track of spawned objects

    void Start()
    {
        SpawnEnvironment();
    }

    public void RespawnEnvironment()
    {
        DespawnEnvironment();
        SpawnEnvironment();
    }

    void SpawnEnvironment()
    {
        foreach (var area in spawnAreas)
        {
            for (int i = 0; i < numberOfSpawns; i++)
            {
                // Randomly choose a position within the specified area
                Vector3 position = new Vector3(
                    Random.Range(area.areaMin.x, area.areaMax.x),
                    Random.Range(area.areaMin.y, area.areaMax.y),
                    Random.Range(area.areaMin.z, area.areaMax.z));

                // Randomly select a prefab based on spawn probability
                GameObject prefabToSpawn = ChoosePrefabToSpawn();
                if (prefabToSpawn != null)
                {
                    GameObject spawnedObject = Instantiate(prefabToSpawn, position, Quaternion.identity);
                    spawnedObjects.Add(spawnedObject); // Keep track of the spawned object
                }
            }
        }
    }

    void DespawnEnvironment()
    {
        // Destroy all previously spawned objects
        foreach (var spawnedObject in spawnedObjects)
        {
            if (spawnedObject != null)
            {
                Destroy(spawnedObject);
            }
        }
        spawnedObjects.Clear(); // Clears list
    }

    GameObject ChoosePrefabToSpawn()
    {
        float totalProbability = 0;
        foreach (var spawnable in spawnables)
        {
            totalProbability += spawnable.spawnProbability;
        }

        float randomPoint = Random.value * totalProbability;

        foreach (var spawnable in spawnables)
        {
            if (randomPoint < spawnable.spawnProbability)
            {
                return spawnable.prefab;
            }
            randomPoint -= spawnable.spawnProbability;
        }

        return null;
    }
}