using System;
using UnityEditor;
using UnityEngine;

namespace UssualMode
{
    public class GameProgressManager : MonoBehaviour
    {
        private GameManager gameManager;
        public void Init()
        {
            gameManager = GameManager.Instance;
        }

        // Метод вызываемые после сбора любого айтема для выполнения
        // проверяющий уровень на смену состояния исходя из количества собранных очков
        private void CheckLevelStatesAffterCollectCompletngItem(int achivmentsPointsCounts)
        {
            var gameData = gameManager.gameData;
            int subLevelInProgress = gameData.subLevelInProgressNumber;

            int pointsCountsForLevelComplete = gameData.pointsCountsForLevelComplete -= achivmentsPointsCounts;
            int pointsCountsForSubLevelComplete = gameData.subLevels[subLevelInProgress].pointsCountsForLevelComplete -= achivmentsPointsCounts;
            int pointsCountsForGetCurrentStar = gameData.pointsCountsForGetEachStarForProgressCalculating[gameData.countStarsAchivmentOnThisSession] -= achivmentsPointsCounts;
            // Если получил звезду
            if (pointsCountsForGetCurrentStar == 0)
            {
                int currentStarNumber = gameData.countStarsAchivmentOnThisSession;
                gameData.countStarsAchivmentOnThisSession++;
                gameManager.OnCompleteStar(currentStarNumber);
            }
            // Если прошел уровень
            if (pointsCountsForLevelComplete == 0)
            {
                if (gameData.gameIsOvered == false)
                {
                    gameData.gameIsOvered = true;
                    SaveLevelData();
                    gameManager.OnCompleteLevel();
                }
            }
            // Если прошел подуровень
            else if (pointsCountsForSubLevelComplete == 0)
            {
                if (gameData.gameIsOvered == false)
                {
                    gameData.gameIsOvered = true;
                    gameData.subLevelInProgressNumber++;
                    gameManager.OnCompleteSubLevel();
                }
            }
        }

        private void MakeGameOver()
        {
            if (gameManager.gameData.gameIsOvered == false)
            {
                SaveLevelData();
                bool isNeedShowReviveScreen = IsNeedShowReviveScreen();
                gameManager.gameData.gameIsOvered = true;
                gameManager.OnGameOver(isNeedShowReviveScreen);
            }
        }

        private void SaveLevelData()
        {
            var gameData = gameManager.gameData;
            
            DataManager.Instance.SetCoinsCount(DataManager.Instance.GetCoinsCount() + gameData.coinsCounts);
            
            // Если количетво звезд полученых в этой сессии больше чем было до этого
            if (gameData.countStarsAchivmentOnThisSession > gameData.countStarsAchivmentOnLastSessions)
            {
                // Сохранение нового количества звезд у уровня
                var starsCountOnAllLevel = DataManager.Instance.GetStarsCountsAchivmentOnLevels();
                starsCountOnAllLevel[gameData.levelNumber] = gameData.countStarsAchivmentOnThisSession;
                DataManager.Instance.SetStarsCountsAchivmentOnLevels(starsCountOnAllLevel);
            }

            // Если прошел уровень как мининму на 1 звезду то становиться доступен следующий уровень
            if (gameData.countStarsAchivmentOnThisSession >= 1)
            {
                // Если это последний доступный уровень и он не равен максимальному количеству уровней (дальше еще есть уровни)
                if (DataManager.Instance.GetLastAvailableLevelNumber() == gameData.levelNumber &&
                    gameData.levelNumber + 1 < DataManager.COUNT_LEVELS)
                {
                    // Включение следующего доступного уровня
                    DataManager.Instance.SetLastAvailableLevelNumber
                        (gameData.levelNumber + 1);
                }
            }
        }

        private bool IsNeedShowReviveScreen()
        {
            int totalPointsCounts = gameManager.gameData.pointsCountsForLevelCompleteDefaultConst;
            int achivmentPointsCounts = totalPointsCounts - gameManager.gameData.pointsCountsForLevelComplete;
            float oneProcentLevelCompleeting = totalPointsCounts / 100;
            float nowProcents = achivmentPointsCounts / oneProcentLevelCompleeting;
            // Если прошел уровень на 25 процентов то нужно вызвать окно возрождения
            return nowProcents >= 25;
        }

