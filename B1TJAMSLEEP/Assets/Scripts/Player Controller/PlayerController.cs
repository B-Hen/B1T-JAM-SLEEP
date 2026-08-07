using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed, movement;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private RectTransform healthBar;

    private Vector2 screenBounds;
    private float playereHalfWidth, playerHalfHeight;

    private void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        playereHalfWidth = spriteRenderer.bounds.extents.x;
        playerHalfHeight = spriteRenderer.bounds.extents.y;

    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.W))
            transform.localPosition += new Vector3(0f, movement, 0f) * (speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            transform.localPosition -= new Vector3(movement, 0f, 0f) * (speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            transform.localPosition -= new Vector3(0f, movement, 0f) * (speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.localPosition += new Vector3(movement, 0f, 0f) * (speed * Time.deltaTime);

        float clampX = Mathf.Clamp(transform.localPosition.x, -screenBounds.x + playereHalfWidth, screenBounds.x - playereHalfWidth);
        float clampY = Mathf.Clamp(transform.localPosition.y, -screenBounds.y + playerHalfHeight, screenBounds.y - playerHalfHeight);
        transform.localPosition = new Vector3(clampX, clampY, transform.localPosition.z);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if(enemy != null)
        {
            float x = healthBar.localScale.x - (enemy.EnemyData.attackPower / 100f);

            if (x < 0)
            {
                Debug.Log("Player has died");
                x = 0f;
            }

            healthBar.localScale = new Vector3(x, healthBar.localScale.y, healthBar.localScale.z);
        }
    }
}
