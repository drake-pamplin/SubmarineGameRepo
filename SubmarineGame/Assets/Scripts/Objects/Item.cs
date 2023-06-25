using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemDisplayName;
    public string GetItemDisplayName() { return itemDisplayName; }
    public Texture itemIcon;
    public Texture GetItemIcon() { return itemIcon; }
    public string itemId;
    public string GetItemId() { return itemId; }

    public void CloneItemValues(Item item) {
        itemDisplayName = item.GetItemDisplayName();
        itemIcon = item.GetItemIcon();
        itemId = item.GetItemId();
    }
}
