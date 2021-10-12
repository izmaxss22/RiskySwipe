using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class Screen_Shop : MonoBehaviour
    // IUnityAdsListener
{
    public DataManager DataManager;
    private AudioManager AudioManager;
    private Panel_EnergyAndCoinsDesc Panel_EnergyAndCoinsDesc;

    public GameObject subScreen_purchases;
    public GameObject subScreen_mapDesign;

    public Image button_toSubScreen_purchases;
    public Image button_toSubScreen_mapDesign;
    public Button button_getCoins;

    public Sprite spriteFor_buttonToSubScreenPurchases_enabled;
    public Sprite spriteFor_buttonToSubScreenPurchases_disabled;
    public Sprite spriteFor_buttonToSubScreenMapDesigns_enabled;
    public Sprite spriteFor_buttonToSubScreenMapDesign_disabled;

    private void Start()
    {
        DataManager = DataManager.Instance;
        AudioManager = AudioManager.Instance;
        Panel_EnergyAndCoinsDesc = GameObject.Find("panelFor_energyAndCoinsDesc").GetComponent<Panel_EnergyAndCoinsDesc>();
        // Advertisement.AddListener(this);

        // //Если реклама не доступна то кнопка тоже
        // if (Advertisement.IsReady(DataManager.AdSettingsData.placmentName_getCoins) == false)
        // {
        //     button_getCoins.GetComponent<Animator>().enabled = false;
        //     button_getCoins.interactable = false;
        // }
    }

    private void OnDestroy()
    {
        // Advertisement.RemoveListener(this);
    }


    public void OnClick_ButtonClose()
    {
        AudioManager.Play_ASource_2(1.3f);
        gameObject.GetComponent<Animator>().SetTrigger("hide");
        Destroy(gameObject, 1.2f);
    }

    public void OnClick_GetCoinsButton()
    {
        AnalyticsEventsManager.Instance.OnEvent_rewarded_ad_watch("get_coins_in_shop");

        // Advertisement.Show(DataManager.AdSettingsData.placmentName_getCoins);
    }

    public void OnClick_ToSubScreenPurchasesButton()
    {
        button_toSubScreen_mapDesign.sprite = spriteFor_buttonToSubScreenMapDesign_disabled;
        subScreen_mapDesign.SetActive(false);
        button_toSubScreen_purchases.sprite = spriteFor_buttonToSubScreenPurchases_enabled;
        subScreen_purchases.SetActive(true);
    }

    public void OnClick_ToSubScreenMapDesignButton()
    {
        button_toSubScreen_purchases.sprite = spriteFor_buttonToSubScreenPurchases_disabled;
        subScreen_purchases.SetActive(false);
        button_toSubScreen_mapDesign.sprite = spriteFor_buttonToSubScreenMapDesigns_enabled;
        subScreen_mapDesign.SetActive(true);
    }

    private void GiveCoins_AffterAdWatch()
    {
        DataManager.SetCoinsCount(DataManager.GetCoinsCount() + 200);
    }

    #region КОЛБЭКИ РЕКЛАМЫ
    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == DataManager.AdSettingsData.placmentName_getCoins)
        {
            button_getCoins.GetComponent<Animator>().enabled = true;
            button_getCoins.interactable = true;
        }
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
        if (placementId == DataManager.AdSettingsData.placmentName_getCoins)
        {
            // // Если досмотрел рекламу
            // if (showResult == ShowResult.Finished)
            // {
            //     GiveCoins_AffterAdWatch();
            // }
        }
    }
    #endregion
}
