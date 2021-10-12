using UnityEngine;
using UssualMode;

public class Gun_BulletItem : MonoBehaviour
{
    private GameManager gameManager;
    public Rigidbody2D rb;
    public MeshRenderer meshRenderer;
    public Collider2D itemCollider;
    private Vector2 bulletVector;
    private readonly float bulletSpeed = 15.5F;
    private int colliderCollisiosCount = 0;
    private bool bulletIsActive = true;

    public void Init(Vector2 bulletVector)
    {
        gameObject.name = "gunBullet";
        gameManager = GameManager.Instance;
        this.bulletVector = bulletVector;
    }

    private void FixedUpdate()
    {
        if (bulletIsActive)
        {
            rb.velocity = bulletVector * bulletSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string name = collision.gameObject.name;

        // Если пуля столкнулся с колайдером стены
        if (name == "prefabFor_collider(Clone)")
        {
            // Срабатывает только после одного столкновения
            // (потому-что при спавне пули она сразу же сталкиваеться с этим колайдером но уничтожатсья не должна)
            if (colliderCollisiosCount == 1)
            {
                DestroyBullet();
            }
            else
            {
                colliderCollisiosCount++;
            }
        }
        // Но это не монетка не большое очко не маленькое очко и не другая пуля
        else if (name != "point" && name != "coin" && name != "gunBullet")
        {
            if (collision.gameObject.CompareTag("player"))
            {
                gameManager.OnColisionWithGunBullet();
            }
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {
        bulletIsActive = false;
        itemCollider.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        meshRenderer.enabled = false;
        Destroy(gameObject, 0.5f);
    }
}
