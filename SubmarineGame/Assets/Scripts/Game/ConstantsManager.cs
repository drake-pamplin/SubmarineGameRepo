using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantsManager : MonoBehaviour
{
    public static readonly string animationBreastStrokeName = "BreastStroke";
    public static readonly string animationChargeBase = "Charge";
    public static readonly string animationDefaultName = "Default";
    public static readonly string animationEquipBase = "Equip";
    public static readonly string animationEquipName = "Equip";
    public static readonly string animationFreestyleStrokeName = "FreestyleStroke";
    public static readonly string animationIdleBase = "Idle";
    public static readonly string animationIdleName = "Idle";
    public static readonly string animationRunBase = "Run";
    public static readonly string animationRunName = "Run";
    public static readonly string animationThrowBase = "Throw";
    public static readonly string animationTreadName = "Tread";
    public static readonly string animationWalkBase = "Walk";
    public static readonly string animationWalkName = "Walk";

    public static readonly string fileInterfaceValue = "Interface/";
    public static readonly string filePrefabValue = "Prefabs/";
    public static readonly string fileToolsValue = "Tools/";
    public static readonly string fileWorldValue = "World/";
    
    public static readonly string gameObjectAnimationName = "Animation";
    public static readonly string gameObjectBackgroundName = "Background";
    public static readonly string gameObjectCameraName = "Camera";
    public static readonly string gameObjectDisplaySpaceName = "DisplaySpace";
    public static readonly string gameObjectHighlightContainerName = "HighlightContainer";
    public static readonly string gameObjectHighlightName = "Highlight";
    public static readonly string gameObjectHotBarObject = "HotBarObject";
    public static readonly string gameObjectHotBarSlotBase = "Slot";
    public static readonly string gameObjectIconName = "Icon";
    public static readonly string gameObjectInventoryItemTileName = "ItemTile";
    public static readonly string gameObjectInventoryScreenObjectName = "InventoryScreenObject";
    public static readonly string gameObjectItemName = "Item";
    public static readonly string gameObjectItemPromptObjectName = "ItemPromptObject";
    public static readonly string gameObjectNetObjectName = "net";
    public static readonly string gameObjectRopeAnchor = "RopeAnchor";
    public static readonly string gameObjectRopeCoilObjectName = "RopeCoilObject";
    public static readonly string gameObjectRopeObjectName = "RopeObject";
    public static readonly string gameObjectTextName = "Text";

    public static readonly string itemIdNet = "net";
    public static readonly List<string> itemIdThrowable = new List<string> {
        itemIdNet
    };

    public static readonly char splitCharUnderscore = '_';

    public static readonly string tagCanvas = "Canvas";
    public static readonly string tagHotBar = "HotBar";
    public static readonly string tagItem = "Item";
    public static readonly string tagItemReference = "ItemReference";
    public static readonly string tagLeftHandMount = "LeftHandMount";
    public static readonly string tagPlayer = "Player";
    public static readonly string tagRightHandMount = "RightHandMount";

    public static readonly string textPickUpBase = "Press E to pick up";
}
