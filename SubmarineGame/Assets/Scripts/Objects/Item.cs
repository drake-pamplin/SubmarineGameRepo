using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemDisplayName;
    public string GetItemDisplayName() { return itemDisplayName; }
    public string itemId;
    public string GetItemId() { return itemId; }
}
