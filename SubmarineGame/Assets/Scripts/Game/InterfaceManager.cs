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
    private Item moveItem = new Item();
    private GameObject startTile = null;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHotBarSlots();
        UpdateInventorySlots();
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
        if (endTile.GetComponent<ItemSlot>().IsSlotNowhere()) {
            playerEquipmentManager.DropItemAtIndex(startTile.GetComponent<ItemSlot>().GetItemInventoryLocation(), startTile.GetComponent<ItemSlot>().GetIndex());
        }
        if (endTile.GetComponent<ItemSlot>().IsSlotInHotbar()) {
            endTile.GetComponent<Item>().CloneItemValues(moveItem);
            playerEquipmentManager.SetItemInInventoryHotBarByIndex(slotIndex, moveItem);
        }
        if (endTile.GetComponent<ItemSlot>().IsSlotInInventory()) {
            endTile.GetComponent<Item>().CloneItemValues(moveItem);
            playerEquipmentManager.SetItemInInventoryByIndex(slotIndex, moveItem);
        }

        // Set start tile to nothing if the end tile was empty, or the item in the end tile slot if occupied.
        ItemSlot startTileScript = startTile.GetComponent<ItemSlot>();
        startTile.GetComponent<Item>().CloneItemValues(moveItem);
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

        if (startTile.GetComponent<ItemSlot>().IsSlotInHotbar() && startTile.GetComponent<ItemSlot>().GetIndex() == GetHotBarIndex()) {
            playerEquipmentManager.UpdateEquippedObject();
        }
        if (endTile.GetComponent<ItemSlot>().IsSlotInHotbar() && endTile.GetComponent<ItemSlot>().GetIndex() == GetHotBarIndex()) {
            playerEquipmentManager.UpdateEquippedObject();
        }

        moveItem = new Item();
        startTile = null;
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
        moveItem.CloneItemValues(item);
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

    public void UpdateInventorySlots() {
        if (interfaceDisplayObject == null) {
            return;
        }
        
        GameObject slotContainer = interfaceDisplayObject.transform.Find(ConstantsManager.gameObjectBackgroundName).Find(ConstantsManager.gameObjectDisplaySpaceName).gameObject;
        for (int slotIndex = 0; slotIndex < GameManager.instance.GetPlayerInventorySize(); slotIndex++) {
            Item[] item = GameObject.FindGameObjectWithTag(ConstantsManager.tagPlayer).GetComponent<PlayerEquipmentManager>().GetItemFromInventoryByIndex(slotIndex);
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
