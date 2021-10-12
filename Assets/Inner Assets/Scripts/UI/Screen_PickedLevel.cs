using System.Collections;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UssualMode;

public class Screen_PickedLevel : MonoBehaviour
{
    #region ПЕРЕМЕННЫЕ
    public GameObject prefab_rateScreen;

    public DataManager DataManager;
    private GameObject MainCanvas;
    private AudioManager AudioManager;
    private Panel_EnergyAndCoinsDesc panel_EnergyAndCoinsDesc;
    private Panel_RewardsAndChest panelFor_RewardsAndChest;
    public GameObject prefab_gameManager;
    public GameObject prefab_screenGetEnergy;
    public GameObject prefab_screenPickLevels;
    public GameObject prefab_panelShopAndSettingsButtons;
    public GameObject prefab_panelForDimmerSplash;

    public Text textFor_levelNumber;
    public GameObject starsCont;
    public Image[] starsIcons;
    public Text textFor_shieldEffectsCount;
    public Button button_buySheildsEffects;

    public Image button_switchToPreviousLevel;
    public Image button_switchToNextLevel;
    public Button butt_startGame;

    public Sprite spriteFor_buttonBuyShieldsEffects_enabled;
    public Sprite spriteFor_buttonBuyShieldsEffects_disabled;
    public Sprite spriteFor_starIcons_yellow;
    public Sprite spriteFor_starIcons_gray;
    public Sprite spriteFor_switchToPreviousLevelButton_enable;
    public Sprite spriteFor_switchToPreviousLevelButton_disable;
    public Sprite spriteFor_switchToNextLevelButton_enable;
    public Sprite spriteFor_switchToNextLevelButton_disable;


    private int levelNumber;
    private int starsCount;
    private int shieldsCount;
    private int coinsCount;
    #endregion

    private void Awake()
    {
        DataManager = DataManager.Instance;
    }

    private void Start()
    {
        MainCanvas = GameObject.Find("MainCanvas").gameObject;
        panel_EnergyAndCoinsDesc = GameObject.Find("panelFor_energyAndCoinsDesc").GetComponent<Panel_EnergyAndCoinsDesc>();
        panelFor_RewardsAndChest = Panel_RewardsAndChest.Instance;

        AudioManager = AudioManager.Instance;
        // Advertisement.AddListener(this);
    }

    public IEnumerator CreateScreen_FromPickLevelsScreen(
    int levelNumber)
    {
        LoadLevelData(levelNumber);
        CheckSwitchLevelsButton();
        CreateLevelNumber(levelNumber);
        CreateStarsIcons();
        CreateShieldItems();
        GetComponent<Animator>().SetTrigger("show");
        yield break;
    }

    public void CreateScreen_FromGameScreen(
        int levelNumber,
        int beforeStarsCount,
        int nowStarsCount)
    {
        StartCoroutine(CreateScreen_FromGameScreen_Coroutine(levelNumber, beforeStarsCount, nowStarsCount));
    }

    private IEnumerator CreateScreen_FromGameScreen_Coroutine(
        int levelNumber,
        int beforeStarsCount,
        int nowStarsCount
       )
    {
        LoadLevelData(levelNumber);
        CheckSwitchLevelsButton();
        CreateLevelNumber(levelNumber);
        CreateStarsIcons();
        CreateShieldItems();
        GetComponent<Animator>().SetTrigger("show");
        //
        // if (Advertisement.IsReady(DataManager.AdSettingsData.placmentName_interstitialAd) && AdsShowingManager.GetCanShowAdState())
        // {
        //     Advertisement.Show(DataManager.AdSettingsData.placmentName_interstitialAd);
        // }

        transform.SetSiblingIndex(transform.GetSiblingIndex() - 2);

        for (int i = 0; i < beforeStarsCount; i++)
        {
            starsIcons[i].sprite = spriteFor_starIcons_yellow;
        }

        if (beforeStarsCount != 3 && nowStarsCount == 3)
        {
            butt_startGame.interactable = false;
        }
        yield return new WaitForSeconds(0.6f);

        // Если за игру было получено больше звезд чем было до этого
        if (beforeStarsCount < nowStarsCount)
        {
            // Обновления количества звезд для сундука звезд
            int starsCounts = DataManager.Get_StarChest_StarsCounts();
            starsCounts += nowStarsCount - beforeStarsCount;
            DataManager.Set_StarChest_StarsCounts(starsCounts);

            // Анимация пульсации у полученных звезд
            for (int i = beforeStarsCount; i < nowStarsCount; i++)
            {
                starsIcons[i].GetComponent<Animator>().SetTrigger("pulse");
                yield return new WaitForSeconds(0.05f);
                starsIcons[i].GetComponent<Image>().sprite = spriteFor_starIcons_yellow;
            }
        }
        // Если пользователь достиг в первый раз 3 звезды то идет переключение
        if (beforeStarsCount != 3 && nowStarsCount == 3)
        {
            // Небольшая задержка чтобы закончилась анимация пульсации звезд
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(SwitchLevel(levelNumber + 1));
            butt_startGame.interactable = true;
        }

        panelFor_RewardsAndChest.InitChestButtons();

        if (DataManager.Get_CountGameIsPlayedForAd() >= 10 &&
            DataManager.Get_AdIsRemovedState() == false)
        {
            // if (Advertisement.IsReady(DataManager.AdSettingsData.placmentName_interstitialAd))
            //     Advertisement.Show(DataManager.AdSettingsData.placmentName_interstitialAd);
            DataManager.Set_CountGameIsPlayedForAd(0);
        }
        yield break;
    }

