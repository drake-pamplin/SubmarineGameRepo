using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    private PlayerAnimationController playerAnimationController;
    private PlayerMovementManager playerMovementManager;
    private PlayerStateManager playerStateManager;
    
    private Item[] equippedObject = new Item[0];
    public Item GetEquippedItem() {
        return equippedObject[0];
    }
    public void EquipItem(Item item) {
        equippedObject = new Item[] { item };
        Debug.Log("Equipped " + item.GetItemDisplayName());
        playerAnimationController.HandleItemEquipped();
    }
    private void UnequipItem() {
        equippedObject = new Item[0];
        playerAnimationController.HandleItemEquipped();
    }
    public bool IsItemEquipped() {
        return equippedObject.Length > 0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerMovementManager = GetComponent<PlayerMovementManager>();
        playerStateManager = GetComponent<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessDropInput();
        ProcessWaterState();
    }

    private void ProcessDropInput() {
        if (!InputManager.instance.GetDropInput()) {
            return;
        }

        if (!playerStateManager.IsMovementStateGrounded()) {
            return;
        }

        if (!playerMovementManager.IsPlayerGrounded()) {
            return;
        }

        if (!IsItemEquipped()) {
            return;
        }

        Item itemToDrop = equippedObject[0];
        UnequipItem();

        GameObject itemDrop = Instantiate(
            PrefabManager.instance.GetPrefabItem(),
            transform.position,
            Quaternion.identity
        );
        Item itemDropScript = itemDrop.GetComponent<Item>();
        itemDropScript.CloneItemValues(itemToDrop);
    }

    private void ProcessWaterState() {
        if (!playerStateManager.IsMovementStateWater()) {
            return;
        }

        if (!IsItemEquipped()) {
            return;
        }

        UnequipItem();
    }
}
