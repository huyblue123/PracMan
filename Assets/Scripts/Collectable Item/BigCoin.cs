using UnityEngine;

public class BigCoin : MonoBehaviour
{
    public int scoreValue = 100;

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ActivePowerMode();
            ScoreManager.Instance.AddScore(scoreValue);

            // Thay vì Destroy, tắt object để trả về pool
            gameObject.SetActive(false);
        }
    }
}
