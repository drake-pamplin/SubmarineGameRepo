using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;

    private int hotBarIndex = 1;
    public int GetHotBarIndex() { return hotBarIndex; }
    /*
        Menu layers: 
        - 0: interfaceDisplayObject
    */
    private GameObject interfaceDisplayObject = null;
    private GameObject pickUpTextObject = null;

    void Awake() {
        instance = this;
    }
    
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
        if (newIndex > 10) {
            newIndex = 1;
        }
        if (newIndex < 1) {
            newIndex = 10;
        }
        hotBarIndex = newIndex;

        int xCoord = (hotBarIndex - 1) * GameManager.instance.GetInterfaceHotBarTileSideLength();
        highlightObject.transform.localPosition = new Vector3(xCoord, highlightObject.transform.localPosition.y, 0);
    }

    public bool IsDisplayOpen() {
        if (interfaceDisplayObject != null) {
            return true;
        }
        
        return false;
    }

    public void ToggleInventoryDisplay() {
        if (interfaceDisplayObject == null) {
            interfaceDisplayObject = Instantiate(
                PrefabManager.instance.GetPrefabInventoryScreenObject(),
                GameObject.FindGameObjectWithTag(ConstantsManager.tagCanvas).transform
            );

            GameObject player = GameObject.FindGameObjectWithTag(ConstantsManager.tagPlayer);
            PlayerEquipmentManager playerEquipmentManager = player.GetComponent<PlayerEquipmentManager>();
            List<Item> inventory = playerEquipmentManager.GetInventory();

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
            Destroy(interfaceDisplayObject);
            interfaceDisplayObject = null;
            return;
        }
    }

    public void UpdateHotBarSlots() {
        GameObject slotContainer = GameObject.FindGameObjectWithTag(ConstantsManager.tagHotBar).transform.Find(ConstantsManager.gameObjectBackgroundName).gameObject;
        for (int slotIndex = 1; slotIndex <= 10; slotIndex++) {
            Item[] item = GameObject.FindGameObjectWithTag(ConstantsManager.tagPlayer).GetComponent<PlayerEquipmentManager>().GetItemFromInventoryHotBar(slotIndex);
            GameObject slotObject = slotContainer.transform.Find(ConstantsManager.gameObjectHotBarSlotBase + ConstantsManager.splitCharUnderscore + slotIndex).gameObject;
            if (item.Length == 0) {
                slotObject.GetComponent<Item>().CloneItemValues(new Item());
                slotObject.transform.Find(ConstantsManager.gameObjectIconName).gameObject.GetComponent<Image>().sprite = null;
                continue;
            }
            slotObject.GetComponent<Item>().CloneItemValues(item[0]);
            slotObject.transform.Find(ConstantsManager.gameObjectIconName).gameObject.GetComponent<Image>().sprite = item[0].GetItemIcon();
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
