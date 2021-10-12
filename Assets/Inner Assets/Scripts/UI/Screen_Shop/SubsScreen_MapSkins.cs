using UnityEngine;
using UnityEngine.UI;

public class SubsScreen_MapSkins : MonoBehaviour
{
    public DataManager DataManager;
    private AudioManager AudioManager;
    private Panel_EnergyAndCoinsDesc Panel_EnergyAndCoinsDesc;

    public Button[] mapSkin_buttons;
    public GameObject[] contsFor_priceItems;
    public Text[] texts_prices;
    public GameObject[] iconsFor_buttonWhenItemPicked;
    public GameObject[] iconsFor_buttonWhenItemNotPicked;

    public Sprite spriteFor_mapSkinButton_buy;
    public Sprite spriteFor_mapSkinButton_picked;
    public Sprite spriteFor_mapSkinButton_notPicked;

    private int[] pricesFor_MapSkins;
    private int[] statesFor_MapSkins;

    void Start()
    {
        DataManager = DataManager.Instance;
        AudioManager = AudioManager.Instance;

        Panel_EnergyAndCoinsDesc = GameObject.Find("panelFor_energyAndCoinsDesc")
            .GetComponent<Panel_EnergyAndCoinsDesc>();
        pricesFor_MapSkins = DataManager.MAP_SKIN_PRICES;
        statesFor_MapSkins = DataManager.Get_StatesForMapSkins();
        Init_MapSkinButtons();
    }

    private void Init_MapSkinButtons()
    {
        for (int i = 0; i < pricesFor_MapSkins.Length; i++)
        {
            switch (statesFor_MapSkins[i])
            {
                // Если скин в состоянии "ПОКУПКА"
                case 0:
                    mapSkin_buttons[i].GetComponent<Image>().sprite = spriteFor_mapSkinButton_buy;
                    contsFor_priceItems[i].SetActive(true);
                    texts_prices[i].text = pricesFor_MapSkins[i].ToString();
                    break;
                // Если скин в состоянии "ВЫБРАН"
                case 1:
                    mapSkin_buttons[i].GetComponent<Image>().sprite = spriteFor_mapSkinButton_picked;
                    iconsFor_buttonWhenItemPicked[i].SetActive(true);
                    break;
                // Если скин в состоянии "НЕ ВЫБРАН"
                case 2:
                    mapSkin_buttons[i].GetComponent<Image>().sprite = spriteFor_mapSkinButton_notPicked;
                    iconsFor_buttonWhenItemNotPicked[i].SetActive(true);
                    break;
                default:
                    break;
            }
            int buttonNumber = i;
            mapSkin_buttons[i].onClick.AddListener(() => OnClick_MapSkinButton(buttonNumber));
        }

    }

    private void OnClick_MapSkinButton(int buttonNumber)
    {
        switch (statesFor_MapSkins[buttonNumber])
        {
            // Если скин в состоянии "ПОКУПКА"
            case 0:
                // Если хватате денег на скин
                int coinsCount = DataManager.GetCoinsCount();
                if (coinsCount >= pricesFor_MapSkins[buttonNumber])
                {
                    AnalyticsEventsManager.Instance.OnEvent_buy_map_design(buttonNumber, pricesFor_MapSkins[buttonNumber]);

                    coinsCount -= pricesFor_MapSkins[buttonNumber];
                    DataManager.SetCoinsCount(coinsCount);
                    statesFor_MapSkins[buttonNumber] = 1;
                    DataManager.Set_StatesForMapSkins(statesFor_MapSkins);
                    mapSkin_buttons[buttonNumber].GetComponent<Image>().sprite = spriteFor_mapSkinButton_picked;
                    contsFor_priceItems[buttonNumber].SetActive(false);
                    iconsFor_buttonWhenItemPicked[buttonNumber].SetActive(true);
                }
                break;
            // Если скин в состоянии "ВЫБРАН"
            case 1:
                int pickedSkinsCounts = 0;
                foreach (var i in statesFor_MapSkins)
                {
                    if (i == 1) pickedSkinsCounts++;
                }
                // Если выбрано больше 1 скиннов (нельзя чтобы не было выбранно ни одного скина)
                if (pickedSkinsCounts > 1)
                {
                    statesFor_MapSkins[buttonNumber] = 2;
                    DataManager.Set_StatesForMapSkins(statesFor_MapSkins);
                    mapSkin_buttons[buttonNumber].GetComponent<Image>().sprite = spriteFor_mapSkinButton_notPicked;
                    iconsFor_buttonWhenItemPicked[buttonNumber].SetActive(false);
                    iconsFor_buttonWhenItemNotPicked[buttonNumber].SetActive(true);
                }
                break;
            // Если скин в состоянии "НЕ ВЫБРАН"
            case 2:
                statesFor_MapSkins[buttonNumber] = 1;
                DataManager.Set_StatesForMapSkins(statesFor_MapSkins);
                mapSkin_buttons[buttonNumber].GetComponent<Image>().sprite = spriteFor_mapSkinButton_picked;
                iconsFor_buttonWhenItemNotPicked[buttonNumber].SetActive(false);
                iconsFor_buttonWhenItemPicked[buttonNumber].SetActive(true);
                break;
            default:
                break;
        }
    }

}
