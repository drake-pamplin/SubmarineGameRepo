using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager instance;

    private Dictionary<string, GameObject> prefabLibrary = new Dictionary<string, GameObject>();
    public GameObject GetPrefabBubblesObject() { return prefabLibrary[ConstantsManager.gameObjectBubblesObjectName]; }
    public GameObject GetPrefabDebrisItem() { return prefabLibrary[ConstantsManager.gameObjectDebrisObjectName]; }
    public GameObject GetPrefabInventoryScreenObject() { return prefabLibrary[ConstantsManager.gameObjectInventoryScreenObjectName]; }
    public GameObject GetPrefabInventoryItemTile() { return prefabLibrary[ConstantsManager.gameObjectInventoryItemTileName]; }
    public GameObject GetPrefabItem() { return prefabLibrary[ConstantsManager.gameObjectItemName]; }
    public GameObject GetPrefabNetObject() { return prefabLibrary[ConstantsManager.gameObjectNetObjectName]; }
    public GameObject GetPrefabPickUpText() { return prefabLibrary[ConstantsManager.gameObjectItemPromptObjectName]; }
    public GameObject GetPrefabRopeCoilObject() { return prefabLibrary[ConstantsManager.gameObjectRopeCoilObjectName]; }
    public GameObject GetPrefabRopeObject() { return prefabLibrary[ConstantsManager.gameObjectRopeObjectName]; }
    public GameObject GetPrefabByName(string prefabName) {
        GameObject prefab = null;
        prefabLibrary.TryGetValue(prefabName, out prefab);
        return prefab;
    }

    private Dictionary<string, Sprite> textureLibrary = new Dictionary<string, Sprite>();
    public Sprite GetTextureById(string itemId) {
        Sprite texture = null;
        textureLibrary.TryGetValue(itemId, out texture);
        return texture;
    }

    void Awake() {
        instance = this;
        LoadPrefabs();
        LoadTextures();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadPrefabs() {
        string fileName = "";
        string prefabName = "";

        fileName = ConstantsManager.filePrefabValue + ConstantsManager.fileWorldValue + ConstantsManager.gameObjectBubblesObjectName;
        prefabLibrary.Add(ConstantsManager.gameObjectBubblesObjectName ,Resources.Load<GameObject>(fileName));
        
        fileName = ConstantsManager.filePrefabValue + ConstantsManager.fileWorldValue + ConstantsManager.gameObjectDebrisObjectName;
        prefabLibrary.Add(ConstantsManager.gameObjectDebrisObjectName ,Resources.Load<GameObject>(fileName));
        
        fileName = ConstantsManager.filePrefabValue + ConstantsManager.fileInterfaceValue + ConstantsManager.gameObjectInventoryItemTileName;
        prefabLibrary.Add(ConstantsManager.gameObjectInventoryItemTileName ,Resources.Load<GameObject>(fileName));

        fileName = ConstantsManager.filePrefabValue + ConstantsManager.fileInterfaceValue + ConstantsManager.gameObjectInventoryScreenObjectName;
        prefabLibrary.Add(ConstantsManager.gameObjectInventoryScreenObjectName ,Resources.Load<GameObject>(fileName));

        fileName = ConstantsManager.filePrefabValue + ConstantsManager.fileWorldValue + ConstantsManager.gameObjectItemName;
        prefabLibrary.Add(ConstantsManager.gameObjectItemName ,Resources.Load<GameObject>(fileName));
        
        fileName = ConstantsManager.filePrefabValue + ConstantsManager.fileInterfaceValue + ConstantsManager.gameObjectItemPromptObjectName;
        prefabLibrary.Add(ConstantsManager.gameObjectItemPromptObjectName ,Resources.Load<GameObject>(fileName));

        fileName = ConstantsManager.filePrefabValue + ConstantsManager.fileToolsValue + ConstantsManager.gameObjectNetObjectName;
        prefabLibrary.Add(ConstantsManager.gameObjectNetObjectName ,Resources.Load<GameObject>(fileName));

        fileName = ConstantsManager.filePrefabValue + ConstantsManager.fileWorldValue + ConstantsManager.gameObjectRopeCoilObjectName;
        prefabLibrary.Add(ConstantsManager.gameObjectRopeCoilObjectName ,Resources.Load<GameObject>(fileName));
        
        fileName = ConstantsManager.filePrefabValue + ConstantsManager.fileWorldValue + ConstantsManager.gameObjectRopeObjectName;
        prefabLibrary.Add(ConstantsManager.gameObjectRopeObjectName ,Resources.Load<GameObject>(fileName));

        prefabName = ConstantsManager.gameObjectGenericDisplayObject;
        fileName = ConstantsManager.fileToolsPrefabsValue + prefabName;
        prefabLibrary.Add(prefabName, Resources.Load<GameObject>(fileName));
    }

    private void LoadTextures() {
        string fileName = "";
        string textureName = "";

        // Item textures.
        textureName = ConstantsManager.itemIdNet;
        fileName = ConstantsManager.fileItemTexturesValue + textureName;
        textureLibrary.Add(textureName, Resources.Load<Sprite>(fileName));

        textureName = ConstantsManager.itemIdWood;
        fileName = ConstantsManager.fileItemTexturesValue + textureName;
        textureLibrary.Add(textureName, Resources.Load<Sprite>(fileName));
    }
}
