using UnityEngine;
using System.Collections;

public class Screen_Rate : MonoBehaviour
{
    public DataManager DataManager;
    private AudioManager AudioManager;

    public GameObject buttonSkip;
    public GameObject buttonNotcieLater;


    void Start()
    {
        DataManager = DataManager.Instance;

        AudioManager = AudioManager.Instance;

        // Если окно еще не вызывалось
        if (DataManager.Get_CountCallsRateScreen() == 0)
        {
            DataManager.Set_CountCallsRateScreen(1);
            buttonNotcieLater.SetActive(true);
        }
        else
        {
            DataManager.Set_CountCallsRateScreen(2);
            buttonSkip.SetActive(true);
        }
    }

    public void OnClickButtonNotectLater()
    {
        AnalyticsEventsManager.Instance.OnEvent_rate_us(0);
        AudioManager.Play_ASource_1(1f);
        gameObject.GetComponent<Animator>().SetTrigger("hide");
    }
    public void OnClickButtonSkip()
    {
        AnalyticsEventsManager.Instance.OnEvent_rate_us(1);
        AudioManager.Play_ASource_1(1f);
        gameObject.GetComponent<Animator>().SetTrigger("hide");
    }

    public void OnAnimHideEnd()
    {
        Destroy(gameObject);
    }

    public void OnClickButtonRate()
    {
        AnalyticsEventsManager.Instance.OnEvent_rate_us(2);

        DataManager.Set_CountCallsRateScreen(2);
        AudioManager.Play_ASource_1(1.8f);
        gameObject.GetComponent<Animator>().SetTrigger("hide");
        if (Application.platform == RuntimePlatform.Android)
            Application.OpenURL(DataManager.GAME_LINK_IN_GOOGLE_PLAY);
        // todo поведение на ipad
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            Application.OpenURL(DataManager.GAME_LINK_IN_APP_STORE);

    }
}
