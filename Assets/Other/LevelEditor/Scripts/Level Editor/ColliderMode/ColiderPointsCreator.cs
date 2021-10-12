using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// Отвечает за создание и удаление точек с карты
// и создание из этих данных массива с точками колайдеров
public class ColiderPointsCreator : MonoBehaviour
{
    public MediatorFor_ColiderMode MediatorFor_ColiderMode;

    public GameObject contFor_creatingPointItems;
    public GameObject prefab_coliderPointItem;

    // Массив точек, ключ доступа это позиция точки по x + "|" + y
    private List<List<GameObject>> collidersDatas = new List<List<GameObject>>();

    public LineRenderer lineRendererFor_linedBetwenColliderPoints;


    #region СОБЫТИЯ НА КОТОРЫЕ РЕАГИРУЕТ ЭТОТ КЛАСС
    public void OnModeStateChanged(bool enable)
    {
        contFor_creatingPointItems.SetActive(enable);
        if (!enable) Clear_LineRendererFor_BetwenDotsLines();

    }

    // При смене уровня
    public void OnLevelChanged(List<OneLevelData.ColliderPoints> createdColiderDatas)
    {
        // Удаление старрых точек
        Delete_CollidersPointsItems();
        // Создание новых потомучто изменлся уровень, изменились и точки которые надо отобразить
        Create_ColidersPointsItems(createdColiderDatas);
        Clear_LineRendererFor_BetwenDotsLines();
    }

    // При добавлении колайдера 
    public void OnClick_AddColliderButton()
    {
        Create_Collider();
    }

    // При удалении колайдера
    public void OnClick_DeleteColiderButton(int colliderForDeletingNumber)
    {
        Delete_Collider(colliderForDeletingNumber);
    }

    public void OnClick_PickColiderButton(int colliderNumber)
    {
        Create_LineRendererFor_BetwenDotLines(colliderNumber);
    }

    public GameObject OnItemPlacedOnMap(Vector3 itemPosition, int coliderNumber)
    {
        GameObject createdPoint = Create_ColiderPointItem(itemPosition, coliderNumber);
        Create_LineRendererFor_BetwenDotLines(coliderNumber);
        return createdPoint;
    }

    public void OnItemDeletedFromMap(GameObject itemGameObject, int coliderNumber)
    {
        Delete_ColiderPointItem(itemGameObject, coliderNumber);
        Create_LineRendererFor_BetwenDotLines(coliderNumber);
    }
    #endregion

    #region  ВНУТРЕНИЕ МЕТОДЫ
    // Создание нового пустого (без точек) коллайдера
    private void Create_Collider()
    {
        collidersDatas.Add(new List<GameObject>());
    }

    private void Delete_Collider(int colliderForDeletingNumber)
    {
        // Удаление всех точек колайдера с карты
        foreach (var pointGameObject in collidersDatas[colliderForDeletingNumber])
        {
            Destroy(pointGameObject);
        }
        collidersDatas.RemoveAt(colliderForDeletingNumber);
    }


    #region МЕТОДЫ СОЗДАНИЯ ТОЧЕК  КОЛАЙДЕРА
    // Создание всех точек у указаного обьекта (обьекта содержащего список всех колайдеров для под уровня)
    private void Create_ColidersPointsItems(List<OneLevelData.ColliderPoints> createdColiderDatas)
    {
        int coliderNumber = 0;
        // Перебор всех 
        foreach (var coliderItem in createdColiderDatas)
        {
            collidersDatas.Add(new List<GameObject>());
            foreach (var pointPos in coliderItem.GetPoints())
            {
                Create_ColiderPointItem(pointPos, coliderNumber);
            }
            coliderNumber++;
        }
    }

    // Создание точки у указаного колайдера  
    private GameObject Create_ColiderPointItem(Vector3 position, int coliderNumber)
    {
        // Если массив пустой значит в редакторе нету ни одного колайдера
        // Значит добавлять айтем не куда
        if (coliderNumber != -1)
        {
            position.z = -3.5f;
            GameObject pointItemGO = Instantiate(prefab_coliderPointItem, position, Quaternion.identity, contFor_creatingPointItems.transform);
            collidersDatas[coliderNumber].Add(pointItemGO);
            return pointItemGO;
        }
        return null;
    }
    #endregion

    #region МЕТОДЫ УДАЛЕНИЯ ТОЧЕК КОЛАЙДЕРА
    // Удаление всех точек у всех созданных колайдеров
    private void Delete_CollidersPointsItems()
    {
        foreach (var coliderData in collidersDatas)
        {
            foreach (var pointGameObject in coliderData)
            {
                Destroy(pointGameObject);
            }
        }
        collidersDatas.Clear();
    }

    // Удаление точки у указаного колайдера 
    private void Delete_ColiderPointItem(GameObject itemForDelete, int coliderNumber)
    {
        if (collidersDatas.Count > 0)
        {
            int itemNumberInArray = collidersDatas[coliderNumber].IndexOf(itemForDelete);
            Destroy(collidersDatas[coliderNumber][itemNumberInArray]);
            collidersDatas[coliderNumber].RemoveAt(itemNumberInArray);
        }
    }
    #endregion

    private void Create_LineRendererFor_BetwenDotLines(int pickedColiderNumber)
    {
        if (pickedColiderNumber != -1)
        {
            List<Vector3> points = new List<Vector3>();
            var coliderPoints = collidersDatas[pickedColiderNumber];

            foreach (var point in coliderPoints)
            {
                points.Add(point.transform.position);
            }

            lineRendererFor_linedBetwenColliderPoints.positionCount = points.Count;
            lineRendererFor_linedBetwenColliderPoints.SetPositions(points.ToArray());
        }
    }

    private void Clear_LineRendererFor_BetwenDotsLines()
    {
        lineRendererFor_linedBetwenColliderPoints.positionCount = 0;
    }

    public List<List<GameObject>> Get_ColliderDatas()
    {
        return collidersDatas;
    }

    #endregion
}
