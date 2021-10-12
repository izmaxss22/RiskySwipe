using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public event Action ONChangeCoinsCount; 
    
    private enum PlayerPrefsIds
    {
        GAME_VERSION = 0,

        ENERGY_COUNT = 1,
        COINS_COUNT = 2,
        REVIVE_EFFECTS_COUNT = 3,
        SHIELD_EFFECTS_COUNT = 4,

        SKIP_LEVEL_EFFECTS_COUNT = 5,
        SKIP_LEVEL_AD_VIEVS_COUNT = 6,

        LAST_AVIALABLE_LEVEL = 7,
        STARS_COUNTS_ACHIVMENTS_ON_LEVELS = 8,

        COUNT_GAME_IS_PLAYED = 9,
        COUNT_CALLS_RATE_SCREEN = 10,

        SOUND_MUTE_STATE = 11,
        FIRST_GAME_LAUNCH_STATE = 12,
        GAME_LANGUAGE = 13,
        MAP_SKINS_STATES = 14,
        NUMBERR_FOR_LEVEL_CHEST = 15,
        NUMBERR_FOR_STARS_CHEST = 16,
        STARS_FOR_STARS_CHEST = 17,

        ENERGY_ENDLESS_STATE = 18,
        COUNT_GAME_IS_PLAYED_FOR_AD = 19,
        AD_REMOVED_STATE = 19,
    };

    #region ДАННЫЕ СУНДУКОВ ЗА ПРОХОЖДЕНИЕ
    // todo (ОБНОВА) делать награды для тех уровне которые еще даже не существуют (в игре 20 уровней сделать сунудук на 40)
    public enum ChestsRewardsIds
    {
        COINS = 0,
        SHIELD = 1,
        REVIVE_EFFECT = 2,
    }

    [Serializable]
    // Класс для описания награды лежащей в сундуке
    public class ChestsRewardData
    {
        public ChestsRewardsIds rewardId;
        public int rewardCount;
    }

    #region ДАННЫЕ ДЛЯ СУНДУКОВ ЗА УРОВНИ
    public List<ChestForLevelsCompleting> chestsForLevelsCompletingsData;
    [Serializable]
    public class ChestForLevelsCompleting
    {
        public string name;
        public int needetLevel;
        public List<ChestsRewardData> rewards;
    }
    #endregion

    #region ДАННЫЕ ДЛЯ СУНДУКОВ ЗА ЗВЕЗДЫ
    public List<ChestForStarsData> chestsForStarsDatas;
    [Serializable]
    public class ChestForStarsData
    {
        public string name;
        public int needetStarsCounts;
        public List<ChestsRewardData> rewards;
    }
    #endregion
    #endregion

    #region ДАННЫЕ ДЛЯ ДИЗАЙНА В ИГРЕ (СКИНЫ ДЛЯ ПЕРСОНАЖА, СКИНЫ ДЛЯ ЭЛЕМЕНТОВ И ТД)
    public GameDesignData gameDesignData;
    [Serializable]
    public class GameDesignData
    {
        public LevelItemsDesignData levelItemsDesignData;

        [Serializable]
        // Класс для работы с данными касающися дизайна элементов в игре (виды дизайнов и другое)
        public class LevelItemsDesignData
        {
            [HideInInspector]
            // Переменая которая хранит номер последнего выбранного скина для переключения к следующему
            // (при перезагрузке игры переменная сбрасываеться)
            public int lastPickedSkinNumber;
            public List<Sprite> spritesForGameBkgParticles;
            public List<SkinItem> skins;
            [Header("Матерьялы айтемов")]
            // Матерьялы для айтемов (им писваювиються текстуры)
            public List<Material> materialsFor_wallItems = new List<Material>();
            public List<Material> materialsFor_floorItems = new List<Material>();
            [Serializable]
            // Класс для хранения информации об игровом дизайне всех элементов
            public class SkinItem
            {
                public string name;
                public Texture planeTextrure;
                public Texture[] walls_textures;
                public Texture[] floor_textures;
            }
            // Установка скина (присвоением матерьялам текстуры)
            public void SetItemsSkin(SkinItem skinItem, GameObject gameBkgPlane, ParticleSystem gameBkgPartcicle)
            {
                // Установка партиклов фона
                gameBkgPartcicle.textureSheetAnimation.RemoveSprite(0);
                // Рандомная выбираю из списка спрайтов
                int newSpriteNumber = UnityEngine.Random.Range(0, spritesForGameBkgParticles.Count);
                gameBkgPartcicle.textureSheetAnimation.AddSprite(spritesForGameBkgParticles[newSpriteNumber]);
                // Установка фона
                gameBkgPlane.GetComponent<MeshRenderer>().material.mainTexture = skinItem.planeTextrure;
                // Установка матерьяла для элеметов стен
                for (int i = 0; i < 8; i++)
                    materialsFor_wallItems[i].mainTexture = skinItem.walls_textures[i];
                // Установка матерьяла для элеметов пола
                for (int i = 0; i < 9; i++)
                    materialsFor_floorItems[i].mainTexture = skinItem.floor_textures[i];
            }

            public SkinItem GetNextSkin()
            {
                int[] mapSkinStates = DataManager.Instance.Get_StatesForMapSkins();
                bool bkgIsSet = false;
                //  Выбор фона, начиная с прошлого фона
                for (int i = lastPickedSkinNumber + 1; i < mapSkinStates.Length; i++)
                {
                    // Если фон был выбран для применения в игре
                    if (mapSkinStates[i] == 1)
                    {
                        lastPickedSkinNumber = i;
                        bkgIsSet = true;
                        break;
                    }
                }
                // Если фон не был установлен значит после последнего фона нету больше доступных фонов
                // Поэтому отсчет начинаеться с первого доступного для применения фона (выбраного)
                if (bkgIsSet == false)
                {
                    for (int i = 0; i < mapSkinStates.Length; i++)
                    {
                        if (mapSkinStates[i] == 1)
                        {
                            lastPickedSkinNumber = i;
                            break;
                        }
                    }
                }
                return skins[lastPickedSkinNumber];
            }
        }
    }
    #endregion

    #region ДАННЫЕ ДЛЯ ГРУПИРОВКИ СОЗДАННЫХ УРОВНЕЙ В ОДИН СОСТОЯЩИЙ ИЗ НЕСКОЛЬКИХ
    public List<GroupedLevelsItem> groupedLevelsItems = new List<GroupedLevelsItem>();
    [Serializable]
    public class GroupedLevelsItem
    {
        public List<int> countsPointsForGetEachStar = new List<int>();
        public List<int> usedLevelNumbers = new List<int>();
    }
    #endregion

    #region ДАННЫЕ ДЛЯ РЕКЛАМЫ
    [Serializable]
    public class AdSettingsData
    {
        public const int gameId_appStore = 4173772;
        public const int gameId_googlePLay = 4173773;
        public const string placmentName_rewardedVideo = "reviveAd";
        public const string placmentName_getCoins = "getEnergyAd";
        public const string placmentName_interstitialAd = "shortAd";
    }
    #endregion

    #region ПРЕФАБЫ ДЛЯ ИГРЫ
    public List<GameObject> prefabsForGameItems;
    [HideInInspector]
    public Dictionary<int, GameObject> prefabsForGameItemsUsedInGameManager = new Dictionary<int, GameObject>();
    #endregion

    #region ПРОЧЕЕ
    public bool haveInternerConection = false;
    //todo
    public const string GAME_LINK_IN_GOOGLE_PLAY = "https://play.google.com/store/apps/details?id=com.Risky.Swipe";
    public const string GAME_LINK_IN_APP_STORE = "https://apps.apple.com/us/app/gravity-jumping/id1539803278";

    public static string subLevelsPath = Application.streamingAssetsPath + "/Levels/SubLevels/";
    public static string createdLevelsPath = Application.streamingAssetsPath + "/Levels/CreatedLevels/";

    public int[] MAP_SKIN_PRICES = new int[] { 800, 900, 1300, 900, 1300, 800, 1300, 1500, 1500 };

    // Число уровней в игре (должно быть кратно 10 потому-что на странице всегда отображаеться по 10 уровней)
    public const int COUNT_LEVELS = 50;
    #endregion

    private void Awake()
    {
        Instance = this;
        CreateGamePrefabsDict();
    }

    public void CalculatePointCountsForReachStars()
    {
        foreach (var item in groupedLevelsItems)
        {
            float totalPointsCountsOnLevel = 0;
            foreach (var levelNumber in item.usedLevelNumbers)
            {
                string filePath = "NewLevelsFormat/" + levelNumber.ToString() + ".data";
                var loadedLevel = SerealizationManager.LoadConstantSerealizedObject<OneLevelData>(filePath);
                totalPointsCountsOnLevel += loadedLevel.pointsCountsForLevelComplete;
            }
            Debug.Log(totalPointsCountsOnLevel / 100);
            int firstStarValue = Convert.ToInt32(Math.Round((totalPointsCountsOnLevel / 100) * 40));
            int secondStarValue = Convert.ToInt32(Math.Round((totalPointsCountsOnLevel / 100) * 75));
            int lastStarValue = Convert.ToInt32(totalPointsCountsOnLevel);

            while (item.countsPointsForGetEachStar.Count != 3)
            {
                item.countsPointsForGetEachStar.Add(0);
            }
            item.countsPointsForGetEachStar[0] = firstStarValue;
            item.countsPointsForGetEachStar[1] = secondStarValue;
            item.countsPointsForGetEachStar[2] = lastStarValue;
        }

    }

    public void CalculateRewardsOnChests()
    {
        int coinsCountsOnChestForLevels = 0;
        int shieldEffectsCountsOnChestForLevels = 0;
        int reviveEffectsCountsOnChestForLevels = 0;

        foreach (var item in chestsForLevelsCompletingsData)
        {
            foreach (var reward in item.rewards)
            {
                switch (reward.rewardId)
                {
                    case ChestsRewardsIds.COINS:
                        coinsCountsOnChestForLevels += reward.rewardCount;
                        break;
                    case ChestsRewardsIds.SHIELD:
                        shieldEffectsCountsOnChestForLevels += reward.rewardCount;
                        break;
                    case ChestsRewardsIds.REVIVE_EFFECT:
                        reviveEffectsCountsOnChestForLevels += reward.rewardCount;
                        break;
                }
            }
        }

        Debug.Log("REWARDS ON LEVELS CHEST = " +
            " | COINS:" + coinsCountsOnChestForLevels +
            " | SHIELD:" + shieldEffectsCountsOnChestForLevels +
            " | REVIVE EFFECTS: " + reviveEffectsCountsOnChestForLevels);

        int coinsCountsOnChestForStars = 0;
        int shieldEffectsCountsOnChestForStars = 0;
        int reviveEffectsCountsOnChestForStars = 0;
        foreach (var item in chestsForStarsDatas)
        {
            foreach (var reward in item.rewards)
            {
                switch (reward.rewardId)
                {
                    case ChestsRewardsIds.COINS:
                        coinsCountsOnChestForStars += reward.rewardCount;
                        break;
                    case ChestsRewardsIds.SHIELD:
                        shieldEffectsCountsOnChestForStars += reward.rewardCount;
                        break;
                    case ChestsRewardsIds.REVIVE_EFFECT:
                        reviveEffectsCountsOnChestForStars += reward.rewardCount;
                        break;
                }
            }
        }

        Debug.Log("REWARDS ON LEVELS CHEST = " +
            " | COINS:" + coinsCountsOnChestForStars +
            " | SHIELD:" + shieldEffectsCountsOnChestForStars +
            " | REVIVE EFFECTS: " + reviveEffectsCountsOnChestForStars);


        Debug.Log("TOTAL REWARDS ON ALL CHESTS = " +
            " | COINS:" + (coinsCountsOnChestForStars + coinsCountsOnChestForLevels) +
            " | SHIELD:" + (shieldEffectsCountsOnChestForStars + shieldEffectsCountsOnChestForLevels) +
            " | REVIVE EFFECTS: " + (reviveEffectsCountsOnChestForStars + reviveEffectsCountsOnChestForLevels));
    }

    private void CreateGamePrefabsDict()
    {
        foreach (var item in prefabsForGameItems)
        {
            prefabsForGameItemsUsedInGameManager.Add(item.GetComponent<ItemIdsManager>().ItemsId.GetHashCode(), item);
        }
    }

    #region МЕТОДЫ ПОЛУЧЕНИЯ ЗНАЧЕНИЙ

    #region КОЛИЧЕСТВО СЫГРАННЫХ ИГР (В ООБЩЕМ)
    public int Get_CountGameIsPlayedForReviveScreen()
    {
        return PlayerPrefs.GetInt(PlayerPrefsIds.COUNT_GAME_IS_PLAYED.ToString(), 0);
    }

    public void Set_CountGameIsPlayedForReviveScreen(int count)
    {
        PlayerPrefs.SetInt(PlayerPrefsIds.COUNT_GAME_IS_PLAYED.ToString(), count);

    }
    #endregion

    #region КОЛИЧЕСТВО СЫГРАННЫХ ИГР ДЛЯ ОТСЧЕТА КОГДА ПОКАЗЫВАТЬ РЕКЛАМУ (ПОСЛЕ ПОКАЗА РЕКЛАМЫ СБРАСЫВАЕТЬСЯ)
    public int Get_CountGameIsPlayedForAd()
    {
        return PlayerPrefs.GetInt(PlayerPrefsIds.COUNT_GAME_IS_PLAYED_FOR_AD.ToString(), 0);
    }

    public void Set_CountGameIsPlayedForAd(int count)
    {
        PlayerPrefs.SetInt(PlayerPrefsIds.COUNT_GAME_IS_PLAYED_FOR_AD.ToString(), count);

    }
    #endregion

    #region КОЛИЧЕСТВО ВЫЗОВОВ ОКНА ОЦЕНКИ
    public int Get_CountCallsRateScreen()
    {
        return PlayerPrefs.GetInt(PlayerPrefsIds.COUNT_CALLS_RATE_SCREEN.ToString(), 0);
    }

    public void Set_CountCallsRateScreen(int count)
    {
        PlayerPrefs.SetInt(PlayerPrefsIds.COUNT_CALLS_RATE_SCREEN.ToString(), count);

    }
    #endregion

    #region СОСТОЯНИЕ ЗВУКА
    public bool Get_SoundIsMuteState()
    {
        int state = PlayerPrefs.GetInt(PlayerPrefsIds.SOUND_MUTE_STATE.ToString(), 0);
        return ValueParsers.ConvertIntToBool(state);
    }

    public void Set_SoundIsMuteState(bool state)
    {
        PlayerPrefs.SetInt(
            PlayerPrefsIds.SOUND_MUTE_STATE.ToString(),
            ValueParsers.ConvertBoolToInt(state));
    }
    #endregion

    #region ПЕРВЫЙ ЛИ ЭТО ЗАПУСК ИГРЫ
    public bool Get_FirstGameLaunchState()
    {
        int state = PlayerPrefs.GetInt(PlayerPrefsIds.FIRST_GAME_LAUNCH_STATE.ToString(), 1);
        return ValueParsers.ConvertIntToBool(state);
    }

    public void Set_FirstGameLaunchState(bool state)
    {
        PlayerPrefs.SetInt(
            PlayerPrefsIds.FIRST_GAME_LAUNCH_STATE.ToString(),
           ValueParsers.ConvertBoolToInt(state));
    }
    #endregion

    #region ЯЗЫК УСТАНОВЛЕННЫЙ В ИГРЕ
    public enum LanguageIds
    {
        RUSSIAN = 0,
        ENGLISH = 1,
    }
    public string Get_Language()
    {
        return PlayerPrefs.GetString(PlayerPrefsIds.ENERGY_COUNT.ToString(), LanguageIds.ENGLISH.ToString());
    }

    public void Set_Language(LanguageIds language)
    {
        PlayerPrefs.SetString(PlayerPrefsIds.ENERGY_COUNT.ToString(), language.ToString());
    }
    #endregion

    #region КОЛИЧЕСТВО МОНЕТ
    public int GetCoinsCount()
    {
        return PlayerPrefs.GetInt(PlayerPrefsIds.COINS_COUNT.ToString(), 0);
    }

    public void SetCoinsCount(int value)
    {
        PlayerPrefs.SetInt(PlayerPrefsIds.COINS_COUNT.ToString(), value);
        ONChangeCoinsCount?.Invoke();
    }
    #endregion

    #region КОЛИЧЕСТВО ЭФФЕКТОВ ВОЗРОЖДЕНИЯ
    public int Get_ReviveEffectsCount()
    {
        return PlayerPrefs.GetInt(PlayerPrefsIds.REVIVE_EFFECTS_COUNT.ToString(), 0);
    }

    public void Set_ReviveEffectsCount(int value)
    {
        PlayerPrefs.SetInt(PlayerPrefsIds.REVIVE_EFFECTS_COUNT.ToString(), value);
    }
    #endregion

    #region КОЛИЧЕСТВО ЭФФЕКТОВ ЩИТА
    public int Get_ShieldEffectsCount()
    {
        return PlayerPrefs.GetInt(PlayerPrefsIds.SHIELD_EFFECTS_COUNT.ToString(), 3);
    }

    public void SetShieldEffectsCount(int value)
    {
        PlayerPrefs.SetInt(PlayerPrefsIds.SHIELD_EFFECTS_COUNT.ToString(), value);
    }
    #endregion

    #region НОМЕР ПОСЛЕДНЕГО УРОВНЯ ДОСТУПНОГО ДЛЯ ПРОХОЖДЕНИЯ
    public int GetLastAvailableLevelNumber()
    {
        return PlayerPrefs.GetInt(PlayerPrefsIds.LAST_AVIALABLE_LEVEL.ToString(), 0);
    }

    public void SetLastAvailableLevelNumber(int value)
    {
        PlayerPrefs.SetInt(PlayerPrefsIds.LAST_AVIALABLE_LEVEL.ToString(), value);

    }
    #endregion

    #region МАССИВ КОЛИЧЕСТВО ПОЛУЧЕННЫХ ЗВЕЗД У КАЖДОГО УРОВНЯ
    public int[] GetStarsCountsAchivmentOnLevels()
    {
        string defaultValue = ValueParsers.ConvertIntArrayToString(new int[COUNT_LEVELS]);
        // Получения строки 
        string value = PlayerPrefs.GetString(
            PlayerPrefsIds.STARS_COUNTS_ACHIVMENTS_ON_LEVELS.ToString(),
            defaultValue);

        return ValueParsers.ConvertStringToIntArray(value);
    }

    public void SetStarsCountsAchivmentOnLevels(int[] value)
    {
        // Преобразование массив в строку для сохранения
        string stringValue = ValueParsers.ConvertIntArrayToString(value);
        PlayerPrefs.SetString(
            PlayerPrefsIds.STARS_COUNTS_ACHIVMENTS_ON_LEVELS.ToString(),
            stringValue);
    }
    #endregion

    #region МАССИВ УКАЗЫВАЮЩИЙ НА СОСТОЯНИЕ СКИНОВ ДЛЯ КАРТЫ ("НУЖНО КУПИТЬ" // "ВЫБРАН" // "НЕ ВЫБРАН")
    public int[] Get_StatesForMapSkins()
    {
        // Создание дефолтного значения и выбор 1 и 2 скина по умолчанию (их не надо покупать)
        int[] array = new int[MAP_SKIN_PRICES.Length];
        array[0] = 1;
        array[1] = 1;
        string defaultValue = ValueParsers.ConvertIntArrayToString(array);
        // Получения строки 
        string value = PlayerPrefs.GetString(
            PlayerPrefsIds.MAP_SKINS_STATES.ToString(),
            defaultValue);

        return ValueParsers.ConvertStringToIntArray(value);
    }

    public void Set_StatesForMapSkins(int[] value)
    {
        // Преобразование массив в строку для сохранения
        string stringValue = ValueParsers.ConvertIntArrayToString(value);
        PlayerPrefs.SetString(
            PlayerPrefsIds.MAP_SKINS_STATES.ToString(),
            stringValue);
    }
    #endregion

    #region НОМЕР ПОЛУЧАЕМОГО СУНДУКА УРОВНЯ 
    public int Get_NumberForLevelChest()
    {
        return PlayerPrefs.GetInt(PlayerPrefsIds.NUMBERR_FOR_LEVEL_CHEST.ToString(), 0);
    }

    public void Set_NumberForLevelChest(int value)
    {
        PlayerPrefs.SetInt(PlayerPrefsIds.NUMBERR_FOR_LEVEL_CHEST.ToString(), value);
    }
    #endregion

    #region НОМЕР ПОЛУЧАЕМОГО СУНДУКА ЗВЕЗД 
    public int Get_NumberForStarsChest()
    {
        return PlayerPrefs.GetInt(PlayerPrefsIds.NUMBERR_FOR_STARS_CHEST.ToString(), 0);
    }

    public void Set_NumberForStarsChest(int value)
    {
        PlayerPrefs.SetInt(PlayerPrefsIds.NUMBERR_FOR_STARS_CHEST.ToString(), value);
    }
    #endregion

    #region КОЛИЧЕСТВО ЗВЕЗД ПОЛУЧЕНЫХ ЗА ИГРУ (ИСПОЛЬЗУЕТЬСЯ В ПОДСЧЕТАХ СУНДУКА ЗВЕЗД)
    public int Get_StarChest_StarsCounts()
    {
        return PlayerPrefs.GetInt(PlayerPrefsIds.STARS_FOR_STARS_CHEST.ToString(), 0);
    }


    public void Set_StarChest_StarsCounts(int value)
    {
        PlayerPrefs.SetInt(PlayerPrefsIds.STARS_FOR_STARS_CHEST.ToString(), value);
    }
    #endregion

    #region КУПЛЕНА ЛИ БЕСКОНЕЧНАЯ ЭНЕРГИЯ
    public void Set_EnergyIsEndlessState(bool state)
    {
        PlayerPrefs.SetInt(
                 PlayerPrefsIds.ENERGY_ENDLESS_STATE.ToString(),
                ValueParsers.ConvertBoolToInt(state));
    }

    public bool Get_EnergyIsEndlessState()
    {
        int state = PlayerPrefs.GetInt(PlayerPrefsIds.ENERGY_ENDLESS_STATE.ToString(), 0);
        return ValueParsers.ConvertIntToBool(state);
    }
    #endregion

    #region КУПЛЕНО ЛИ УДАЛЕНИЕ РЕКЛАМЫ
    public void Set_AdIsRemovedState(bool state)
    {
        PlayerPrefs.SetInt(
                 PlayerPrefsIds.AD_REMOVED_STATE.ToString(),
                 ValueParsers.ConvertBoolToInt(state));
    }

    public bool Get_AdIsRemovedState()
    {
        int state = PlayerPrefs.GetInt(PlayerPrefsIds.AD_REMOVED_STATE.ToString(), 0);
        return ValueParsers.ConvertIntToBool(state);
    }
    #endregion

    #endregion
}

