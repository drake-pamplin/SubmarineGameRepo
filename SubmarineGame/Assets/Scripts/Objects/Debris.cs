using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    private enum DebrisState {
        bobbing,
        sinking,
        surfacing
    }
    private DebrisState debrisState = DebrisState.surfacing;
    
    private float spawnTime = 0;
    public void SetSpawnTime(float time) { spawnTime = time; }

    private float targetTime;
    
    // Start is called before the first frame update
    void Start()
    {
        targetTime = Time.time + GameManager.instance.GetDebrisSurfaceDuration();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTime == 0) {
            return;
        }

        if (spawnTime >= targetTime) {
            if (debrisState == DebrisState.surfacing) {
                transform.Find(ConstantsManager.gameObjectDebrisItemName).gameObject.GetComponent<Animator>().Play(ConstantsManager.animationBobName);
                debrisState = DebrisState.bobbing;
                targetTime = Time.time + GameManager.instance.GetDebrisBobbingDuration();
                return;
            }
            if (debrisState == DebrisState.bobbing) {
                transform.Find(ConstantsManager.gameObjectDebrisItemName).gameObject.GetComponent<Animator>().Play(ConstantsManager.animationSinkName);
                debrisState = DebrisState.sinking;
                targetTime = Time.time + GameManager.instance.GetDebrisSinkDuration();
                return;
            }
            if (debrisState == DebrisState.sinking) {
                Destroy(gameObject);
                return;
            }
        }

        spawnTime += Time.deltaTime;
    }
}