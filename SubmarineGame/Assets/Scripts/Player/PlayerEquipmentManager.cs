using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    private PlayerAnimationController playerAnimationController;
    private PlayerInteractionManager playerInteractionManager;
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

    private Dictionary<int, Item> inventory = new Dictionary<int, Item>();
    public Dictionary<int, Item> GetInventory() { return inventory; }
    public Item[] GetItemFromInventoryByIndex(int index) {
        Item[] item = new Item[0];
        if (inventory.TryGetValue(index, out Item itemFound)) {
            item = new Item[] { itemFound };
        }
        return item;
    }
    public void RemoveItemFromInventoryByIndex(int index) {
        inventory.Remove(index);
    }
    public void SetItemInInventoryByIndex(int index, Item item) {
        inventory[index] = item;
    }
    private Dictionary<int, Item> inventoryHotBar = new Dictionary<int, Item>();
    public Dictionary<int, Item> GetInventoryHotBar() { return inventoryHotBar; }
    public Item[] GetItemFromInventoryHotBarByIndex(int index) {
        Item[] item = new Item[0];
        if (inventoryHotBar.TryGetValue(index, out Item itemFound)) {
            item = new Item[] { itemFound };
        }
        return item;
    }
    public void RemoveItemFromInventoryHotBarByIndex(int index) {
        inventoryHotBar.Remove(index);
    }
    public void SetItemInInventoryHotBarByIndex(int index, Item item) {
        inventoryHotBar[index] = item;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
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
        // Check for existing item and add new item quantity to the existing item.
        Debug.Log("Checking for existing item.");
        if (inventoryHotBar.Count > 0) {
            for (int slotIndex = 0; slotIndex <= 10; slotIndex++) {
                Item[] itemInSlot = GetItemFromInventoryHotBarByIndex(slotIndex);
                if (itemInSlot.Length > 0 && itemInSlot[0].GetItemId() == item.GetItemId()) {
                    itemInSlot[0].SetItemQuantity(itemInSlot[0].GetItemQuantity() + item.GetItemQuantity());
                    return;
                }
            }
        }
        
        // Add item to hotbar if a slot is available.
        Debug.Log("Checking for empty slot.");
        int emptySlot = -1;
        if (inventoryHotBar.Count < 10) {
            for (int slotIndex = 0; slotIndex <= 10; slotIndex++) {
                if (emptySlot >= 0) {
                    continue;
                }
                Item[] itemInSlot = GetItemFromInventoryHotBarByIndex(slotIndex);
                if (itemInSlot.Length == 0) {
                    emptySlot = slotIndex;
                }
            }
            if (emptySlot >= 0) {
                inventoryHotBar.Add(emptySlot, item);
                item.SetItemInventoryLocationToHotbar();
            }
            if (emptySlot == InterfaceManager.instance.GetHotBarIndex()) {
                UpdateEquippedObject();
            }
            if (emptySlot != -1) {
                return;
            }
        }

        // Update item quantity or add item as new to inventory if no hotbar slot was available.
        Debug.Log("Adding item to inventory.");
        // Attempting to add item to an existing stack.
        for (int inventoryIndex = 0; inventoryIndex < GameManager.instance.GetPlayerInventorySize(); inventoryIndex++) {
            Item itemInSlot = null;
            if (inventory.TryGetValue(inventoryIndex, out itemInSlot) && itemInSlot.GetItemId() == item.GetItemId()) {
                itemInSlot.SetItemQuantity(itemInSlot.GetItemQuantity() + item.GetItemQuantity());
                break;
            }
        }

        // Attempting to add item to a new slot.
        emptySlot = -1;
        for (int inventoryIndex = 0; inventoryIndex < GameManager.instance.GetPlayerInventorySize(); inventoryIndex++) {
            if (emptySlot >= 0) {
                return;
            }

            if (!inventory.ContainsKey(inventoryIndex)) {
                emptySlot = inventoryIndex;
                inventory.Add(inventoryIndex, item);
                item.SetItemInventoryLocationToInventory();
            }
        }
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
        playerInteractionManager.DestroyDisplayObjects();
        playerStateManager.TriggerHeldState();
        RemoveItemFromInventoryHotBarByIndex(InterfaceManager.instance.GetHotBarIndex());

        GameObject itemDrop = Instantiate(
            PrefabManager.instance.GetPrefabItem(),
            transform.position,
            Quaternion.identity
        );
        Item itemDropScript = itemDrop.GetComponent<Item>();
        itemDropScript.CloneItemValues(itemToDrop);
        itemDrop.transform.Find(ConstantsManager.gameObjectAnimationName).Find(ConstantsManager.gameObjectMesh).GetComponent<SpriteRenderer>().sprite = itemDropScript.GetItemIcon();
    }

    private void ProcessInventoryInput() {
        if (!InputManager.instance.GetInventoryInput()) {
            return;
        }

        InterfaceManager.instance.ToggleInventoryDisplay();
    }

    private void ProcessWaterState() {
        if (!playerStateManager.IsMovementStateWater()) {
            if (!IsItemEquipped() && GetItemFromInventoryHotBarByIndex(InterfaceManager.instance.GetHotBarIndex()).Length == 1) {
                UpdateEquippedObject();
            }
            return;
        }

        if (!IsItemEquipped()) {
            return;
        }

        UnequipItem();
        playerInteractionManager.DestroyDisplayObjects();
        playerStateManager.TriggerHeldState();
    }

    public void UpdateEquippedObject() {
        int hotBarIndex = InterfaceManager.instance.GetHotBarIndex();
        Item[] hotBarItem = GetItemFromInventoryHotBarByIndex(hotBarIndex);
        if (equippedObject.Length == 0 && hotBarItem.Length == 0) {
            return;
        }
        EquipItem(hotBarItem.Length == 0 ? new Item[0] : hotBarItem);
    }
}
