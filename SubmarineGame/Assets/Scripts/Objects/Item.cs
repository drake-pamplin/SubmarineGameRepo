using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemDisplayName;
    public string GetItemDisplayName() { return itemDisplayName; }
    public Sprite itemIcon;
    public Sprite GetItemIcon() { return itemIcon; }
    public string itemId;
    public string GetItemId() { return itemId; }
    public int itemQuantity;
    public int GetItemQuantity() { return itemQuantity; }

    public Item() {
        itemIcon = null;
        itemId = null;
        itemDisplayName = null;
        itemQuantity = 0;
    }
    
    public Item(
        Sprite itemIcon,
        string itemId,
        string itemDisplayName,
        int itemQuantity
    ) {
        this.itemIcon = itemIcon;
        this.itemId = itemId;
        this.itemDisplayName = itemDisplayName;
        this.itemQuantity = itemQuantity;
    }
    
    public void CloneItemValues(Item item) {
        itemDisplayName = item.GetItemDisplayName();
        itemIcon = item.GetItemIcon();
        itemId = item.GetItemId();
        itemQuantity = item.GetItemQuantity();
    }
}
