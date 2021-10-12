
// Отвечает за перекраску элементов точек на карте
using System.Collections.Generic;
using UnityEngine;

public class СoliderPointsPainter : MonoBehaviour
{

    #region СОБЫТИЯ НА КОТОРЫЕ РЕАГИРУЕТ  ЭТОТ КЛАСС

    public void OnItemPlacedOnMap(GameObject gameObject)
    {
        Repaint_PointItem(gameObject, Color.green);
    }

    public void OnClick_PickColliderButton(
        int numberCollider_ForUnselecting,
        int numberCollider_ForSelecting,
        List<List<GameObject>> collidersDatas)

    {
        // Перекрашивание всех точех прошлого колайдера в белый
        if (numberCollider_ForUnselecting != -1) Repaint_PointItemsOnMap_AtColider(collidersDatas[numberCollider_ForUnselecting], Color.white);
        // Все точки выбраного колайедра перекрашиваються в зеленный цвет
        Repaint_PointItemsOnMap_AtColider(collidersDatas[numberCollider_ForSelecting], Color.green);

    }

    #endregion

    // Перекрасить все точки у указаного колайдера в указанный цвет
    private void Repaint_PointItemsOnMap_AtColider(
       List<GameObject> colliderPoints,
        Color color)

    {
        foreach (var pointGameObjects in colliderPoints)
        {
            pointGameObjects.GetComponent<MeshRenderer>().material.color = color;
        }
    }

    private void Repaint_PointItem(GameObject pointGameObjects, Color color)
    {
        pointGameObjects.GetComponent<MeshRenderer>().material.color = color;

    }
    //// Перекрассить все точки в указанный цвет
    //private void Repaint_ColiderPlacingDots_All(Color color)
    //{
    //    foreach (var dot in array_coliderPlacingDots)
    //    {
    //        dot.Value.GetComponent<MeshRenderer>().material.color = color;
    //    }
    //}

}
