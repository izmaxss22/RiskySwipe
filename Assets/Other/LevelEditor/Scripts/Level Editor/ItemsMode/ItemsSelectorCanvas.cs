
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsSelectorCanvas : MonoBehaviour
{
    private Dictionary<int, ItemForLevelEditor> itemsAddedInEditor;
    public MediatorFor_ItemsMode MediatoFor_ModeItemCreator;

    public GameObject selectorCanvas;
    private int numberFor_pickedButton = -1;

    public GameObject contForButtons;
    public GameObject prefabFor_pickButton;
    public GameObject prefabFor_itemIcon_InPickButton;

    public Sprite spriteFor_pickButton_usual;
    public Sprite spriteFor_pickButton_picked;

    private List<GameObject> list_PickButtons = new List<GameObject>();

    #region СОБЫТИЯ НА КОТОРЫЕ РЕАГИРУЕТ ЭТОТ КЛАСС
    public void InitMode(Dictionary<int, ItemForLevelEditor> itemsList)
    {
        this.itemsAddedInEditor = itemsList;
        Delete_PickButtons();
        Create_PickButtons();
    }

    public void OnModeEnable()
    {
        selectorCanvas.SetActive(true);
    }

    public void OnModeDisable()
    {
        selectorCanvas.SetActive(false);
        numberFor_pickedButton = -1;
    }

    public void OnClick_PickItemButton(int itemId, int buttonNumber)
    {
        if (Select_PickButton(buttonNumber)) MediatoFor_ModeItemCreator.OnСlick_PickItemButton(itemId);
    }
    #endregion

    // Удаление созданых кнопок
    private void Delete_PickButtons()
    {
        foreach (GameObject button in list_PickButtons) Destroy(button);
        list_PickButtons.Clear();
    }

    // Создание кнопок из списка
    private void Create_PickButtons()
    {
        int itemsCount = 0;
        foreach (var item in itemsAddedInEditor)
        {
            // Создание кнопки
            GameObject pickButton = Instantiate(
                prefabFor_pickButton,
                Vector3.zero,
                Quaternion.identity,
                contForButtons.transform);
            pickButton.GetComponent<Image>().sprite = spriteFor_pickButton_usual;
            int buttonNumber = itemsCount;
            int itemId = item.Value.itemGameObjectForPlacingOnMap.GetComponent<ItemIdsManager>().ItemsId.GetHashCode();
            pickButton.GetComponent<Button>().onClick.AddListener(() => OnClick_PickItemButton(itemId, buttonNumber));
            // Создание иконки у кнопки
            GameObject itemIcon_inPickButton = Instantiate(
                prefabFor_itemIcon_InPickButton,
                Vector3.zero,
                Quaternion.identity,
                pickButton.transform);
            itemIcon_inPickButton.GetComponent<Image>().sprite = item.Value.iconForSelectorCanvas;

            list_PickButtons.Add(pickButton);
            itemsCount++;
        }
    }

    private bool Select_PickButton(int buttonNumber)
    {
        // Если нажал на не одну и туже кнопку
        if (buttonNumber != numberFor_pickedButton)
        {
            // Если до этого уже была нажата кнопка то её иконка становиться обычной
            if (numberFor_pickedButton != -1) list_PickButtons[numberFor_pickedButton].GetComponent<Image>().sprite = spriteFor_pickButton_usual;
            list_PickButtons[buttonNumber].GetComponent<Image>().sprite = spriteFor_pickButton_picked;

            numberFor_pickedButton = buttonNumber;
            return true;
        }
        return false;
    }
}
