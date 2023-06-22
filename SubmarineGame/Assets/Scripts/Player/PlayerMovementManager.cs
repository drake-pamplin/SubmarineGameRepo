using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    private bool jumpInput = false;
    private bool jumpQueued = false;
    private Vector3 jumpVector = Vector3.zero;
    
    private float lookHorizontalInput = 0;
    private float lookVerticalInput = 0;

    private int movementForwardInput = 0;
    private int movementHorizontalInput = 0;
    private Vector3 movementVector = Vector3.zero;

    private CharacterController referenceCharacterController;
    
    // Start is called before the first frame update
    void Start()
    {
        referenceCharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessPlayerJumpInput();
        ProcessPlayerLookInput();
        ProcessPlayerMoveInput();

        HandlePlayerLookInput();

        if (jumpInput) {
            jumpQueued = true;
        }
    }

    void FixedUpdate() {
        HandlePlayerMoveInput();
    }

    private GameObject GetCameraObject() {
        GameObject cameraObject = transform.Find(ConstantsManager.gameObjectCameraName).gameObject;
        return cameraObject;
    }

    private void HandlePlayerLookInput() {
        Vector3 cameraObjectRotation = GetCameraObject().transform.rotation.eulerAngles;
        cameraObjectRotation.x += (lookVerticalInput * GameManager.instance.GetPlayerLookSensitivityVertical());
        Debug.Log(cameraObjectRotation.x);
        if (cameraObjectRotation.x < GameManager.instance.GetPlayerLookClamp() || cameraObjectRotation.x > (360 - GameManager.instance.GetPlayerLookClamp())) {
            GetCameraObject().transform.rotation = Quaternion.Euler(cameraObjectRotation);
        }

        Vector3 playerRotation = transform.rotation.eulerAngles;
        playerRotation.y += (lookHorizontalInput * GameManager.instance.GetPlayerLookSensitivityHorizontal());
        transform.rotation = Quaternion.Euler(playerRotation);
    }

    private void HandlePlayerMoveInput() {
        bool isGrounded = referenceCharacterController.isGrounded;

        movementVector = transform.TransformDirection(movementVector);
        referenceCharacterController.Move(movementVector * Time.deltaTime * GameManager.instance.GetPlayerWalkSpeed());

        if (isGrounded) {
            jumpVector.y = 0;
        }
        if (jumpQueued && isGrounded) {
            jumpVector.y += GameManager.instance.GetPlayerJumpForce();
            jumpQueued = false;
        } else {
            jumpQueued = false;
        }
        jumpVector.y -= GameManager.instance.GetWorldGravityValue();
        referenceCharacterController.Move(jumpVector * Time.deltaTime);
    }

    public bool IsPlayerMoving() {
        return movementForwardInput != 0 || movementHorizontalInput != 0;
    }

    private void ProcessPlayerJumpInput() {
        jumpInput = InputManager.instance.GetPlayerJumpInput();
    }
    
    private void ProcessPlayerLookInput() {
        Vector2 mouseLookInput = InputManager.instance.GetMouseLookInput();
        lookHorizontalInput = mouseLookInput.x;
        lookVerticalInput = mouseLookInput.y;
    }

    private void ProcessPlayerMoveInput() {
        movementVector = Vector3.zero;
        movementForwardInput += InputManager.instance.GetForwardInput();
        movementHorizontalInput += InputManager.instance.GetHorizontalInput();

        movementVector = new Vector3(movementHorizontalInput, 0, movementForwardInput);
        movementVector = movementVector.normalized;
    }
}
