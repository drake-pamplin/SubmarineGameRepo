using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    private PlayerAnimationController playerAnimationController;
    private PlayerEquipmentManager playerEquipmentManager;
    private PlayerRaycastManager playerRaycastManager;
    private PlayerStateManager playerStateManager;

    private float chargedTime = 0;
    public bool IsTimeCharged() { return chargedTime > 0; }
    private bool charging = false;
    public bool IsPlayerCharging() { return charging; }
    
    // Start is called before the first frame update
    void Start()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerRaycastManager = GetComponent<PlayerRaycastManager>();
        playerStateManager = GetComponent<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.IsGameStateGame()) {
            return;
        }
        
        ProcessInteractionInput();
        ProcessPullInput();
        ProcessScrollInput();
        ProcessThrowInput();
    }

    private void ProcessInteractionInput() {
        bool interactionInput = InputManager.instance.GetInteractInput();
        
        if (!interactionInput) {
            return;
        }

        GameObject interactableObject = playerRaycastManager.GetScannedObject();

        if (interactableObject == null) {
            return;
        }

        Item item = interactableObject.GetComponent<Item>();
        Debug.Log("Picked up " + item.GetItemDisplayName());
        playerEquipmentManager.PickUpItem(item);
        Destroy(interactableObject);
    }

    private void ProcessPullInput() {
        if (!playerStateManager.IsThrowStateThrown()) {
            return;
        }

        if (!InputManager.instance.GetRightClick()) {
            return;
        }

        playerStateManager.TriggerHeldState();
    }

    private void ProcessScrollInput() {
        if (playerStateManager.IsThrowStateThrown()) {
            return;
        }
        
        float scrollInput = InputManager.instance.GetScrollValue();

        if (scrollInput == 0) {
            return;
        }

        InterfaceManager.instance.HotBarScrollHighlight(scrollInput > 0);
        if (playerStateManager.IsMovementStateGrounded()) {
            GetComponent<PlayerEquipmentManager>().UpdateEquippedObject();
        }
    }

    private void ProcessThrowInput() {
        if (playerAnimationController.IsPlayerEquipping()) {
            charging = false;
            return;
        }
        
        if (!playerEquipmentManager.IsItemEquipped()) {
            charging = false;
            return;
        }

        Item item = playerEquipmentManager.GetEquippedItem()[0];
        if (!ConstantsManager.itemIdThrowable.Contains(item.GetItemId())) {
            charging = false;
            return;
        }

        if (playerStateManager.IsThrowStateThrown()) {
            return;
        }
        
        if (InputManager.instance.GetLeftClickDown()) {
            if (charging) {
                chargedTime += Time.deltaTime;
                return;
            }
            charging = true;
            chargedTime = 0;
            return;
        }
        
        if (!charging) {
            return;
        }

        charging = false;
        float chargedPowerModifier = 0;
        if (chargedTime >= GameManager.instance.GetPlayerThrowChargeTime()) {
            chargedPowerModifier = 1;
        } else {
            chargedPowerModifier = chargedTime / GameManager.instance.GetPlayerThrowChargeTime();
        }
        float chargedPower = GameManager.instance.GetPlayerThrowMaxForce() * chargedPowerModifier;
        playerStateManager.TriggerThrowFlag();

        Debug.Log("Charged throw for " + chargedPower);
    }
}
