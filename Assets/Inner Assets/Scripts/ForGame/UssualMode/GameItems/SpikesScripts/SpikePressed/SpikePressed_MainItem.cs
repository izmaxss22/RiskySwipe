using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UssualMode;

public class SpikePressed_MainItem : MainItem
{
    private GameManager gameManager;
    private bool isPressed;

    public override void Init(Dictionary<string, string> specData = null)
    {
        gameManager = GameManager.Instance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            // При столкновении с персонажем начинаеться выдвижение шипов если нету шита и шипу еще не выдвинуты
            if (isPressed == false && gameManager.gameData.shieldIsActive == false)
            {
                StartCoroutine(OnPress());
            }
        }
    }

    public IEnumerator OnPress()
    {
        isPressed = true;
        GetComponent<Animator>().SetTrigger("onPress");
        yield return new WaitForSeconds(2.5f);
        GetComponent<Animator>().SetTrigger("hide");
        yield return new WaitForSeconds(0.3f);
        isPressed = false;
    }
}
