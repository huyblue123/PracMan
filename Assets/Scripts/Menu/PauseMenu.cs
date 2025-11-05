using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public GameObject pauseGame; 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame.SetActive(true);
            AudioManager.Instance.StopAllAudio();
            Time.timeScale = 0;
        }
    }
    public void ContinueButton()
    {
        pauseGame.SetActive(false);
        AudioManager.Instance.PlayGhostMoveSound();
        Time.timeScale = 1;
    }
    public void MainMenuButton()
    {
        if (GameManager.Instance != null) Destroy(GameManager.Instance.gameObject);
        if (AudioManager.Instance != null) Destroy(AudioManager.Instance.gameObject);

        SceneManager.LoadScene("MainMenu");
    }
}
