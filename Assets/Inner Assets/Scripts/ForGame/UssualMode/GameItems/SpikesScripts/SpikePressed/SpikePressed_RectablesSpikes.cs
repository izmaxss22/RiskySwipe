using UnityEngine;
using UssualMode;

public class SpikePressed_RectablesSpikes : MainItem
{
    private GameManager gameManager;

    public SpikePressed_MainItem SpikePressed_MainItem;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            gameManager.OnCollisionWithSpikeForPressed();
        }
    }
}
