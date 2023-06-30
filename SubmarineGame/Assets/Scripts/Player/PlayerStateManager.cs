using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerInteractionManager playerInteractionManager;
    private PlayerMovementManager playerMovementManager;
    
    public enum MovementState {
        grounded,
        water
    }
    private MovementState movementState = MovementState.grounded;
    public bool IsMovementStateGrounded() { return movementState == MovementState.grounded; }
    public bool IsMovementStateWater() { return movementState == MovementState.water; }
    
    public enum PlayerState {
        breastStroke,
        charging,
        freestyleStroke,
        idle,
        pulling,
        sprint,
        throwing,
        tread,
        walk
    }
    private PlayerState playerState = PlayerState.idle;
    public bool IsPlayerBreastStrokeState() { return playerState == PlayerState.breastStroke; }
    public bool IsPlayerChargingState() { return playerState == PlayerState.charging; }
    public bool IsPlayerFreestyleStrokeState() { return playerState == PlayerState.freestyleStroke; }
    public bool IsPlayerIdleState() { return playerState == PlayerState.idle; }
    public bool IsPlayerPullingState() { return playerState == PlayerState.pulling; }
    public bool IsPlayerSprintState() { return playerState == PlayerState.sprint; }
    public bool IsPlayerThrowingState() { return playerState == PlayerState.throwing; }
    public bool IsPlayerTreadState() { return playerState == PlayerState.tread; }
    public bool IsPlayerWalkState() { return playerState == PlayerState.walk; }

    private bool throwFlag = false;
    public void TriggerThrowFlag() { throwFlag = true; }

    public enum ThrowState {
        held,
        thrown
    }
    public ThrowState throwState = ThrowState.held;
    public bool IsThrowStateThrown() { return throwState == ThrowState.thrown; }
    public void TriggerHeldState() { throwState = ThrowState.held; }
    
    // Start is called before the first frame update
    void Start()
    {
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
        playerMovementManager = GetComponent<PlayerMovementManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMovementState();
        ProcessPlayerState();
    }

    private void ProcessMovementState() {
        /*
        Logic:
        - Grounded: Player above waterline.
        - Water: Player below waterline.
        */
        float playerHeight = transform.position.y;
        if (playerHeight > GameManager.instance.GetWorldWaterLine()) {
            movementState = MovementState.grounded;
            return;
        }
        movementState = MovementState.water;
    }

    private void ProcessPlayerState() {
        /*
        Priority:
        - Charging
        - Throwing
        - Pulling
        - Sprint
        - Walk
        - Idle
        */
        if (IsMovementStateGrounded()) {
            if (playerInteractionManager.IsPlayerCharging()) {
                playerState = PlayerState.charging;
                return;
            }
            if (throwFlag) {
                playerState = PlayerState.throwing;
                throwFlag = false;
                throwState = ThrowState.thrown;
                return;
            }
            if (playerInteractionManager.IsPlayerPulling()) {
                playerState = PlayerState.pulling;
                return;
            }
            if (playerMovementManager.IsPlayerSprinting()) {
                playerState = PlayerState.sprint;
                return;
            }
            if (playerMovementManager.IsPlayerWalking()) {
                playerState = PlayerState.walk;
                return;
            }
            playerState = PlayerState.idle;
            return;
        }

        /*
        Priority:
        - FreestyleStroke
        - BreastStroke
        - Tread
        */
        if (IsMovementStateWater()) {
            if (playerMovementManager.IsPlayerFreestyleStroking()) {
                playerState = PlayerState.freestyleStroke;
                return;
            }
            if (playerMovementManager.IsPlayerBreastStroking()) {
                playerState = PlayerState.breastStroke;
                return;
            }
            playerState = PlayerState.tread;
            return;
        }
    }
}
