using System.Collections;
using UnityEngine;
using UssualMode;

public class WindowsManager : MonoBehaviour
{
    public static WindowsManager Instance;

    public Transform mainCanvas;
    public GameObject prefabForReviveScreen;
    public GameObject prefabForPickedLevelScreen;
    public GameObject prefabForPanelForEnergyAndCoins;
    public GameObject prefabForPanelForRewardsAndChest;

    private void Awake()
    {
        Instance = this;
    }

    public void FromGameToReviveScreen(
        GameManager gameManager,
        int playedLevelNumber,
        int achivmentsStarsCountsInThisSession,
        int achivmentsStarsCountsInLastSession)
    {

        GameObject go = Instantiate(prefabForReviveScreen, mainCanvas.transform);
        go.GetComponent<Screen_Revive>().CreateScreen(
            gameManager,
            playedLevelNumber,
            achivmentsStarsCountsInLastSession,
            achivmentsStarsCountsInThisSession);
    }

    public void FromGameToPickedLevelScreen(
        GameObject gameManager,
        int playedLevelNumber,
        int achivmentsStarsCountsInThisSession,
        int achivmentsStarsCountsInLastSession
        )
    {
        StartCoroutine(FromGameToPickedLevelScreenCoroutine(
            gameManager,
            playedLevelNumber,
            achivmentsStarsCountsInThisSession,
            achivmentsStarsCountsInLastSession
            ));
    }

    private IEnumerator FromGameToPickedLevelScreenCoroutine(
        GameObject gameManager,
        int playedLevelNumber,
        int achivmentsStarsCountsInThisSession,
        int achivmentsStarsCountsInLastSession)
    {
        yield return new WaitForSeconds(0.8f);
        GameObject go = Instantiate(prefabForPanelForEnergyAndCoins, mainCanvas.transform);
        go.name = "panelFor_energyAndCoinsDesc";

        GameObject go2 = Instantiate(prefabForPanelForRewardsAndChest, mainCanvas.transform);
        go2.name = "panelFor_RewardsAndChest";

        GameObject go1 = Instantiate(prefabForPickedLevelScreen, mainCanvas.transform);
        go1.GetComponent<Screen_PickedLevel>().CreateScreen_FromGameScreen(
            playedLevelNumber,
            achivmentsStarsCountsInLastSession,
            achivmentsStarsCountsInThisSession
            );

        yield return new WaitForSeconds(0.7f);
        Destroy(gameManager);
        yield break;
    }

    public void FromReviveScreenToPickedLevelsScreen(
        GameManager gameManager,
        GameObject reviveScreen,
        int playedLevelNumber,
        int achivmentsStarsCountsInThisSession,
        int achivmentsStarsCountsInLastSession)
    {
        Destroy(gameManager.gameObject);
        GameObject go = Instantiate(prefabForPanelForEnergyAndCoins, mainCanvas.transform);
        go.name = "panelFor_energyAndCoinsDesc";

        GameObject go2 = Instantiate(prefabForPanelForRewardsAndChest, mainCanvas.transform);
        go2.name = "panelFor_RewardsAndChest";

        GameObject go1 = Instantiate(prefabForPickedLevelScreen, mainCanvas.transform);
        go1.GetComponent<Screen_PickedLevel>().CreateScreen_FromGameScreen(
            playedLevelNumber,
            achivmentsStarsCountsInLastSession,
            achivmentsStarsCountsInThisSession);

        reviveScreen.GetComponent<Animator>().SetTrigger("hide");
        Destroy(reviveScreen, 0.7f);
    }
}