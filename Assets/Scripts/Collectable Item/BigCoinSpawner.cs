using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bigCoinPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int poolSize = 5;          // Số lượng coin trong pool
    [SerializeField] private float timeBtwSpawn = 10f;  // Thời gian giữa mỗi lần spawn

    private List<GameObject> coinPool = new List<GameObject>();

    private void Start()
    {
        // Tạo pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject coin = Instantiate(bigCoinPrefab);
            coin.SetActive(false); // Tắt coin cho đến khi cần dùng
            coinPool.Add(coin);
        }

        // Bắt đầu vòng spawn
        StartCoroutine(SpawnBigCoinCoroutine());
    }

    private IEnumerator SpawnBigCoinCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBtwSpawn);
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        // Tìm coin chưa được sử dụng
        GameObject coin = GetPooledCoin();
        if (coin != null)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            coin.transform.position = spawn.position;
            coin.SetActive(true);
        }
    }

    private GameObject GetPooledCoin()
    {
        foreach (GameObject coin in coinPool)
        {
            if (!coin.activeInHierarchy)
                return coin;
        }
        return null;
    }
}
