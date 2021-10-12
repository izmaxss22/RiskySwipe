
using UnityEngine;

public static class AdsShowingManager
{
    private static int lastCountForShowAd = 5;

    public static void OnStartLevel()
    {
        lastCountForShowAd--;
    }

    public static void OnShowReviveAd()
    {
        lastCountForShowAd += 3;
    }

    public static void OnInterestialAdWasShowed()
    {
        lastCountForShowAd = 5;
    }

    public static bool GetCanShowAdState()
    {
        // Количество и проверка на 5 уровень
        if (DataManager.Instance.GetLastAvailableLevelNumber() >= 4 &&
           lastCountForShowAd <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