[Serializable]
public class OneLevelData
{
    public float cameraPosX;
    public float cameraPosY;
    public float cameraPosZ;

    public float cameraScale;
    public float levelSpeed;

    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;

    // Номер варината очков который сейчас проходиться
    public int pointVariantInProgressNumber = 0;
    // Количество варинатов у каждого обьекта очков
    public List<int> countsForEachPointItemsVariant = new List<int>();
    public int pointsCountsForLevelComplete = 0;
    public List<Item> items = new List<Item>();
    // Обьект со всеми колайдерами
    public List<ColliderPoints> collidersPoints = new List<ColliderPoints>();

    public OneLevelData()
    {
        this.cameraPosX = 20;
        this.cameraPosY = 20;
        this.cameraPosZ = -40;
        cameraScale = 25;
        levelSpeed = 10;
        playerPosX = 0;
        playerPosY = 0;
        playerPosZ = 0;
        items = new List<Item>();
        collidersPoints = new List<ColliderPoints>();
    }

    public OneLevelData(
        Vector3 cameraPos,
        float cameraScale,
        float levelSpeed,
        Vector3 playerPos,
        int pointsCountsForLevelComplete,
        List<Item> items,
        List<ColliderPoints> colliderPoints
        )
    {
        this.cameraPosX = cameraPos.x;
        this.cameraPosY = cameraPos.y;
        this.cameraPosZ = cameraPos.z;
        this.cameraScale = cameraScale;

        this.levelSpeed = levelSpeed;

        this.playerPosX = playerPos.x;
        this.playerPosY = playerPos.y;
        this.playerPosZ = playerPos.z;

        countsForEachPointItemsVariant = new List<int>();
        this.pointsCountsForLevelComplete = pointsCountsForLevelComplete;
        this.items = items;
        this.collidersPoints = colliderPoints;
    }

