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
    public float gameSpeed = 5f;
    public float tileSpawnInterval = 1f;
    public int lanes = 4;

    [Header("Scoring")]
    public int perfectScore = 100;
    public int goodScore = 50;
    public int missScore = -25;
    private int currentStreak;

    private int score = 0;
    private int combo = 0;

    private bool gameOver = false;
    public bool IsGameOver => gameOver;
    [SerializeField] private GameObject GameOverPanel;


    [Header("Audio")]
    private BeatManager beat;
    [SerializeField] private AudioBeatSO audioBeatSO;
    public AudioSource musicSource;

    public GameObject targerBar;

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

        musicSource.clip = audioBeatSO.audio;
        beat = (new GameObject("Beats")).AddComponent<BeatManager>();
        beat.beats = audioBeatSO.beats;
    }

    private void Start()
    {
        Init();
        PlayMusic();
    }
    private float calculateDelayedTime()
    {
        Vector3 topCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f));
        topCenter.z = 0;
        return Vector3.Distance(topCenter, targerBar.transform.position) / gameSpeed ;
    }

    private void PlayMusic()
    {
        var delay = calculateDelayedTime();
        beat.DelayTime = delay;
        musicSource.PlayDelayed(delay);
    }

    private void Init()
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

        musicSource.Stop();

        Debug.Log($"Game Over!");
    }

    public void AddScore(int points)
    {
        if (gameOver) return;

        if(currentStreak != points)
        {            
            score += combo * currentStreak;
            combo = 0;
            currentStreak = points;
        }
        else
        {
            combo = points > 0 ? combo + 1 : 0;
        }

        score = (int)Mathf.Clamp(score + points, 0, Mathf.Infinity);
        ScoreUI.Instance.SetScoreUI(score);
        ScoreUI.Instance.SetComboUI(combo);

    }

}
