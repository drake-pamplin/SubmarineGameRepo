using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    private PlayerAnimationController playerAnimationController;
    private PlayerEquipmentManager playerEquipmentManager;
    private PlayerMovementManager playerMovementManager;
    private PlayerRaycastManager playerRaycastManager;
    private PlayerStateManager playerStateManager;

    private float chargedTime = 0;
    public bool IsTimeCharged() { return chargedTime > 0; }
    private bool charging = false;
    public bool IsPlayerCharging() { return charging; }

    private Vector3 thrownObjectMotionVector = Vector3.zero;
    private Vector3 thrownObjectPullVector = Vector3.zero;
    private GameObject thrownDisplayObject = null;
    private GameObject thrownRopeObject = null;
    
    // Start is called before the first frame update
    void Start()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerMovementManager = GetComponent<PlayerMovementManager>();
        playerRaycastManager = GetComponent<PlayerRaycastManager>();
        playerStateManager = GetComponent<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.IsGameStateGame()) {
            return;
        }
        
        ProcessInteractionInput();
        ProcessPullInput();
        ProcessScrollInput();
        ProcessThrowInput();
        ProcessThrownObjectMotion();
    }

    public void DestroyDisplayObjects() {
        if (thrownRopeObject != null) {
            Destroy(thrownRopeObject);
            thrownRopeObject = null;
        }
        if (thrownDisplayObject != null) {
            Destroy(thrownDisplayObject);
            thrownDisplayObject = null;
        }
    }

    public bool IsPlayerPulling() {
        return thrownObjectPullVector != Vector3.zero;
    }

    private bool IsToolRetrievable() {
        Vector3 toolPos = thrownDisplayObject.transform.position;
        toolPos.y = 0;
        Vector3 playerPos = transform.position;
        playerPos.y = 0;
        float toolDistance = Vector3.Distance(toolPos, playerPos);
        if (toolDistance < GameManager.instance.GetPlayerToolRetrievalRange()) {
            return true;
        }
        return false;
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

    private void ProcessPullInput() {
        thrownObjectPullVector = Vector3.zero;

        if (!playerStateManager.IsThrowStateThrown()) {
            return;
        }

        if (!InputManager.instance.GetRightClickDown()) {
            return;
        }

        if (IsToolRetrievable()) {
            DestroyDisplayObjects();
            playerStateManager.TriggerHeldState();
            return;
        }

        float toolHeight = thrownDisplayObject.transform.position.y;
        Vector3 playerPosition = transform.position;
        playerPosition.y = toolHeight;
        thrownObjectPullVector = playerPosition - thrownDisplayObject.transform.position;
        thrownObjectPullVector = thrownObjectPullVector.normalized;
        thrownObjectPullVector *= GameManager.instance.GetPlayerPullSpeed();
    }

    private void ProcessScrollInput() {
        if (playerStateManager.IsThrowStateThrown()) {
            return;
        }
        
        float scrollInput = InputManager.instance.GetScrollValue();

        if (scrollInput == 0) {
            return;
        }

        InterfaceManager.instance.HotBarScrollHighlight(scrollInput > 0);
        if (playerStateManager.IsMovementStateGrounded()) {
            GetComponent<PlayerEquipmentManager>().UpdateEquippedObject();
        }
    }

    private void ProcessThrowInput() {
        if (playerAnimationController.IsPlayerEquipping()) {
            charging = false;
            return;
        }
        
        if (!playerEquipmentManager.IsItemEquipped()) {
            charging = false;
            return;
        }

        Item item = playerEquipmentManager.GetEquippedItem()[0];
        if (!ConstantsManager.itemIdThrowable.Contains(item.GetItemId())) {
            charging = false;
            return;
        }

        if (playerStateManager.IsThrowStateThrown()) {
            return;
        }
        
        if (InputManager.instance.GetLeftClickDown()) {
            if (charging) {
                chargedTime += Time.deltaTime;
                return;
            }
            charging = true;
            chargedTime = 0;
            return;
        }
        
        if (!charging) {
            return;
        }

        charging = false;
        float chargedPowerModifier = 0;
        if (chargedTime >= GameManager.instance.GetPlayerThrowChargeTime()) {
            chargedPowerModifier = 1;
        } else {
            chargedPowerModifier = chargedTime / GameManager.instance.GetPlayerThrowChargeTime();
        }
        float chargedPower = GameManager.instance.GetPlayerThrowMaxForce() * chargedPowerModifier;
        thrownObjectMotionVector = playerMovementManager.GetCameraObject().transform.forward * chargedPower;
        
        playerStateManager.TriggerThrowFlag();

        Debug.Log("Charged throw for " + chargedPower);
    }

    private void ProcessThrownObjectMotion() {
        if (!playerStateManager.IsThrowStateThrown()) {
            return;
        }

        if (thrownDisplayObject == null) {
            GameObject prefab = playerAnimationController.GetDisplayPrefab(playerEquipmentManager.GetEquippedItem()[0].GetItemId());
            thrownDisplayObject = Instantiate(
                prefab,
                new Vector3(transform.position.x, GameManager.instance.GetRaycastOriginHeight(), transform.position.z),
                Quaternion.identity
            );
        }

        if (thrownRopeObject == null) {
            GameObject ropePrefab = PrefabManager.instance.GetPrefabRopeObject();
            thrownRopeObject = Instantiate(
                ropePrefab,
                Vector3.zero,
                Quaternion.identity
            );
            Rope rope = thrownRopeObject.GetComponent<Rope>();
            rope.SetAnchorOne(thrownDisplayObject.transform.Find(ConstantsManager.gameObjectRopeAnchor).gameObject);
            rope.SetAnchorTwo(GameObject.FindGameObjectWithTag(ConstantsManager.tagRightHandMount));
        }

        Vector3 motionVector = Vector3.zero;
        if (thrownDisplayObject.transform.position.y <= GameManager.instance.GetWorldNetLine()) {
            thrownObjectMotionVector = Vector3.zero;
        }
        if (thrownDisplayObject.transform.position.y > GameManager.instance.GetWorldNetLine()) {
            motionVector = thrownObjectMotionVector * (Time.deltaTime * GameManager.instance.GetPlayerThrowSpeedSlowdown());
        } else {
            motionVector = thrownObjectPullVector * Time.deltaTime;
        }
        thrownDisplayObject.transform.Translate(motionVector);
        thrownObjectMotionVector.y -= GameManager.instance.GetWorldGravityValue();
    }
}
