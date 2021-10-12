
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UssualMode;

public class DangerObstacle : MainItem
{
    private GameManager gameManager;
    public Rigidbody2D rb;
    public DangerObstacleSpecData specData;

    public override void Init(Dictionary<string, string> specData = null)
    {
        gameManager = GameManager.Instance;
        this.specData.SetSpecDataValues(specData);
        InitMovingByRange();
        return;
    }

    #region MOVING BY RANGE METHODS
    private void InitMovingByRange()
    {
        if (specData.movingByRangeSpecData.itemIsMovedByRange)
        {
            StartCoroutine(MoveItemByRange());
        }
    }

    private Tween moveByRangeTween;
    private IEnumerator MoveItemByRange()
    {
        var savedPoints = specData.movingByRangeSpecData.movingPoints;

        List<Vector3> endRangeMovePoints = new List<Vector3>();
        endRangeMovePoints.Add(transform.position + savedPoints[0].rangeMovingPoint);

        List<Ease> easingTypes = new List<Ease>();
        easingTypes.Add(savedPoints[0].easyngType);

        List<float> pausesForEachPoints = new List<float>();
        pausesForEachPoints.Add(savedPoints[0].pauseLenghtWhenReachPoint);

        List<float> secondsInMove = new List<float>();
        secondsInMove.Add(savedPoints[0].movingInSecondsFromLastPoint);
        savedPoints.RemoveAt(0);

        int currentPointNumber = 0;

        foreach (var item in savedPoints)
        {
            endRangeMovePoints.Add(endRangeMovePoints.Last() + item.rangeMovingPoint);
            easingTypes.Add(item.easyngType);
            pausesForEachPoints.Add(item.pauseLenghtWhenReachPoint);
            secondsInMove.Add(item.movingInSecondsFromLastPoint);
        }

        while (true)
        {
            if (currentPointNumber == endRangeMovePoints.Count)
            {
                currentPointNumber = 0;
            }

            moveByRangeTween = rb.DOMove(endRangeMovePoints[currentPointNumber], secondsInMove[currentPointNumber])
               .SetEase(easingTypes[currentPointNumber]);
            yield return moveByRangeTween.WaitForCompletion();
            yield return new WaitForSeconds(pausesForEachPoints[currentPointNumber]);
            currentPointNumber++;
        }
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            gameManager.OnCollectDangerObstacle();
        }
    }

    private void OnDestroy()
    {
        moveByRangeTween.Kill();
    }

}
