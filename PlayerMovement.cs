using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    // Start of Variables //

    [Range(0.1f, 1000.0f)]
    public float moveSpeed = 1.0f;

    [Range(0, 1000)]
    public int runningBackwards = 50;

    [Range(1.0f, 250.0f)]
    public float jumpStrength = 10.0f;

    [Range(0, 100)]
    public int inAirPaneltyPercentage = 90;

    [Range(0.1f, 100.0f)]
    public float gravity = 2.0f;

    [Range(0.1f, 100.0f)]
    public float maxGravity = 100.0f;

    private CharacterController characterController;
    private bool isGoingBackwars = false;
    private bool isGrounded = true;
    bool isRunning;

    private float verticalVelocity = 1.0f;
    private Vector3 move = Vector3.zero;

    public PhotonView view;

    // End of Variables //

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
    }

    private void Update()
    {
        CheckPlayerGoingBackwards();
        isGrounded = characterController.isGrounded;
        MovePlayer();
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
    }

    private void MovePlayer()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");
        Vector3 lateralMove = (transform.forward * inputVertical + transform.right * inputHorizontal) * (moveSpeed / 8.0f);

        if (!isGrounded)
            lateralMove *= (1.0f - inAirPaneltyPercentage / 100.0f);
        else if (isGoingBackwars)
            lateralMove *= (1.0f - runningBackwards / 100.0f);

        move.x = lateralMove.x;
        move.z = lateralMove.z;

        ApplyGravity();
        ApplyJump();
        characterController.Move(move * Time.deltaTime);

        //Running
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = 750f;
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 400f;
            isRunning = false;
        }
    }

    private void ApplyGravity()
    {
        if (isGrounded)
        {
            verticalVelocity = 0.0f;
        }

        verticalVelocity -= Mathf.Sqrt(gravity * Time.fixedDeltaTime);
        verticalVelocity = Mathf.Clamp(verticalVelocity, -maxGravity, float.MaxValue);

        move.y = verticalVelocity;
    }

    private void ApplyJump()
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            verticalVelocity += Mathf.Sqrt(jumpStrength * 2.0f * gravity);
            move.y = verticalVelocity;
        }
    }

    private void CheckPlayerGoingBackwards()
    {
        isGoingBackwars = Input.GetAxis("Vertical") <= -1.0f;
    }
}




