using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    private PlayerStateManager playerStateManager;

    private float tickTimer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        playerStateManager = GameObject.FindGameObjectWithTag(ConstantsManager.tagPlayer).GetComponent<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tickTimer >= GameManager.instance.GetWorldNetRaycastTick()) {
            tickTimer = 0;
            CheckForDebris();
        } else {
            tickTimer += Time.deltaTime;
        }
    }

    private void CheckForDebris() {
        // Do not perform check if the net is held.
        if (!playerStateManager.IsThrowStateThrown()) {
            return;
        }

        List<GameObject> hits = new List<GameObject>();
        float deltaAngle = 360 / GameManager.instance.GetWorldNetRaycastCount();
        
        // Perform circle of raycasts.
        for (int raycastIndex = 0; raycastIndex < GameManager.instance.GetWorldNetRaycastCount(); raycastIndex++) {
            Vector3 raycastDirection = Quaternion.Euler(0, raycastIndex * deltaAngle, 0) * transform.forward;
            Debug.DrawRay(transform.position, raycastDirection * GameManager.instance.GetWorldNetRaycastDistance(), Color.red);
        }

        // Check for hits.

        // Add hit debris to net.
    }
}
