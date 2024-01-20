using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowMouseOnCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = InputManager.instance.GetMousePosition();
        float horizontalRatio = mousePosition.x / Screen.width;
        float verticalRatio = mousePosition.y / Screen.height;

        Vector2 referenceResolution = GameObject.FindGameObjectWithTag(ConstantsManager.tagCanvas).GetComponent<CanvasScaler>().referenceResolution;
        Vector2 normalizedMousePosition = new Vector2(
            referenceResolution.x * horizontalRatio,
            referenceResolution.y * verticalRatio
        );
        GetComponent<RectTransform>().anchoredPosition = normalizedMousePosition;
    }
}
