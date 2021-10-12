using System;
using UnityEngine;
using UnityEngine.UI;

public class Panel_EnergyAndCoinsDesc : MonoBehaviour
{
    private DataManager DataManager;
    private AudioManager AudioManager;
    private GameObject mainCanvas;
    public GameObject prefab_screenShop;

    public Text text_counCrystal;
    
    private void Start()
    {
        DataManager = DataManager.Instance;
        mainCanvas = GameObject.Find("MainCanvas").gameObject;
        AudioManager = AudioManager.Instance;
        OnChange_CoinsCount();

        DataManager.Instance.ONChangeCoinsCount += OnChange_CoinsCount;
    }

    private void OnChange_CoinsCount()
    {
        text_counCrystal.text = DataManager.GetCoinsCount().ToString();
    }

    public void OnClick_ButtonBuyCoins()
    {
        AudioManager.Play_ASource_1(1);
        Instantiate(prefab_screenShop, mainCanvas.transform);
        gameObject.transform.SetAsLastSibling();
    }


    private void OnDestroy()
    {
        DataManager.Instance.ONChangeCoinsCount -= OnChange_CoinsCount;
    }
}
