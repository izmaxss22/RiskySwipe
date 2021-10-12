using System.Collections.Generic;
using UnityEngine;

//TODO добавить требования айди мененджера
public class MainItem : MonoBehaviour
{
    //public SpecDataScriptType specData;

    public virtual void Init(Dictionary<string, string> specData = null)
    {
        //this.specData.SetSpecDataValues(specData);

        return;
    }
}

#region ITEM METHODS
#region PLAYER CAN MOVE ITEM METHODS
//private void InitPlayerCanMoveItem()
//{
//    if (ringSpecData.isPlayerCanMoveItem)
//        rb.isKinematic = false;
//}
#endregion

#region ROTATING ITEM METHODS
//private void InitRotating()
//{
//    // Надо ли вращать элемент
//    if (ringSpecData.itemIsRottating)
//    {
//        StartCoroutine(RotatingItem(
//                ringSpecData.rottatingSpeed,
//                ringSpecData.rotatingVector));
//    }
//}


//// Корутина которая плавно вращает обьект
//private IEnumerator RotatingItem(float rotationSpeed, RingSpecData.RotatingVectors rotatingVector)
//{
//    Quaternion vectorRot =
//        rotatingVector == RingSpecData.RotatingVectors.left ?
//        Quaternion.Euler(new Vector3(0, -1, 0) * rotationSpeed) :
//        Quaternion.Euler(new Vector3(0, 1, 0) * rotationSpeed);

//    while (true)
//    {
//        rb.MoveRotation(rb.rotation * vectorRot);
//        yield return new WaitForFixedUpdate();
//    }
//}
#endregion

#region MOVING BY POINTS ITEM METHODS
//private void InitMovingItem()
//{
//    // Нужно ли двигать элемент по точкам
//    if (ringSpecData.itemIsMovedByPoints)
//    {
//        StartCoroutine(MovingItemFromPointToPoint
//            (
//            ringSpecData.sideMovingSpeed,
//            ringSpecData.moveIsLooped,
//            ringSpecData.movePoints
//            ));
//    }
//}

//// Перемещает элемент от одной точке к другой
//private IEnumerator MovingItemFromPointToPoint(
//    float movingSpeed,
//    bool isLooped,
//    //int moveVector,
//    List<Vector3> movePoints)
//{
//    Vector3 basePos = transform.position;
//    movePoints.Insert(0, basePos);
//    Vector3 direction = (basePos - movePoints[1]).normalized;
//    float distance = Vector3.Distance(basePos, movePoints[1]);
//    int currentPointNumber = 1;
//    int countPoints = movePoints.Count;
//    int moveVector = 1;

//    while (true)
//    {
//        rb.MovePosition(rb.position + (-direction * movingSpeed));
//        // Если достиг точки
//        if (Vector3.Distance(basePos, rb.position) >= distance)
//        {
//            // Если движение должно быть зацикленно
//            if (isLooped)
//            {
//                // Если достиг последней точки
//                if (currentPointNumber == countPoints - 1)
//                {
//                    // То меняю движение к нулевой точке
//                    currentPointNumber = 0;
//                    var nextPos = movePoints[0];
//                    CalculateNexPosData(out basePos, out direction, out distance, nextPos);
//                } // Иначе продолжаю двигаться к след точки
//                else
//                {
//                    currentPointNumber++;
//                    var nextPos = movePoints[currentPointNumber];
//                    CalculateNexPosData(out basePos, out direction, out distance, nextPos);
//                }
//            } // Если движение туда-обратно
//            else
//            {
//                // Если достиг последней точки
//                if (currentPointNumber == countPoints - 1)
//                {
//                    // То меняю движение на обратное
//                    moveVector = -1;
//                    currentPointNumber += moveVector;
//                    var nextPos = movePoints[currentPointNumber];
//                    CalculateNexPosData(out basePos, out direction, out distance, nextPos);
//                }
//                // Если достиг первой точки
//                else if (currentPointNumber == 0)
//                {
//                    // То меняю движение на обратное
//                    moveVector = 1;
//                    currentPointNumber += moveVector;
//                    var nextPos = movePoints[currentPointNumber];
//                    CalculateNexPosData(out basePos, out direction, out distance, nextPos);
//                }
//                // Иначе продолжаю двигаться к след точки
//                else
//                {
//                    currentPointNumber += moveVector;
//                    var nextPos = movePoints[currentPointNumber];
//                    CalculateNexPosData(out basePos, out direction, out distance, nextPos);
//                }
//            }
//        }
//        yield return new WaitForFixedUpdate();
//    }
//}

//// Подсчет следующей позиции передвижения
//private void CalculateNexPosData(
//    out Vector3 basePos,
//    out Vector3 direction,
//    out float distance,
//    Vector3 nextPos)
//{
//    basePos = transform.position;
//    direction = (basePos - nextPos).normalized;
//    distance = Vector3.Distance(basePos, nextPos);
//}
#endregion

#region SPLASH MOVING POINTS METHODS
//private void InitSplashMovingItemData()
//{
//    // Нужно ли телепортировать элемент по точкам
//    if (specData.itemIsMovedBySplashPoints)
//    {
//        StartCoroutine(MovingItemFromSplashPoints
//            (
//            specData.defaultSplashPointsTimer,
//            specData.changeSplashPointsTimers,
//            specData.moveIsRandom,
//            specData.splashMovePoints
//            ));
//    }
//}

//// Перемещает элемент от одной точке к другой
//private IEnumerator MovingItemFromSplashPoints(
//    float defaultSplashPointsTimer,
//    List<float> changeSplashPointsTimer,
//    bool moveIsRandom,
//    List<Vector3> movePoints)
//{
//    Vector3 basePos = transform.position;
//    movePoints.Insert(0, basePos);

//    int currentPointNumber = 0;
//    int countPoints = movePoints.Count;

//    if (moveIsRandom)
//        ValueParsers.ShuffleList(movePoints);

//    while (true)
//    {
//        if (changeSplashPointsTimer.Count == 0)
//        {
//            yield return new WaitForSeconds(defaultSplashPointsTimer);
//        }
//        else
//        {
//            yield return new WaitForSeconds(changeSplashPointsTimer[currentPointNumber]);
//        }
//        // Рандомная сменна точек
//        if (moveIsRandom)
//        {
//            // Если точка не последнняя
//            if (currentPointNumber != countPoints - 1)
//            {
//                currentPointNumber++;
//                rb.transform.position = movePoints[currentPointNumber];
//            }
//            // Если последнняя
//            else
//            {
//                currentPointNumber = 0;
//                ValueParsers.ShuffleList(movePoints);
//                // Если новая точка в перемешанном массиве совпадает с тем что сейчас
//                if (movePoints[0] == rb.transform.position)
//                {
//                    currentPointNumber++;
//                }
//                rb.transform.position = movePoints[currentPointNumber];
//            }
//        }
//        // Смена точек по положению в массиве
//        else
//        {
//            // Если не достиг последней точки
//            if (currentPointNumber != countPoints - 1)
//                currentPointNumber++;
//            // Если на последней точке
//            else
//                currentPointNumber = 0;
//            rb.transform.position = movePoints[currentPointNumber];
//        }
//    }
//}
#endregion

#endregion