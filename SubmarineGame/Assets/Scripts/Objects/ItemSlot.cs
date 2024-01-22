using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int index;
    public int GetIndex() { return index; }
    public Item.ItemInventoryLocation inventoryLocation;
    public bool IsSlotInHotbar() { return inventoryLocation == Item.ItemInventoryLocation.Hotbar; }
    public bool IsSlotInInventory() { return inventoryLocation == Item.ItemInventoryLocation.Inventory; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MoveEnd(GameObject selectedTile) {
        InterfaceManager.instance.MoveEnd(selectedTile);
    }

    private void MoveStart(GameObject selectedTile) {
        InterfaceManager.instance.MoveStart(selectedTile);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            Debug.Log("Mouse left down on " + eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.name);
            MoveStart(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject);
        }
        if (eventData.button == PointerEventData.InputButton.Right) {
            Debug.Log("Mouse right down on " + eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.name);
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            Debug.Log("Mouse left up on " + eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.name);
            MoveEnd(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject);
        }
        if (eventData.button == PointerEventData.InputButton.Right) {
            Debug.Log("Mouse right up on " + eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.name);
        }
    }
}
