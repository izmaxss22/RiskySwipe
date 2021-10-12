using UnityEngine;
using UnityEngine.UI;

public class Item_ChestReward : MonoBehaviour
{
    public Image mainItemCont;
    public Text textFor_rewardCount;
    public GameObject[] iconsFor_reward;
    public Sprite[] spritesFor_rewardItemCont;

    public void CreateItem(DataManager.ChestsRewardsIds rewardId, int rewardCount)
    {
        mainItemCont.sprite = spritesFor_rewardItemCont[rewardId.GetHashCode()];
        textFor_rewardCount.text = rewardCount.ToString();
        iconsFor_reward[rewardId.GetHashCode()].gameObject.SetActive(true);
    }
}
