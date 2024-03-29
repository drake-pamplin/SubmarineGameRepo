using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerEquipmentManager playerEquipmentManager;
    private PlayerStateManager playerStateManager;

    private Animator animator;
    private GameObject displayObject = null;
    private string[] equipStateNames = new string[] {
        ConstantsManager.animationEquipName
    };
    
    // Start is called before the first frame update
    void Start()
    {
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerStateManager = GetComponent<PlayerStateManager>();

        animator = transform.Find(ConstantsManager.gameObjectCameraName).Find(ConstantsManager.gameObjectAnimationName).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessAnimationState();
        ProcessThrownObjectState();
    }

    public GameObject GetDisplayPrefab(string itemCode) {
        GameObject prefab = PrefabManager.instance.GetPrefabByName(ConstantsManager.gameObjectGenericDisplayObject);

        if (ConstantsManager.itemIdNet.Equals(itemCode)) {
            prefab = PrefabManager.instance.GetPrefabNetObject();
        }

        return prefab;
    }

    private string GetItemAnimationName(string baseName) {
        string animationName = baseName;

        if (playerEquipmentManager.IsItemEquipped()) {
            string equippedItemId = playerEquipmentManager.GetEquippedItem()[0].GetItemId();
            if (equippedItemId == ConstantsManager.itemIdNet) {
                animationName += ConstantsManager.splitCharUnderscore + equippedItemId;
            }
        }

        return animationName;
    }

    public void HandleItemEquipped() {
        Destroy(displayObject);
        
        if (playerEquipmentManager.IsItemEquipped()) {
            Item equippedItem = playerEquipmentManager.GetEquippedItem()[0];
            string itemId = equippedItem.GetItemId();

            Transform referenceTransform = transform.Find(ConstantsManager.gameObjectCameraName).Find(ConstantsManager.gameObjectAnimationName).Find(ConstantsManager.gameObjectArmMesh).Find(ConstantsManager.gameObjectLeftArm).Find(ConstantsManager.gameObjectFist);
            if (itemId == ConstantsManager.itemIdNet) {
                referenceTransform = GameObject.FindGameObjectWithTag(ConstantsManager.tagItemReference).transform;
            }
            displayObject = Instantiate(
                GetDisplayPrefab(itemId),
                referenceTransform
            );
        }

        string animationName = GetItemAnimationName(ConstantsManager.animationEquipBase);
        animator.Play(animationName);
    }

    public bool IsPlayerEquipping() {
        string equipStateName = GetItemAnimationName(ConstantsManager.animationEquipBase);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(equipStateName)) {
            return true;
        }
        return false;
    }

    public bool IsPlayerPulling() {
        string equipStateName = GetItemAnimationName(ConstantsManager.animationPullBase);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(equipStateName)) {
            return true;
        }
        return false;
    }
    
    public bool IsPlayerThrowing() {
        string throwStateName = GetItemAnimationName(ConstantsManager.animationThrowBase);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(throwStateName)) {
            return true;
        }
        return false;
    }

    private void ProcessAnimationState() {
        string animationName = "";
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(ConstantsManager.animationDefaultName)) {
            animationName = GetItemAnimationName(ConstantsManager.animationEquipBase);
            animator.Play(animationName);
        }
        // Additional checks for default since it takes a minute to register when going into the scene.
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(ConstantsManager.animationDefaultName)) {
            return;
        }

        if (IsPlayerEquipping()) {
            return;
        }
        // if (IsPlayerPulling()) {
        //     return;
        // }
        if (IsPlayerThrowing()) {
            return;
        }
        
        if (playerStateManager.IsPlayerBreastStrokeState()) {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(ConstantsManager.animationBreastStrokeName)) {
                animator.Play(ConstantsManager.animationBreastStrokeName);
            }
        }
        if (playerStateManager.IsPlayerChargingState()) {
            animationName = GetItemAnimationName(ConstantsManager.animationChargeBase);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
                animator.Play(animationName);
            }
        }
        if (playerStateManager.IsPlayerFreestyleStrokeState()) {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(ConstantsManager.animationFreestyleStrokeName)) {
                animator.Play(ConstantsManager.animationFreestyleStrokeName);
            }
        }
        if (playerStateManager.IsPlayerIdleState()) {
            animationName = GetItemAnimationName(ConstantsManager.animationIdleBase);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
                animator.Play(animationName);
            }
        }
        if (playerStateManager.IsPlayerPullingState()) {
            animationName = GetItemAnimationName(ConstantsManager.animationPullBase);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
                animator.Play(animationName);
            }
        }
        if (playerStateManager.IsPlayerSprintState()) {
            animationName = GetItemAnimationName(ConstantsManager.animationRunBase);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
                animator.Play(animationName);
            }
        }
        if (playerStateManager.IsPlayerThrowingState()) {
            animationName = GetItemAnimationName(ConstantsManager.animationThrowBase);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
                animator.Play(animationName);
            }
        }
        if (playerStateManager.IsPlayerTreadState()) {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(ConstantsManager.animationTreadName)) {
                animator.Play(ConstantsManager.animationTreadName);
            }
        }
        if (playerStateManager.IsPlayerWalkState()) {
            animationName = GetItemAnimationName(ConstantsManager.animationWalkBase);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
                animator.Play(animationName);
            }
        }
    }

    private void ProcessThrownObjectState() {
        if (!playerEquipmentManager.IsItemEquipped()) {
            return;
        }

        Item item = playerEquipmentManager.GetEquippedItem()[0];
        if (!ConstantsManager.itemIdThrowable.Contains(item.GetItemId())) {
            return;
        }

        if (playerStateManager.IsThrowStateThrown()) {
            if (displayObject != null) {
                if (!ConstantsManager.gameObjectRopeCoilObjectName.Equals(displayObject.name)) {
                    Destroy(displayObject);
                }
            }
            if (displayObject == null) {
                displayObject = Instantiate(
                    PrefabManager.instance.GetPrefabRopeCoilObject(),
                    GameObject.FindGameObjectWithTag(ConstantsManager.tagLeftHandMount).transform
                );
                displayObject.name = ConstantsManager.gameObjectRopeCoilObjectName;
                displayObject.transform.Find(ConstantsManager.gameObjectRopeObjectName).GetComponent<Rope>().SetAnchorOne(
                    GameObject.FindGameObjectWithTag(ConstantsManager.tagLeftHandMount).gameObject
                );
                displayObject.transform.Find(ConstantsManager.gameObjectRopeObjectName).GetComponent<Rope>().SetAnchorTwo(
                    GameObject.FindGameObjectWithTag(ConstantsManager.tagRightHandMount).gameObject
                );
            }
        } else {
            GameObject prefab = GetDisplayPrefab(playerEquipmentManager.GetEquippedItem()[0].GetItemId());
            if (displayObject != null) {
                if (!prefab.name.Equals(displayObject.name)) {
                    Destroy(displayObject);
                }
            }
            if (displayObject == null) {
                displayObject = Instantiate(
                    prefab,
                    GameObject.FindGameObjectWithTag(ConstantsManager.tagItemReference).transform
                );
                displayObject.name = prefab.name;
            }
        }
    }
}
