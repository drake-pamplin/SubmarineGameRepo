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

    public void DropItemAtIndex(Item.ItemInventoryLocation inventoryLocation, int index) {
        Item[] itemToDrop = new Item[0];
        switch (inventoryLocation) {
            case Item.ItemInventoryLocation.HotBar:
                itemToDrop = GetItemFromInventoryHotBarByIndex(index);
                RemoveItemFromInventoryHotBarByIndex(index);
                break;
            case Item.ItemInventoryLocation.Inventory:
                itemToDrop = GetItemFromInventoryByIndex(index);
                RemoveItemFromInventoryByIndex(index);
                break;
            default:
                break;
        }
        if (itemToDrop.Length == 0) {
            Debug.LogError("Item not fond for drop at index " + index + " of inventory " + inventoryLocation.ToString() + ".");
        }

        GameObject itemDrop = Instantiate(
            PrefabManager.instance.GetPrefabItem(),
            transform.position,
            Quaternion.identity
        );
        Item itemDropScript = itemDrop.GetComponent<Item>();
        itemDropScript.CloneItemValues(itemToDrop[0]);
        itemDrop.transform.Find(ConstantsManager.gameObjectAnimationName).Find(ConstantsManager.gameObjectMesh).GetComponent<SpriteRenderer>().sprite = itemDropScript.GetItemIcon();
    }

    public void PickUpItem(Item item, bool combineStack) {
        // Check for existing item and add new item quantity to the existing item.
        if (combineStack && inventoryHotBar.Count > 0) {
            Debug.Log("Checking for existing item in hot bar.");
            for (int slotIndex = 0; slotIndex <= 10; slotIndex++) {
                Item[] itemInSlot = GetItemFromInventoryHotBarByIndex(slotIndex);
                if (itemInSlot.Length > 0 && itemInSlot[0].GetItemId() == item.GetItemId()) {
                    itemInSlot[0].SetItemQuantity(itemInSlot[0].GetItemQuantity() + item.GetItemQuantity());
                    return;
                }
            }
        }

        // Update item quantity in inventory if possible.
        if (combineStack && inventory.Count > 0) {
            Debug.Log("Checking for existing item in inventory.");
            for (int inventoryIndex = 0; inventoryIndex < GameManager.instance.GetPlayerInventorySize(); inventoryIndex++) {
                Item[] itemInSlot = GetItemFromInventoryByIndex(inventoryIndex);
                if (itemInSlot.Length > 0 && itemInSlot[0].GetItemId() == item.GetItemId()) {
                    itemInSlot[0].SetItemQuantity(itemInSlot[0].GetItemQuantity() + item.GetItemQuantity());
                    return;
                }
            }
        }
        
        // Add item to hotbar if a slot is available.
        int emptySlot = -1;
        if (inventoryHotBar.Count < 10) {
            Debug.Log("Checking for empty slot in hot bar.");
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

        // Attempting to add item to a new slot.
        emptySlot = -1;
        for (int inventoryIndex = 0; inventoryIndex < GameManager.instance.GetPlayerInventorySize(); inventoryIndex++) {
            Debug.Log("Checking for empty slot in inventory.");
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

        UnequipItem();
        DropItemAtIndex(Item.ItemInventoryLocation.HotBar, InterfaceManager.instance.GetHotBarIndex());
        playerInteractionManager.DestroyDisplayObjects();
        playerStateManager.TriggerHeldState();
    }

    private void ProcessInventoryInput() {
        if (playerStateManager.IsThrowStateThrown()) {
            return;
        }
        
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

    public void SplitStack(int index, Item.ItemInventoryLocation inventoryLocation) {
        Item[] item = new Item[0];
        if (inventoryLocation == Item.ItemInventoryLocation.HotBar) {
            item = GetItemFromInventoryHotBarByIndex(index);
        }
        if (inventoryLocation == Item.ItemInventoryLocation.Inventory) {
            item = GetItemFromInventoryByIndex(index);
        }
        if (item.Length == 0) {
            Debug.LogError("Selected tile for split is empty.");
            return;
        }
        if (item[0].GetItemQuantity() < 2) {
            Debug.LogError("Selected item for split has too few items to split.");
            return;
        }

        Item newItem = new Item();
        newItem.CloneItemValues(item[0]);
        int quantityForNewStack = newItem.GetItemQuantity() / 2;
        newItem.SetItemQuantity(quantityForNewStack);
        item[0].SetItemQuantity(item[0].GetItemQuantity() - quantityForNewStack);
        PickUpItem(newItem, false);
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
