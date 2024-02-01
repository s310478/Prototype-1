using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // for input system

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horsePower = 3000f;
    [SerializeField] private float initialForce = 2000f; // To get the car moving
    private float speed;
    private float turnSpeed = 75.0f;
    private float horizontalInput;
    private float forwardInput;

    private Rigidbody playerRb;
    [SerializeField] GameObject centerOfMass;
    [SerializeField] TextMeshProUGUI speedometerText;
    //[SerializeField] float rpm;
    //[SerializeField] TextMeshProUGUI rpmText;

    public Camera mainCamera;
    public Camera hoodCamera;
    public KeyCode switchKey;

    private bool isFirstInput = true;
    private bool controlsEnabled = false;

    [SerializeField] List<WheelCollider> allWheels;
    private int wheelsOnGround;
    public GameObject leftWheel;
    public GameObject rightWheel;
    private float currentWheelAngle = 0f;
    private float maxWheelTurnAngle = 45f;
    private float turnRate = 5;

    // (input system) Processes player movement input, updating forward and horizontal movement variables.
    private void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        forwardInput = inputVector.y;
        horizontalInput = inputVector.x;
    }

    // (input system) Change camera
    private void OnSwitchCamera(InputValue value)
    {
        if (value.isPressed)
        {
            mainCamera.enabled = !mainCamera.enabled;
            hoodCamera.enabled = !hoodCamera.enabled;
        }
    }
    

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.centerOfMass = centerOfMass.transform.position;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!controlsEnabled || !IsOnGround()) return; // ignore everything if controls on player are not enabled OR car is not on ground

        moveCar();
        ApplyInitialForce();
        ResetFirstInput();
    }

    private void moveCar()
    {
        playerRb.AddRelativeForce(Vector3.forward * forwardInput * horsePower);
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

        WheelTurn(horizontalInput);

        UpdateSpeedometer();
    }

    private void WheelTurn(float horizontalInput)
    {
        // Determine the target angle based on the input
        float targetAngle = maxWheelTurnAngle * horizontalInput;

        // Smoothly interpolate the current wheel angle towards the target angle
        currentWheelAngle = Mathf.Lerp(currentWheelAngle, targetAngle, Time.deltaTime * turnRate);

        // Apply the rotation to the wheels
        Quaternion wheelRotation = Quaternion.Euler(0, currentWheelAngle, 0);
        leftWheel.transform.localRotation = wheelRotation;
        rightWheel.transform.localRotation = wheelRotation;
    }

    private void UpdateSpeedometer()
    {
        float corrctionFactor = 0.5f; // Speedometer rose to quick
        speed = Mathf.RoundToInt(playerRb.velocity.magnitude * 3.6f * corrctionFactor);
        speedometerText.SetText("Speed: " + speed + "km/h");

        //rpm = Mathf.Round((speed % 30) * 40);
        //rpmText.SetText("RPM: " + rpm);
    }

    private void ApplyInitialForce()
    {
        if (isFirstInput && Mathf.Abs(forwardInput) > 0 && playerRb.velocity.magnitude < 1f)
        {
            playerRb.AddRelativeForce(Vector3.forward * forwardInput * initialForce, ForceMode.Impulse);
            isFirstInput = false;
        }
    }

    private void ResetFirstInput()
    {
        if (playerRb.velocity.magnitude < 0.5f)
        {
            isFirstInput = true;
        }
    }

    public void EnableControls(bool enabled)
    {
        controlsEnabled = enabled;
    }

    bool IsOnGround()
    {
        wheelsOnGround = 0;
        foreach (WheelCollider wheel in allWheels)
        {
            if (wheel.isGrounded)
            {
                wheelsOnGround++;
            }
        }

        if(wheelsOnGround == 4)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
