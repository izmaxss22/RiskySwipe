using System.Collections.Generic;
using UnityEngine;
using UssualMode;

public class SpikeItem : MainItem
{
    private GameManager gameManager;
    public SpriteRenderer SpriteRenderer;
    public Sprite spriteFor_disabled;
    public Sprite spriteFor_enabled;

    private bool isWasCollisionWithPlayerWithShield = false;

    public override void Init(Dictionary<string, string> specData = null)
    {
        gameManager = GameManager.Instance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            gameManager.OnCollisionWithSpike();
            if (gameManager.gameData.shieldIsActive)
            {
                isWasCollisionWithPlayerWithShield = true;
                SpriteRenderer.sprite = spriteFor_disabled;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Если было столкновение с песонажем но у него был щит то после того как персонаж отпрыгнет от шипов они снова станут опысными
        if (isWasCollisionWithPlayerWithShield)
        {
            isWasCollisionWithPlayerWithShield = false;
            SpriteRenderer.sprite = spriteFor_enabled;
        }
    }

}
