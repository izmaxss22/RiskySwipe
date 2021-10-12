using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UssualMode;

public class GunItem : MainItem
{
    private GameManager gameManager;
    public GunSpecData specData;
    public Animator animator;
    public GameObject prefab_gunBullet;
    public Transform contForBullets;

    public SpriteRenderer spriteRenderer;
    public Sprite spriteFor_gun_disabled;
    public Sprite spriteFor_gun_enabled;

    private Quaternion bulletRotation = Quaternion.Euler(0, 0, 0);
    private Vector2 shootVector;

    public override void Init(Dictionary<string, string> specData = null)
    {
        contForBullets = GameObject.Find("contForItems").transform;
        gameManager = GameManager.Instance;
        this.specData.SetSpecDataValues(specData);
        SetShootVector();
        StartCoroutine(Shot_Taker());
    }

    private void SetShootVector()
    {
        switch (this.specData.shootVector)
        {
            case GunSpecData.ShootVector.left:
                bulletRotation = Quaternion.Euler(0, 0, 0);
                shootVector = new Vector2(-1, 0);
                break;
            case GunSpecData.ShootVector.top:
                bulletRotation = Quaternion.Euler(0, 0, 90);
                shootVector = new Vector2(0, 1);
                break;
            case GunSpecData.ShootVector.right:
                bulletRotation = Quaternion.Euler(0, 0, 0);
                shootVector = new Vector2(1, 0);
                break;
            case GunSpecData.ShootVector.down:
                bulletRotation = Quaternion.Euler(0, 0, 90);
                shootVector = new Vector2(0, -1);
                break;
        }
    }

    public IEnumerator Shot_Taker()
    {
        while (true)
        {
            yield return new WaitForSeconds(specData.shootPeriodicityInSecond);
            animator.SetTrigger("pulse");
            // Чтобы не было вибрации в окне возрождения
            if (gameManager.gameData.gameIsOvered == false)
            {
                MMVibrationManager.Haptic(HapticTypes.Failure);
            }
            Instantiate(prefab_gunBullet, transform.position, bulletRotation, contForBullets)
                .GetComponent<Gun_BulletItem>()
                .Init(shootVector);
        }
    }
}