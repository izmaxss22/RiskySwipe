using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor_Manager : MonoBehaviour
{
    public LevelEditor_ItemsList itemsForLevelEditor;
    public DataManager LevelEditorDataManager;

    public MediatorFor_ItemsMode mediatorFor_ItemsMode;
    public MediatorFor_ColiderMode mediatorFor_ColiderMode;


    //todoeditor удалить
    //private SubLevelData SubLevelData = new SubLevelData();
    private OneLevelData SubLevelData = new OneLevelData();

    private int pickedLevelNumber;

    private Camera mainCamera;
    public GameObject buttonsPanel;

    private int activeEditorModeNumber = -1;
    private enum editorModeNumbers
    {
        ItemMode,
        ColliderMode
    }

    public GameObject prefab_cubeForGridMaking_0;
    public GameObject prefab_cubeForGridMaking_1;
    public GameObject contFor_editorGrid;
    public InputField inputField_specifiedLevelNumberForLoad;

    public InputField inputFieldFor_playerXPos;
    public InputField inputFieldFor_playerYPos;
    public InputField inputFieldFor_playerZPos;
    public InputField inputFieldForLevelSpeed;

    public InputField inputFieldForAllLevelsCounts;

    void Start()
    {
        mainCamera = Camera.main;
        DrawGrid();
        //todoeditor
        // Обнуление уровня (чтобы прошел проверку при вызове LoadLevel()
        //SubLevelData.levelNumber_ForDewelop = -1;
        LoadSubLevelData(0);
    }

    private float lookSpeedH = 2f;
    private float lookSpeedV = 2f;
    private float zoomSpeed = 7f;
    private float dragSpeed = 60f;
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            mainCamera.transform.Translate(
                -Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed,
                -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed,
                0);
        }
        else if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
        {
            // Чтобы приближение поля на срабатывало при промотке канваса выбора элементов
            if (Input.mousePosition.y > 350)
            {
                mainCamera.orthographicSize += -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            }
        }
    }

    private void DrawGrid()
    {
        Vector3 position = new Vector3(0, 0, 1);
        for (int x = 0; x < 50; x++)
        {
            for (int y = 1; y < 50; y++)
            {
                position.x = x;
                position.y = y;

                if ((x + y) % 2 == 0) Instantiate(prefab_cubeForGridMaking_0,
                    position,
                    Quaternion.identity,
                    contFor_editorGrid.transform);


                else
                {
                    Instantiate(
                  prefab_cubeForGridMaking_1,
                  position,
                  Quaternion.identity,
                  contFor_editorGrid.transform);
                }
            }
        }
    }

    private void Change_EditorMode(int modeNumber)
    {
        if (activeEditorModeNumber != modeNumber)
        {
            // Выключение прошлого режима
            switch (activeEditorModeNumber)
            {
                case (int)editorModeNumbers.ItemMode:
                    mediatorFor_ItemsMode.OnModeDisable();
                    break;
                case (int)editorModeNumbers.ColliderMode:
                    mediatorFor_ColiderMode.OnModeDisable();
                    break;
            }
            // Включение нового режима
            switch (modeNumber)
            {
                case (int)editorModeNumbers.ItemMode:
                    mediatorFor_ItemsMode.OnModeEnable();
                    break;
                case (int)editorModeNumbers.ColliderMode:
                    mediatorFor_ColiderMode.OnModeEnable();
                    break;
            }
            activeEditorModeNumber = modeNumber;
        }
    }

    private void SaveSubLevelData(int levelNumberForSave)
    {
        int levelNumber = levelNumberForSave;
        Vector3 cameraPos = mainCamera.transform.position;
        float cameraScale = mainCamera.orthographicSize;
        Vector3 playerPos = new Vector3(
            float.Parse(inputFieldFor_playerXPos.text),
            float.Parse(inputFieldFor_playerYPos.text),
            float.Parse(inputFieldFor_playerYPos.text));
        var speed = float.Parse(inputFieldForLevelSpeed.text);
        var itemsDatas = mediatorFor_ItemsMode.GetPlacedItems();
        var colidersDatas = mediatorFor_ColiderMode.GetCreatedColliderPoints();

        new NewLevelSaving().SaveLevel(levelNumber, cameraPos, cameraScale, playerPos, speed, itemsDatas, colidersDatas);
    }

    private bool LoadSubLevelData(int levelNumber)
    {
        bool levelIsLoaded = false;
        // Если уровень отличаеться от того что сейчас и уровень не меньше 0
        if (true)
        {
            var levelData = new NewLevelSaving().LoadLevel(levelNumber);
            mainCamera.transform.position = levelData.GetCameraPos();
            mainCamera.orthographicSize = levelData.cameraScale;
            inputFieldFor_playerXPos.text = levelData.GetPlayerPos().x.ToString();
            inputFieldFor_playerYPos.text = levelData.GetPlayerPos().y.ToString();
            inputFieldFor_playerZPos.text = "-0.5";

            var speed = levelData.levelSpeed;
            var defSpeed = 3.5f;
            inputFieldForLevelSpeed.text = speed > 0 ? speed.ToString() : defSpeed.ToString();

            inputField_specifiedLevelNumberForLoad.text = levelNumber.ToString();

            mediatorFor_ItemsMode.InitMode(levelData, itemsForLevelEditor.GetItems());
            mediatorFor_ColiderMode.InitMode(levelData);
            levelIsLoaded = true;
        }
        return levelIsLoaded;
    }

    #region Методы нажатия на кнопки
    public void OnClick_ShowButtonsPanelButton()
    {
        buttonsPanel.SetActive(!buttonsPanel.activeSelf);
    }

    // Загрузить уровень на +1
    public void OnClick_LoadNextLevelButton()
    {
        if (LoadSubLevelData(pickedLevelNumber + 1))
        {
            pickedLevelNumber++;
        }
    }

    // Загрузить уровень на -1
    public void OnClick_LoadPreviousLevelButton()
    {
        if (LoadSubLevelData(pickedLevelNumber - 1))
        {
            pickedLevelNumber--;
        }
    }

    // Загрузить указанный уровень
    public void OnClick_LoadSpecifiedLevelButton()
    {
        int levelForLoadNumber = int.Parse(inputField_specifiedLevelNumberForLoad.text);
        if (LoadSubLevelData(levelForLoadNumber))
        {
            pickedLevelNumber = levelForLoadNumber;
        }
    }

    // Сохранить уровень который сейчас редактируеться
    public void OnClick_SaveLevelDatasButton()
    {
        SaveSubLevelData(pickedLevelNumber);
    }

    public void OnClickSaveAllLevels()
    {
        int levelsCounts = int.Parse(inputFieldForAllLevelsCounts.text);

        if (levelsCounts == 0)
        {
            Debug.LogError("УКАЗАТЬ В ОКНЕ СПРАВА СКОЛЬКО ВСЕГО УРОВНЕЙ В ИГРЕ");
        }
        else
        {
            for (int i = 0; i < levelsCounts; i++)
            {
                LoadSubLevelData(i);
                SaveSubLevelData(i);
            }
        }
    }

    // Переключение режимов
    public void OnCLick_ButtonPickEditorMode(int modeNumber)
    {
        Change_EditorMode(modeNumber);
    }
    #endregion

    private class NewLevelSaving
    {
        #region СОХРАНЕНИЕ
        public void SaveLevel(
            int levelNumber,
            Vector3 cameraPos,
            float cameraScale,
            Vector3 playerPos,
            float levelSpeed,
            List<GameObject> placedItems,
            List<List<Vector2>> collidersPoints
            )
        {
            var items = GetItems(placedItems);
            int pointsCountsForLevelCompleting = GetPointsCountsForCompleteLevel(placedItems);
            var collidersPoints1 = GetColliderPoints(collidersPoints);

            var itemForSaving = new OneLevelData
                (
                cameraPos,
                cameraScale,
                levelSpeed,
                playerPos,
                pointsCountsForLevelCompleting,
                items,
                collidersPoints1
                );
            SerealizationManager.SaveObjectToSerealizedFile<OneLevelData>(
                itemForSaving,
                SerealizationManager.SavingType.StreamingAssets,
                "NewLevelsFormat/" + levelNumber.ToString() + ".data");

            Debug.Log(levelNumber + " IS SAVED");

        }

        private List<OneLevelData.Item> GetItems(List<GameObject> placedItems)
        {
            List<OneLevelData.Item> items = new List<OneLevelData.Item>();
            foreach (var item in placedItems)
            {
                var itemIdsManager = item.GetComponent<ItemIdsManager>();
                bool itemIsHaveSpecData = item.TryGetComponent(out MainSpecData specDataComp);
                Dictionary<string, string> specData = itemIsHaveSpecData ? specDataComp.GetSpecDataValues() : null;


                int itemId = itemIdsManager.ItemsId.GetHashCode();
                //Рандомизация элементов стен и пола(делаються с разным поворотом), потому - что в редакторе для создания уровней
                //создаються только с поворотом 0 градусов(потому - что так проще делать)
                if (itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_0.GetHashCode())
                {
                    var randomNumber = Random.Range(0, 4);
                    switch (randomNumber)
                    {
                        case 0:
                            itemId = ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_0.GetHashCode();
                            break;
                        case 1:
                            itemId = ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_180.GetHashCode();
                            break;
                        case 2:
                            itemId = ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_270.GetHashCode();
                            break;
                        case 3:
                            itemId = ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_90.GetHashCode();
                            break;
                    }
                }
                else if (itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_WALL_0.GetHashCode())
                {
                    var randomNumber = Random.Range(0, 4);
                    switch (randomNumber)
                    {
                        case 0:
                            itemId = ItemIdsManager.ItemsIds.USSUAL_MODE_WALL_0.GetHashCode();
                            break;
                        case 1:
                            itemId = ItemIdsManager.ItemsIds.USSUAL_MODE_WALL_90.GetHashCode();
                            break;
                        case 2:
                            itemId = ItemIdsManager.ItemsIds.USSUAL_MODE_WALL_180.GetHashCode();
                            break;
                        case 3:
                            itemId = ItemIdsManager.ItemsIds.USSUAL_MODE_WALL_270.GetHashCode();
                            break;
                    }
                }

                items.Add(new OneLevelData.Item
                    (
                    item.transform.position,
                    item.transform.rotation,
                    item.transform.localScale,
                    itemId,
                    itemIsHaveSpecData,
                    specData
                    ));
            }
            return items;
        }

        private int GetPointsCountsForCompleteLevel(List<GameObject> placedItems)
        {
            int pointsCounts = 0;
            foreach (var item in placedItems)
            {
                var itemIdsManager = item.GetComponent<ItemIdsManager>();
                if (itemIdsManager.itemIsForCompleting)
                {
                    pointsCounts += itemIdsManager.itemForCompletePointsCounts;
                }
            }
            return pointsCounts;
        }

        private List<OneLevelData.ColliderPoints> GetColliderPoints(List<List<Vector2>> collidersPoints)
        {
            List<OneLevelData.ColliderPoints> colliderPoints1 = new List<OneLevelData.ColliderPoints>();
            // Перебор созданных колайдеров
            foreach (var colliderPoints in collidersPoints)
            {
                List<Vector2> currentColliderPoints = new List<Vector2>();
                // Перебор колайдера 
                foreach (var colliderPoint in colliderPoints)
                {
                    // Добавление точек этого колайдера в список
                    currentColliderPoints.Add(new Vector2(colliderPoint.x, colliderPoint.y));
                }
                // Добавление списка точек этого колайдера как новый обьект колайдера
                colliderPoints1.Add(new OneLevelData.ColliderPoints(currentColliderPoints));
            }

            return colliderPoints1;
        }
        #endregion

        #region ЗАГРУЗКА УРОВНЯ
        public OneLevelData LoadLevel(int levelNumber)
        {
            string filePath = "NewLevelsFormat/" + levelNumber.ToString() + ".data";
            if (File.Exists(Application.streamingAssetsPath + "/" + filePath))
            {
                var level = SerealizationManager.LoadConstantSerealizedObject<OneLevelData>(filePath);
                RegenerateItemIdForWallAndFloors(level.items);
                return level;
            }
            else
            {
                return new OneLevelData();
            }
        }

        private void RegenerateItemIdForWallAndFloors(List<OneLevelData.Item> items)
        {
            //Рандомизация элементов стен и пола(делаються с разным поворотом), потому - что в редакторе для создания уровней
            //создаються только с поворотом 0 градусов(потому - что так проще делать)
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_180.GetHashCode())
                {
                    items[i].itemId = ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_0.GetHashCode();
                }
                else if (items[i].itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_270.GetHashCode())
                {
                    items[i].itemId = ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_0.GetHashCode();
                }
                else if (items[i].itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_90.GetHashCode())
                {
                    items[i].itemId = ItemIdsManager.ItemsIds.USSUAL_MODE_FLOOR_0.GetHashCode();
                }

                else if (items[i].itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_WALL_180.GetHashCode())
                {
                    items[i].itemId = ItemIdsManager.ItemsIds.USSUAL_MODE_WALL_0.GetHashCode();
                }
                else if (items[i].itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_WALL_270.GetHashCode())
                {
                    items[i].itemId = ItemIdsManager.ItemsIds.USSUAL_MODE_WALL_0.GetHashCode();
                }
                else if (items[i].itemId == ItemIdsManager.ItemsIds.USSUAL_MODE_WALL_90.GetHashCode())
                {
                    items[i].itemId = ItemIdsManager.ItemsIds.USSUAL_MODE_WALL_0.GetHashCode();
                }
            }

        }
        #endregion
    }
}
