using UnityEngine;
using UnityEngine.UI;

public class Screen_Main : MonoBehaviour
{
    public DataManager DataManager;
    public RectTransform rectTransFor_chalengessProgressBarIcon;
    public Text chalengessProgressText;

    public GameObject MainCanvas;
    public GameObject prefab_screenForPickLevels;
    public GameObject prefab_panelForRewardsAndChest;

    void Start()
    {
        DataManager = DataManager.Instance;
        MainCanvas = GameObject.Find("MainCanvas").gameObject;

        InitChalengessModeProgressBar();
    }

    #region НАЖАТИЕ НА КНОПКИ
    public void OnClick_ChalengesModeButton()
    {
        var screen = Instantiate(prefab_screenForPickLevels, MainCanvas.transform);
        screen.transform.SetSiblingIndex(1);
        var panel = Instantiate(prefab_panelForRewardsAndChest, MainCanvas.transform);
        panel.transform.SetAsLastSibling();
        screen.GetComponent<ManagerFor_Screen_PickLevel>().CreateScreen_Default();
        Destroy(gameObject);
    }

    public void OnClick_ArcadeModeButton()
    {

    }

    public void OnClick_EndlessModeButton()
    {

    }
    #endregion

    private void InitChalengessModeProgressBar()
    {
        int starsCounts = 0;
        var array = DataManager.GetStarsCountsAchivmentOnLevels();
        foreach (var item in array)
        {
            starsCounts += item;
        }

        int maxStarsCounts = DataManager.COUNT_LEVELS * 3;
        chalengessProgressText.text = starsCounts.ToString() + "/" + maxStarsCounts.ToString();

        float starsRatio = (float)starsCounts / maxStarsCounts;
        float progressBarLenght = 758 * starsRatio;

        rectTransFor_chalengessProgressBarIcon.sizeDelta = new Vector2(
              progressBarLenght,
              rectTransFor_chalengessProgressBarIcon.sizeDelta.y);
    }

}
