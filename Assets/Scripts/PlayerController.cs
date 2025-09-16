using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public CinemachineSwitcher CS;
    public float speed = 5f;
    public float jumpPower = 5f;
    public float gravity = -9.81f;
    public CinemachineVirtualCamera virtualCam;
    public float rotationSpeed = 10f;
    public float fovSmoothSpeed = 5f;
    public float targetFov = 60f;

    private CinemachinePOV pov;
    private CharacterController controller;
    private Vector3 velocity;

    public bool isGrounded;
    private bool isCursorLocked = true;


    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();
        pov = virtualCam.GetCinemachineComponent<CinemachinePOV>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isCursorLocked = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (!isCursorLocked && Input.GetMouseButtonDown(0))
        {
            isCursorLocked = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (!CS.usingFreeLook)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 camForward = virtualCam.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = virtualCam.transform.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 move = (camForward * z + camRight * x).normalized;
            controller.Move(move * speed * Time.deltaTime);

            bool isMoving = (x != 0 || z != 0);


            float cameraYaw = pov.m_HorizontalAxis.Value;
            Quaternion targetRot = Quaternion.Euler(0f, cameraYaw, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            if (isMoving && Input.GetKey(KeyCode.LeftShift))
            {
                speed = 10f;
                targetFov = 150f;
            }
            else
            {
                speed = 5f;
                targetFov = 60f;
            }

            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpPower;
            }
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        float currentFov = virtualCam.m_Lens.FieldOfView;
        virtualCam.m_Lens.FieldOfView = Mathf.Lerp(currentFov, targetFov, fovSmoothSpeed * Time.deltaTime);
    }
}
