using System;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

namespace UssualMode
{
    public class GameItemsManager : MonoBehaviour
    {
        private GameManager GameManager;

        public GameObject levelItemsCont;
        private List<GameObject> createdLevelItems = new List<GameObject>();

        public GameObject contForColliders;
        public GameObject prefabForCollider;
        private List<GameObject> createdColliders = new List<GameObject>();

        public void Init()
        {
            GameManager = GameManager.Instance;
            CreateItems(GameManager.gameData.subLevelInProgressNumber);
            CreateColliders(GameManager.gameData.subLevelInProgressNumber);
            SetItemsSkin(GameManager.gameData.subLevelInProgressNumber);
            SetGameBkgPlanePos();
            SetBkgPlaneScale();
            SetGameBkgParticlesPos();
        }

        private void SetItemsSkin(int subLevelNumber)
        {
            GameObject game_bkg = GameObject.Find("gameBkgPlane");
            ParticleSystem particles = GameObject.Find("gameBkgParticles").GetComponent<ParticleSystem>();
            var skin = GameManager.gameData.itemsSkinsForEachSubLevel[subLevelNumber];
            DataManager.Instance.gameDesignData.levelItemsDesignData.SetItemsSkin(skin, game_bkg, particles);
        }

        private void SetGameBkgPlanePos()
        {
            GameObject game_bkg = GameObject.Find("gameBkgPlane");
            var planePos = Camera.main.transform.position;
            planePos.z = 0.2f;
            game_bkg.transform.position = planePos;
        }


        private void SetBkgPlaneScale()
        {
            //Создание, установка маштаба(исходя из размера экрана), установка положения
            var camera = Camera.main;
            float height = camera.pixelHeight;
            float width = camera.pixelWidth + 100;
            Vector3 leftSidePoint = Camera.main.ScreenToWorldPoint(new Vector3(0, 0.5f));
            Vector3 rightSidePoint = Camera.main.ScreenToWorldPoint(new Vector3(width, 0.5f));
            Vector3 topSidePoint = Camera.main.ScreenToWorldPoint(new Vector3(0.5f, height));
            Vector3 downSidePoint = Camera.main.ScreenToWorldPoint(new Vector3(0.5f, 0));
            float xScale = Math.Abs(rightSidePoint.x - leftSidePoint.x);
            float zScale = Math.Abs(topSidePoint.y - downSidePoint.y);
            var plane = GameObject.Find("gameBkgPlane");
            plane.transform.localScale = new Vector3(xScale, 1, zScale) / 9f;
        }

        private void SetGameBkgParticlesPos()
        {
            GameObject particles = GameObject.Find("gameBkgParticles");
            var pos = Camera.main.transform.position;
            pos.z = 0.2f;
            particles.transform.position = pos;

        }

        #region СОЗДАНИЕ АЙТЕМОВ
        private void CreateItems(int subLevelNumber)
        {
            var prefabs = DataManager.Instance.prefabsForGameItemsUsedInGameManager;
            var subLevelData = GameManager.gameData.subLevels[subLevelNumber];

            #region НАСТРОЙКА КАМЕРЫ
            CameraShaker cameraShaker = Camera.main.GetComponent<CameraShaker>();
            cameraShaker.enabled = false;

            Vector3 cameraPositons = subLevelData.GetCameraPos();
            Camera.main.transform.position = cameraPositons;
            Camera.main.orthographicSize = subLevelData.cameraScale;

            cameraShaker.RestPositionOffset = Camera.main.transform.position;
            cameraShaker.enabled = true;
            #endregion

            //  Создание обьектов всех припятсвий
            foreach (var item in subLevelData.items)
            {
                int itemId = item.itemId;
                Vector3 itemPosition = item.GetPositon();
                Quaternion itemRotation = item.GetRotation();

                // Создание айтема на карте
                GameObject itemGameObject = Instantiate(
                    prefabs[itemId],
                    itemPosition,
                    itemRotation,
                    levelItemsCont.transform);

                // Если у него есть скрипт то туда кидаються спец данные либо пустой обьект
                if (itemGameObject.TryGetComponent<MainItem>(out MainItem mainItem))
                {
                    mainItem.Init(item.specData);
                }

                createdLevelItems.Add(itemGameObject);
            }
        }

        private void DestroyItems()
        {
            foreach (var item in createdLevelItems)
                if (item != null)
                    Destroy(item);
            createdLevelItems.Clear();
        }
        #endregion

        #region СОЗДАНИЕ КОЛАЙДЕРОВ
        private void CreateColliders(int subLevelInProgressNumber)
        {
            var colliderDatas = GameManager.gameData.subLevels[subLevelInProgressNumber].collidersPoints;

            foreach (var collider in colliderDatas)
            {
                var colliderPoints = collider.GetPoints();
                GameObject colliderGO = Instantiate(
                    prefabForCollider,
                    new Vector3(0, 0, -0.5f),
                    Quaternion.identity,
                    contForColliders.transform);
                colliderGO.GetComponent<EdgeCollider2D>().points = colliderPoints.ToArray();
                createdColliders.Add(colliderGO);
            }
        }

        private void DestroyColliders()
        {
            foreach (var collider in createdColliders)
                Destroy(collider);
        }
        #endregion

        public IEnumerator DestroyItemForSwitchSubLevel(int subLevelNumber)
        {
            yield return new WaitForSeconds(0.5f);
            DestroyItems();
            DestroyColliders();
            CreateItems(subLevelNumber);
            CreateColliders(subLevelNumber);
            SetItemsSkin(subLevelNumber);
            SetGameBkgPlanePos();
            SetBkgPlaneScale();
            SetGameBkgParticlesPos();
            yield break;
        }

        public void OnCompleteSubLevel(int subLevelNumber)
        {
            StartCoroutine(DestroyItemForSwitchSubLevel(subLevelNumber));
        }
    }
}

