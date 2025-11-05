using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private GameObject coinPrefab;
    public int poolSize = 200;
    public float coinOffsetY = 0.3f;
    public float spawnChance = 0.4f;
    public float respawnDelay = 5f;

    private GridGraph grid;
    private List<Vector3> walkablePositions = new List<Vector3>();
    private List<GameObject> coinPool = new List<GameObject>();

    private void Start()
    {
        grid = AstarPath.active.data.gridGraph;

        if (grid == null)
        {
            Debug.LogError("Không tìm thấy GridGraph! Hãy chắc chắn bạn đã Scan map trong A* Inspector.");
            return;
        }

        CreateCoinPool();
        CacheWalkableNodes();
        SpawnCoins();
        InvokeRepeating(nameof(CheckAllCoinsCollected), 1f, 1f);
    }

    public void CreateCoinPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform);
            coin.SetActive(false);
            coinPool.Add(coin);
        }
    }

    public void CacheWalkableNodes()
    {
        walkablePositions.Clear();
        foreach (GraphNode node in grid.nodes)
        {
            if (node.Walkable)
            {
                Vector3 pos = (Vector3)node.position;
                pos.y += coinOffsetY;
                walkablePositions.Add(pos);
            }
        }
    }

    public void SpawnCoins()
    {
        int index = 0;
        foreach (Vector3 pos in walkablePositions)
        {
            if (Random.value < spawnChance && index < coinPool.Count)
            {
                GameObject coin = coinPool[index];
                coin.transform.position = pos;
                coin.SetActive(true);
                index++;
            }
        }
        Debug.Log("Total coin spawned: " + index);
    }

    public void CheckAllCoinsCollected()
    {
        bool allInactive = true;
        foreach (var coin in coinPool)
        {
            if (coin.activeSelf)
            {
                allInactive = false;
                break;
            }
        }

        if (allInactive)
        {
            Invoke(nameof(RespawnCoin), respawnDelay);
        }
    }

    public void RespawnCoin()
    {
        SpawnCoins();
    }
}
