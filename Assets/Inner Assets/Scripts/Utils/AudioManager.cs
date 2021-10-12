using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private DataManager DataManager;

    public AudioSource aSource_1;
    public AudioSource aSource_2;
    public AudioSource aSource_3;
    public AudioSource aSource_wrong;
    public AudioSource aSource_levelScreen_pickLevel;
    public AudioSource aSource_levelScreen_switchLevelPage;

    public AudioSource aSource_pickedLevelScreen_start;
    public AudioSource aSource_pickedLevelScreen_switchLevel;
    public AudioSource aSource_pickedLevelScreen_buyShield;
    public AudioSource aSource_pickedLevelScreen_close;
    public AudioSource aSource_pickedLevelSceeen_showEnergyScreen;

    public AudioSource aSource_chestRewardsScreen_collect;
    public AudioSource aSource_reviveScreen_reviveButton;
    public AudioSource aSource_reviveScreen_closeButton;

    public AudioSource aSource_game_gameOver;
    //public AudioSource aSource_game_jump;
    //public AudioSource aSource_game_collectCoin;
    public AudioSource aSource_game_collectPoint;
    public AudioSource aSource_game_collectStar;
    public AudioSource aSource_game_completeLevel;
    public AudioSource aSource_game_collectCoin;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DataManager = DataManager.Instance;
    }

    public void Play_ASource_1(float pitch)
    {
        if (CanPlayAudio())
        {
            aSource_1.pitch = pitch;
            aSource_1.Play();
        }
    }
    public void Play_ASource_2(float pitch)
    {
        if (CanPlayAudio())
        {
            aSource_2.pitch = pitch;
            aSource_2.Play();
        }
    }
    public void Play_ASource_3(float pitch)
    {
        if (CanPlayAudio())
        {
            aSource_3.pitch = pitch;
            aSource_3.Play();
        }
    }
    public void Play_Wrong(float pitch)
    {
        if (CanPlayAudio())
        {
            aSource_wrong.pitch = pitch;
            aSource_wrong.Play();
        }
    }

    #region LEVEL SCREEN SOUNDS
    public void Play_LevelsScreen_OnClick_PickLevel()
    {
        if (CanPlayAudio()) aSource_levelScreen_pickLevel.Play();
    }

    public void Play_LevelsScreen_OnClick_SwithLevelPage(float pitch)
    {
        if (CanPlayAudio())
        {
            aSource_levelScreen_switchLevelPage.pitch = pitch;
            aSource_levelScreen_switchLevelPage.Play();
        }
    }
    #endregion

    #region PICKED LEVEL SCREEN SOUNDS
    public void Play_PickedLevelScreen_Start()
    {
        if (CanPlayAudio())
        {
            aSource_pickedLevelScreen_start.Play();
        }
    }
    public void Play_PickedLevelScreen_SwitchLevel()
    {
        if (CanPlayAudio())
        {
            aSource_pickedLevelScreen_switchLevel.Play();
        }
    }
    public void Play_PickedLevelScreen_CloseScreen()
    {
        if (CanPlayAudio())
        {
            aSource_pickedLevelScreen_close.Play();
        }
    }

    public void Play_PickedLevelScreen_BuyShield()
    {
        if (CanPlayAudio())
        {
            aSource_pickedLevelScreen_buyShield.Play();
        }
    }

    public void Play_PickedLevelSceeen_ShowEnergyScreen()
    {
        if (CanPlayAudio())
        {
            aSource_pickedLevelSceeen_showEnergyScreen.Play();
        }
    }

    #endregion

    public void Play_ChestRewardsScreen_Collect()
    {
        if (CanPlayAudio())
        {
            aSource_chestRewardsScreen_collect.Play();
        }
    }

    public void Play_ReviveScreen_ReviveButton()
    {
        if (CanPlayAudio())
        {
            aSource_reviveScreen_reviveButton.Play();
        }
    }

    public void Play_ReviveScreen_CloseButton()
    {
        if (CanPlayAudio())
        {
            aSource_reviveScreen_closeButton.Play();
        }
    }


    #region GAME_SOUNDS

    // КОСТЫЛЬ (чтобы состояние звука не загружалось во время игры из PPrefs  оно загружаються перед игрой здесь)
    private bool gameMuteStateForGame;
    public void Game_SetSoundsMuteState()
    {
        gameMuteStateForGame = CanPlayAudio();
    }
    //public void Play_Game_Jump()
    //{
    //    if (gameMuteStateForGame)
    //    {
    //        //aSource_game_jump.pitch = Random.Range(0.9f, 1.1f);
    //        aSource_game_jump.Play();
    //    }
    //}

    public void Play_Game_GameOver()
    {
        if (CanPlayAudio())
        {
            aSource_game_gameOver.Play();
        }
    }

    //public void Play_Game_CollectCoin()
    //{
    //    if (gameMuteStateForGame)
    //    {
    //        //aSource_game_collectCoin.pitch = Random.Range(0.9f, 1.1f);
    //        aSource_game_collectCoin.Play();
    //    }
    //}

    public void Play_Game_CollectPoint()
    {
        if (CanPlayAudio())
        {
            //aSource_game_collectPoint.pitch = Random.Range(0.9f, 1.1f);
            aSource_game_collectPoint.Play();
        }
    }

    public void Play_Game_CollectStar()
    {
        if (CanPlayAudio())
        {
            aSource_game_collectStar.Play();
        }
    }

    public void Play_Game_CompleteLevel()
    {
        if (CanPlayAudio())
        {
            aSource_game_completeLevel.Play();
        }
    }

    public void Play_Game_CollectCoin()
    {
        if (CanPlayAudio())
        {
            aSource_game_collectCoin.Play();
        }
    }

    #endregion

    private bool CanPlayAudio()
    {
        return !DataManager.Get_SoundIsMuteState();
    }
}
