using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    private PlayerAnimationController playerAnimationController;
    private PlayerMovementManager playerMovementManager;
    private PlayerStateManager playerStateManager;
    
    private Item[] equippedObject = new Item[0];
    public Item[] GetEquippedItem() {
        return equippedObject;
    }
    public void EquipItem(Item[] item) {
        equippedObject = item.Length == 0 ? new Item[0] : new Item[] { item[0] };
        playerAnimationController.HandleItemEquipped();
    }
    private void UnequipItem() {
        equippedObject = new Item[0];
        playerAnimationController.HandleItemEquipped();
    }
    public bool IsItemEquipped() {
        return equippedObject.Length > 0;
    }

    private List<Item> inventory = new List<Item>();
    public List<Item> GetInventory() { return inventory; }
    private Dictionary<int, Item> inventoryHotBar = new Dictionary<int, Item>();
    public Dictionary<int, Item> GetInventoryHotBar() { return inventoryHotBar; }
    public Item[] GetItemFromInventoryHotBar(int index) {
        Item[] item = new Item[0];
        if (inventoryHotBar.TryGetValue(index, out Item itemFound)) {
            item = new Item[] { itemFound };
        }
        return item;
    }
    private void RemoveItemFromInventoryHotBar(int index) {
        inventoryHotBar.Remove(index);
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
        ProcessInventoryInput();
        
        if (!GameManager.instance.IsGameStateGame()) {
            return;
        }
        
        ProcessDropInput();
        ProcessWaterState();
    }

    public void PickUpItem(Item item) {
        if (inventoryHotBar.Count < 10) {
            int emptySlot = -1;
            for (int slotIndex = 1; slotIndex <= 10; slotIndex++) {
                if (emptySlot > 0) {
                    continue;
                }
                Item[] itemInSlot = GetItemFromInventoryHotBar(slotIndex);
                if (itemInSlot.Length == 0) {
                    emptySlot = slotIndex;
                }
            }
            inventoryHotBar.Add(emptySlot, item);
            if (emptySlot == InterfaceManager.instance.GetHotBarIndex()) {
                UpdateEquippedObject();
            }
            return;
        }
        inventory.Add(item);
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
        RemoveItemFromInventoryHotBar(InterfaceManager.instance.GetHotBarIndex());

        GameObject itemDrop = Instantiate(
            PrefabManager.instance.GetPrefabItem(),
            transform.position,
            Quaternion.identity
        );
        Item itemDropScript = itemDrop.GetComponent<Item>();
        itemDropScript.CloneItemValues(itemToDrop);
    }

    private void ProcessInventoryInput() {
        if (!InputManager.instance.GetInventoryInput()) {
            return;
        }

        InterfaceManager.instance.ToggleInventoryDisplay();
    }

    private void ProcessWaterState() {
        if (!playerStateManager.IsMovementStateWater()) {
            if (!IsItemEquipped() && GetItemFromInventoryHotBar(InterfaceManager.instance.GetHotBarIndex()).Length == 1) {
                UpdateEquippedObject();
            }
            return;
        }

        if (!IsItemEquipped()) {
            return;
        }

        UnequipItem();
    }

    public void UpdateEquippedObject() {
        int hotBarIndex = InterfaceManager.instance.GetHotBarIndex();
        Item[] hotBarItem = GetItemFromInventoryHotBar(hotBarIndex);
        if (equippedObject.Length == 0 && hotBarItem.Length == 0) {
            return;
        }
        EquipItem(hotBarItem.Length == 0 ? new Item[0] : hotBarItem);
    }
}
