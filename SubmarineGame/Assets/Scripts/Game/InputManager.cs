using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    void Awake() {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetDropInput() {
        bool inputValue = false;

        if (Keyboard.current.qKey.wasPressedThisFrame) {
            return true;
        }

        return inputValue;
    }

    public int GetForwardInput() {
        int inputValue = 0;

        if (Keyboard.current.wKey.wasPressedThisFrame) {
            inputValue++;
        }
        if (Keyboard.current.wKey.wasReleasedThisFrame) {
            inputValue--;
        }

        if (Keyboard.current.sKey.wasPressedThisFrame) {
            inputValue--;
        }
        if (Keyboard.current.sKey.wasReleasedThisFrame) {
            inputValue++;
        }

        return inputValue;
    }

    public int GetHorizontalInput() {
        int inputValue = 0;

        if (Keyboard.current.aKey.wasPressedThisFrame) {
            inputValue--;
        }
        if (Keyboard.current.aKey.wasReleasedThisFrame) {
            inputValue++;
        }

        if (Keyboard.current.dKey.wasPressedThisFrame) {
            inputValue++;
        }
        if (Keyboard.current.dKey.wasReleasedThisFrame) {
            inputValue--;
        }

        return inputValue;
    }

    public bool GetInteractInput() {
        bool inputValue = false;

        if (Keyboard.current.eKey.wasPressedThisFrame) {
            inputValue = true;
        }

        return inputValue;
    }

    public bool GetInventoryInput() {
        bool inputValue = false;

        if (Keyboard.current.tabKey.wasPressedThisFrame) {
            inputValue = true;
        }

        return inputValue;
    }

    public bool GetJumpInput() {
        bool inputValue = false;

        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            inputValue = true;
        }

        return inputValue;
    }

    public bool GetLeftClickDown() {
        return Mouse.current.leftButton.isPressed;
    }

    public Vector2 GetMouseLookInput() {
        Vector2 inputValue = Vector2.zero;

        inputValue = Mouse.current.delta.ReadValue();
        inputValue.y *= -1;

        return inputValue;
    }

    public bool GetRightClick() {
        return Mouse.current.rightButton.wasPressedThisFrame;
    }

    public float GetScrollValue() {
        float inputValue = 0;

        inputValue = Mouse.current.scroll.y.ReadValue();

        return inputValue;
    }

    public bool GetSprintInput() {
        bool inputValue = false;

        if (Keyboard.current.shiftKey.isPressed) {
            inputValue = true;
        }

        return inputValue;
    }

    public int GetSwimVerticalInput() {
        int inputValue = 0;

        if (Keyboard.current.spaceKey.isPressed) {
            inputValue++;
        }
        if (Keyboard.current.altKey.isPressed) {
            inputValue--;
        }

        return inputValue;
    }
}