    [Serializable]
    public class Item
    {
        public float itemPosX;
        public float itemPosY;
        public float itemPosZ;

        public float itemRotationX;
        public float itemRotationY;
        public float itemRotationZ;
        public float itemRotationW;

        public float itemScaleX;
        public float itemScaleY;
        public float itemScaleZ;

        public int itemId;

        public bool isHaveSpecData;
        public Dictionary<string, string> specData = new Dictionary<string, string>();

        public Item(
            Vector3 itemPos,
            Quaternion itemRot,
            Vector3 itemScale,
            int itemId,
            bool isHaveSpecData,
            Dictionary<string, string> specData
            )
        {
            itemPosX = itemPos.x;
            itemPosY = itemPos.y;
            itemPosZ = itemPos.z;

            this.itemRotationX = itemRot.x;
            this.itemRotationY = itemRot.y;
            this.itemRotationZ = itemRot.z;
            this.itemRotationW = itemRot.w;

            this.itemScaleX = itemScale.x;
            this.itemScaleY = itemScale.y;
            this.itemScaleZ = itemScale.z;

            this.itemId = itemId;

            this.isHaveSpecData = isHaveSpecData;
            this.specData = specData;
        }

        public Vector3 GetPositon()
        {
            return new Vector3(itemPosX, itemPosY, itemPosZ);
        }

