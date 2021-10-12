using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ColiderPickButtonsManager : MonoBehaviour
{
    public MediatorFor_ColiderMode ColiderModeManager;

    public GameObject contFor_pickButtons;
    public GameObject prefab_pickButton;
    public Sprite spriteFor_pickButton_selected;
    public Sprite spriteFor_pickButton_nonSelected;

    private int pickedColiderNumber = -1;
    private List<GameObject> array_createdPickColliderButtons = new List<GameObject>();

    #region СОБЫТИЯ НА КОТОРЫЕ РЕАГИРУЕТ ЭТОТ КЛАСС
    public void On_LevelChanged(int countButtonsForCreating)
    {
        DeleteAll_PickColidersButtons();
        CreateAll_PickColiderButtons(countButtonsForCreating);
        pickedColiderNumber = -1;
    }

    public void OnClick_PickColliderButton(int buttonNumber)
    {
        Unselect_LastPickColiderButton();
        Selects_PickColiderButton(buttonNumber);
    }

    public void OnClick_AddColliderButton()
    {
        Create_PickColiderButton();
    }

    public void OnClick_DeleteColiderButton(int buttonForDeletingNumber)
    {
        Delete_PickColiderButton(buttonForDeletingNumber);
    }

    #endregion

    #region ВНУТРЕНИЕ МЕТОДЫ
    #region МЕТОДЫ СОЗДАНИЯ КНОПОК
    private void CreateAll_PickColiderButtons(int countButtonsForCreating)
    {
        for (int i = 0; i < countButtonsForCreating; i++)
        {
            Create_PickColiderButton();
        }
    }

    private void Create_PickColiderButton()
    {
        int thisButtonNumber = array_createdPickColliderButtons.Count;
        Button pickButton = Instantiate(prefab_pickButton, contFor_pickButtons.transform).GetComponent<Button>();
        pickButton.onClick.AddListener(() => ColiderModeManager.OnClick_PickColliderButton(thisButtonNumber));
        array_createdPickColliderButtons.Add(pickButton.gameObject);
    }
    #endregion

    #region  МЕТОДЫ УДАЛЕНИЯ КНОПОК
    private void DeleteAll_PickColidersButtons()
    {
        for (int i = 0; i < array_createdPickColliderButtons.Count; i++)   
            Destroy(array_createdPickColliderButtons[i].gameObject);
        array_createdPickColliderButtons.Clear();
    }

    private void Delete_PickColiderButton(int buttonForDeletingNumber)
    {
        Destroy(array_createdPickColliderButtons[buttonForDeletingNumber]);
        array_createdPickColliderButtons.RemoveAt(buttonForDeletingNumber);
    }

    #endregion

    public void Selects_PickColiderButton(int buttonNumber)
    {
        array_createdPickColliderButtons[buttonNumber].GetComponent<Image>().sprite = spriteFor_pickButton_selected;
        pickedColiderNumber = buttonNumber;
    }

    public void Unselect_LastPickColiderButton()
    {
        // Если выбран хоть какой-нибудь колайдер
        if (pickedColiderNumber != -1)
        {
            // То его кнопка делаеться не выбранной
            array_createdPickColliderButtons[pickedColiderNumber].GetComponent<Image>().sprite = spriteFor_pickButton_nonSelected;

        }
    }
    #endregion

    public int Get_PickedColiderNumber()
    {
        return pickedColiderNumber;
    }


}
