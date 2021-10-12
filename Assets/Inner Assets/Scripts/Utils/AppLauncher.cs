using System.Collections;
using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Networking;

public class AppLauncher : MonoBehaviour
{
    private DataManager DataManager;
    public DataSynchronizerFor_GameUpdates DataSynchronizerFor_GameUpdates;

    private void Awake()
    {
        DataManager = DataManager.Instance;

        // DataManager.SetCoinsCount(7000);
        // DataManager.SetLastAvailableLevelNumber(49);

        Application.targetFrameRate = 60;
        DataSynchronizerFor_GameUpdates.Update_DatasStructures();
        InitAds();
        DefineLanguage();
        StartCoroutine(CheckInternetConnection());
        DataManager.Set_FirstGameLaunchState(false);
    }

    private bool isFocusWasLost;
    private void OnApplicationFocus(bool focus)
    {
        if (focus == false) isFocusWasLost = true;
        // Если фокус был потерян (это нужно чтобы при первом запуске этот код не запускался, т.к все уже было инициализировано в awake)
        else if (isFocusWasLost) InitAds();
    }
    

    // Инициализация рекламы
    private void InitAds()
    {
//         //TODO
//         bool testMode = false;
//         //Если реклама не готова
//         if (!Advertisement.IsReady())
//         {
// #if UNITY_ANDROID
//             Advertisement.Initialize(DataManager.AdSettingsData.gameId_googlePLay.ToString(), testMode);
// #endif
// #if UNITY_IOS
//             Advertisement.Initialize(DataManager.AdSettingsData.gameId_appStore.ToString(), testMode);
// #endif
//         }
    }

    //  Установка языка игры
    private void DefineLanguage()
    {
        // Если это первый запуск игры
        if (DataManager.Get_FirstGameLaunchState() == true)
        {
            DataManager.Set_Language(DataManager.LanguageIds.ENGLISH);
            //todo
            //// То язык устанавливаеться исходя из системного языка
            //if (Application.systemLanguage == SystemLanguage.Russian)
            //    DataManager.Set_Language(DataManager.LanguageIds.RUSSIAN);
            //else
            //DataManager.Set_Language(DataManager.LanguageIds.ENGLISH);
        }

        LocalizationManager.Read();

        if (DataManager.Get_Language() == DataManager.LanguageIds.ENGLISH.ToString())
            LocalizationManager.Language = "English";
        else
            LocalizationManager.Language = "Russian";
    }

    private IEnumerator CheckInternetConnection()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://google.com");
        yield return www.SendWebRequest();
        if (www.error != null)
        {
            DataManager.haveInternerConection = false;

        }
        else
        {
            DataManager.haveInternerConection = true;
        }
        yield break;
    }
}
