using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    private PlayerStateManager playerStateManager;
    
    private bool jumpInput = false;
    private bool jumpQueued = false;
    
    private float lookHorizontalInput = 0;
    private float lookVerticalInput = 0;

    private int movementForwardInput = 0;
    private int movementHorizontalInput = 0;
    private Vector3 movementVector = Vector3.zero;

    private CharacterController referenceCharacterController;

    private Vector3 verticalVector = Vector3.zero;
    private int verticalSwimInput = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        playerStateManager = GetComponent<PlayerStateManager>();
        
        referenceCharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.IsGameStateGame()) {
            movementVector = Vector3.zero;
            verticalSwimInput = 0;
            return;
        }
        
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

    public GameObject GetCameraObject() {
        GameObject cameraObject = transform.Find(ConstantsManager.gameObjectCameraName).gameObject;
        return cameraObject;
    }

    private void HandleGroundedMoveInput() {
        bool isGrounded = referenceCharacterController.isGrounded;

        movementVector = movementVector.normalized;
        float moveSpeed = IsPlayerSprinting() ? GameManager.instance.GetPlayerSprintSpeed() : GameManager.instance.GetPlayerWalkSpeed();
        referenceCharacterController.Move(movementVector * moveSpeed * Time.deltaTime);

        if (isGrounded) {
            verticalVector.y = 0;
        }
        if (jumpQueued && isGrounded) {
            verticalVector.y += GameManager.instance.GetPlayerJumpForce();
            jumpQueued = false;
        } else {
            jumpQueued = false;
        }
        verticalVector.y -= GameManager.instance.GetWorldGravityValue();
        referenceCharacterController.Move(verticalVector * Time.deltaTime);
    }

    private void HandlePlayerLookInput() {
        Vector3 cameraObjectRotation = GetCameraObject().transform.rotation.eulerAngles;
        cameraObjectRotation.x += (lookVerticalInput * GameManager.instance.GetPlayerLookSensitivityVertical());
        if (cameraObjectRotation.x < GameManager.instance.GetPlayerLookClamp() || cameraObjectRotation.x > (360 - GameManager.instance.GetPlayerLookClamp())) {
            GetCameraObject().transform.rotation = Quaternion.Euler(cameraObjectRotation);
        }

        Vector3 playerRotation = transform.rotation.eulerAngles;
        playerRotation.y += (lookHorizontalInput * GameManager.instance.GetPlayerLookSensitivityHorizontal());
        transform.rotation = Quaternion.Euler(playerRotation);
    }

    private void HandlePlayerMoveInput() {
        if (playerStateManager.IsMovementStateGrounded()) {
            HandleGroundedMoveInput();
            return;
        }
        if (playerStateManager.IsMovementStateWater()) {
            HandleWaterMoveInput();
            return;
        }
    }

    private void HandleWaterMoveInput() {
        Vector3 verticalSwimVector = new Vector3(0, verticalSwimInput, 0);
        movementVector += verticalSwimVector;
        movementVector = movementVector.normalized;
        float moveSpeed = IsPlayerSprinting() ? GameManager.instance.GetPlayerFreestyleStrokeSpeed() : GameManager.instance.GetPlayerBreastStrokeSpeed();
        referenceCharacterController.Move(movementVector * moveSpeed * Time.deltaTime);

        if (verticalVector.y <= GameManager.instance.GetWorldWaterIntertiaValue() && verticalVector.y >= (-1 * GameManager.instance.GetWorldWaterIntertiaValue())) {
            verticalVector.y = 0;
        } else {
            if (verticalVector.y > 0.2f) {
                verticalVector.y -= GameManager.instance.GetWorldWaterIntertiaValue();
            } else {
                verticalVector.y += GameManager.instance.GetWorldWaterIntertiaValue();
            }
        }
        if (verticalVector.y >= 0 && verticalSwimVector.y > 0 && GameManager.instance.GetWorldWaterLine() - transform.position.y < 0.2f) {
            verticalVector.y = GameManager.instance.GetPlayerJumpForce();
        }
        referenceCharacterController.Move(verticalVector * Time.deltaTime);
    }

    public bool IsPlayerBreastStroking() {
        return movementForwardInput != 0 || movementHorizontalInput != 0 || verticalSwimInput != 0;
    }

    public bool IsPlayerFreestyleStroking() {
        return ((movementForwardInput > 0) && InputManager.instance.GetSprintInput()) ||
            ((movementForwardInput == 0 && (movementHorizontalInput != 0 || verticalSwimInput != 0)) && InputManager.instance.GetSprintInput());
    }

    public bool IsPlayerGrounded() {
        return referenceCharacterController.isGrounded;
    }

    public bool IsPlayerSprinting() {
        return ((movementForwardInput > 0) && InputManager.instance.GetSprintInput()) ||
            ((movementForwardInput == 0 && movementHorizontalInput != 0) && InputManager.instance.GetSprintInput());
    }

    public bool IsPlayerWalking() {
        return (movementForwardInput != 0 || movementHorizontalInput != 0) && !IsPlayerSprinting();
    }

    private void ProcessPlayerJumpInput() {
        jumpInput = InputManager.instance.GetJumpInput();
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

        Transform referenceObject = playerStateManager.IsMovementStateWater() ? GetCameraObject().transform : transform;
        movementVector = referenceObject.forward * movementForwardInput + transform.right * movementHorizontalInput;

        verticalSwimInput = InputManager.instance.GetSwimVerticalInput();
    }
}
