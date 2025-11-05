using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    public LayerMask wallLayer;

    private Vector2 direction = Vector2.zero;
    private Vector2 nextDirection = Vector2.zero;

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private Animator animator;

    public bool canTeleport = true;
    private bool isMoving = false; // kiểm tra có đang di chuyển hay không

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        GetInput();

        if (nextDirection != Vector2.zero)
        {
            ChangeDirection();
        }

        rb.linearVelocity = direction * speed;

        // Cập nhật Animator
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);

        // Xử lý âm thanh liên tục
        if (direction != Vector2.zero)
        {
            if (!isMoving)
            {
                isMoving = true;
                AudioManager.Instance.PlayPracManMoveLoop();
            }
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                AudioManager.Instance.StopPracManMoveLoop();
            }
        }
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) nextDirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) nextDirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) nextDirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) nextDirection = Vector2.right;
    }

    private bool isWall(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.55f, wallLayer);
        return hit.collider != null;
    }

    private void ChangeDirection()
    {
        if (!isWall(nextDirection))
        {
            direction = nextDirection;
            nextDirection = Vector2.zero;
        }
    }

    // --- LIVE MANAGER --- //
    public void Die()
    {
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        // Tạm dừng di chuyển
        rb.linearVelocity = Vector2.zero;

        // Giữ loop âm thanh liên tục: không StopPracManMoveLoop ở đây
        // AudioManager.Instance.StopPracManMoveLoop();
        AudioManager.Instance.StopAllAudio();
        sr.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.OnPlayerDie();
    }
}
