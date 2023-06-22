using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerMovementManager playerMovementManager;
    
    public enum PlayerState {
        idle,
        walk
    }
    private PlayerState playerState = PlayerState.idle;
    public bool IsPlayerIdleState() { return playerState == PlayerState.idle; }
    public bool IsPlayerWalkState() { return playerState == PlayerState.walk; }
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovementManager = GetComponent<PlayerMovementManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessPlayerState();
    }

    private void ProcessPlayerState() {
        /*
        Priority:
        - Walk
        - Idle
        */

        if (playerMovementManager.IsPlayerMoving()) {
            playerState = PlayerState.walk;
            return;
        }
        playerState = PlayerState.idle;
    }
}
