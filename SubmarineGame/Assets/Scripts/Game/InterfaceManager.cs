using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;

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
