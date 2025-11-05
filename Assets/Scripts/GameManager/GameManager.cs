using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Power Mode Setting")]
    public bool isPowerMode = false;
    public float powerScaredDuration = 2f;

    [Header("Player Setting")]
    public GameObject playerPrefab;
    public Transform[] playerSpawnPoint;
    private GameObject currentPlayer;

    [Header("Player's Lives Setting")]
    [SerializeField] private int lives = 3;
    private Vector3 lastDeathPosition;
    [SerializeField] private GameObject gameOverMenu;

    private void Awake()
    {

        Instance = this;
    }

    private void Start()
    {
        // Reset trạng thái khi scene load
        lastDeathPosition = Vector3.zero;
        if (gameOverMenu != null)
            gameOverMenu.SetActive(false);

        SpawnPlayer();
    }

    // - POWER MODE - //
    public void ActivePowerMode()
    {
        if (!isPowerMode)
            StartCoroutine(PowerModeRoutine());
    }

    private IEnumerator PowerModeRoutine()
    {
        isPowerMode = true;
        yield return new WaitForSeconds(powerScaredDuration);
        isPowerMode = false;
    }

    // - PLAYER MANAGEMENT - //
    public void OnPlayerDie()
    {
        lives--;
        if (currentPlayer != null)
            lastDeathPosition = currentPlayer.transform.position;

        LivesUIManager.Instance.UpdateLives(lives);

        // Dừng tất cả enemy
        foreach (EnemyAI enemy in FindObjectsByType<EnemyAI>(FindObjectsSortMode.None))
        {
            enemy.player = null;
        }

        if (lives > 0)
            StartCoroutine(RespawnPlayer());
        else
            GameOver();
    }

    public void SpawnPlayer()
    {
        if (playerPrefab == null || playerSpawnPoint == null || playerSpawnPoint.Length == 0)
            return;

        // Tìm spawn point xa vị trí chết
        Transform safeSpawnPoint = playerSpawnPoint[0];
        float maxDistance = 0f;

        foreach (var spawn in playerSpawnPoint)
        {
            float dist = Vector3.Distance(spawn.position, lastDeathPosition);
            if (dist > maxDistance)
            {
                maxDistance = dist;
                safeSpawnPoint = spawn;
            }
        }

        currentPlayer = Instantiate(playerPrefab, safeSpawnPoint.position, Quaternion.identity);

        // Gán player mới cho enemy
        foreach (EnemyAI enemy in FindObjectsByType<EnemyAI>(FindObjectsSortMode.None))
        {
            enemy.player = currentPlayer.transform;
        }
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(1.5f);
        SpawnPlayer();
    }

    public void GameOver()
    {
        AudioManager.Instance.StopAllAudio();
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayPracManDieClip();

        if (gameOverMenu != null)
            gameOverMenu.SetActive(true);

        Debug.Log("THUA GAME RỒI!");
    }
}
