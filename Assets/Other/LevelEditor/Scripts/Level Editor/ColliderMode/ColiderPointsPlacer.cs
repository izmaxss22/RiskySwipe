using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class ColiderPointsPlacer : MonoBehaviour
{
    public MediatorFor_ColiderMode ColiderManager;
    public GameObject prefabFor_folowingItem;
    private GameObject folowingItem;

    private bool updateIsWork = false;

    private void Start()
    {
        folowingItem = Instantiate(prefabFor_folowingItem, Vector3.zero, Quaternion.identity);
        folowingItem.name = "coliderFolowingItem";
    }

    void Update()
    {
        if (updateIsWork)
        {
            FolowItem();
            // Проверка на то находиться ли курсор над пользовательским интерфейсом (если да то нажатия не произойдет)
            bool cursorIsUnderUi = EventSystem.current.IsPointerOverGameObject();
            if (Input.GetMouseButtonDown(0) && !cursorIsUnderUi)
            {
                Ray ray = new Ray(folowingItem.transform.position, folowingItem.transform.forward);
                RaycastHit raycastHit = new RaycastHit();
                bool raycastState = Physics.Raycast(ray.origin, ray.direction, out raycastHit, 100);
                if (raycastState &&
                    raycastHit.collider.gameObject.tag == "itemForColliderCreating")
                    ColiderManager.OnItemDeletedFromMap(raycastHit.collider.gameObject);
                else
                {
                    Vector3 position = new Vector3(
                    folowingItem.transform.position.x,
                    folowingItem.transform.position.y,
                    -3.5f);
                    ColiderManager.OnItemPlacedOnMap(position);
                }
            }

            // Изменение шага с которой передвигаеться мышка
            else if (Input.GetKeyDown(KeyCode.Alpha1)) Set_MoveStep(1);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) Set_MoveStep(0.5f);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) Set_MoveStep(0.25f);
            else if (Input.GetKeyDown(KeyCode.Alpha4)) Set_MoveStep(0.125f);
        }
    }

    public void OnModeStateChanged(bool toEnable)
    {
        updateIsWork = toEnable;
        folowingItem.SetActive(toEnable);
    }

    private void FolowItem()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float x = (float)(Math.Round(mousePosition.x / moveStep) * moveStep);
        float y = (float)(Math.Round(mousePosition.y / moveStep) * moveStep);
        folowingItem.transform.position = new Vector3(x, y, -5);
    }

    private float moveStep = 1;
    private void Set_MoveStep(float moveStep)
    {
        this.moveStep = moveStep;
    }

}
