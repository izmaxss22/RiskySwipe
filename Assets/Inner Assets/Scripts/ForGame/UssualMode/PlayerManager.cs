using System;
using System.Collections;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace UssualMode
{
    public class PlayerManager : MonoBehaviour
    {
        private GameManager GameManager;

        public GameObject playerPrefab;
        private GameObject createdPlayer;
        private Rigidbody2D playerRb;

        private Vector3 startMousePosition;
        private float clickDeltaY;
        private float clickDeltaX;
        private bool swipeIsMaded;

        private Vector3 velocity;
        private float speed = 3.5f;
        private bool playerIsCanMove = true;

        public void Init()
        {
            GameManager = GameManager.Instance;
            CreatePlayer(0);
        }

        void Update()
        {
            if (createdPlayer != null && playerIsCanMove)
            {
                playerRb.velocity = velocity * speed;

                MovingWithWASD();

                if (Input.GetMouseButtonDown(0))
                    startMousePosition = Input.mousePosition;
                else if (Input.GetMouseButton(0)
                    //&& swipeIsMaded
                    )
                {
                    MovingPlayerWithSwipe();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    swipeIsMaded = false;
                    //velocity = new Vector3(0, 0, 0);
                }
            }
        }

        private void CreatePlayer(int subLevelNumber)
        {
            createdPlayer = Instantiate(playerPrefab, transform);
            createdPlayer.tag = "player";
            createdPlayer.transform.position = GameManager.gameData.subLevels[subLevelNumber].GetPlayerPos();
            speed = GameManager.gameData.subLevels[subLevelNumber].levelSpeed;
            playerRb = createdPlayer.GetComponent<Rigidbody2D>();
        }

        private void DestroyPlayer()
        {
            Destroy(createdPlayer);
        }

        private void SetActivePlayerPhysics(bool isActive)
        {
            if (isActive)
            {
                playerIsCanMove = true;
                createdPlayer.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                createdPlayer.GetComponent<PolygonCollider2D>().enabled = true;
            }
            else
            {
                playerIsCanMove = false;
                createdPlayer.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                createdPlayer.GetComponent<PolygonCollider2D>().enabled = false;
            }

            velocity = Vector3.zero;
        }

        private IEnumerator CompleteSubLevel()
        {
            SetActivePlayerPhysics(false);
            yield return new WaitForSeconds(0.5F);
            DestroyPlayer();
            CreatePlayer(GameManager.gameData.subLevelInProgressNumber);
            SetActivePlayerPhysics(true);
            yield break;
        }

        #region МЕТОДЫ ПЕРЕДВИЖЕНИЯ ПЕРСОНАЖА
        private void MovingPlayerWithSwipe()
        {
            clickDeltaY = startMousePosition.y - Input.mousePosition.y;
            clickDeltaY = -clickDeltaY;

            clickDeltaX = startMousePosition.x - Input.mousePosition.x;
            clickDeltaX = -clickDeltaX;
            swipeIsMaded = true;
            // Если был свайп вверх
            if (clickDeltaY >= 80)
            {
                startMousePosition = Input.mousePosition;
                velocity = new Vector3(0, 1, 0);
                MMVibrationManager.Haptic(HapticTypes.SoftImpact);
            }

            else if (clickDeltaY <= -80)
            {
                startMousePosition = Input.mousePosition;
                velocity = new Vector3(0, -1, 0);
                MMVibrationManager.Haptic(HapticTypes.SoftImpact);
            }

            else if (clickDeltaX >= 80)
            {
                startMousePosition = Input.mousePosition;
                velocity = new Vector3(1, 0, 0);
                MMVibrationManager.Haptic(HapticTypes.SoftImpact);
            }

            else if (clickDeltaX <= -80)
            {
                startMousePosition = Input.mousePosition;
                velocity = new Vector3(-1, 0, 0);
                MMVibrationManager.Haptic(HapticTypes.SoftImpact);
            }

            else swipeIsMaded = false;
        }

        private void MovingWithWASD()
        {
            if (Input.GetKey(KeyCode.W))
            {
                velocity = new Vector3(0, 1, 0);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                velocity = new Vector3(-1, 0, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                velocity = new Vector3(0, -1, 0);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                velocity = new Vector3(1, 0, 0);
            }
        }
        #endregion

        #region СОБЫТИЯ ИЗ ГМ
        public void OnRevive()
        {
            SetActivePlayerPhysics(true);
        }

        public void OnCompleteLevel()
        {
            SetActivePlayerPhysics(false);
            createdPlayer.GetComponent<Animator>().SetTrigger("levelComplete");
        }

        public void OnCompleteSubLevel()
        {
            StartCoroutine(CompleteSubLevel());
        }

        public void OnGameOver()
        {
            SetActivePlayerPhysics(false);
        }
        #endregion
    }
}