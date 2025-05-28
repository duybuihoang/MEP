using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public List<Vector3> spawnPoints;
    public float baseSpawnRate = 1f;

    private float nextSpawnTime;
    private float currentSpawnRate;

    private void Start()
    {
        currentSpawnRate = baseSpawnRate;
        nextSpawnTime = Time.time + (1 / currentSpawnRate);
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

    void SpawnTile()
    {
        if (spawnPoints.Count == 0) return;

        int randomLane = Random.Range(0, spawnPoints.Count);
        Vector3 spawnPoint = spawnPoints[randomLane];

        GameObject newTile = Instantiate(tilePrefab, spawnPoint, Quaternion.identity);
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
