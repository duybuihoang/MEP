using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    private Queue<GameObject> availableTiles = new Queue<GameObject>();


    public List<Vector3> spawnPoints;
    public float baseSpawnRate = 1f;

    private float nextSpawnTime;
    private float currentSpawnRate;

    public static TileSpawner Instance;
    private void Awake()
    {
        Instance = this;
        GrowPool();
    }
    private void Start()
    {
        currentSpawnRate = baseSpawnRate;
        nextSpawnTime = Time.time + (1 / currentSpawnRate);
    }

    private void GrowPool()
    {
        for (int i = 0; i < 10; i++)
        {
            var instanceToAdd = Instantiate(tilePrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }
    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableTiles.Enqueue(instance);
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnTile();
            nextSpawnTime = Time.time + (1f / currentSpawnRate) + Random.Range(-0.2f, 0.2f);
        }
    }

    public GameObject GetFromPool()
    {
        if (availableTiles.Count == 0)
        {
            GrowPool();
        }
        var instance = availableTiles.Dequeue();
        instance.SetActive(true);
        return instance;
    }

    void SpawnTile()
    {
        if (spawnPoints.Count == 0) return;

        int randomLane = Random.Range(0, spawnPoints.Count);
        Vector3 spawnPoint = spawnPoints[randomLane];

        GameObject newTile = GetFromPool();
        newTile.transform.position = spawnPoint;
    }
    public void Init(int lanes, float screenWidth)
    {
        SetSpawnPosition(lanes, screenWidth);
    }

    private void SetSpawnPosition(int lanes, float screenWidth)
    {
        spawnPoints.Clear();

        float laneWidth = screenWidth / lanes;
        Vector3 topCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f));
        float topY = topCenter.y;

        float startX = -screenWidth * 0.5f + laneWidth * 0.5f;


        for (int i = 0; i < lanes; i++)
        {
            float xPos = startX + (i * laneWidth);
            spawnPoints.Add(new Vector3(xPos, topY, 0f));
            Debug.DrawLine(new Vector3(xPos, topY, 0f), new Vector3(xPos, topY - 100, 0f), Color.white, 100);
        }
    }
}
