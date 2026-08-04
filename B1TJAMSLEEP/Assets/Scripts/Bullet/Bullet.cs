using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletData bulletData;

    private Vector3 direction;
    private float speed;
    private bool moveBullet = false;

    public BulletData BulletData
    {
        get { return bulletData; }
    }

    private void Update()
    {
        if (!moveBullet) return;

        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetData(Vector3 targetPosition, float speed, float duration, float angleOffset = 0f)
    {
        targetPosition.z = -Camera.main.transform.position.z;
        Vector3 worldTarget = Camera.main.ScreenToWorldPoint(targetPosition);
        direction = (worldTarget - transform.position).normalized;
        direction = Quaternion.Euler(0, 0, angleOffset) * direction;

        this.speed = speed;
        moveBullet = true;

        Destroy(gameObject, duration);
    }
}
