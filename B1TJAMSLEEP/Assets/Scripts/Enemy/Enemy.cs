using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Transform healthBar;

    [SerializeField] private float separationDistance = 0.5f;
    [SerializeField] private float separationStrength = 0.5f;
    [SerializeField] private float randomOffsetAmount = 0.05f;
    [SerializeField] private float playerStoppingDistance = 0.35f;
    [SerializeField] private float stoppingTime = 0.75f;

    private Transform player;
    private Vector3 randomOffset;
    private float stoppingTimer;
    private bool isStopping;

    public Transform Player
    {
        get { return player; }
        set
        {
            player = value;

            UpdateRandomOffset();
        }
    }

    public EnemyData EnemyData
    {
        get { return enemyData; }
    }

    private void Update()
    {
        if (player == null) return;

        if (isStopping)
        {
            stoppingTimer -= Time.deltaTime;

            if (stoppingTimer > 0f)
            {
                return;
            }

            isStopping = false;
        }

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distanceToPlayer <= playerStoppingDistance)
        {
            isStopping = true;
            stoppingTimer = stoppingTime;

            return;
        }

        Vector3 movement = Vector3.zero;
        movement += (player.position - transform.position).normalized;
        movement += GetSeparationForce();
        movement += randomOffset * 0.1f;
        movement.Normalize();
        transform.position += movement * enemyData.speed * Time.deltaTime;
    }

    private Vector3 GetSeparationForce()
    {
        Vector3 separation = Vector3.zero;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            separationDistance
        );

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.transform == transform)
                continue;

            Vector3 away = transform.position - enemy.transform.position;
            float distance = away.magnitude;

            if (distance > 0.001f)
            {
                float strength = 1f - (distance / separationDistance);
                separation += away.normalized * Mathf.Clamp01(strength);
            }
        }

        return separation * separationStrength;
    }

    private void UpdateRandomOffset()
    {
        randomOffset = new Vector3(
            Random.Range(-randomOffsetAmount, randomOffsetAmount),
            Random.Range(-randomOffsetAmount, randomOffsetAmount),
            0
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bullet = collision.collider.GetComponent<Bullet>();

        if (bullet != null)
        {
            float x = healthBar.localScale.x - (bullet.BulletData.damage / 100f);

            Destroy(bullet.gameObject);

            if (x < 0) x = 0;

            healthBar.localScale = new Vector3(x, healthBar.localScale.y, healthBar.localScale.z);

            if (healthBar.localScale.x <= 0f) Destroy(gameObject);
        }
    }
}
