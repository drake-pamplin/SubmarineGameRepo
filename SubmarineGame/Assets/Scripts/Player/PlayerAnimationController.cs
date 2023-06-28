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
    }

    private string GetItemAnimationName(string baseName) {
        string animationName = "";

        if (!playerEquipmentManager.IsItemEquipped()) {
            animationName = baseName;
        } else {
            Item equippedItem = playerEquipmentManager.GetEquippedItem()[0];
            string itemId = equippedItem.GetItemId();
            animationName = baseName + ConstantsManager.splitCharUnderscore + itemId;
        }

        return animationName;
    }

    public void HandleItemEquipped() {
        Destroy(displayObject);
        
        if (playerEquipmentManager.IsItemEquipped()) {
            Item equippedItem = playerEquipmentManager.GetEquippedItem()[0];
            string itemId = equippedItem.GetItemId();

            
            displayObject = Instantiate(
                PrefabManager.instance.GetPrefabNetObject(),
                GameObject.FindGameObjectWithTag(ConstantsManager.tagItemReference).transform
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
        if (playerStateManager.IsPlayerSprintState()) {
            animationName = GetItemAnimationName(ConstantsManager.animationRunBase);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
                animator.Play(animationName);
            }
        }
        if (playerStateManager.IsPlayerThrowingState()) {
            animationName = GetItemAnimationName(ConstantsManager.animationThrowBase);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
                Debug.Log("Throwing");
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
}
