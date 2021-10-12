using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerFor_Screen_PickLevel : MonoBehaviour
{
    private GameObject MainCanvas;
    private DataManager DataManager;
    private AudioManager AudioManager;
    private GameObject panelFor_energyAndCoinsDesc;
    public GameObject prefab_pickedLevelScreen;
    public GameObject prefab_panelForRewardsAndChest;
    public GameObject prefab_screenMain;


    public GameObject contFor_levelButtons;
    public Text textFor_pageNumber;

    public GameObject prefab_levelButton;
    public GameObject prefab_invisibleLevelButton;

    public Sprite spriteFor_levelButton_available;
    public Sprite spriteFor_levelButton_notAvailable;
    public Sprite spriteFor_levelButton_bonus_avialable;
    public Sprite spriteFor_levelButton_bonus_notAvialable;
    public Sprite spriteFor_starIcon_yellow;

    private int lastAvialableLevel;
    private int[] levelsStarsCounts;

    private List<GameObject> levelButtons = new List<GameObject>();
    private int pageNumber = -1;

    bool screenCreatedFromPickedLevelScreen;

    private void Awake()
    {
        AudioManager = AudioManager.Instance;
        DataManager = DataManager.Instance;
    }

    private void Start()
    {
        AudioManager = AudioManager.Instance;
        DataManager = DataManager.Instance;
        MainCanvas = GameObject.Find("MainCanvas").gameObject;
        panelFor_energyAndCoinsDesc = GameObject.Find("panelFor_energyAndCoinsDesc");

        if (screenCreatedFromPickedLevelScreen == false)
        {
            CreateScreen_Default();
            transform.SetSiblingIndex(1);
            var panel = Instantiate(prefab_panelForRewardsAndChest, MainCanvas.transform);
            panel.transform.SetAsLastSibling();
        }
    }

    public void CreateScreen_Default()
    {
        Get_LevelsDatas();
        Create_Page(Get_PageNumber_FromLevelNumber(lastAvialableLevel));
    }

    public void CreateScreen_FromLevelScreen(int levelNumber)
    {
        screenCreatedFromPickedLevelScreen = true;
        Get_LevelsDatas();
        Create_Page(Get_PageNumber_FromLevelNumber(levelNumber));
    }

    public void OnClickLevelButton(int levelNumber)
    {
        StartCoroutine(Switch_ToPickedLevelScreen(levelNumber));
    }

    public void OnClick_NextPageButton()
    {
        Swtich_ToNextPage();

    }

    public void OnClick_PreviousPageButton()
    {
        Swtich_ToPrevious();
    }

    public void OnClick_CloseButton()
    {
        var screen = Instantiate(prefab_screenMain, MainCanvas.transform);
        screen.transform.SetSiblingIndex(1);
        Destroy(Panel_RewardsAndChest.Instance.gameObject);
        Destroy(gameObject);
    }

    #region ПЕРЕКЛЮЧЕНИЕ СТРАНИЦЫ УРОВНЕЙ
    private void Swtich_ToNextPage()
    {
        int maxPageNumber = levelsStarsCounts.Length / 10;
        if (pageNumber < maxPageNumber - 1)
        {
            AudioManager.Play_LevelsScreen_OnClick_SwithLevelPage(1);
            Clear_Page();
            Create_Page(pageNumber + 1);
        }
    }

    private void Swtich_ToPrevious()
    {
        if (pageNumber > 0)
        {
            AudioManager.Play_LevelsScreen_OnClick_SwithLevelPage(1.2f);
            Clear_Page();
            Create_Page(pageNumber - 1);
        }
    }
    #endregion

    private IEnumerator Switch_ToPickedLevelScreen(int levelNumber)
    {
        AudioManager.Play_LevelsScreen_OnClick_PickLevel();


        //bool haveInternerConection = DataManager_AllData.haveInternerConection;

        // TODO проверить на айндрод изменить количество игры и настроить место появления, события и для другого режима сделать вызов тоже
        //if (haveInternerConection &&
        //    DataManager_AllData.Get_CountGameIsPlayedForReviveScreen() >= 100 &&
        //    DataManager_AllData.Get_CountCallsRateScreen() == 1)
        //{
        //    DataManager_AllData.Set_CountCallsRateScreen(2);
        //    StoreReview.RequestRating();
        //}

        //else if (haveInternerConection &&
        //    DataManager_AllData.Get_CountGameIsPlayedForReviveScreen() >= 5 &&
        //    DataManager_AllData.Get_CountCallsRateScreen() == 0
        //    )
        //{
        //    DataManager_AllData.Set_CountCallsRateScreen(1);
        //    StoreReview.RequestRating();
        //}
        // todo else
        if (true)
        {
            GameObject go1 = Instantiate(prefab_pickedLevelScreen, MainCanvas.transform);
            go1.transform.SetAsLastSibling();
            panelFor_energyAndCoinsDesc.transform.SetAsLastSibling();
            Panel_RewardsAndChest.Instance.transform.SetAsLastSibling();


            StartCoroutine(go1.GetComponent<Screen_PickedLevel>().CreateScreen_FromPickLevelsScreen(levelNumber));
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
            GameObject.Find("mainBkg_forAllCanvases").GetComponent<Image>().enabled = false;
            GameObject go = GameObject.Find("panelFor_shopAndSettingsButtons");
            Destroy(go);
        }
        yield break;
    }

    private void Get_LevelsDatas()
    {
        lastAvialableLevel = DataManager.GetLastAvailableLevelNumber();
        levelsStarsCounts = DataManager.GetStarsCountsAchivmentOnLevels();
    }

    private int Get_PageNumber_FromLevelNumber(int levelNumber)
    {
        return (int)Mathf.Floor(levelNumber / 10);
    }

    private void Create_Page(int pageNumber)
    {

        this.pageNumber = pageNumber;
        textFor_pageNumber.text = (1 + pageNumber).ToString();

        int countingPoint = int.Parse((pageNumber).ToString() + "0");
        int equalsPoint = int.Parse((pageNumber + 1).ToString() + "1");
        for (int i = countingPoint; i < equalsPoint; i++)
        {
            if (i < equalsPoint - 2) Create_LevelButton(i);

            else if (i == equalsPoint - 2)
            {
                GameObject go = Instantiate(
                   prefab_invisibleLevelButton,
                   Vector3.zero,
                   Quaternion.identity,
                   contFor_levelButtons.transform);
                levelButtons.Add(go);

            }

            else Create_LevelButton(i - 1);
        }
    }

    private void Clear_Page()
    {
        foreach (var item in levelButtons) Destroy(item);
    }

    private void Create_LevelButton(int levelNumber)
    {
        #region ИНИЦИАЛИЗАЦИЯ ПРЕФАБА И ПОЛУЧЕНИЕ ЕГО ПОД АЙТЕМОВ
        GameObject levelButton = Instantiate(
            prefab_levelButton,
            Vector3.zero,
            Quaternion.identity,
            contFor_levelButtons.transform);

        GameObject starsCont = levelButton.transform.GetChild(0).gameObject;
        GameObject[] starsIcons = new GameObject[3];
        for (int i = 0; i < 3; i++) starsIcons[i] = starsCont.transform.GetChild(i).gameObject;
        Text levelNumberText_enabled = levelButton.transform.GetChild(1).gameObject.GetComponent<Text>();
        Text levelNumberText_disabled = levelButton.transform.GetChild(2).gameObject.GetComponent<Text>();
        GameObject loockIcon = levelButton.transform.GetChild(3).gameObject;
        #endregion

        string levelNumberForText = (levelNumber + 1).ToString();

        // Если уровень доступен
        if (levelNumber <= lastAvialableLevel)
        {
            // Если это обычный уровень
            levelButton.GetComponent<Image>().sprite = spriteFor_levelButton_available;

            // Отображение звезд
            starsCont.SetActive(true);
            for (int i = 0; i < levelsStarsCounts[levelNumber]; i++)
                starsIcons[i].GetComponent<Image>().sprite = spriteFor_starIcon_yellow;
            // Кнопка доступна для нажатия
            levelButton.GetComponent<Button>().interactable = true;
            levelButton.GetComponent<Button>().onClick.AddListener(() => OnClickLevelButton(levelNumber));
            // Установка номера уровня
            levelNumberText_enabled.gameObject.SetActive(true);
            levelNumberText_enabled.text = levelNumberForText;
        } // Иначе уровень не доступен
        else
        {
            // Если обычный уровень
            levelButton.GetComponent<Image>().sprite = spriteFor_levelButton_notAvailable;

            levelNumberText_disabled.gameObject.SetActive(true);
            levelNumberText_disabled.text = levelNumberForText;
            loockIcon.SetActive(true);
        }

        levelButtons.Add(levelButton);
    }

}
