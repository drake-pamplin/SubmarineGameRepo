using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
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
        idle,
        sprint,
        tread,
        walk
    }
    private PlayerState playerState = PlayerState.idle;
    public bool IsPlayerBreastStrokeState() { return playerState == PlayerState.breastStroke; }
    public bool IsPlayerIdleState() { return playerState == PlayerState.idle; }
    public bool IsPlayerSprintState() { return playerState == PlayerState.sprint; }
    public bool IsPlayerTreadState() { return playerState == PlayerState.tread; }
    public bool IsPlayerWalkState() { return playerState == PlayerState.walk; }
    
    // Start is called before the first frame update
    void Start()
    {
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
        - Sprint
        - Walk
        - Idle
        */
        if (IsMovementStateGrounded()) {
            if (playerMovementManager.IsPlayerSprinting()) {
                playerState = PlayerState.sprint;
                return;
            }
            if (playerMovementManager.IsPlayerWalking()) {
                playerState = PlayerState.walk;
                return;
            }
            playerState = PlayerState.idle;
        }

        /*
        Priority:
        - BreastStroke
        - Tread
        */
        if (IsMovementStateWater()) {
            if (playerMovementManager.IsPlayerBreastStroking()) {
                playerState = PlayerState.breastStroke;
                return;
            }
            playerState = PlayerState.tread;
        }
    }
}
