using Unity.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Transform healthBar;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bullet = collision.collider.GetComponent<Bullet>();

        if(bullet != null)
        {
            float x = healthBar.localScale.x - bullet.BulletData.damage / 100;
            if (x < 0) x = 0;

            healthBar.localScale = new Vector3(x, healthBar.localScale.y, healthBar.localScale.z);

            if (healthBar.localScale.x <= 0f) Destroy(gameObject);
        }
    }
}