        private void SwitchPointItemToNextType()
        {
            var gameData = gameManager.gameData;
            int subLevelInProgress = gameData.subLevelInProgressNumber;
            int completedTypeNumber = gameData.subLevels[subLevelInProgress].pointVariantInProgressNumber;
            int lastTypeNumber = gameData.subLevels[subLevelInProgress].countsForEachPointItemsVariant.Count - 1;
            // Если текущий номер был не последний (дальше еще есть типы для прохождения)
            if (completedTypeNumber != lastTypeNumber)
            {
                // Переход к следущему доступному типу
                int nextTypeNumber = ++gameData.subLevels[subLevelInProgress].pointVariantInProgressNumber;
                // Поиск следующего доступного для прохождения типа элементов
                while (true)
                {
                    // Если текущий номер не последний
                    if (nextTypeNumber != lastTypeNumber + 1)
                    {
                        int lastCountsForOnNextType = gameData.subLevels[subLevelInProgress].countsForEachPointItemsVariant[nextTypeNumber];
                        // Если у текущего номера еще остались элементы для выполнения
                        if (lastCountsForOnNextType != 0)
                        {
                            // То делаю его следующим типом
                            gameManager.OnChangePointItemsType(completedTypeNumber, nextTypeNumber);
                            break;
                        } // Иначе перехожу к следующему 
                        else
                        {
                            nextTypeNumber = ++gameData.subLevels[subLevelInProgress].pointVariantInProgressNumber;
                        }
                    } // Если последний то завершаю поиск
                    else
                    {
                        break;
                    }
                }
            }
        }

        #region СОБЫТИЯ ГМ
        public void OnActivateShield()
        {
            int shieldCounts = --gameManager.gameData.shieldsCounts;
            DataManager.Instance.SetShieldEffectsCount(shieldCounts);
            gameManager.gameData.shieldIsActive = true;
        }

        public void OnDeactivateShield()
        {
            gameManager.gameData.shieldIsActive = false;
        }

        public bool OnCollectPointItem(int pointItemNumber, int achivmentsPointsCounts)
        {
            bool itemIsCollected = false;
            var gameData = gameManager.gameData;
            int subLevelInProgress = gameData.subLevelInProgressNumber;
            int pointTypeInProgressNumber = gameData.subLevels[subLevelInProgress].pointVariantInProgressNumber;
            // Если собрал верный элемент либо активен щит
            if (pointTypeInProgressNumber == pointItemNumber || gameData.shieldIsActive)
            {
                // Отнимаю количество оставшихся элементов очков этого типа
                int remainingPointsThisTypeCounts = --gameData.subLevels[subLevelInProgress].countsForEachPointItemsVariant[pointItemNumber];
                // Если полностью выполнил этот тип элементов и этот элемент это тот который сейчас выполняеться (проверка чтобы
                // не переключал тип, если собрал все элементы другого типа под щитом)
                if (remainingPointsThisTypeCounts == 0 &&
                    pointItemNumber == pointTypeInProgressNumber)
                {
                    SwitchPointItemToNextType();
                }
                CheckLevelStatesAffterCollectCompletngItem(achivmentsPointsCounts);
                itemIsCollected = true;
            }
            else
            {
                MakeGameOver();
            }
            return itemIsCollected;
        }

        public void OnColisionWithDangerItems()
        {
            if (gameManager.gameData.shieldIsActive == false)
            {
                MakeGameOver();
            }
        }

        public void OnCollectCoin()
        {
             ++gameManager.gameData.coinsCounts;
        }

        public void OnRevive()
        {
            gameManager.gameData.gameIsOvered = false;
        }

        // При созданиии обьекта очка ))
        public void OnCreatePointItem(int pointNumberVariant)
        {
            // Увеличиваю количество обьектов очков этого варианта на текущем подуровне
            var gameData = gameManager.gameData;
            int subLevelNumber = gameData.subLevelInProgressNumber;
            var subLevel = gameData.subLevels[subLevelNumber];
            // Если элемента в массиве для обьектов  этого типа еще нету со создаеться
            if (pointNumberVariant >= subLevel.countsForEachPointItemsVariant.Count)
            {
                while (pointNumberVariant != subLevel.countsForEachPointItemsVariant.Count - 1)
                {
                    subLevel.countsForEachPointItemsVariant.Add(0);
                }
            }

            subLevel.countsForEachPointItemsVariant[pointNumberVariant]++;
        }
        #endregion
    }
}