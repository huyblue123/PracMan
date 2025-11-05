using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue = 10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // + điểm
            // GameManager.Instance.AddScore(1);

            // Ẩn coin (pool)
            ScoreManager.Instance.AddScore(scoreValue);
            gameObject.SetActive(false);
        }
    }
}
