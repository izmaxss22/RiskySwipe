using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UssualMode;

public class CoinItem : MainItem
{
    private GameManager gameManager;

    public override void Init(Dictionary<string, string> specData = null)
    {
        gameObject.name = "coin";
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            AudioManager.Instance.Play_Game_CollectCoin();
            MMVibrationManager.Haptic(HapticTypes.SoftImpact);
            Destroy(gameObject);
            gameManager.OnCollectCoin();
        }
    }
}
