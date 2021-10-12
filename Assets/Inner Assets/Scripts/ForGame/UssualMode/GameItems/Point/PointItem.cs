using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UssualMode;

public class PointItem : MainItem
{
    private GameManager gameManager;
    private Animator animator;
    public int pointNumberVariant;
    public int pointsOnCollectingCounts;
    public PointItemSpecData specData;
    public Rigidbody2D rb;
    public MeshRenderer meshRenderer;
    public Material activeMaterial;
    public Material dangerousMaterial;

    public override void Init(Dictionary<string, string> specData = null)
    {
        gameObject.name = "point";
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
        gameManager.OnCreatePointItem(pointNumberVariant);

        gameManager.onShieldActivate += OnShieldActivate;
        gameManager.onShieldDeactivate += OnShielDeactivate;
        gameManager.onChangePointItemsType += OnChangePointItemsType;

        this.specData.SetSpecDataValues(specData);
        InitMovingByRange();

        // Если тип элемента это первый для выполнения то запусск анимации пульсации
        if (pointNumberVariant == 0)
        {
            animator.SetTrigger("pulse");
        }
        else
        {
            meshRenderer.material = dangerousMaterial;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            AudioManager.Instance.Play_Game_CollectPoint();
            MMVibrationManager.Haptic(HapticTypes.RigidImpact);
            Destroy(gameObject);
            gameManager.OnCollectPointItem(pointNumberVariant, pointsOnCollectingCounts);
        }
    }

    public void OnShieldActivate()
    {
        // При запуске щита запускаю анимацию для сигнализирования того что элемент можно собирать
        animator.SetTrigger("pulse");
        meshRenderer.material = activeMaterial;
    }

    public void OnShielDeactivate()
    {
        int subLevelNumber = gameManager.gameData.subLevelInProgressNumber;
        // Если тип этого элемента не тот который выполняеться сейчас то останавливаю пульсацию
        if (gameManager.gameData.subLevels[subLevelNumber].pointVariantInProgressNumber != pointNumberVariant)
        {
            animator.SetTrigger("stopPulse");
            meshRenderer.material = dangerousMaterial;
        }
    }

    private void OnChangePointItemsType(int completedTypeNumber, int nowTypeNumber)
    {
        // Если переключился на тип этого элемента
        if (nowTypeNumber == pointNumberVariant)
        {
            animator.SetTrigger("pulse");
            meshRenderer.material = activeMaterial;
        }
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

    private void OnDestroy()
    {
        gameManager.onShieldActivate -= OnShieldActivate;
        gameManager.onShieldDeactivate -= OnShielDeactivate;
        gameManager.onChangePointItemsType -= OnChangePointItemsType;

        moveByRangeTween.Kill();
    }
}