    #region НАЖАТИЯ НА КНОПКИ
    public void OnClick_NextLevelButton()
    {
        AudioManager.Play_PickedLevelScreen_SwitchLevel();
        StartCoroutine(SwitchLevel(levelNumber + 1));
    }

    public void OnClick_PreviousLevelButton()
    {
        AudioManager.Play_PickedLevelScreen_SwitchLevel();
        StartCoroutine(SwitchLevel(levelNumber - 1));
    }

    public void OnClick_StartButton()
    {
        if (screenIsClosed == false)
        {
            MMVibrationManager.Haptic(HapticTypes.SoftImpact);

            StartCoroutine(StartGame());
        }
    }

    public void OnClickCloseButton()
    {
        StartCoroutine(CloseScreen());
    }

    public void OnClick_ButtonBuyShieldEffect()
    {
        if (coinsCount >= 250)
        {
            AnalyticsEventsManager.Instance.OnEvent_user_buy_shield();
            AudioManager.Play_PickedLevelScreen_BuyShield();
            coinsCount -= 250;
            DataManager.SetCoinsCount(coinsCount);

            shieldsCount++;
            DataManager.SetShieldEffectsCount(shieldsCount);

            textFor_shieldEffectsCount.text = shieldsCount.ToString();
            if (coinsCount < 250)
            {
                button_buySheildsEffects.interactable = false;
                button_buySheildsEffects.GetComponent<Image>().sprite = spriteFor_buttonBuyShieldsEffects_disabled;
                button_buySheildsEffects.GetComponent<Animator>().enabled = false;
                button_buySheildsEffects.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            }
        }
    }
    #endregion

    #region МЕТОДЫ СОЗДАНИЯ ДАННЫХ ОКНА
    private void LoadLevelData(int levelNumber)
    {
        this.levelNumber = levelNumber;
        starsCount = DataManager.GetStarsCountsAchivmentOnLevels()[levelNumber];
        shieldsCount = DataManager.Get_ShieldEffectsCount();
        coinsCount = DataManager.GetCoinsCount();
    }

    private void CreateLevelNumber(int levelNumber)
    {
        if (DataManager.Get_Language() == DataManager.LanguageIds.ENGLISH.ToString())
            textFor_levelNumber.text = "LEVEL  " + (levelNumber + 1).ToString();
        else
            textFor_levelNumber.text = "УРОВЕНЬ " + (levelNumber + 1).ToString();
    }

    private void CreateStarsIcons()
    {
        // Обнуление всех иконок в исходное состояние
        foreach (var item in starsIcons)
            item.GetComponent<Image>().sprite = spriteFor_starIcons_gray;

        for (int i = 0; i < starsCount; i++)
            starsIcons[i].sprite = spriteFor_starIcons_yellow;
    }

    private void CreateShieldItems()
    {
        textFor_shieldEffectsCount.text = shieldsCount.ToString();
        if (coinsCount >= 100)
        {
            button_buySheildsEffects.interactable = true;
            button_buySheildsEffects.GetComponent<Image>().sprite = spriteFor_buttonBuyShieldsEffects_enabled;
            button_buySheildsEffects.GetComponent<Animator>().enabled = true;
        }
    }

    #endregion

