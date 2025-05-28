using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }


    [Header("Game Settings")]
    public float gameSpeed = 1f;
    public float tileSpawnInterval = 1f;
    public int lanes = 4;

    [Header("Scoring")]
    public int perfectScore = 100;
    public int goodScore = 50;
    public int missScore = -25;

    private int score = 0;
    private int combo = 0;

    private bool gameOver = false;
    public bool IsGameOver => gameOver;
    [SerializeField] private GameObject GameOverPanel;

    public float ScreenWorldWidth => Camera.main.orthographicSize * 2f * Camera.main.aspect;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        float screenWorldWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;

        TileSpawner.Instance.Init(lanes, screenWorldWidth);
    }
    public void TriggerGameOver()
    {
        if (gameOver) return;

        gameOver = true;
        GameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        Debug.Log($"Game Over!");
    }

    public void AddScore(int points)
    {
        if (gameOver) return;

        score += points;
        combo = points > 0 ? combo + 1 : 0;

        // TODO: UPDATE UI
        //Debug.Log($"Score: {score}, Combo: {combo}");
    }

}
