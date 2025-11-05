using UnityEngine;
using System.Collections;

public class PacmanPortal : MonoBehaviour
{
    [SerializeField] private Transform exitPoint;
    [SerializeField] private float teleportCooldown = 0.2f; // tránh kẹt

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerController>();
            if (player != null && player.canTeleport)
            {
                StartCoroutine(TeleportPlayer(player));
            }
        }
    }

    private IEnumerator TeleportPlayer(PlayerController player)
    {
        player.canTeleport = false;

        // Dịch chuyển
        player.transform.position = exitPoint.position;

        // Reset vận tốc
        if (player.TryGetComponent<Rigidbody2D>(out var rb))
            rb.linearVelocity = Vector2.zero;

        // Delay ngắn tránh trigger ngay lại
        yield return new WaitForSeconds(teleportCooldown);
        player.canTeleport = true;
    }
}
