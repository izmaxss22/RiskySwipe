using UnityEngine;
using System.Collections.Generic;

public class AnalyticsEventsManager : MonoBehaviour
{
    public static AnalyticsEventsManager Instance;
    void Start()
    {
        Instance = this;
    }

    public void OnEvent_level_finished(int levelNumber, int nowStarsCount, float timePlayed, bool playerIsRevived)
    {
        string result;
        if (nowStarsCount == 3) result = "complete";
        else if (nowStarsCount == 0) result = "lose";
        else result = "not_full_complete";

        var dict = new Dictionary<string, object>();
        // Номер уровня
        dict.Add("level_number", levelNumber);
        // Прошел ли на 3 звезды (если нет то пусто)
        dict.Add("result", result);
        // Время сколько игрок играл в уровень
        dict.Add("time", timePlayed);
        // Количество звезд в конце
        dict.Add("progress", nowStarsCount);
        dict.Add("playerIsRevived", playerIsRevived);
        //AppMetrica.Instance.ReportEvent("level_finished", dict);
        //AppMetrica.Instance.SendEventsBuffer();
    }

    public void OnEvent_level_start(int levelNumber)
    {
        var dict = new Dictionary<string, object>();
        dict.Add("level_number", levelNumber);
        //AppMetrica.Instance.ReportEvent("level_start", dict);
        //AppMetrica.Instance.SendEventsBuffer();
    }

    public void OnEvent_buy_map_design(int skinNumber, int price)
    {
        var dict = new Dictionary<string, object>();
        dict.Add("skin_number", skinNumber);
        dict.Add("price", price);
        //AppMetrica.Instance.ReportEvent("buy_map_design", dict);
    }

    public void OnEvent_rate_us(int rate_result)
    {
        string result = "";
        if (rate_result == 0) result = "skiped";
        if (rate_result == 1) result = "never";
        if (rate_result == 2) result = "rate";

        var dict = new Dictionary<string, object>();
        dict.Add("rate_result", result);

        //AppMetrica.Instance.ReportEvent("rate_us", dict);
    }

    public void OnEvent_user_buy_shield()
    {
        //AppMetrica.Instance.ReportEvent("user_buy_shield");
    }

    public void OnEvent_user_use_shield(int levelNumber)
    {
        var dict = new Dictionary<string, object>();
        dict.Add("leve_number_where_use_shield", levelNumber);

        //AppMetrica.Instance.ReportEvent("user_use_shield");
    }

    public void OnEvent_rewarded_ad_watch(string name)
    {
        var dict = new Dictionary<string, object>();
        dict.Add("ad_name", name);

        //AppMetrica.Instance.ReportEvent("rewarded_ad_watch", dict);
    }

    public void OnEvent_change_sound_state(string state)
    {
        var dict = new Dictionary<string, object>();
        dict.Add("sound_state", state);

        //AppMetrica.Instance.ReportEvent("sound_state", dict);
    }

    public void OnEvent_open_level_chest(string chestLevel)
    {
        var dict = new Dictionary<string, object>();
        dict.Add("chestLevel", chestLevel);

        //AppMetrica.Instance.ReportEvent("open_level_chest", dict);
    }
    public void OnEvent_open_stars_chest(string chestLevel)
    {
        var dict = new Dictionary<string, object>();
        dict.Add("chestLevel", chestLevel);

        //AppMetrica.Instance.ReportEvent("open_stars_chest", dict);
    }
}


