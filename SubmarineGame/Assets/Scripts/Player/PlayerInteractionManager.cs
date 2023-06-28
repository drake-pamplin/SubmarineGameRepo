using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    private PlayerEquipmentManager playerEquipmentManager;
    private PlayerRaycastManager playerRaycastManager;
    
    // Start is called before the first frame update
    void Start()
    {
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerRaycastManager = GetComponent<PlayerRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.IsGameStateGame()) {
            return;
        }
        
        ProcessInteractionInput();
        ProcessScrollInput();
    }

    private void ProcessInteractionInput() {
        bool interactionInput = InputManager.instance.GetInteractInput();
        
        if (!interactionInput) {
            return;
        }

        GameObject interactableObject = playerRaycastManager.GetScannedObject();

        if (interactableObject == null) {
            return;
        }

        Item item = interactableObject.GetComponent<Item>();
        Debug.Log("Picked up " + item.GetItemDisplayName());
        playerEquipmentManager.PickUpItem(item);
        Destroy(interactableObject);
    }

    private void ProcessScrollInput() {
        float scrollInput = InputManager.instance.GetScrollValue();

        if (scrollInput == 0) {
            return;
        }

        InterfaceManager.instance.HotBarScrollHighlight(scrollInput > 0);
        GetComponent<PlayerEquipmentManager>().UpdateEquippedObject();
    }
}
