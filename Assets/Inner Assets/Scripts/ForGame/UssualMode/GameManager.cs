using System;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace UssualMode
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public GameProgressManager GameProgressManager;
        public GameUIManager GameUIManager;
        public PlayerManager PlayerManager;
        public GameItemsManager GameItemsManager;

        public GameData gameData;
        public class GameData
        {
            public int levelNumber;
            public readonly int pointsCountsForLevelCompleteDefaultConst;
            public int pointsCountsForLevelComplete;
            public readonly int[] pointsCountsForGetEachStar = new int[3];
            public int[] pointsCountsForGetEachStarForProgressCalculating = new int[3];

            public bool gameIsOvered;

            public int subLevelInProgressNumber;
            public int shieldsCounts;
            public int coinsCounts;
            public bool shieldIsActive = false;
            public float shieldDuration = 7f;

            public List<OneLevelData> subLevels = new List<OneLevelData>();
            public List<DataManager.GameDesignData.LevelItemsDesignData.SkinItem> itemsSkinsForEachSubLevel =
                new List<DataManager.GameDesignData.LevelItemsDesignData.SkinItem>();

            public int countStarsAchivmentOnLastSessions;
            public int countStarsAchivmentOnThisSession;

            public GameData(
                  int levelNumber,
                  List<int> pointsCountsForGetEachStar,
                  List<OneLevelData> subLevels)
            {
                this.levelNumber = levelNumber;
                pointsCountsForGetEachStar.CopyTo(this.pointsCountsForGetEachStar);
                pointsCountsForGetEachStarForProgressCalculating = (int[])this.pointsCountsForGetEachStar.Clone();
                pointsCountsForGetEachStarForProgressCalculating[2] -= pointsCountsForGetEachStarForProgressCalculating[1];
                pointsCountsForGetEachStarForProgressCalculating[1] -= pointsCountsForGetEachStarForProgressCalculating[0];
                subLevelInProgressNumber = 0;
                shieldsCounts = DataManager.Instance.Get_ShieldEffectsCount();
                coinsCounts = 0;
                this.subLevels = subLevels;
                // Создание скина для каждого подуровня и получения общего числа очков для прохождения уровня
                foreach (var item in subLevels)
                {
                    var skin = DataManager.Instance.gameDesignData.levelItemsDesignData.GetNextSkin();
                    itemsSkinsForEachSubLevel.Add(skin);
                    pointsCountsForLevelComplete += item.pointsCountsForLevelComplete;
                }
                pointsCountsForLevelCompleteDefaultConst = pointsCountsForLevelComplete;

                countStarsAchivmentOnLastSessions = DataManager.Instance.GetStarsCountsAchivmentOnLevels()[levelNumber];
            }
        }

        #region ИНИЦИАЛИЗАЦИЯ
        private void Awake()
        {
            Instance = this;
        }

        public void StartGame(int levelNumber, DataManager.GroupedLevelsItem groupedLevelsItem)
        {
            CreateGameData(levelNumber, groupedLevelsItem);
            GameProgressManager.Init();
            GameItemsManager.Init();
            GameUIManager.Init();
            PlayerManager.Init();
        }

        private void CreateGameData(int levelNumber, DataManager.GroupedLevelsItem groupedLevelsItem)
        {
            List<OneLevelData> levels = new List<OneLevelData>();
            foreach (var item in groupedLevelsItem.usedLevelNumbers)
            {
                // todo захаркодить значение пути
                var level = SerealizationManager.LoadConstantSerealizedObject<OneLevelData>("NewLevelsFormat/" + item.ToString() + ".data");
                levels.Add(level);
            }
            gameData = new GameData(
                levelNumber,
                groupedLevelsItem.countsPointsForGetEachStar,
                levels
                );
        }
        #endregion

        private IEnumerator CompleteSubLevel()
        {
            GameUIManager.OnCompleteSubLevel();
            GameItemsManager.OnCompleteSubLevel(gameData.subLevelInProgressNumber);
            PlayerManager.OnCompleteSubLevel();
            yield return new WaitForSeconds(0.5f);
            gameData.gameIsOvered = false;
            yield break;
        }

        public void OnCompleteLevel()
        {
            AudioManager.Instance.Play_Game_CompleteLevel();
            GameUIManager.OnCompleteLevel();
            PlayerManager.OnCompleteLevel();
            WindowsManager.Instance.FromGameToPickedLevelScreen(
                gameObject,
                gameData.levelNumber,
                gameData.countStarsAchivmentOnThisSession,
                gameData.countStarsAchivmentOnLastSessions);
        }

        public void OnCompleteSubLevel()
        {
            StartCoroutine(CompleteSubLevel());
        }

        public void OnCompleteStar(int achivmentStarNumber)
        {
            AudioManager.Instance.Play_Game_CollectStar();
            GameUIManager.OnCollectStar(achivmentStarNumber);
        }

        public void OnGameOver(bool isNeedShowReviveScreen)
        {
            AudioManager.Instance.Play_Game_GameOver();
            CameraShaker.Instance.ShakeOnce(1.5f, 5f, 0.3f, 1);
            MMVibrationManager.Haptic(HapticTypes.Failure);
            PlayerManager.OnGameOver();
            if (isNeedShowReviveScreen)
            {
                WindowsManager.Instance.FromGameToReviveScreen(
                    this,
                    gameData.levelNumber,
                    gameData.countStarsAchivmentOnThisSession,
                    gameData.countStarsAchivmentOnLastSessions);
            }
            else
            {
                WindowsManager.Instance.FromGameToPickedLevelScreen(
                    gameObject,
                    gameData.levelNumber,
                    gameData.countStarsAchivmentOnThisSession,
                    gameData.countStarsAchivmentOnLastSessions);
            }
        }

        public void OnRevive()
        {
            GameProgressManager.OnRevive();
            GameUIManager.OnRevive();
            PlayerManager.OnRevive();
        }

        public event Action onShieldActivate;
        public void OnActivateShield()
        {
            GameProgressManager.OnActivateShield();
            onShieldActivate?.Invoke();
        }

        public event Action onShieldDeactivate;
        public void OnDeactivateShield()
        {
            GameProgressManager.OnDeactivateShield();
            onShieldDeactivate?.Invoke();
        }

        public void OnCollisionWithSpike()
        {
            GameProgressManager.OnColisionWithDangerItems();
        }

        public void OnCollisionWithSqareSpike()
        {
            GameProgressManager.OnColisionWithDangerItems();
        }


        public void OnCollectDangerObstacle()
        {
            GameProgressManager.OnColisionWithDangerItems();
        }

        public void OnCollisionWithSpikeForPressed()
        {
            GameProgressManager.OnColisionWithDangerItems();

        }

        public void OnColisionWithGunBullet()
        {
            GameProgressManager.OnColisionWithDangerItems();
        }

        public void OnCollectCoin()
        {
            GameProgressManager.OnCollectCoin();
            GameUIManager.OnCollectCoin();
        }

        public void OnCollectPointItem(int pointItemNumber, int achivmentsPointsCounts)
        {
            bool itemIsCollectedCorrect = GameProgressManager.OnCollectPointItem(pointItemNumber, achivmentsPointsCounts);
            if (itemIsCollectedCorrect)
                GameUIManager.OnCollectPointItem(achivmentsPointsCounts);
        }

        public event Action<int, int> onChangePointItemsType;
        public void OnChangePointItemsType(int previousType, int nowType)
        {
            onChangePointItemsType?.Invoke(previousType, nowType);
        }

        public void OnCreatePointItem(int pointNumberVariant)
        {
            GameProgressManager.OnCreatePointItem(pointNumberVariant);
        }
    }
}