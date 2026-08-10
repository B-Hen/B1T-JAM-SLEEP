using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed, movement;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private RectTransform healthBar, healthBarParent, backgroundMask;
    [SerializeField] private MaskFollowPlayer maskFollowPlayer;
    [SerializeField] private float damageCooldown = 1.6f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> playerHitSFX;
    [SerializeField] private List<AudioClip> playerDeathSFX;

    private Vector2 screenBounds;
    private float playereHalfWidth, playerHalfHeight, damageTimer;
    private bool finished = false;
    private int healthBarAnimationID = -1;
    private Vector3 originalRotation;

    private void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        playereHalfWidth = spriteRenderer.bounds.extents.x;
        playerHalfHeight = spriteRenderer.bounds.extents.y;
        originalRotation = healthBarParent.localRotation.eulerAngles;

    }

    private void Update()
    {
        Movement();

        if(damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;

            if(damageTimer <= 0f)
            {
                animator.Play("Magoo-Idle");
            }
        }
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
        if (finished) return;

        if (damageTimer > 0f) return;

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if(enemy != null)
        {
            if (enemy.StopAllDamage) return;

            float x = healthBar.localScale.x - (enemy.EnemyData.attackPower / 100f);
            damageTimer = damageCooldown;

            animator.Play("Magoo-damange");

            if (x < 0)
            {
                x = 0f;
            }

            healthBar.localScale = new Vector3(x, healthBar.localScale.y, healthBar.localScale.z);

            int index = Random.Range(0, playerHitSFX.Count);
            audioSource.PlayOneShot(playerHitSFX[index], 0.5f);

            if (healthBarAnimationID != -1) LeanTween.cancel(healthBarAnimationID);
            LeanTween.scale(healthBarParent.gameObject, new Vector3(1.05f, 1.05f, 1.05f), 0.35f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong(1);
            LeanTween.rotate(healthBarParent.gameObject, new Vector3(0f, 0f, 1.5f), 0.35f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
            {
                LeanTween.rotate(healthBarParent.gameObject, new Vector3(0f, 0f, -1.5f), 0.35f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
                {
                    LeanTween.rotate(healthBarParent.gameObject, originalRotation, 0.35f).setEase(LeanTweenType.easeInOutQuad);
                });
            });

            if(healthBar.localScale.x == 0f)
            {
                finished = true;
                maskFollowPlayer.enabled = false;

                int playerDeathIndex = Random.Range(0, playerDeathSFX.Count);
                audioSource.Stop();
                audioSource.PlayOneShot(playerDeathSFX[index], 0.5f);

                LeanTween.value(
                    backgroundMask.gameObject,
                    backgroundMask.anchoredPosition,
                    new Vector2(160f, 90f),
                    1f
                    )
                    .setEase(LeanTweenType.easeInOutQuad)
                    .setOnUpdate((Vector2 position) =>
                    {
                        backgroundMask.anchoredPosition = position;
                    });
                LeanTween.scale(backgroundMask.gameObject, Vector3.one, 1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
                {
                    SceneManager.LoadScene(5);
                });
            }
        }
    }
}
