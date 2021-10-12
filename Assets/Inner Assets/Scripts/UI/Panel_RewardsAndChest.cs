using UnityEngine;
using UnityEngine.UI;

public class Panel_RewardsAndChest : MonoBehaviour
{
    private AudioManager AudioManager;
    public DataManager DataManager;

    private GameObject MainCanvas;
    public GameObject prefabFor_screenChestRewards;

    public Button button_getLevelsChest;
    public Text text_levelChestNumber;

    public Button button_getStarsChest;
    public Text text_starsChestStarsCounts;

    public static Panel_RewardsAndChest Instance;

    void Start()
    {
        gameObject.name = "panelFor_RewardsAndChest";
        Instance = this;
        DataManager = DataManager.Instance;
        AudioManager = AudioManager.Instance;

        MainCanvas = GameObject.Find("MainCanvas").gameObject;
        InitChestButtons();
    }

    public void OnClick_ChestLevelButton()
    {
        AudioManager.Play_PickedLevelScreen_BuyShield();
        AnalyticsEventsManager.Instance.OnEvent_open_level_chest(DataManager.Get_NumberForLevelChest().ToString());
        Instantiate(prefabFor_screenChestRewards, MainCanvas.transform).GetComponent<Screen_ChestRewards>().CreateScreen(true);
    }

    public void OnClick_ChestStarsButton()
    {
        AudioManager.Play_PickedLevelScreen_BuyShield();
        AnalyticsEventsManager.Instance.OnEvent_open_stars_chest(DataManager.Get_NumberForStarsChest().ToString());
        Instantiate(prefabFor_screenChestRewards, MainCanvas.transform).GetComponent<Screen_ChestRewards>().CreateScreen(false);
    }


    public void InitChestButtons()
    {
        InitButtonForLevelCompletingChest();
        InitButtonForStarsChest();
    }

    private void InitButtonForLevelCompletingChest()
    {
        int nowMaxLevel = DataManager.GetLastAvailableLevelNumber() + 1;
        int levelChestNumber = DataManager.Get_NumberForLevelChest();
        // Уровень необходимый для получения сундука
        int neededLevelNumber = DataManager.chestsForLevelsCompletingsData[levelChestNumber].needetLevel;
        if (DataManager.Get_Language() == DataManager.LanguageIds.ENGLISH.ToString())
        {
            text_levelChestNumber.text = neededLevelNumber.ToString() + " LEVEL";
        }
        else
        {
            text_levelChestNumber.text = neededLevelNumber.ToString() + " УРОВЕНЬ";
        }
        // Если еще недостигнут нужный уровень для открытия сундука
        if (nowMaxLevel < neededLevelNumber)
        {

            //  То кнопка недоступна
            button_getLevelsChest.interactable = false;
            button_getLevelsChest.GetComponent<Animator>().enabled = false;
            button_getLevelsChest.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 0.6f);
        }
        else
        {
            button_getLevelsChest.interactable = true;
            button_getLevelsChest.GetComponent<Animator>().enabled = true;
            button_getLevelsChest.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 1f);
        }
    }
    private void InitButtonForStarsChest()
    {
        int starsChestNumber = DataManager.Get_NumberForStarsChest();
        int starsCounts = DataManager.Get_StarChest_StarsCounts();
        int neededStarsCounts = DataManager.chestsForStarsDatas[starsChestNumber].needetStarsCounts;

        text_starsChestStarsCounts.text = starsCounts.ToString() + "/" + neededStarsCounts.ToString();
        // Если звезд дял открытия сундука звезд недостаточно
        if (starsCounts < neededStarsCounts)
        {
            // То кнопка недоступна 
            button_getStarsChest.interactable = false;
            button_getStarsChest.GetComponent<Animator>().enabled = false;
            button_getStarsChest.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 0.6f);
        }
        else
        {
            button_getStarsChest.interactable = true;
            button_getStarsChest.GetComponent<Animator>().enabled = true;
            button_getStarsChest.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 1f);
        }
    }
}