        //public Vector3 GetRotation()
        //{
        //    return new Vector3(itemRotationX, itemRotationY, itemRotationZ);
        //}

        public Quaternion GetRotation()
        {
            return new Quaternion(itemRotationX, itemRotationY, itemRotationZ, itemRotationW);
        }

        public Quaternion GetRotationInEuler()
        {
            return Quaternion.Euler(new Vector3(itemRotationX, itemRotationY, itemRotationZ));
        }

        public Vector3 GetScale()
        {
            return new Vector3(itemScaleX, itemScaleY, itemScaleZ);
        }
    }

    [Serializable]
    // Обьект одного колайдера на уровне (содержит все точки этого колайдера)
    public class ColliderPoints
    {
        public List<float> colliderXPoints = new List<float>();
        public List<float> colliderYPoints = new List<float>();

        public ColliderPoints(
            List<Vector2> colliderPoints)
        {
            //Если последняя точка не равна первой (для замыкания колайдера)
            if (colliderPoints[0] != colliderPoints.Last())
            {
                // То добавляю
                var firstPoint = colliderPoints[0];
                colliderPoints.Add(firstPoint);
            }

            foreach (var item in colliderPoints)
            {
                colliderXPoints.Add(item.x);
                colliderYPoints.Add(item.y);
            }
        }

        public List<Vector2> GetPoints()
        {
            List<Vector2> points = new List<Vector2>();
            for (int i = 0; i < colliderXPoints.Count; i++)
            {
                Vector2 point = new Vector2(colliderXPoints[i], colliderYPoints[i]);
                points.Add(point);
            }
            return points;
        }
    }

    public Vector3 GetCameraPos()
    {
        return new Vector3(cameraPosX, cameraPosY, cameraPosZ);
    }

    public Vector3 GetPlayerPos()
    {
        return new Vector3(playerPosX, playerPosY, -0.5f);
    }
}