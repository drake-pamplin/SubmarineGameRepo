using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header ("Player Variables")]
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
    public float playerSwimSpeed;
    public float GetPlayerSwimSpeed() { return playerSwimSpeed; }
    public float playerWalkSpeed;
    public float GetPlayerWalkSpeed() { return playerWalkSpeed; }

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
        
    }
}
