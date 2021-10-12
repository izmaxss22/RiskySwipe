using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

// Дефолтный обьект для всех скриптов со спец данными
public class MainSpecData : MonoBehaviour
{
    // Использование заранее подготовленого варианта спец данных
    //public PlayerCanMoveItemData playerCanMoveItemData;
    //public int customVarible;

    public virtual Dictionary<string, string> GetSpecDataValues()
    {
        var dict = new Dictionary<string, string>();

        //dict.Add(nameof(customVarible), customVarible.ToString());
        //playerCanMoveItemData.GetData(dict);

        return dict;
    }

    public virtual void SetSpecDataValues(Dictionary<string, string> specData)
    {
        //neededPassCounts = int.Parse(specData[nameof(neededPassCounts)]);
        //playerCanMoveItemData.SetData(specData);
        return;
    }

    #region PLAYER CAN MOVE ITEM
    [Serializable]
    public class PlayerCanMoveItemData
    {
        public bool isPlayerCanMoveItem = false;
        public void GetData(ref Dictionary<string, string> dict)
        {
            dict.Add(nameof(isPlayerCanMoveItem), isPlayerCanMoveItem.ToString());
        }

        public void SetData(Dictionary<string, string> specData)
        {
            isPlayerCanMoveItem = bool.Parse(specData[nameof(isPlayerCanMoveItem)]);
        }
    }
    #endregion

    #region DEACTIVATE ON REPEATED COLLISION
    [Serializable]
    public class DeactivateAffterRepeatCollisionSpecData
    {
        [Header("*")]
        [Space(-5)]
        [Tooltip("Деактивируються ли элемент после повторного касания когда он уже был активирован")]
        public bool deactivateOnRepeatCollision = false;
        public void GetData(ref Dictionary<string, string> dict)
        {
            dict.Add(nameof(deactivateOnRepeatCollision), deactivateOnRepeatCollision.ToString());
        }

        public void SetData(Dictionary<string, string> specData)
        {
            deactivateOnRepeatCollision = bool.Parse(specData[nameof(deactivateOnRepeatCollision)]);
        }
    }
    #endregion

    #region DANGER TIMERS
    [Serializable]
    public class DangerTimersSpecData
    {
        public bool itemIsHaveDangerTimers = false;
        [Space(-5)]
        [Header("*")]
        [Tooltip("Дефолтные таймеры которые используються если массивы пустые. " +
            "Массивы ниже отвечают за определение как долго айтем безопасен и наоборот")]
        public float defaultNotDangerTimer = 2;
        public float defaultDangerTimer = 1;
        public List<float> notDangerItemTimers = new List<float>();
        public List<float> dangerItemTimers = new List<float>();

        public void GetData(ref Dictionary<string, string> dict)
        {
            dict.Add(nameof(itemIsHaveDangerTimers), itemIsHaveDangerTimers.ToString());
            dict.Add(nameof(defaultNotDangerTimer), defaultNotDangerTimer.ToString());
            dict.Add(nameof(defaultDangerTimer), defaultDangerTimer.ToString());
            dict.Add(nameof(notDangerItemTimers), ValueParsers.ListToString(notDangerItemTimers));
            dict.Add(nameof(dangerItemTimers), ValueParsers.ListToString(dangerItemTimers));
        }

        public void SetData(Dictionary<string, string> specData)
        {
            itemIsHaveDangerTimers = bool.Parse(specData[nameof(itemIsHaveDangerTimers)]);

            defaultNotDangerTimer = float.Parse(specData[nameof(defaultNotDangerTimer)], CultureInfo.InvariantCulture);
            defaultDangerTimer = float.Parse(specData[nameof(defaultDangerTimer)], CultureInfo.InvariantCulture);

            notDangerItemTimers.Clear();
            if (specData[nameof(notDangerItemTimers)].Length != 0)
                notDangerItemTimers = ValueParsers.StringToListFloats(specData[nameof(notDangerItemTimers)]);

            dangerItemTimers.Clear();
            if (specData[nameof(dangerItemTimers)].Length != 0)
                dangerItemTimers = ValueParsers.StringToListFloats(specData[nameof(dangerItemTimers)]);
        }
    }
    #endregion

