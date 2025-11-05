using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnStartGameButton()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene("SampleScene");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
