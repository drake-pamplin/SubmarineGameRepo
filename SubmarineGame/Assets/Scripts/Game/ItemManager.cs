using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    void Awake() {
        instance = this;
    }

    private List<string> itemIdList = new List<string>() {
        ConstantsManager.itemIdNet,
        ConstantsManager.itemIdWood
    };

    private Dictionary<string, Item> itemLibrary = new Dictionary<string, Item>();
    private void CreateItemInLibrary(string itemId) {
        string itemDisplayName = ConstantsManager.instance.GetItemDisplayNameById(itemId);
        if (itemDisplayName == "") {
            Debug.LogError("Display name not set for \"" + itemId + "\".");
            return;
        }
        
        Sprite itemIcon = PrefabManager.instance.GetTextureById(itemId);
        if (itemIcon == null) {
            Debug.LogError("Item icon not set for \"" + itemId + "\".");
            return;
        }

        bool[] itemStackable = ConstantsManager.instance.GetItemStackableById(itemId);
        if (itemStackable.Length == 0) {
            Debug.LogError("Stackable not set for \"" + itemId + "\".");
            return;
        }
        
        Item newItem = new Item(
            itemIcon,
            itemId,
            Item.ItemInventoryLocation.None,
            itemStackable[0],
            itemDisplayName,
            0
        );
        itemLibrary.Add(itemId, newItem);
    }
    public Item GetItemById(string itemId) {
        Item item = new Item();
        itemLibrary.TryGetValue(itemId, out item);
        return item;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        LoadItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadItems() {
        foreach (string itemId in itemIdList) {
            CreateItemInLibrary(itemId);
        }
    }
}
