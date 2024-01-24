using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemInventoryLocation {
        None,
        HotBar,
        Inventory
    }
    
    public string itemDisplayName;
    public string GetItemDisplayName() { return itemDisplayName; }
    public Sprite itemIcon;
    public Sprite GetItemIcon() { return itemIcon; }
    public string itemId;
    public string GetItemId() { return itemId; }
    public ItemInventoryLocation itemInventoryLocation;
    public ItemInventoryLocation GetItemInventoryLocation() { return itemInventoryLocation; }
    public void SetItemInventoryLocationToHotbar() { itemInventoryLocation = ItemInventoryLocation.HotBar; }
    public void SetItemInventoryLocationToInventory() { itemInventoryLocation = ItemInventoryLocation.Inventory; }
    public bool IsItemInventoryLocationHotbar() { return itemInventoryLocation == ItemInventoryLocation.HotBar; }
    public bool IsItemInventoryLocationInventory() { return itemInventoryLocation == ItemInventoryLocation.Inventory; }
    public bool itemIsStackable;
    public bool IsItemStackable() { return itemIsStackable; }
    public int itemQuantity;
    public int GetItemQuantity() { return itemQuantity; }
    public void SetItemQuantity(int itemQuantity) { this.itemQuantity = itemQuantity; }

    public Item() {
        itemIcon = null;
        itemId = null;
        itemInventoryLocation = ItemInventoryLocation.None;
        itemIsStackable = false;
        itemDisplayName = null;
        itemQuantity = 0;
    }
    
    public Item(
        Sprite itemIcon,
        string itemId,
        ItemInventoryLocation itemInventoryLocation,
        bool itemIsStackable,
        string itemDisplayName,
        int itemQuantity
    ) {
        this.itemIcon = itemIcon;
        this.itemId = itemId;
        this.itemInventoryLocation = itemInventoryLocation;
        this.itemIsStackable = itemIsStackable;
        this.itemDisplayName = itemDisplayName;
        this.itemQuantity = itemQuantity;
    }

    public Item(Item item) {
        CloneItemValues(item);
    }
    
    public void CloneItemValues(Item item) {
        itemDisplayName = item.GetItemDisplayName();
        itemIcon = item.GetItemIcon();
        itemId = item.GetItemId();
        itemInventoryLocation = item.GetItemInventoryLocation();
        itemIsStackable = item.IsItemStackable();
        itemQuantity = item.GetItemQuantity();
    }
}
