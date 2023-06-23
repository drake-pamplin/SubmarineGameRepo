using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerStateManager playerStateManager;

    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        playerStateManager = GetComponent<PlayerStateManager>();

        animator = transform.Find(ConstantsManager.gameObjectCameraName).Find(ConstantsManager.gameObjectAnimationName).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStateManager.IsPlayerIdleState()) {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(ConstantsManager.animationIdleName)) {
                animator.Play(ConstantsManager.animationIdleName);
            }
        }
        if (playerStateManager.IsPlayerSprintState()) {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(ConstantsManager.animationRunName)) {
                animator.Play(ConstantsManager.animationRunName);
            }
        }
        if (playerStateManager.IsPlayerWalkState()) {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(ConstantsManager.animationWalkName)) {
                animator.Play(ConstantsManager.animationWalkName);
            }
        }
    }
}
