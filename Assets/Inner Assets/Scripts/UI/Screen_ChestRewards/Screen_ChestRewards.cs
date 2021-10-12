using UnityEngine;

public class Screen_ChestRewards : MonoBehaviour
{
    private AudioManager AudioManager;
    public DataManager DataManager;

    public GameObject prefabFor_itemChestReward;
    public GameObject contFor_itemsChestRewards;
    private bool screenForLevelChest = false;

    private void Awake()
    {
        DataManager = DataManager.Instance;
        AudioManager = AudioManager.Instance;
    }

    public void CreateScreen(bool screenForLevelChest)
    {
        this.screenForLevelChest = screenForLevelChest;
        // Сохдание окна наград для сундука уровней
        if (screenForLevelChest)
        {
            // Получения значения обо всех наград в сундуке за уровень
            int levelChestNumber = DataManager.Get_NumberForLevelChest();
            var rewards = DataManager.chestsForLevelsCompletingsData[levelChestNumber].rewards;
            // Создание иконки для каждой награды
            foreach (var item in rewards)
            {
                CreateItemForChestReward(item.rewardId, item.rewardCount);
            }
        }
        // Иначе создание окна наград для сундука звезд
        else
        {
            // Получения значения обо всех наград в сундуке за звезды
            int starChestNumber = DataManager.Get_NumberForStarsChest();
            var rewards = DataManager.chestsForStarsDatas[starChestNumber].rewards;
            // Создание иконки для каждой награды
            foreach (var item in rewards)
            {
                CreateItemForChestReward(item.rewardId, item.rewardCount);
            }
        }
    }

    // Создание икони награды
    private void CreateItemForChestReward(DataManager.ChestsRewardsIds rewardId, int rewardCount)
    {
        Instantiate(prefabFor_itemChestReward, contFor_itemsChestRewards.transform)
            .GetComponent<Item_ChestReward>().CreateItem(rewardId, rewardCount);

    }

    private bool getButtonIsPressed;
    // По нажатию кнопки забрать награду
    public void OnClick_ButtonGetRewards()
    {
        if (getButtonIsPressed == false)
        {
            AudioManager.Play_ChestRewardsScreen_Collect();
            getButtonIsPressed = true;

            // Выдача награды из сундука уровней
            if (screenForLevelChest)
            {
                InitGivingRewardForLevelsCompletingChest();
            }
            // Иначе выдача награды из сундука звезд
            else
            {
                InitGivingRewardForStartsChest();
            }

            // Закрытие
            GetComponent<Animator>().SetTrigger("hide");
            Destroy(gameObject, 0.5f);
            
            Panel_RewardsAndChest panelFor_RewardsAndChest = GameObject.Find("panelFor_RewardsAndChest").GetComponent<Panel_RewardsAndChest>();
            panelFor_RewardsAndChest.InitChestButtons();
        }
        // todo обновить количество щит в окне уровня
    }

    private void InitGivingRewardForLevelsCompletingChest()
    {
        int levelChestNumber = DataManager.Get_NumberForLevelChest();
        var rewards = DataManager.chestsForLevelsCompletingsData[levelChestNumber].rewards;
        // Выдача награды
        foreach (var item in rewards)
        {
            GiveReward(item.rewardId, item.rewardCount);
        }
        levelChestNumber++;
        DataManager.Set_NumberForLevelChest(levelChestNumber);
    }

    private void InitGivingRewardForStartsChest()
    {
        int starsChestNumber = DataManager.Get_NumberForStarsChest();
        var rewards = DataManager.chestsForStarsDatas[starsChestNumber].rewards;
        // Выдача награды
        foreach (var item in rewards)
        {
            GiveReward(item.rewardId, item.rewardCount);
        }

        // Установка нового количества собранных звезд
        int nowStarsCounts = DataManager.Get_StarChest_StarsCounts();
        int neededStarsCounts = DataManager.chestsForStarsDatas[starsChestNumber].needetStarsCounts;
        nowStarsCounts -= neededStarsCounts;
        DataManager.Set_StarChest_StarsCounts(nowStarsCounts);
        // Установка нового уровня
        starsChestNumber++;
        DataManager.Set_NumberForStarsChest(starsChestNumber);
    }

    private void GiveReward(DataManager.ChestsRewardsIds rewardId, int rewardCount)
    {
        switch (rewardId)
        {
            case DataManager.ChestsRewardsIds.COINS:
                DataManager.SetCoinsCount(DataManager.GetCoinsCount() + rewardCount);
                break;
            case DataManager.ChestsRewardsIds.SHIELD:
                DataManager.SetShieldEffectsCount(DataManager.Get_ShieldEffectsCount() + rewardCount);
                break;
            case DataManager.ChestsRewardsIds.REVIVE_EFFECT:
                DataManager.Set_ReviveEffectsCount(DataManager.Get_ReviveEffectsCount() + rewardCount);
                break;
        }
    }
}
