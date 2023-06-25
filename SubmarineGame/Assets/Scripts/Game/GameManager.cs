using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header ("Interface Variables")]
    public int interfaceDisplayHeight;
    public int GetInterfaceDisplayHeight() { return interfaceDisplayHeight; }
    public int interfaceDisplayWidth;
    public int GetInterfaceDisplayWidth() { return interfaceDisplayWidth; }
    public int interfaceInventoryTileSideLength;
    public int GetInterfaceInventoryTileSideLength() { return interfaceInventoryTileSideLength; }
    public int interfaceInventoryTileSpacing;
    public int GetInterfaceInventoryTileSpacing() { return interfaceInventoryTileSpacing; }
    
    [Header ("Player Variables")]
    public float playerBreastStrokeSpeed;
    public float GetPlayerBreastStrokeSpeed() { return playerBreastStrokeSpeed; }
    public float playerFreestyleStrokeSpeed;
    public float GetPlayerFreestyleStrokeSpeed() { return playerFreestyleStrokeSpeed; }
    public float playerJumpForce;
    public float GetPlayerJumpForce() { return playerJumpForce; }
    public float playerLookClamp;
    public float GetPlayerLookClamp() { return playerLookClamp; }
    public float playerLookSensitivityHorizontal;
    public float GetPlayerLookSensitivityHorizontal() { return playerLookSensitivityHorizontal; }
    public float playerLookSensitivityVertical;
    public float GetPlayerLookSensitivityVertical() { return playerLookSensitivityVertical; }
    public float playerSprintSpeed;
    public float GetPlayerSprintSpeed() { return playerSprintSpeed; }
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
