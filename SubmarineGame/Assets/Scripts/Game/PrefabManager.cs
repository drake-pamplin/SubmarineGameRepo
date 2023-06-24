using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager instance;

    private Dictionary<string, GameObject> prefabLibrary = new Dictionary<string, GameObject>();
    public GameObject GetPrefabNetObject() { return prefabLibrary[ConstantsManager.gameObjectNetObjectName]; }
    public GameObject GetPrefabPickUpText() { return prefabLibrary[ConstantsManager.gameObjectItemPromptObjectName]; }

    void Awake() {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        LoadPrefabs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadPrefabs() {
        string fileName = "";

        fileName = ConstantsManager.filePrefabValue + ConstantsManager.fileInterfaceValue + ConstantsManager.gameObjectItemPromptObjectName;
        prefabLibrary.Add(ConstantsManager.gameObjectItemPromptObjectName ,Resources.Load<GameObject>(fileName));

        fileName = ConstantsManager.filePrefabValue + ConstantsManager.fileToolsValue + ConstantsManager.gameObjectNetObjectName;
        prefabLibrary.Add(ConstantsManager.gameObjectNetObjectName ,Resources.Load<GameObject>(fileName));
    }
}