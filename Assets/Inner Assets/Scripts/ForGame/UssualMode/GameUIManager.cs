using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UssualMode
{
    public class GameUIManager : MonoBehaviour
    {
        #region ПЕРЕМЕННЫЕ
        private GameManager GameManager;

        public GameObject dimmer;

        #region Переменные связанные со стастус баром уровня
        public RectTransform rectTransForLevelProgressBar;
        private float heightForLevelProgressBarIcon;
        private const float maxWidthForLevelProgressBarIcon = 432;
        // Размер увелечения програсс бара при получении одного очка
        private float onePointSizeForLevelProgressBar;

        public List<GameObject> contForStarsIconsPlacedOnLevelProgressBar;
        public List<GameObject> starsIconsPlacedOnLevelProgressBar;
        public Sprite spriteForContForStarsIconWhenCollected;
        public Sprite spriteForStarsIconWhenCollected;
        #endregion

        #region Переменные связанные с щитом
        public Button buttonUseShieldEffect;
        public Text textForShieldCounts;
        public GameObject contForShieldProgressBar;
        public RectTransform rectTransformForShieldProgressBar;
        private const float maxWidthForShieldProgressBarIcon = 430;
        private IEnumerator shieldCountdownCoroutine;
        #endregion

        #endregion

        #region ИНИЦИАЛИЗАЦИЯ
        public void Init()
        {
            GameManager = GameManager.Instance;
            CreateLevelProgressStatusBar();
            CreateShieldItems();
        }

        private void CreateLevelProgressStatusBar()
        {
            heightForLevelProgressBarIcon = rectTransForLevelProgressBar.sizeDelta.y;
            // размер увелечения прогресс бара при получении одного очка = макс длина / количесво очков 
            onePointSizeForLevelProgressBar = maxWidthForLevelProgressBarIcon / GameManager.gameData.pointsCountsForLevelComplete;

            var pointsCountsForGetEachStar = GameManager.gameData.pointsCountsForGetEachStar;
            // Расстановка иконок звездочек
            for (int i = 0; i < 3; i++)
            {
                // Позиция иконки звезды по X = размер увелечения прогресс бара при получении одного очка
                // * количество очков для получения звезды[номер звезды]
                float xPositionFor_starIcon_OnStatusBar = (onePointSizeForLevelProgressBar * pointsCountsForGetEachStar[i]) + 52.5f;
                contForStarsIconsPlacedOnLevelProgressBar[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(xPositionFor_starIcon_OnStatusBar, 0);
                // Если звезды уже была полученна этого то спрайт у контейнера другого цвета
                if (GameManager.gameData.countStarsAchivmentOnLastSessions >= i + 1)
                {
                    contForStarsIconsPlacedOnLevelProgressBar[i].GetComponent<Image>().sprite = spriteForContForStarsIconWhenCollected;
                    starsIconsPlacedOnLevelProgressBar[i].GetComponent<Image>().sprite = spriteForStarsIconWhenCollected;
                }
            }
        }

        private void CreateShieldItems()
        {
            textForShieldCounts.text = GameManager.gameData.shieldsCounts.ToString();
        }
        #endregion

        #region РАБОТА С ЩИТОМ
        private void ActivateShield(float shieldDuration)
        {
            int shieldCounts = GameManager.gameData.shieldsCounts;
            bool shieldIsActive = GameManager.gameData.shieldIsActive;
            if (shieldCounts > 0 && shieldIsActive == false)
            {
                GameManager.OnActivateShield();
                textForShieldCounts.text = GameManager.gameData.shieldsCounts.ToString();
                shieldCountdownCoroutine = ShieldCountdown(shieldDuration);
                StartCoroutine(shieldCountdownCoroutine);
                contForShieldProgressBar.GetComponent<Animator>().SetTrigger("show");
            }
        }

        private void DisableShieldlEffect()
        {
            if (GameManager.gameData.shieldIsActive)
            {
                GameManager.OnDeactivateShield();
                contForShieldProgressBar.GetComponent<Animator>().SetTrigger("hide");
                StopCoroutine(shieldCountdownCoroutine);
            }
        }

        private IEnumerator ShieldCountdown(float shieldDuration1)
        {
            Vector2 vector2ForShieldProgressBar = new Vector2()
            {
                x = maxWidthForShieldProgressBarIcon,
                y = rectTransformForShieldProgressBar.sizeDelta.y
            };
            float passedTime = 0;
            float shieldDuration = shieldDuration1;
            while (true)
            {
                passedTime += Time.deltaTime;
                // Получение соотношения времени которое прошло относительно длительности
                // Если прошло 5 секунл из 10 максимальных то это соотношение 0.5
                //float passedTimeInPercent = passedTime / shieldDuration;
                // Ивертированное значение для обратного отсчета
                float passedTimeInPercent = 1 - (passedTime / shieldDuration);
                if (passedTimeInPercent < 0)
                {
                    vector2ForShieldProgressBar.x = 0;
                    rectTransformForShieldProgressBar.sizeDelta = vector2ForShieldProgressBar;
                    DisableShieldlEffect();
                    yield break;
                }
                else
                {
                    // Маскимальная длина * cоотношение = какая должна бы длина на данном этапе
                    // 500 * 0.8 (соотношение оставшегося времени)  = 400 (длина прогресс бара)
                    vector2ForShieldProgressBar.x = maxWidthForShieldProgressBarIcon * passedTimeInPercent;
                    rectTransformForShieldProgressBar.sizeDelta = vector2ForShieldProgressBar;
                    yield return null;
                }
            }
        }


        #endregion

        #region РАБОТА С ПРОГРЕСС БАРОМ УРОВНЯ
        private IEnumerator IncreaseStarusBarCoroutine(int pointCount)
        {
            // Количество означающее сколько еще осталось добавить размера в статус бару
            float increaseSize = pointCount * onePointSizeForLevelProgressBar;
            float oneStepIncrease = 7.5f;
            Vector2 sizeFor_statusBar = new Vector2(0, heightForLevelProgressBarIcon);
            while (true)
            {
                sizeFor_statusBar.x = rectTransForLevelProgressBar.sizeDelta.x;

                // Если размер после увечелечения будет равен или больше необходимого 
                if (rectTransForLevelProgressBar.sizeDelta.x + oneStepIncrease >= rectTransForLevelProgressBar.sizeDelta.x + increaseSize)
                {
                    // Устанавливаю окончательный размер прогресс бара и завершаю корутину
                    sizeFor_statusBar.x = rectTransForLevelProgressBar.sizeDelta.x + increaseSize;
                    rectTransForLevelProgressBar.sizeDelta = sizeFor_statusBar;
                    yield break;
                } // Иначе происходит увелечение размера прогресс бара дальше
                else
                {
                    sizeFor_statusBar.x += oneStepIncrease;
                    increaseSize -= oneStepIncrease;
                    rectTransForLevelProgressBar.sizeDelta = sizeFor_statusBar;
                }

                yield return null;
            }
        }

        private void GiveLevelProgressStar(int lastAchimventStarNumber)
        {
            contForStarsIconsPlacedOnLevelProgressBar[lastAchimventStarNumber]
                .GetComponent<Animator>().SetTrigger("onCollectStar");
        }
        #endregion

        private IEnumerator SubLevelCompleteDimmering()
        {
            dimmer.GetComponent<Animator>().SetTrigger("show");
            yield return new WaitForSeconds(0.5f);
            dimmer.GetComponent<Animator>().SetTrigger("hide");
            yield break;
        }

        #region СОБЫТИЕ ИЗ ГМ
        public void OnClickUseShieldButton()
        {
            ActivateShield(GameManager.gameData.shieldDuration);
        }

        public void OnCompleteLevel()
        {
            DisableShieldlEffect();
        }

        public void OnCompleteSubLevel()
        {
            StartCoroutine(SubLevelCompleteDimmering());
            DisableShieldlEffect();
        }

        public void OnCollectCoin()
        {
            //todo
        }

        public void OnCollectPointItem(int pointsCounts)
        {
            StartCoroutine(IncreaseStarusBarCoroutine(pointsCounts));
        }

        public void OnCollectStar(int achivmentStarNumber)
        {
            GiveLevelProgressStar(achivmentStarNumber);
        }

        public void OnRevive()
        {
            ActivateShield(2.5f);
        }
        #endregion
    }
}