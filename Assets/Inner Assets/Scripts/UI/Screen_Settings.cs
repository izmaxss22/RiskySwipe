using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Settings : MonoBehaviour
{
    public DataManager DataManager;
    private AudioManager AudioManager;

    public Text textFor_soundState;

    private void Start()
    {
        DataManager = DataManager.Instance;
        AudioManager = AudioManager.Instance;

        if (DataManager.Get_SoundIsMuteState())
            textFor_soundState.text = LocalizationManager.Localize("SnSettings.SoundOff");
        else
            textFor_soundState.text = LocalizationManager.Localize("SnSettings.SoundOn");
    }

    public void OnClickButtonBack()
    {
        AudioManager.Play_ASource_2(1.3f);
        GetComponent<Animator>().SetTrigger("hide");
    }

    public void OnAnimHideEnd()
    {
        Destroy(gameObject);

    }

    public void ChangeLanguage()
    {
        AudioManager.Play_ASource_2(2f);

        if (DataManager.Get_Language() == DataManager.LanguageIds.ENGLISH.ToString())
        {
            LocalizationManager.Language = "Russian";
            DataManager.Set_Language(DataManager.LanguageIds.RUSSIAN);
        }
        else
        {
            LocalizationManager.Language = "English";
            DataManager.Set_Language(DataManager.LanguageIds.ENGLISH);
        }
        Set_TextLanguageFor_OffSoundButton();
    }

    public void OnClickButtonOffSounds()
    {
        AudioManager.Play_ASource_2(2f);

        // Если звук выключен то включаю его
        if (DataManager.Get_SoundIsMuteState())
        {
            AnalyticsEventsManager.Instance.OnEvent_change_sound_state("not_muted");
            DataManager.Set_SoundIsMuteState(false);
        }
        else
        {
            AnalyticsEventsManager.Instance.OnEvent_change_sound_state("muted");

            DataManager.Set_SoundIsMuteState(true);
        }
        Set_TextLanguageFor_OffSoundButton();
    }

    private void Set_TextLanguageFor_OffSoundButton()
    {
        // Если звук выключен то включаю его
        if (DataManager.Get_SoundIsMuteState())
            textFor_soundState.text = LocalizationManager.Localize("SnSettings.SoundOff");
        else
            textFor_soundState.text = LocalizationManager.Localize("SnSettings.SoundOn");
    }
}
