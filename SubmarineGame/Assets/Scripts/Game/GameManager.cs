using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header ("Debris Variables")]
    public float debrisBobbingDuration;
    public float GetDebrisBobbingDuration() { return debrisBobbingDuration; }
    public float debrisMaxSpawns;
    public float GetDebrisMaxSpawns() { return debrisMaxSpawns; }
    public float debrisMaxSpawnRange;
    public float GetDebrisMaxSpawnRange() { return debrisMaxSpawnRange; }
    public float debrisSinkDuration;
    public float GetDebrisSinkDuration() { return debrisSinkDuration; }
    public float debrisSpawnTimerMaximum;
    public float GetDebrisSpawnTimerMaximum() { return debrisSpawnTimerMaximum; }
    public float debrisSpawnTimerMinimum;
    public float GetDebrisSpawnTimerMinimum() { return debrisSpawnTimerMinimum; }
    public float debrisSurfaceDuration;
    public float GetDebrisSurfaceDuration() { return debrisSurfaceDuration; }
    public float debrisTransitionTime;
    public float GetDebrisTransitionTime() { return debrisTransitionTime; }
    
    [Header ("Interface Variables")]
    public int interfaceInventoryDisplayHeight;
    public int GetInterfaceInventoryDisplayHeight() { return interfaceInventoryDisplayHeight; }
    public int interfaceInventoryDisplayWidth;
    public int GetInterfaceInventoryDisplayWidth() { return interfaceInventoryDisplayWidth; }
    public int interfaceHotBarTileSideLength;
    public int GetInterfaceHotBarTileSideLength() { return interfaceHotBarTileSideLength; }
    public int interfaceInventoryTileSideLength;
    public int GetInterfaceInventoryTileSideLength() { return interfaceInventoryTileSideLength; }
    public int interfaceInventoryTileSpacing;
    public int GetInterfaceInventoryTileSpacing() { return interfaceInventoryTileSpacing; }
    
    [Header ("Player Variables")]
    public float playerBreastStrokeSpeed;
    public float GetPlayerBreastStrokeSpeed() { return playerBreastStrokeSpeed; }
    public float playerFreestyleStrokeSpeed;
    public float GetPlayerFreestyleStrokeSpeed() { return playerFreestyleStrokeSpeed; }
    public int playerInventorySize = 30;
    public int GetPlayerInventorySize() { return playerInventorySize; }
    public float playerJumpForce;
    public float GetPlayerJumpForce() { return playerJumpForce; }
    public float playerLookClamp;
    public float GetPlayerLookClamp() { return playerLookClamp; }
    public float playerLookSensitivityHorizontal;
    public float GetPlayerLookSensitivityHorizontal() { return playerLookSensitivityHorizontal; }
    public float playerLookSensitivityVertical;
    public float GetPlayerLookSensitivityVertical() { return playerLookSensitivityVertical; }
    public float playerPullSpeed;
    public float GetPlayerPullSpeed() { return playerPullSpeed; }
    public float playerToolRetrievalRange;
    public float GetPlayerToolRetrievalRange() { return playerToolRetrievalRange; }
    public float playerRopeWidth;
    public float GetPlayerRopeWidth() { return playerRopeWidth; }
    public float playerSprintSpeed;
    public float GetPlayerSprintSpeed() { return playerSprintSpeed; }
    public float playerThrowChargeTime;
    public float GetPlayerThrowChargeTime() { return playerThrowChargeTime; }
    public float playerThrowMaxForce;
    public float GetPlayerThrowMaxForce() { return playerThrowMaxForce; }
    public float playerThrowSpeedSlowdown;
    public float GetPlayerThrowSpeedSlowdown() { return playerThrowSpeedSlowdown; }
    public float playerWalkSpeed;
    public float GetPlayerWalkSpeed() { return playerWalkSpeed; }

    [Header ("Raycast Variables")]
    public float raycastInteractDistance;
    public float GetRaycastInteractDistance() { return raycastInteractDistance; }
    public float raycastOriginHeight;
    public float GetRaycastOriginHeight() { return raycastOriginHeight; }

    // State Variables
    public enum GameState {
        game,
        menu
    }
    private GameState gameState = GameState.game;
    public bool IsGameStateGame() { return gameState == GameState.game; }
    private void SetGameStateGame() { gameState = GameState.game; }
    public bool IsGameStateMenu() { return gameState == GameState.menu; }
    private void SetGameStateMenu() { gameState = GameState.menu; }

    [Header ("World Variables")]
    public float worldGravityValue;
    public float GetWorldGravityValue() { return worldGravityValue; }
    public float worldNetLineOffset;
    public float GetWorldNetLine() { return worldWaterLine + worldNetLineOffset; }
    public float worldNetRaycastCount;
    public float GetWorldNetRaycastCount() { return worldNetRaycastCount; }
    public float worldNetRaycastDistance;
    public float GetWorldNetRaycastDistance() { return worldNetRaycastDistance; }
    public float worldNetRaycastTick;
    public float GetWorldNetRaycastTick() { return worldNetRaycastTick; }
    public float worldWaterIntertiaValue;
    public float GetWorldWaterIntertiaValue() { return worldWaterIntertiaValue; }
    public float worldWaterLine;
    public float GetWorldWaterLine() { return worldWaterLine; }

    void Awake() {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGameState();
    }

    private void UpdateGameState() {
        if (InterfaceManager.instance.IsDisplayOpen()) {
            if (IsGameStateGame()) {
                SetGameStateMenu();
            }
        } else {
            if (IsGameStateMenu()) {
                SetGameStateGame();
            }
        }
    }
}
