using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UssualMode;

public class Screen_Revive : MonoBehaviour
{
    public DataManager DataManager;
    private AudioManager AudioManager;

    private GameManager GameManager;
    private int levelNumber;
    private int beforeStarsCount;
    private int nowStarsCount;

    private int reviveEffectsCount;

    public Button button_reviveForAd;
    public Button button_reviveForEffects;
    public Image iconFor_buttonReviveForEffects;
    public Image contFor_reviveEffectsCounts;
    public Text text_reviveEffectsCounts;

    public Sprite spriteFor_buttonForReviveEffects_disabled;
    public Sprite spriteFor_iconFor_buttonForReviveEffects_disabled;
    public Sprite spriteFor_contForReviveEffectsCount_disabled;

    private void Awake()
    {
        DataManager = DataManager.Instance;
        AudioManager = AudioManager.Instance;
    }

    public void CreateScreen(GameManager gameManager, int levelNumber, int beforeStarsCount, int nowStarsCount)
    {
        GameManager = gameManager;
        this.levelNumber = levelNumber;
        this.beforeStarsCount = beforeStarsCount;
        this.nowStarsCount = nowStarsCount;

        reviveEffectsCount = DataManager.Get_ReviveEffectsCount();
        if (reviveEffectsCount == 0)
        {
            button_reviveForEffects.interactable = false;

            button_reviveForEffects.GetComponent<Image>().sprite = spriteFor_buttonForReviveEffects_disabled;
            iconFor_buttonReviveForEffects.sprite = spriteFor_iconFor_buttonForReviveEffects_disabled;
            contFor_reviveEffectsCounts.sprite = spriteFor_contForReviveEffectsCount_disabled;
        }

        // //Если реклама доступна то кнопка тоже
        // if (Advertisement.IsReady(DataManager.AdSettingsData.placmentName_rewardedVideo))
        // {
        //     button_reviveForAd.interactable = true;
        // }

        text_reviveEffectsCounts.text = reviveEffectsCount.ToString();
    }

    public void OnClick_ButtonReviveForAd()
    {
        AnalyticsEventsManager.Instance.OnEvent_rewarded_ad_watch("revive");
        AudioManager.Play_ReviveScreen_ReviveButton();
        // Advertisement.Show(DataManager.AdSettingsData.placmentName_rewardedVideo);
    }

    public void OnClick_ButtonReviveForEffect()
    {
        AdsShowingManager.OnShowReviveAd();
        AudioManager.Play_ReviveScreen_ReviveButton();
        reviveEffectsCount--;
        text_reviveEffectsCounts.text = reviveEffectsCount.ToString();
        DataManager.Set_ReviveEffectsCount(reviveEffectsCount);

        GetComponent<Animator>().SetTrigger("hide");
        Destroy(gameObject, 0.5f);
        GameManager.OnRevive();
    }

    public void OnClick_ButtonClose()
    {
        AudioManager.Play_ReviveScreen_CloseButton();
        WindowsManager.Instance.FromReviveScreenToPickedLevelsScreen(GameManager, gameObject, levelNumber, nowStarsCount, beforeStarsCount);
    }

    #region Кэлбэки рекламы
    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == DataManager.AdSettingsData.placmentName_rewardedVideo)
        {
            button_reviveForAd.interactable = true;
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
        // if (placementId == DataManager.AdSettingsData.placmentName_rewardedVideo)
        // {
        //     if (showResult == ShowResult.Skipped)
        //     {
        //         GetComponent<Animator>().SetTrigger("hide");
        //         Destroy(gameObject, 0.5f);
        //         GameManager.OnRevive();
        //     }
        //     // Если досмотрел рекламу
        //     if (showResult == ShowResult.Finished)
        //     {
        //         GetComponent<Animator>().SetTrigger("hide");
        //         Destroy(gameObject, 0.5f);
        //         GameManager.OnRevive();
        //     }
        // }
    }
    #endregion
}
