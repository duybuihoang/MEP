using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float fallSpeed;

    [Header("Timing Windows")]
    [SerializeField] private float perfectWindow = 0.3f;
    [SerializeField] private float goodWindow = 0.6f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private bool hasBeenTapped = false;

    private float targetY = -2.5f;

    private void OnDisable()
    {
        hasBeenTapped = false;
    }

    private void Start()
    {
        SetupComponents();
        ScaleTolane();
        fallSpeed = GameManager.Instance.gameSpeed;
    }


    private void SetupComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }
    private void ScaleTolane()
    {
        if(spriteRenderer == null) return;

        float screenWidth = GameManager.Instance.ScreenWorldWidth;
        float laneWidth = screenWidth / GameManager.Instance.lanes;

        float currentWidth = spriteRenderer.bounds.size.x;
        float targetWidth = laneWidth * 1; // multiply < 1 for padding ratio 
        float scaleMultiplier = targetWidth / currentWidth;

        transform.localScale = Vector3.one * scaleMultiplier;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;
        MoveTile();
        CheckGameOverBoundary();

    }

    private void MoveTile() {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }

    public void TapTile(Vector3 tapPosition)
    {
        if (hasBeenTapped || GameManager.Instance.IsGameOver) return;

        hasBeenTapped = true;
        EvaluateHit(tapPosition);
        //Destroy(gameObject, 0.1f);
        TileSpawner.Instance.AddToPool(gameObject);
    }

    private void EvaluateHit(Vector3 tapPosition)
    {
        float distanceToTarget = Vector3.Distance(transform.position, new Vector3 (transform.position.x, targetY, transform.position.z));

        if (distanceToTarget <= perfectWindow)
        {
            GameManager.Instance.AddScore(GameManager.Instance.perfectScore);
            CreateFloatingText("PERFECT!", Color.green);
            Debug.Log("PERFECT: " + distanceToTarget);

        }
        else if (distanceToTarget <= goodWindow)
        {
            GameManager.Instance.AddScore(GameManager.Instance.goodScore);
            CreateFloatingText("GOOD!", Color.yellow);

            Debug.Log("GOOD: " + distanceToTarget);

        }
        else
        {
            GameManager.Instance.AddScore(GameManager.Instance.missScore);
            CreateFloatingText("MISS!", Color.red);
            Debug.Log("MISS: " + distanceToTarget);

        }
    }

    private void CheckGameOverBoundary()
    {
        float screenBottomY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float tileYSize = spriteRenderer.bounds.size.y;
        float gameOverY = screenBottomY - tileYSize / 2;

        if (transform.position.y < gameOverY && !hasBeenTapped)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }

    private void CreateFloatingText(string text, Color color)
    {
        GameObject floatingText = new GameObject("FloatingText");
        floatingText.transform.position = transform.position + Vector3.up * 0.5f;

        TextMesh tm = floatingText.AddComponent<TextMesh>();
        tm.text = text;
        tm.color = color;
        tm.fontSize = 4;
        tm.anchor = TextAnchor.MiddleCenter;

        // Animate floating text
        FloatingText ft = floatingText.AddComponent<FloatingText>();
        ft.Setup(1f, Vector3.up * 2f);
    }
}
