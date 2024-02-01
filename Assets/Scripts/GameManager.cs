using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
    [SerializeField] Toggle cameraToggle;
    [SerializeField] GameObject startButton;
    [SerializeField] Dropdown difficultyDropdown;
    [SerializeField] GameObject controls;
    [SerializeField] GameObject speedometer;
    [SerializeField] GameObject restartButton2;
    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject timer;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject restartButton;

    [SerializeField] GameObject GetToTheTunnel;
    [SerializeField] float totalFlashingTime = 4.0f;
    [SerializeField] float flashInterval = 0.5f;

    [SerializeField] GameObject player;
    [SerializeField] PlayerController playerController;
    [SerializeField] FollowPlayer followPlayer;
    [SerializeField] Camera hoodCamera;
    [SerializeField] Camera mainCamera;

    [SerializeField] AudioSource carAudioSource;
    private float startTime;
    private bool timerRunning = false;
    [SerializeField] AudioListener mainCamAudioListener;
    [SerializeField] AudioListener hoodCameraAudioListener;

    [SerializeField] SpawnFlora spawnFlora;
    [SerializeField] SpawnVehicles spawnVehicles;

    public void StartGame()
    {
        titleScreen.SetActive(false);
        endScreen.SetActive(false);
        controls.SetActive(false);
        SetCameraState(cameraToggle.isOn);
        restartButton2.SetActive(true);
        speedometer.SetActive(true);
        StartCoroutine(FlashGetToTheTunnelText());
        EventSystem.current.SetSelectedGameObject(restartButton2);

        player.SetActive(true);
        player.transform.position = new Vector3(0, 0.15f, 0);
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        playerController.EnableControls(true);
        hoodCameraAudioListener.enabled = true;
        mainCamAudioListener.enabled = false;

        mainCamera.transform.position = new Vector3(0, 6.5f, -7.75f);
        mainCamera.transform.rotation = Quaternion.Euler(21.751f, 0, 0);
        followPlayer.enabled = true;

        spawnFlora.RespawnEnvironment();
        spawnVehicles.RespawnVehicles();

        startTime = Time.time;
        timerRunning = true;
        timer.SetActive(true);

    }

    public void PlayerReachedGoal()
    {
        timerRunning = false;
        UpdateTimerDisplay();

        followPlayer.enabled = false;
        player.SetActive(false);
        playerController.hoodCamera.enabled = false;
        playerController.mainCamera.enabled = true;
        speedometer.SetActive(false);
        restartButton2.SetActive(false);
        endScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartButton);

        mainCamAudioListener.enabled = true;
    }

    private void Update()
    {
        if (timerRunning)
        {
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        float elapsedTime = Time.time - startTime;
        timerText.text = FormatTime(elapsedTime);
    }

    private string FormatTime(float time)
    {
        int seconds = (int)time;
        int milliseconds = (int)((time - seconds) * 1000);
        return string.Format("Time: {0:00}:{1:000} seconds", seconds, milliseconds);
    }


    public void OnCameraToggleChanged()
    {
        SetCameraState(cameraToggle.isOn);
    }

    private void SetCameraState(bool isHoodCamera)
    {
        // Enable hood camera and disable main camera if isHoodCamera is true,
        // otherwise enable main camera and disable hood camera
        hoodCamera.enabled = isHoodCamera;
        mainCamera.enabled = !isHoodCamera;
        followPlayer.enabled = !isHoodCamera; // Adjust FollowPlayer script if needed
    }

    // Update is called once per frame
    public void EndGame()
    {
        
        playerController.hoodCamera.enabled = false;
        playerController.mainCamera.enabled = true;
        titleScreen.SetActive(true);
        endScreen.SetActive(false);
        controls.SetActive(false);
        restartButton2.SetActive(false);
        speedometer.SetActive(false);
        GetToTheTunnel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(startButton);

        player.SetActive(false);
        playerController.EnableControls(false);

        followPlayer.enabled = false;
        mainCamera.transform.position = new Vector3(33, 65, -10);
        mainCamera.transform.rotation = Quaternion.Euler(8, -15, 0);
        hoodCameraAudioListener.enabled = false;
        mainCamAudioListener.enabled = true;
    }

    IEnumerator FlashGetToTheTunnelText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < totalFlashingTime)
        {
            GetToTheTunnel.SetActive(!GetToTheTunnel.activeSelf); // Toggle visibility
            yield return new WaitForSeconds(flashInterval); // Wait for the flash interval
            elapsedTime += flashInterval;
        }
        GetToTheTunnel.SetActive(false); // Ensure the text is hidden after flashing
    }

    public void ShowControls()
    {
        if(controls.activeSelf)
        {
            controls.SetActive(false);
        }
        else
        {
            controls.SetActive(true);
        }
        //EventSystem.current.SetSelectedGameObject(null);
    }

    private void Start()
    {
        if (difficultyDropdown != null)
        {
            spawnVehicles.SetDifficulty(difficultyDropdown.value);
        }
        
        EndGame();
    }
}
