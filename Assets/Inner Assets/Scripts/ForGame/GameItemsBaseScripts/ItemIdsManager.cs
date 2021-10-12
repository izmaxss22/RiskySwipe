using UnityEngine;

public class ItemIdsManager : MonoBehaviour
{
    public enum ItemsIds
    {
        #region АЙТЕМЫ ОБЫЧНЫГО РЕЖИМА
        USSUAL_MODE_WALL_0 = 0,
        USSUAL_MODE_WALL_90 = 1,
        USSUAL_MODE_WALL_180 = 2,
        USSUAL_MODE_WALL_270 = 3,
        USSUAL_MODE_WALL_HALF_HORIZONTAL_1 = 4,
        USSUAL_MODE_WALL_HALF_HORIZONTAL_2 = 5,
        USSUAL_MODE_WALL_HALF_VERTICAL_1 = 6,
        USSUAL_MODE_WALL_HALF_VERTICAL_2 = 7,

        USSUAL_MODE_FLOOR_0 = 8,
        USSUAL_MODE_FLOOR_90 = 9,
        USSUAL_MODE_FLOOR_180 = 10,
        USSUAL_MODE_FLOOR_270 = 11,
        USSUAL_MODE_FLOOR_HALF_HORIZONTAL_1 = 12,
        USSUAL_MODE_FLOOR_HALF_HORIZONTAL_2 = 13,
        USSUAL_MODE_FLOOR_HALF_VERTICAL_1 = 14,
        USSUAL_MODE_FLOOR_HALF_VERTICAL_2 = 15,
        USSUAL_MODE_FLOOR_SMAL_CUBE = 16,

        USSUAL_MODE_COIN = 17,
        USSUAL_MODE_SPIKE = 18,
        USSUAL_MODE_SPIKE_CUBE = 19,
        USSUAL_MODE_SPIKE_FOR_PRESSED = 20,
        USSUAL_MODE_GUN = 21,

        USUAL_MODE_POINT_SMALL_0 = 22,
        USUAL_MODE_POINT_SMALL_1 = 23,
        USUAL_MODE_POINT_SMALL_2 = 24,
        USUAL_MODE_POINT_SMALL_3 = 25,
        USUAL_MODE_POINT_SMALL_4 = 36,

        USUAL_MODE_POINT_MID_0 = 30,
        USUAL_MODE_POINT_MID_1 = 31,
        USUAL_MODE_POINT_MID_2 = 32,
        USUAL_MODE_POINT_MID_3 = 33,
        USUAL_MODE_POINT_MID_4 = 34,

        USUAL_MODE_POINT_BIG_0 = 26,
        USUAL_MODE_POINT_BIG_1 = 27,
        USUAL_MODE_POINT_BIG_2 = 28,
        USUAL_MODE_POINT_BIG_3 = 29,
        USUAL_MODE_POINT_BIG_4 = 35,

        USUAL_MODE_DANGER_OBSTACLE_RECT_0 = 38,
        USUAL_MODE_DANGER_OBSTACLE_RECT_1 = 39,
        USUAL_MODE_DANGER_OBSTACLE_RECT_2 = 40,
        USUAL_MODE_DANGER_OBSTACLE_RECT_3 = 41,
        USUAL_MODE_DANGER_OBSTACLE_RECT_4 = 42,
        USUAL_MODE_DANGER_OBSTACLE_SLIIM_RECT_0 = 43,
        USUAL_MODE_DANGER_OBSTACLE_SLIIM_RECT_1 = 44,
        USUAL_MODE_DANGER_OBSTACLE_SLIIM_RECT_2 = 45,
        USUAL_MODE_DANGER_OBSTACLE_SLIIM_RECT_3 = 46,
        USUAL_MODE_DANGER_OBSTACLE_SLIIM_RECT_4 = 47,

        LAST_NUMBER = 49
        #endregion
    }

    public ItemsIds ItemsId;

    #region ДОП ЗНАЧЕНИЯ ДЛЯ ПОНИМАНИЯ ТОГО ЧТО ЭТО ЗА ЭЛЕМЕНТ
    [Tooltip("Значение для описания того нужно ли элемент поомечать как для выполнения на уровне " +
        "ч(статические припятсвия не должны выполняться соотвественно их нету в количестве элементов для выполнения)")]
    public bool itemIsForCompleting = false;
    [Tooltip("Сколько даеться очков за выполнения уровня")]
    public int itemForCompletePointsCounts;
    #endregion
}
