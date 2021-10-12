using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor_ItemsList : MonoBehaviour
{
    public List<ItemForLevelEditor> itemsList = new List<ItemForLevelEditor>();

    public Dictionary<int, ItemForLevelEditor> GetItems()
    {
        Dictionary<int, ItemForLevelEditor> prefabsForGameItemsUsedInGameManager = new Dictionary<int, ItemForLevelEditor>();
        foreach (var item in itemsList)
        {
            prefabsForGameItemsUsedInGameManager.Add(
                item.itemGameObjectForPlacingOnMap.GetComponent<ItemIdsManager>().ItemsId.GetHashCode(),
                item);
        }
        return prefabsForGameItemsUsedInGameManager;
    }
}

[Serializable]
public class ItemForLevelEditor
{
    public string itemName;
    public GameObject itemGameObjectForPlacingOnMap;
    public Sprite iconForSelectorCanvas;
}

