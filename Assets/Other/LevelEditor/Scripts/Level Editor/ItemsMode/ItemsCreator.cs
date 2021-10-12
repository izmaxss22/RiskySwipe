
using System.Collections.Generic;
using UnityEngine;

public class ItemsCreator : MonoBehaviour
{
    private Dictionary<int, ItemForLevelEditor> itemsAddedInEditor;

    private List<GameObject> createdItems = new List<GameObject>();
    public GameObject itemsCont;

    #region СОБЫТИЯ НА КОТОРЫЕ РЕАГИРУЕТ ЭТОТ КЛАСС
    //todoeditor deelete
    public void InitMode(OneLevelData levelData, Dictionary<int, ItemForLevelEditor> itemsList)
    {
        this.itemsAddedInEditor = itemsList;
        Delete_AllItems();
        Create_AllItems(levelData);
        itemsCont.SetActive(true);
    }

    public void OnModeEnable()
    {
    }

    public void OnModeDisable()
    {
    }

    public void OnClick_DeleteItem(GameObject GOForDelete)
    {
        DeleteItem(GOForDelete);
    }

    public void OnClick_PutItem(int id, Vector3 pos, Quaternion rot)
    {
        Create_Item(id, pos, rot);
    }

    public void OnClick_ReplaceItem(GameObject GOForDelete, int id, Vector3 pos, Quaternion rot)
    {
        DeleteItem(GOForDelete);
        Create_Item(id, pos, rot);
    }

    #endregion

    #region МЕТОДЫ ДЛЯ РАБОТЫ ЭТОГО КЛАССА
    // Удаление всех обьектов с карты
    private void Delete_AllItems()
    {
        foreach (var item in createdItems) Destroy(item);
        createdItems.Clear();
    }

    // Создание всех обьектов из списка обьектов
    private void Create_AllItems(OneLevelData levelData)
    {
        foreach (var item in levelData.items)
        {
            int itemId = item.itemId;
            Vector3 pos = item.GetPositon();
            Quaternion rot = item.GetRotation();
            Create_Item(itemId, pos, rot);
        }
    }

    // Создание указаного элемента на карте
    private void Create_Item(int itemId, Vector3 pos, Quaternion rot)
    {
        GameObject GO = Instantiate(
            itemsAddedInEditor[itemId].itemGameObjectForPlacingOnMap,
            pos,
            rot,
            itemsCont.transform);
        GO.name = itemId.ToString();
        GO.AddComponent<BoxCollider>();
        createdItems.Add(GO);
    }

    // Удаление указаного элемента с карты
    private void DeleteItem(GameObject GOForDelete)
    {
        int itemNumberInArray = createdItems.IndexOf(GOForDelete);
        Destroy(createdItems[itemNumberInArray]);
        createdItems.RemoveAt(itemNumberInArray);
    }

    #endregion

    #region ВНЕШНИЕ МЕТОДЫ
    public List<GameObject> GetCreatedItems()
    {
        return createdItems;
    }
    #endregion
}
