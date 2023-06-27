using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;

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
                    GameObject.FindGameObjectWithTag(ConstantsManager.tagCanvas).transform
                );
                float xCoord = itemTile.GetComponent<Transform>().localPosition.x + (((float)itemIndex % (float)spacesWide) * GameManager.instance.GetInterfaceInventoryTileSideLength());
                float yCoord = itemTile.GetComponent<Transform>().localPosition.y - (((float)itemIndex / (float)spacesWide)  * GameManager.instance.GetInterfaceInventoryTileSideLength());

                itemTile.GetComponent<Transform>().localPosition = new Vector3(xCoord, yCoord, 0);
            }

            return;
        } else {
            Destroy(interfaceDisplayObject);
            interfaceDisplayObject = null;
            return;
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
