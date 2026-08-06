using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Transform healthBar;

    [SerializeField] private float separationDistance = 0.10f;
    [SerializeField] private float separationStrength = 0.15f;
    [SerializeField] private float randomOffsetAmount = 0.05f;

    private Transform player;
    private Vector3 randomOffset;

    public Transform Player
    {
        get { return player; }
        set
        {
            player = value;

            randomOffset = new Vector3(
                Random.Range(-randomOffsetAmount, randomOffsetAmount),
                Random.Range(-randomOffsetAmount, randomOffsetAmount),
                0
            );
        }
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        direction += randomOffset * 0.1f;
        Vector3 separation = Vector3.zero;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            separationDistance
        );

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.transform != transform)
            {
                Vector3 away = transform.position - enemy.transform.position;
                float distance = away.magnitude;

                if (distance > 0.001f)
                {
                    float strength = 1f - (distance / separationDistance);
                    separation += away.normalized * Mathf.Clamp01(strength);
                }
            }
        }

        direction += separation * separationStrength;

        direction.Normalize();

        transform.position += direction * enemyData.speed * Time.deltaTime;
    }

private void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bullet = collision.collider.GetComponent<Bullet>();

        if(bullet != null)
        {
            float x = healthBar.localScale.x - bullet.BulletData.damage / 100;
            if (x < 0) x = 0;

            healthBar.localScale = new Vector3(x, healthBar.localScale.y, healthBar.localScale.z);

            if (healthBar.localScale.x <= 0f) Destroy(gameObject);

            Destroy(bullet.gameObject);
        }
    }
}