    #region ITEM ROTATING DATA
    [Serializable]
    public class RotatinSpecData
    {
        public bool itemIsRottating = false;
        public float rottatingSpeed = 1;
        public enum RotatingVectors
        {
            left = 0,
            righ = 1
        }
        public RotatingVectors rotatingVector = RotatingVectors.left;

        public void GetData(ref Dictionary<string, string> dict)
        {
            dict.Add(nameof(itemIsRottating), itemIsRottating.ToString());
            dict.Add(nameof(rottatingSpeed), rottatingSpeed.ToString());
            dict.Add(nameof(rotatingVector), rotatingVector.ToString());
        }

        public void SetData(Dictionary<string, string> specData)
        {
            itemIsRottating = bool.Parse(specData[nameof(itemIsRottating)]);
            rottatingSpeed = float.Parse(specData[nameof(rottatingSpeed)], CultureInfo.InvariantCulture);
            rotatingVector = specData[nameof(rotatingVector)] == "left" ?
                rotatingVector = RotatingVectors.left :
                rotatingVector = RotatingVectors.righ;

        }
    }
    #endregion

    #region MOVING BY POINTS
    [Serializable]
    public class MovingByPointsSpecData
    {
        [HideInInspector]
        public bool itemIsMovedByPoints;
        public List<Vector3> movePoints = new List<Vector3>();
        public bool moveIsLooped = false;
        public float sideMovingSpeed = 0.05f;

        public void GetData(ref Dictionary<string, string> dict)
        {
            itemIsMovedByPoints = movePoints.Count > 0;
            dict.Add(nameof(itemIsMovedByPoints), itemIsMovedByPoints.ToString());
            dict.Add(nameof(movePoints), ValueParsers.ListToString(movePoints));
            dict.Add(nameof(moveIsLooped), moveIsLooped.ToString());
            dict.Add(nameof(sideMovingSpeed), sideMovingSpeed.ToString());
        }

        public void SetData(Dictionary<string, string> specData)
        {
            itemIsMovedByPoints = bool.Parse(specData[nameof(itemIsMovedByPoints)]);
            movePoints.Clear();
            if (specData[nameof(movePoints)].Length != 0)
                movePoints = ValueParsers.StringWithVectors3ToListVectors3(specData[nameof(movePoints)]);
            moveIsLooped = bool.Parse(specData[nameof(moveIsLooped)]);
            sideMovingSpeed = float.Parse(specData[nameof(sideMovingSpeed)], CultureInfo.InvariantCulture);
        }
    }

    #endregion

    #region MOVING BY RANGE
    [Serializable]
    public class MovingByRangeSpecData
    {
        [HideInInspector]
        public bool itemIsMovedByRange;
        public List<RangeMovingPoint> movingPoints = new List<RangeMovingPoint>();

        [HideInInspector]
        public List<Vector3> movingPointsForSave;
        [HideInInspector]
        public List<int> easyingTypes;
        [HideInInspector]
        public List<float> pointsPauses;
        [HideInInspector]
        public List<float> pointsMoveInSecondsValues;


        public void GetData(ref Dictionary<string, string> dict)
        {
            itemIsMovedByRange = movingPoints.Count > 0;
            dict.Add(nameof(itemIsMovedByRange), itemIsMovedByRange.ToString());

            movingPointsForSave.Clear();
            easyingTypes.Clear();
            pointsPauses.Clear();
            pointsMoveInSecondsValues.Clear();

            foreach (var item in movingPoints)
            {
                movingPointsForSave.Add(item.rangeMovingPoint);
                easyingTypes.Add(item.easyngType.GetHashCode());
                pointsPauses.Add(item.pauseLenghtWhenReachPoint);
                pointsMoveInSecondsValues.Add(item.movingInSecondsFromLastPoint);
            }

            dict.Add(nameof(movingPointsForSave), ValueParsers.ListToString(movingPointsForSave));
            dict.Add(nameof(easyingTypes), ValueParsers.ListToString(easyingTypes));
            dict.Add(nameof(pointsPauses), ValueParsers.ListToString(pointsPauses));
            dict.Add(nameof(pointsMoveInSecondsValues), ValueParsers.ListToString(pointsMoveInSecondsValues));
        }

