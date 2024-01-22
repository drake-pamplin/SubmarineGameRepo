using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;
    void Awake() {
        instance = this;
    }

    private int hotBarIndex = 0;
    public int GetHotBarIndex() { return hotBarIndex; }
    /*
        Menu layers: 
        - 0: interfaceDisplayObject
    */
    private GameObject interfaceDisplayObject = null;
    private GameObject pickUpTextObject = null;
    private GameObject moveIndicator = null;
    private Item moveItem = null;
    private GameObject startTile = null;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHotBarSlots();
    }
    
    public void HotBarScrollHighlight(bool up) {
        GameObject highlightObject = GameObject.FindGameObjectWithTag(ConstantsManager.tagHotBar).transform
            .Find(ConstantsManager.gameObjectBackgroundName)
            .Find(ConstantsManager.gameObjectHighlightContainerName)
            .Find(ConstantsManager.gameObjectHighlightName).gameObject;
        int newIndex = hotBarIndex + (1 * (up ? 1 : -1));
        if (newIndex > 9) {
            newIndex = 1;
        }
        if (newIndex < 0) {
            newIndex = 9;
        }
        hotBarIndex = newIndex;

        int xCoord = hotBarIndex * GameManager.instance.GetInterfaceHotBarTileSideLength();
        highlightObject.transform.localPosition = new Vector3(xCoord, highlightObject.transform.localPosition.y, 0);
    }

    public bool IsDisplayOpen() {
        if (interfaceDisplayObject != null) {
            return true;
        }
        
        return false;
    }

    public void MoveEnd(GameObject endTile) {
        Destroy(moveIndicator);

        // Get tile details.
        int slotIndex = endTile.GetComponent<ItemSlot>().GetIndex();

        // Get item at end tile.
        GameObject player = GameObject.FindGameObjectWithTag(ConstantsManager.tagPlayer);
        PlayerEquipmentManager playerEquipmentManager = player.GetComponent<PlayerEquipmentManager>();
        Item[] itemAtDestination = new Item[0];
        if (endTile.GetComponent<ItemSlot>().IsSlotInHotbar()) {
            itemAtDestination = playerEquipmentManager.GetItemFromInventoryHotBarByIndex(slotIndex);
        }
        if (endTile.GetComponent<ItemSlot>().IsSlotInInventory()) {
            itemAtDestination = playerEquipmentManager.GetItemFromInventoryByIndex(slotIndex);
        }

        // Set end tile to move item.
        if (endTile.GetComponent<ItemSlot>().IsSlotInHotbar()) {
            playerEquipmentManager.SetItemInInventoryHotBarByIndex(slotIndex, moveItem);
        }
        if (endTile.GetComponent<ItemSlot>().IsSlotInInventory()) {
            playerEquipmentManager.SetItemInInventoryByIndex(slotIndex, moveItem);
        }

        // Set start tile to nothing if the end tile was empty, or the item in the end tile slot if occupied.
        ItemSlot startTileScript = startTile.GetComponent<ItemSlot>();
        if (itemAtDestination.Length == 0) {
            if (startTileScript.IsSlotInHotbar()) {
                playerEquipmentManager.RemoveItemFromInventoryHotBarByIndex(startTileScript.GetIndex());
            }
            if (startTileScript.IsSlotInInventory()) {
                playerEquipmentManager.RemoveItemFromInventoryByIndex(startTileScript.GetIndex());
            }
        } else {
            if (startTileScript.IsSlotInHotbar()) {
                playerEquipmentManager.SetItemInInventoryHotBarByIndex(startTileScript.GetIndex(), itemAtDestination[0]);
            }
            if (startTileScript.IsSlotInInventory()) {
                playerEquipmentManager.SetItemInInventoryByIndex(startTileScript.GetIndex(), itemAtDestination[0]);
            }
        }

    }

    public void MoveStart(GameObject selectedTile) {
        Item item = selectedTile.GetComponent<Item>();
        if (item == null || string.IsNullOrEmpty(item.GetItemId())) {
            Debug.LogError("No item in slot.");
            return;
        }
        Debug.Log("Beginning move for " + item.GetItemDisplayName());
        moveIndicator = Instantiate(
            PrefabManager.instance.GetPrefabByName(ConstantsManager.gameObjectMoveIndicator),
            GameObject.FindGameObjectWithTag(ConstantsManager.tagCanvas).transform
        );
        moveIndicator.transform.Find(ConstantsManager.gameObjectIconName).GetComponent<Image>().sprite = item.GetItemIcon();
        moveItem = item;
        startTile = selectedTile;
    }

    public void ToggleInventoryDisplay() {
        if (interfaceDisplayObject == null) {
            interfaceDisplayObject = Instantiate(
                PrefabManager.instance.GetPrefabInventoryScreenObject(),
                GameObject.FindGameObjectWithTag(ConstantsManager.tagCanvas).transform
            );
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            GameObject player = GameObject.FindGameObjectWithTag(ConstantsManager.tagPlayer);
            PlayerEquipmentManager playerEquipmentManager = player.GetComponent<PlayerEquipmentManager>();
            Dictionary<int, Item> inventory = playerEquipmentManager.GetInventory();

            if (inventory.Count == 0) {
                return;
            }

            GameObject displayArea = interfaceDisplayObject.transform
                .Find(ConstantsManager.gameObjectBackgroundName)
                .Find(ConstantsManager.gameObjectDisplaySpaceName).gameObject;
            int spacesWide = GameManager.instance.GetInterfaceInventoryDisplayWidth() / GameManager.instance.GetInterfaceInventoryTileSideLength();
            for (int itemIndex = 0; itemIndex < inventory.Count; itemIndex++) {
                GameObject itemTile = Instantiate(
                    PrefabManager.instance.GetPrefabInventoryItemTile(),
                    displayArea.transform
                );
                float xCoord = itemTile.GetComponent<Transform>().localPosition.x + (((float)itemIndex % (float)spacesWide) * (float)GameManager.instance.GetInterfaceInventoryTileSideLength());
                float yCoord = itemTile.GetComponent<Transform>().localPosition.y - (((float)itemIndex / (float)spacesWide)  * (float)GameManager.instance.GetInterfaceInventoryTileSideLength());

                itemTile.GetComponent<Transform>().localPosition = new Vector3(xCoord, yCoord, 0);

                Sprite sprite = inventory[itemIndex].GetItemIcon();
                Debug.Log(sprite.name);
                GameObject iconObject = itemTile.transform.Find(ConstantsManager.gameObjectIconName).gameObject;
                iconObject.GetComponent<Image>().sprite = sprite;
            }

            return;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Destroy(interfaceDisplayObject);
            interfaceDisplayObject = null;
            return;
        }
    }

    public void UpdateHotBarSlots() {
        GameObject slotContainer = GameObject.FindGameObjectWithTag(ConstantsManager.tagHotBar).transform.Find(ConstantsManager.gameObjectBackgroundName).gameObject;
        for (int slotIndex = 0; slotIndex < 10; slotIndex++) {
            Item[] item = GameObject.FindGameObjectWithTag(ConstantsManager.tagPlayer).GetComponent<PlayerEquipmentManager>().GetItemFromInventoryHotBarByIndex(slotIndex);
            GameObject slotObject = slotContainer.transform.Find(ConstantsManager.gameObjectInventoryItemTileName + ConstantsManager.splitCharUnderscore + slotIndex).gameObject;
            if (item.Length == 0) {
                slotObject.GetComponent<Item>().CloneItemValues(new Item());
                slotObject.transform.Find(ConstantsManager.gameObjectIconName).gameObject.GetComponent<Image>().sprite = PrefabManager.instance.GetTextureById(ConstantsManager.itemIdBlankTile);
                slotObject.transform.Find(ConstantsManager.gameObjectIconName).Find(ConstantsManager.gameObjectQuantityName).gameObject.GetComponent<Text>().text = "0";
                continue;
            }
            slotObject.GetComponent<Item>().CloneItemValues(item[0]);
            slotObject.transform.Find(ConstantsManager.gameObjectIconName).gameObject.GetComponent<Image>().sprite = item[0].GetItemIcon();
            slotObject.transform.Find(ConstantsManager.gameObjectIconName).Find(ConstantsManager.gameObjectQuantityName).gameObject.GetComponent<Text>().text = item[0].GetItemQuantity().ToString();
        }
    }

    public void UpdatePickupText(string itemDisplayName) {
        if ("".Equals(itemDisplayName)) {
            if (pickUpTextObject != null) {
                Destroy(pickUpTextObject);
            }
            return;
        }
        
        if (pickUpTextObject == null) {
            pickUpTextObject = Instantiate(
                PrefabManager.instance.GetPrefabPickUpText(),
                GameObject.FindGameObjectWithTag(ConstantsManager.tagCanvas).transform
            );
        }

        GameObject textObject = pickUpTextObject.transform.Find(ConstantsManager.gameObjectTextName).gameObject;
        textObject.GetComponent<Text>().text = ConstantsManager.textPickUpBase + " " + itemDisplayName;
    }
}
