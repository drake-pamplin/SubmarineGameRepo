using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycastManager : MonoBehaviour
{
    private PlayerMovementManager playerMovementManager;
    
    private GameObject scannedObject = null;
    public GameObject GetScannedObject() { return scannedObject; }
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovementManager = GetComponent<PlayerMovementManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractableInRange();
    }

    private void CheckForInteractableInRange() {
        GameObject hitObject = PerformRaycast(GameManager.instance.GetRaycastInteractDistance());
        if (hitObject == null) {
            InterfaceManager.instance.UpdatePickupText("");
            scannedObject = null;
            return;
        }
        if (hitObject.CompareTag(ConstantsManager.tagItem)) {
            HandleItemLookedAt(hitObject);
            return;
        }
    }

    private void HandleItemLookedAt(GameObject itemObject) {
        Item item = itemObject.GetComponent<Item>();
        InterfaceManager.instance.UpdatePickupText(item.GetItemDisplayName());
        scannedObject = itemObject;
    }

    private GameObject PerformRaycast(float raycastDistance) {
        Vector3 raycastOrigin = transform.position;
        Vector3 raycastDirection = playerMovementManager.GetCameraObject().transform.forward;
        raycastOrigin.y += GameManager.instance.GetRaycastOriginHeight();
        Debug.DrawRay(raycastOrigin, raycastDirection * raycastDistance, Color.red);
        RaycastHit raycastHit;
        if (Physics.Raycast(raycastOrigin, raycastDirection, out raycastHit, raycastDistance)) {
            GameObject hitObject = raycastHit.collider.gameObject;
            return hitObject;
        } else {
            return null;
        }
    }
}
