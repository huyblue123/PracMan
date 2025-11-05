using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Retry()
    {
        if (GameManager.Instance != null) Destroy(GameManager.Instance.gameObject);
        if (AudioManager.Instance != null) Destroy(AudioManager.Instance.gameObject);

        SceneManager.LoadScene("SampleScene");
    }
    public void LoadMainMenu()
    {
        if (GameManager.Instance != null) Destroy(GameManager.Instance.gameObject);
        if (AudioManager.Instance != null) Destroy(AudioManager.Instance.gameObject);

        SceneManager.LoadScene("MainMenu");
    }
}