    private IEnumerator SwitchLevel(int levelNumber)
    {
        // Уровень можно переключить только если он
        // - не меньше 0
        // - не больше максимального количества уровней
        // - не больше максимального доступного уровня
        if (levelNumber >= 0 &&
            levelNumber <= DataManager.COUNT_LEVELS &&
            levelNumber <= DataManager.GetLastAvailableLevelNumber())
        {
            LoadLevelData(levelNumber);
            CheckSwitchLevelsButton();
            textFor_levelNumber.GetComponent<Animator>().SetTrigger("recreate");
            starsCont.GetComponent<Animator>().SetTrigger("recreate");

            yield return new WaitForSeconds(0.2F);
            CreateLevelNumber(levelNumber);
            CreateStarsIcons();
        }
        yield break;
    }

    private bool screenIsClosed;
    private IEnumerator CloseScreen()
    {
        if (screenIsClosed == false)
        {
            AudioManager.Play_PickedLevelScreen_CloseScreen();
            screenIsClosed = true;
            GameObject go = Instantiate(prefab_screenPickLevels, MainCanvas.transform);
            GameObject.Find("mainBkg_forAllCanvases").GetComponent<Image>().enabled = true;

            go.transform.SetSiblingIndex(1);
            go.GetComponent<ManagerFor_Screen_PickLevel>().CreateScreen_FromLevelScreen(levelNumber);
            yield return new WaitForSeconds(0.15f);
            GetComponent<Animator>().SetTrigger("hide");
            yield return new WaitForSeconds(0.65f);
            GameObject go2 = Instantiate(prefab_panelShopAndSettingsButtons, MainCanvas.transform);
            go2.transform.SetSiblingIndex(go2.transform.GetSiblingIndex() - 1);
            Destroy(gameObject);
        }

        yield break;
    }

    private bool gameIsStarted = false;
    private IEnumerator StartGame()
    {
        if (gameIsStarted == false)
        {
            AdsShowingManager.OnStartLevel();
            AnalyticsEventsManager.Instance.OnEvent_level_start(levelNumber);

            AudioManager.Play_PickedLevelScreen_Start();
            AudioManager.Play_ASource_1(2f);
            gameIsStarted = true;

            DataManager.Set_CountGameIsPlayedForReviveScreen(DataManager.Get_CountGameIsPlayedForReviveScreen() + 1);
            DataManager.Set_CountGameIsPlayedForAd(DataManager.Get_CountGameIsPlayedForAd() + 1);

            var groupedLevelData = DataManager.Instance.groupedLevelsItems[levelNumber];
            Instantiate(prefab_gameManager).GetComponent<GameManager>().StartGame(levelNumber, groupedLevelData);

            panel_EnergyAndCoinsDesc.GetComponent<Animator>().SetTrigger("hide");
            panelFor_RewardsAndChest.GetComponent<Animator>().SetTrigger("hide");

            yield return new WaitForSeconds(0.15f);

            gameObject.GetComponent<Animator>().SetTrigger("hide");

            yield return new WaitForSeconds(0.75f);
            Destroy(panel_EnergyAndCoinsDesc.gameObject);
            Destroy(panelFor_RewardsAndChest.gameObject);
            Destroy(gameObject);
        }
        yield break;
    }

    private void CheckSwitchLevelsButton()
    {
        if (levelNumber == 0)
        {
            button_switchToPreviousLevel.sprite = spriteFor_switchToPreviousLevelButton_disable;
            button_switchToPreviousLevel.GetComponent<Button>().interactable = false;
        }
        else
        {
            button_switchToPreviousLevel.sprite = spriteFor_switchToPreviousLevelButton_enable;
            button_switchToPreviousLevel.GetComponent<Button>().interactable = true;
        }
        // Если отображен последний доступный уровень или это впринципе последний уровень в игре то переключение к следующему недоступно
        if (levelNumber == DataManager.GetLastAvailableLevelNumber() || levelNumber + 1 == DataManager.COUNT_LEVELS)
        {
            button_switchToNextLevel.sprite = spriteFor_switchToNextLevelButton_disable;
            button_switchToNextLevel.GetComponent<Button>().interactable = false;
        }
        else
        {
            button_switchToNextLevel.sprite = spriteFor_switchToNextLevelButton_enable;
            button_switchToNextLevel.GetComponent<Button>().interactable = true;
        }
    }

    #region КОЛБЭКИ РЕКЛАМЫ
    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId
        // , ShowResult showResult
        )
    {
        if (placementId == DataManager.AdSettingsData.placmentName_interstitialAd)
        {
            AdsShowingManager.OnInterestialAdWasShowed();
        }
    }
    #endregion
}
