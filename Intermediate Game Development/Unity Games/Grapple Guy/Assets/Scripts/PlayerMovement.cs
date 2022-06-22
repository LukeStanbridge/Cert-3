using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform debugHitPointTransform;
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private Transform hookshotTransform;
    public Text deathCounterDisplay;

    private const float NORMAL_FOV = 60f;
    private const float HOOKSHOT_FOV = 100f;

    private CharacterController characterController;
    private float cameraVerticalAngle;
    private float characterVelocityY;
    private Vector3 characterVelocityMomentum;
    private Camera playerCamera;
    private CameraFOV cameraFov;
    [SerializeField] private ParticleSystem weGoingFast;
    private State state;
    private Vector3 hookshotPosition;
    private float hookshotSize;
    private float hookshotMaxDistance;
    private float hookshotMinDistance;
    private Respawn playerDead;

    private enum State
    {
        Normal,
        HookshotThrown,
        HookshotFlyingPlayer       
    }

    private void Awake()
    {
        playerDead = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Respawn>();
        characterController = GetComponent<CharacterController>();
        playerCamera = transform.Find("Camera").GetComponent<Camera>();
        cameraFov = playerCamera.GetComponent<CameraFOV>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        state = State.Normal;
        hookshotTransform.gameObject.SetActive(false);
        Time.timeScale = 1f; // crucial for resetting player movement on scene reload
    }

    // Update is called once per frame
    private void Update()
    {
        switch (state)
        {
            default:

            case State.Normal:
                HandleCharacterLook();
                HandleCharacterMovement();
                HandleHookShotStart();
                break;

            case State.HookshotThrown:
                HandleCharacterLook();
                HandleCharacterMovement();
                HandleHookshotThrown(); 
                break;

            case State.HookshotFlyingPlayer:
                HandleCharacterLook();
                HandleHookShotMovement();
                break;
        }
    }

    private void HandleCharacterLook()
    {
        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");

        // Rotate the transform with the input speed around its local Y axis
        transform.Rotate(new Vector3(0f, lookX * mouseSensitivity, 0f), Space.Self);

        // Add vertical inputs to the camera's vertical angle
        cameraVerticalAngle -= lookY * mouseSensitivity;

        // Limit the camera's vertical angle to min/max
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);

        // Apply the certical angle as alocal rotation to the camera transform along its right axis (makes it pivot up and down)
        playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);
    }

    private void HandleCharacterMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        float moveSpeed = 20f;

        Vector3 characterVelocity = transform.right * moveX * moveSpeed + transform.forward * moveZ * moveSpeed;

        if (characterController.isGrounded)
        {
            characterVelocityY = 0f;
            // Jump
            if (TestInputJump())
            {                               
                float jumpSpeed = 30f;
                characterVelocityY = jumpSpeed;
                FindObjectOfType<AudioManager>().Play("Jump");
            }
        }

        // Apply gravity to the velocity
        float gravityDownForce = -60f;
        characterVelocityY += gravityDownForce * Time.deltaTime;

        // Apply Y velocity to move vector
        characterVelocity.y = characterVelocityY;

        // Apply Momentum
        characterVelocity += characterVelocityMomentum;

        // Move Character Controller
        characterController.Move(characterVelocity * Time.deltaTime);

        // Dampen Momentum
        if (characterVelocityMomentum.magnitude >= 0f)
        {
            float momentumDrag = 3f;
            characterVelocityMomentum -= characterVelocityMomentum * momentumDrag * Time.deltaTime;
            if (characterVelocityMomentum.magnitude < .0f)
            {
                characterVelocityMomentum = Vector3.zero;
            }
        }

        // Displays death counter in top corner
        deathCounterDisplay.text = playerDead.GetComponent<Respawn>().deathCounter.ToString();
    }

    private void ResetGravityEffect()
    {
        characterVelocityY = 0f;
    }

    private void HandleHookShotStart()
    {
        hookshotMaxDistance = 75f;
        hookshotMinDistance = 10f;
        if (TestInputDownHookShot())
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit) && raycastHit.transform.gameObject.tag == "IsGrappleable" && raycastHit.transform.gameObject.tag != "TooHigh") 
            {
                if (raycastHit.distance > hookshotMinDistance && raycastHit.distance < hookshotMaxDistance)
                {
                    // Hit something
                    debugHitPointTransform.position = raycastHit.point;
                    hookshotPosition = raycastHit.point;
                    hookshotSize = 0f;
                    hookshotTransform.gameObject.SetActive(true);
                    hookshotTransform.localScale = Vector3.zero;
                    FindObjectOfType<AudioManager>().Play("FireGrapple");
                    state = State.HookshotThrown;
                }               
            }         
        }
    }

    private void HandleHookshotThrown()
    {
        hookshotTransform.LookAt(hookshotPosition);

        float hookshotThrowSpeed = 200f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);

        if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition))
        {
            state = State.HookshotFlyingPlayer;
            cameraFov.SetCameraFov(HOOKSHOT_FOV); // Adjust FOV for speed effect
            weGoingFast.Play(); // Playe particles for speed effect
        }
    }

    private void HandleHookShotMovement()
    {
        hookshotTransform.LookAt(hookshotPosition);

        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;

        float hookshotSpeedMin = 10f;
        float hookshotSpeedMax = 40f;
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotSpeedMultiplier = 5f;

        // Move Character Controller
        characterController.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultiplier *Time.deltaTime);

        float reachedHookshotPositionDistance = 1f;
        if (Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance)
        {
            // Reached Hookshot Position
            StopHookshot();
        }

        if (TestInputDownHookShot())
        {
            // Cancel Hookshot
            StopHookshot();
        }

        if (TestInputJump())
        {
            // Cancelled with Jump
            float momentumExtraSpeed = 3f;
            characterVelocityMomentum = hookshotDir * hookshotSpeed * momentumExtraSpeed;
            float jumpSpeed = 70f;
            characterVelocityMomentum += Vector3.up * jumpSpeed;
            FindObjectOfType<AudioManager>().Play("Jump");
            StopHookshot();
        }
    }

    public void StopHookshot()
    {
        state = State.Normal;
        ResetGravityEffect();
        hookshotTransform.gameObject?.SetActive(false);
        cameraFov.SetCameraFov(NORMAL_FOV);
        weGoingFast.Stop();
    }

    private bool TestInputDownHookShot()
    {
        return Input.GetKeyDown(KeyCode.Mouse0);
    }

    private bool TestInputJump()
    {
        return Input.GetKey(KeyCode.Space);
    }
}
