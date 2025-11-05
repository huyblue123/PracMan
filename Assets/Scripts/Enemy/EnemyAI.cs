using Pathfinding;
using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private AIPath path;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    public Transform player;
    public Transform spawnPoint;

    private bool isRespawning = false;
    private bool isEaten = false;

    [SerializeField] private float normalSpeed = 4f;
    [SerializeField] private float eatenSpeed = 10f;

    [SerializeField] private AudioManager audioManager;

    private bool isGhostMoveSoundPlaying = false;
    private bool isGhostScaredSoundPlaying = false;

    private void Start()
    {
        path = GetComponent<AIPath>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Nếu player null hoặc đang chết
        if (player == null || !player.gameObject.activeInHierarchy)
        {
            path.canMove = false;

            // Dừng toàn bộ âm ghost
            if (isGhostMoveSoundPlaying || isGhostScaredSoundPlaying)
            {
                isGhostMoveSoundPlaying = false;
                isGhostScaredSoundPlaying = false;
            }
            return;
        }
        else
        {
            path.canMove = true;
        }

        // Nếu ghost đang respawn
        if (isRespawning)
        {
            path.destination = spawnPoint.position;

            if (Vector2.Distance(transform.position, spawnPoint.position) < 0.3f)
                StartCoroutine(Respawn());

            return;
        }

        // Khi Pacman ăn BigCoin → PowerMode
        if (GameManager.Instance.isPowerMode)
        {
            if (!isEaten)
            {
                col.isTrigger = true;
                spriteRenderer.color = Color.blue;

                // Ghost chạy ngược hướng player
                Vector2 dir = (transform.position - player.position).normalized;
                path.destination = transform.position + (Vector3)dir * 3f;

                // Chỉ đổi sound khi chưa phát âm scared
                if (!isGhostScaredSoundPlaying)
                {
                    
                    audioManager.PlayGhostScared();
                    isGhostScaredSoundPlaying = true;
                    isGhostMoveSoundPlaying = false;
                }
            }
        }
        else // PowerMode kết thúc
        {
            if (!isEaten)
            {
                col.isTrigger = false;
                spriteRenderer.color = Color.white;
                path.destination = player.position;

                // Chỉ đổi sound khi chưa phát âm move
                if (!isGhostMoveSoundPlaying)
                {
                    audioManager.PlayGhostMoveSound();
                    isGhostMoveSoundPlaying = true;
                    isGhostScaredSoundPlaying = false;
                }
            }
        }
    }

    public void OnEaten()
    {
        isEaten = true;
        isRespawning = true;
        spriteRenderer.color = Color.gray;
        col.isTrigger = false;
        path.maxSpeed = eatenSpeed;
        path.destination = spawnPoint.position;

        // Dừng âm khi bị ăn
        if (isGhostMoveSoundPlaying || isGhostScaredSoundPlaying)
        {
            isGhostMoveSoundPlaying = false;
            isGhostScaredSoundPlaying = false;
        }
    }

    private IEnumerator Respawn()
    {
        spriteRenderer.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(1.5f);

        // Reset lại trạng thái ghost
        isEaten = false;
        isRespawning = false;
        spriteRenderer.enabled = true;
        col.enabled = true;
        col.isTrigger = false;
        path.maxSpeed = normalSpeed;
        spriteRenderer.color = Color.white;

        // Phát lại âm di chuyển khi hồi sinh xong
        audioManager.StopAllAudio();
        audioManager.PlayGhostMoveSound();
        isGhostMoveSoundPlaying = true;
        isGhostScaredSoundPlaying = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isEaten && other.CompareTag("Player") && GameManager.Instance.isPowerMode)
        {
            OnEaten();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();
            if (player != null && !GameManager.Instance.isPowerMode)
            {
                player.Die();

                // Pracman chết → tắt âm ghost
                audioManager.StopAllAudio();
                isGhostMoveSoundPlaying = false;
                isGhostScaredSoundPlaying = false;
            }
        }
    }

    public void ResetGhostSound()
    {
        isGhostMoveSoundPlaying = false;
        isGhostScaredSoundPlaying = false;
    }
}
