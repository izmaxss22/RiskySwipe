using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemsPlacer : MonoBehaviour
{
    public MediatorFor_ItemsMode MediatorFor_ItemsMode;
    private Dictionary<int, ItemForLevelEditor> itemsAddedInEditor;

    public GameObject canvasFor_itemsPlacer;

    private GameObject flyingItemGO;
    private int flyingItemId;

    private bool updateIsWork = false;

    public InputField inputFieldFor_MoveStep;
    public Text textFor_itemsZPos;
    private float itemsZPos = 0;


    #region СОБЫТИЯ НА КОТОРЫЕ РЕАГИРУЕТ ЭТОТ КЛАСС
    private void Start()
    {
        flyingItemGO = new GameObject();
    }

    public void InitMode(Dictionary<int, ItemForLevelEditor> itemsList)
    {
        this.itemsAddedInEditor = itemsList;
    }

    public void OnModeEnable()
    {
        updateIsWork = true;
        canvasFor_itemsPlacer.SetActive(true);
        if (flyingItemGO != null) flyingItemGO.SetActive(true);
    }

    public void OnModeDisable()
    {
        updateIsWork = false;
        canvasFor_itemsPlacer.SetActive(false);
        if (flyingItemGO != null) flyingItemGO.SetActive(false);
    }

    public void OnСlick_PickItemButton(int itemId)
    {
        Set_FlyingItem(itemId);
        flyingItemGO.SetActive(true);
    }
    #endregion

    #region РАБОТА В UPDATE
    private void Update()
    {
        if (updateIsWork)
        {
            FolowMouseFor_FlyingItem();
            if (Input.GetMouseButton(0) && !Check_CursorIsUnderUI()) UpdateWork_WorkWithItem();
            else UpdateWork_OtherWork();
        }
    }

    private void UpdateWork_WorkWithItem()
    {
        // Посылаеться луч из позиции обьекта под курсором
        Ray ray = new Ray(flyingItemGO.transform.position, flyingItemGO.transform.forward);
        RaycastHit raycastHit = new RaycastHit();
        Physics.Raycast(ray.origin, ray.direction, out raycastHit, 100);
        bool hitWithItem = raycastHit.collider.gameObject.name != "planeFor_TouchPositionDetecting";

        // Если зажата кнопка Q (для удаления обьекта)
        if (Input.GetKey(KeyCode.Q))
        {
            if (hitWithItem) DeleteItem(raycastHit.collider.gameObject);
        }
        else
        {
            // Если обьектов нету и луч столкнулся с плейном
            if (!hitWithItem) PutItem();
            // Иначе луч столкнулся с уже поставленным айтемом (проверки делаються с первым обьектом с которым столкнулся луч т.е первым по высоте)
            else
            {
                int hitedItemId = int.Parse(raycastHit.collider.gameObject.name);
                float hitedItemZPos = raycastHit.collider.gameObject.transform.position.z;
                // Если позиция равная (т.е нужно поставить там же где и он)
                if (hitedItemZPos == itemsZPos)
                {
                    // Если это одинаковые виды айтемо то ничего не происходит
                    if (hitedItemId == flyingItemId) return;
                    // Если это разные виды айтеом то замена одного другим
                    else if (hitedItemId != flyingItemId) ReplaceItem(raycastHit.collider.gameObject);
                }
                // Если позиции устанавливаемого обьека меньше позиции обьекта с которым стокнулся луч (т.е нужно поставить над ним)
                // Позиция меньше потому-что осчет глубины идет от - к + т.е -1 это ближе значит -2 будет стоять над ним
                else if (itemsZPos < hitedItemZPos) PutItem();
                // Если позиции устанавливаемого обьека меньше позиции обьекта с которым стокнулся луч (т.е нужно поставить под ним)
                // То ничего не делаю, потому-что до нижнего обьекта луч не проходит и значит проверки осуществляються по верхнему а не по нижнему
                else if (itemsZPos > hitedItemZPos) return;
            }
        }
    }

    private void UpdateWork_OtherWork()
    {
        // Изменение угла поворота устанавливаемых обьектов
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Rotate_FlyingItem(-90);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) Rotate_FlyingItem(90);
        // Изменение Z позиции устанавливаемых обьектов
        //else if (Input.GetKeyDown(KeyCode.UpArrow)) AddOrDecrease_ItemZPos(-0.5f);
        //else if (Input.GetKeyDown(KeyCode.DownArrow)) AddOrDecrease_ItemZPos(0.5f);
        // Изменение шага с которой передвигаеться мышка
        else if (Input.GetKeyDown(KeyCode.Alpha1)) Set_MoveStep(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) Set_MoveStep(0.5f);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) Set_MoveStep(0.25f);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) Set_MoveStep(0.125f);
        else if (Input.GetKeyDown(KeyCode.Tab)) Lock_PositionWithOne();
    }
    #endregion


    #region МЕТОДЫ РАБОТЫ С ЛЕТАЮЩИМ ПОД МЫШКОЙ АЙТЕМОМ
    private void Set_FlyingItem(int itemId)
    {
        if (flyingItemGO != null) Destroy(flyingItemGO);
        flyingItemGO = Instantiate(itemsAddedInEditor[itemId].itemGameObjectForPlacingOnMap);
        flyingItemGO.name = "flyingItem";
        flyingItemId = itemId;
        // Установка позиций по умолчанию для элементов пола
        if (itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_0.GetHashCode() ||
            itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_180.GetHashCode() ||
            itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_270.GetHashCode() ||
            itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_HALF_HORIZONTAL_1.GetHashCode() ||
            itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_HALF_HORIZONTAL_2.GetHashCode() ||
            itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_HALF_VERTICAL_1.GetHashCode() ||
            itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_HALF_VERTICAL_2.GetHashCode() ||
            itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_SMAL_CUBE.GetHashCode())
            Set_ItemsZPos(0);
        // Для всех остальных
        else Set_ItemsZPos(-0.5f);
    }

    private void FolowMouseFor_FlyingItem()
    {
        float moveStep = float.Parse(inputFieldFor_MoveStep.text);
        var groundPlane = new Plane(Vector3.forward, Vector3.zero);
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (groundPlane.Raycast(ray2, out float posstiton))
        {
            Vector3 worldPosition = ray2.GetPoint(posstiton);
            float x = (float)(Math.Round(worldPosition.x / moveStep) * moveStep);
            float y = (float)(Math.Round(worldPosition.y / moveStep) * moveStep);
            flyingItemGO.transform.position = new Vector3(x, y, -5);

            if (possitionIsLockedOnOne)
            {
                float x2 = (float)(Math.Round(worldPosition.x / 1) * 1);
                x2 += lockedPossitionModifier.x;
                float y2 = (float)(Math.Round(worldPosition.y / 1) * 1);
                y2 += lockedPossitionModifier.y;
                flyingItemGO.transform.position = new Vector3(x2, y2, -5);
            }
        }
    }

    private void Rotate_FlyingItem(float rotationAngle)
    {
        flyingItemGO.transform.Rotate(new Vector3(0, 0, rotationAngle));
    }
    #endregion

    private void Set_MoveStep(float moveStep)
    {
        inputFieldFor_MoveStep.text = moveStep.ToString();
    }

    private bool possitionIsLockedOnOne = false;
    private Vector3 lockedPossitionModifier;
    // Ограничение позиции для передвижения только на одну ячейку с заморозкой mouseStep
    // Например если я заморозил позиции когда mouseStep был 0.25 и айтем находился по 1.25 а по y на 1 то теперь передвижение по x всегда будет с учетом 0.25
    private void Lock_PositionWithOne()
    {
        if (possitionIsLockedOnOne) possitionIsLockedOnOne = false;
        else
        {
            possitionIsLockedOnOne = true;

            int wholeX = (int)Math.Floor(flyingItemGO.transform.position.x);
            int wholeY = (int)Math.Floor(flyingItemGO.transform.position.y);
            lockedPossitionModifier = new Vector3(flyingItemGO.transform.position.x - wholeX,
                                                  flyingItemGO.transform.position.y - wholeY,
                                                  -5);
        }
    }

    // Установить значение для Z позции устанавливаемых элеметов
    private void Set_ItemsZPos(float zPos)
    {
        itemsZPos = zPos;
        textFor_itemsZPos.text = itemsZPos.ToString();
    }

    // Добавить или отнять значение для Z позции устанавливаемых элеметов
    private void AddOrDecrease_ItemZPos(float value)
    {
        itemsZPos += value;
        textFor_itemsZPos.text = itemsZPos.ToString();
    }

    // Проверка на то находиться ли курсор на ui элементами
    private bool Check_CursorIsUnderUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    // Посыл сигнала с данными того какой обьект нужно установить на карту
    private void PutItem()
    {
        Debug.Log("put");

        Vector3 itemPos = new Vector3(
            flyingItemGO.transform.position.x,
            flyingItemGO.transform.position.y,
            itemsZPos);

        MediatorFor_ItemsMode.OnClick_PutItem_OnMap(
            flyingItemId,
            itemPos,
            flyingItemGO.transform.rotation);
    }

    // Посыл сигнал с данными того какой обьект нужно заменить на карте и на какой обьект его нужно заменить
    private void ReplaceItem(GameObject replacementItem)
    {
        Debug.Log("rep");

        Vector3 newItem_Pos = new Vector3(
           flyingItemGO.transform.position.x,
           flyingItemGO.transform.position.y,
           itemsZPos);

        MediatorFor_ItemsMode.OnClick_ReplaceItem_OnMap(
            replacementItem,
            flyingItemId,
            newItem_Pos,
            flyingItemGO.transform.rotation);
    }

    private void DeleteItem(GameObject GOForDelete)
    {
        // Удаление элемента происходит только если он находиться на той же позиции на которой устанавливаються новые обьекты
        // Так удобнее потому-что иначе бы удалялись все обьекты с разной высотой но расположенные на одной линии
        if (GOForDelete.transform.position.z == itemsZPos)
            MediatorFor_ItemsMode.OnClick_DeleteItem_OnMap(GOForDelete);
    }
}

