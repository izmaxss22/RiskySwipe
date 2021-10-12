using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_ShopAndSettingsButtons : MonoBehaviour
{
    private GameObject mainCanvas;
    private AudioManager AudioManager;


    public GameObject prefab_shopScreen;
    public GameObject prefab_settingsScreen;

    private void Start()
    {
        mainCanvas = GameObject.Find("MainCanvas").gameObject;
        gameObject.name = "panelFor_shopAndSettingsButtons";
        AudioManager = AudioManager.Instance;
    }

    public void OnClick_ShoopButton()
    {
        AudioManager.Play_ASource_1(1);
        Instantiate(prefab_shopScreen, mainCanvas.transform);
        GameObject.Find("panelFor_energyAndCoinsDesc").transform.SetAsLastSibling();
    }
    public void OnClick_SettingsButton()
    {
        AudioManager.Play_ASource_1(1);
        Instantiate(prefab_settingsScreen, mainCanvas.transform);
    }
}


