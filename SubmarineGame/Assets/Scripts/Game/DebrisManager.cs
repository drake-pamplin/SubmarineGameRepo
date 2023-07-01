using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisManager : MonoBehaviour
{
    private int debrisSpawnIndex = 0;
    private float debrisSpawnTimer = 0;
    private float targetTime = 0;
    private bool IsTimerExpired() { return debrisSpawnTimer >= targetTime; }
    
    // Start is called before the first frame update
    void Start()
    {
        SetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessDebrisTimers();
        ProcessSpawnedDebris();
    }

    private void ProcessDebrisTimers() {
        
    }

    private void ProcessSpawnedDebris() {
        if (!IsTimerExpired()) {
            debrisSpawnTimer += Time.deltaTime;
            return;
        }

        GameObject[] debris = GameObject.FindGameObjectsWithTag(ConstantsManager.tagDebris);
        if (debris.Length < GameManager.instance.GetDebrisMaxSpawns()) {
            SpawnDebris();
        }

        SetTimer();
    }

    private void SetTimer() {
        targetTime = Time.time + Random.Range(GameManager.instance.GetDebrisSpawnTimerMinimum(), GameManager.instance.GetDebrisSpawnTimerMaximum());
        debrisSpawnTimer = Time.time;
    }

    private void SpawnDebris() {
        // Get ray in a direction.
        GameObject player = GameObject.FindGameObjectWithTag(ConstantsManager.tagPlayer);
        Vector3 direction = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0) * player.transform.forward;
        Ray ray = new Ray();
        ray.origin = player.transform.position;
        ray.direction = direction;

        // Set distance for point along the ray.
        float distance = UnityEngine.Random.Range(0, GameManager.instance.GetDebrisMaxSpawnRange());

        // Iterate till index is equal to/more than the max range.
        Vector3 spawnPoint = Vector3.zero;
        for (int checkIndex = 0; checkIndex < GameManager.instance.GetDebrisMaxSpawnRange(); checkIndex++) {
            // Spawn point set? Break.
            if (spawnPoint != Vector3.zero) {
                break;
            }

            // Get point along the ray.
            Vector3 point = ray.GetPoint(distance);

            // Raycast from the sky.
            Vector3 origin = point;
            origin.y = 20;
            Vector3 downRay = Vector3.down;
            RaycastHit raycastHit;
            if (Physics.Raycast(origin, downRay, out raycastHit, 100)) {
                GameObject hitObject = raycastHit.collider.gameObject;
                // Hit the water
                if (hitObject.CompareTag(ConstantsManager.tagWater)) {
                    // Set spawn point to the raycast hit point.
                    spawnPoint = raycastHit.point;
                }
                // Not water
                else {
                    // Get point one meter further out.
                    distance += 1;
                    // If point is out of range, subtract range max from point distance
                    if (distance >= GameManager.instance.GetDebrisMaxSpawnRange()) {
                        distance -= GameManager.instance.GetDebrisMaxSpawnRange();
                    }
                }
            } 
            // Hit nothing, try again.
            else {
                // Get point one meter further out.
                distance += 1;
                // If point is out of range, subtract range max from point distance
                if (distance >= GameManager.instance.GetDebrisMaxSpawnRange()) {
                    distance -= GameManager.instance.GetDebrisMaxSpawnRange();
                }
            }
        }

        if (spawnPoint == Vector3.zero) {
            return;
        }

        // Spawn item at location
        spawnPoint.y = GameManager.instance.GetWorldWaterLine();
        GameObject bubblesObject = Instantiate(
            PrefabManager.instance.GetPrefabBubblesObject(),
            spawnPoint,
            Quaternion.identity
        );
        Destroy(bubblesObject, 3);
        GameObject newDebris = Instantiate(
            PrefabManager.instance.GetPrefabDebrisItem(),
            spawnPoint,
            Quaternion.identity
        );
        newDebris.GetComponent<Debris>().SetSpawnTime(Time.time);
    }
}
