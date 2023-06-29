using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private GameObject anchorOne = null;
    public void SetAnchorOne(GameObject anchorOne) { this.anchorOne = anchorOne; }
    private GameObject anchorTwo = null;
    public void SetAnchorTwo(GameObject anchorTwo) { this.anchorTwo = anchorTwo; }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<LineRenderer>().startWidth = GameManager.instance.GetPlayerRopeWidth();
        GetComponent<LineRenderer>().endWidth = GameManager.instance.GetPlayerRopeWidth();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startPos = anchorOne == null ? Vector3.zero : anchorOne.transform.position;
        Vector3 endPos = anchorTwo == null ? Vector3.zero : anchorTwo.transform.position;
        GetComponent<LineRenderer>().SetPositions(new Vector3[] { startPos, endPos });
    }
}
