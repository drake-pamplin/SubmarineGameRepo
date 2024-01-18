using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    private PlayerStateManager playerStateManager;

    private float tickTimer = 0;
    private List<GameObject> caughtDebris = new List<GameObject>();
    
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

        HandleCaughtDebrisTracking();
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
            RaycastHit hit;
            if (Physics.Raycast(transform.position, raycastDirection, out hit, GameManager.instance.GetWorldNetRaycastDistance())) {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.CompareTag(ConstantsManager.tagDebris) && !hits.Contains(hitObject)) {
                    hits.Add(hitObject);
                }
            }
        }

        // Add hit debris to net.
        foreach (GameObject hitObject in hits) {
            hitObject.GetComponent<SphereCollider>().enabled = false;
            hitObject.GetComponent<Debris>().CatchDebris();
        }
        caughtDebris.AddRange(hits);
    }

    public void DestroyCaughtDebris() {
        foreach (GameObject caughtDebrisObject in caughtDebris) {
            Destroy(caughtDebrisObject);
        }
        caughtDebris = new List<GameObject>();
    }

    private void HandleCaughtDebrisTracking() {
        if (!playerStateManager.IsThrowStateThrown()) {
            return;
        }
        
        foreach (GameObject debrisObject in caughtDebris) {
            debrisObject.transform.position = transform.position;
        }
    }

    public void ReleaseCaughtDebris() {
        foreach (GameObject caughtDebrisObject in caughtDebris) {
            caughtDebrisObject.GetComponent<SphereCollider>().enabled = true;
            caughtDebrisObject.GetComponent<Debris>().ReleaseDebris();
        }
        caughtDebris = new List<GameObject>();
    }
}