        public void SetData(Dictionary<string, string> specData)
        {
            itemIsMovedByRange = bool.Parse(specData[nameof(itemIsMovedByRange)]);

            if (itemIsMovedByRange)
            {
                movingPointsForSave.Clear();
                easyingTypes.Clear();
                pointsPauses.Clear();
                pointsMoveInSecondsValues.Clear();

                movingPointsForSave = ValueParsers.StringWithVectors3ToListVectors3(specData[nameof(movingPointsForSave)]);
                easyingTypes = ValueParsers.StringToListInt(specData[nameof(easyingTypes)]).ToList();
                pointsPauses = ValueParsers.StringToListFloats(specData[nameof(pointsPauses)]);
                pointsMoveInSecondsValues = ValueParsers.StringToListFloats(specData[nameof(pointsMoveInSecondsValues)]);
            }

            movingPoints.Clear();
            for (int i = 0; i < movingPointsForSave.Count; i++)
            {
                movingPoints.Add(new RangeMovingPoint(
                    movingPointsForSave[i],
                    easyingTypes[i],
                    pointsPauses[i],
                    pointsMoveInSecondsValues[i]
                    ));
            }
        }

        [Serializable]
        public class RangeMovingPoint
        {
            public Ease easyngType;
            [Tooltip("На сколько айтем отклоняеться от прошлой точки")]
            public Vector3 rangeMovingPoint;
            [Tooltip("При достижении этой точки сколько айтем стоит на месте")]
            public float pauseLenghtWhenReachPoint = 0.2f;
            [Tooltip("Сколько времени он движеться от прошлой точки до этой")]
            public float movingInSecondsFromLastPoint = 1;

            public RangeMovingPoint(
                  Vector3 rangeMovingPoint,
                  int easyngType,
                  float pauseLenghtWhenReachPoint,
                  float movingInSecondsFromLastPoint
                )
            {
                this.rangeMovingPoint = rangeMovingPoint;
                this.easyngType = (Ease)easyngType;
                this.pauseLenghtWhenReachPoint = pauseLenghtWhenReachPoint;
                this.movingInSecondsFromLastPoint = movingInSecondsFromLastPoint;
            }
        }

    }

    #endregion

    #region SPLASH MOVING POINTS
    [Serializable]
    public class SplashMovingPointsSpecData
    {
        [HideInInspector]
        public bool itemIsMovedBySplashPoints;
        public List<Vector3> splashMovePoints = new List<Vector3>();
        public bool moveIsRandom = false;
        [Space(-5)]
        [Header("*")]
        [Tooltip("Если у массива таймеров размер = 0, то используеться этот таймер. Первый элемент в массиве таймеров это таймер на первой позиции")]
        public float defaultSplashPointsTimer = 2f;
        public List<float> changeSplashPointsTimers = new List<float>();

        public void GetData(ref Dictionary<string, string> dict)
        {
            itemIsMovedBySplashPoints = splashMovePoints.Count > 0;
            dict.Add(nameof(itemIsMovedBySplashPoints), itemIsMovedBySplashPoints.ToString());
            dict.Add(nameof(splashMovePoints), ValueParsers.ListToString(splashMovePoints));
            dict.Add(nameof(moveIsRandom), moveIsRandom.ToString());
            dict.Add(nameof(defaultSplashPointsTimer), defaultSplashPointsTimer.ToString());
            dict.Add(nameof(changeSplashPointsTimers), ValueParsers.ListToString(changeSplashPointsTimers));
        }

        public void SetData(Dictionary<string, string> specData)
        {
            itemIsMovedBySplashPoints = bool.Parse(specData[nameof(itemIsMovedBySplashPoints)]);

            splashMovePoints.Clear();
            if (specData[nameof(splashMovePoints)].Length != 0)
                splashMovePoints = ValueParsers.StringWithVectors3ToListVectors3(specData[nameof(splashMovePoints)]);

            moveIsRandom = bool.Parse(specData[nameof(moveIsRandom)]);

            defaultSplashPointsTimer = float.Parse(specData[nameof(defaultSplashPointsTimer)], CultureInfo.InvariantCulture);


            changeSplashPointsTimers.Clear();
            if (specData[nameof(changeSplashPointsTimers)].Length != 0)
                changeSplashPointsTimers = ValueParsers.StringToListFloats(specData[nameof(changeSplashPointsTimers)]);
        }
    }
    #endregion
}
