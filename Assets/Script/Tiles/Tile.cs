using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float fallSpeed = 5f;

    [Header("Timing Windows")]
    [SerializeField] private float perfectWindow = 0.3f;
    [SerializeField] private float goodWindow = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private bool hasBeenTapped = false;

    private float targetY = -2.5f;

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

        Debug.DrawLine(new Vector3(
            transform.position.x - spriteRenderer.bounds.size.x / 2,
            transform.position.y + perfectWindow,
            0),
            new Vector3(
            transform.position.x + spriteRenderer.bounds.size.x / 2,
            transform.position.y + perfectWindow,
            0),
            Color.green
            );

        Debug.DrawLine(new Vector3(
            transform.position.x - spriteRenderer.bounds.size.x / 2,
            transform.position.y - perfectWindow,
            0),
            new Vector3(
            transform.position.x + spriteRenderer.bounds.size.x / 2,
            transform.position.y - perfectWindow,
            0),
            Color.green
            );

    }

    private void MoveTile() {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }

    public void TapTile(Vector3 tapPosition)
    {
        if (hasBeenTapped || GameManager.Instance.IsGameOver) return;

        hasBeenTapped = true;
        EvaluateHit(tapPosition);
        Destroy(gameObject, 0.1f);
    }

    void EvaluateHit(Vector3 tapPosition)
    {
        float distanceToTarget = Mathf.Abs(transform.position.y - targetY);

        if (distanceToTarget <= perfectWindow)
        {
            GameManager.Instance.AddScore(GameManager.Instance.perfectScore);
        }
        else if (distanceToTarget <= goodWindow)
        {
            GameManager.Instance.AddScore(GameManager.Instance.goodScore);
        }
        else
        {
            GameManager.Instance.AddScore(GameManager.Instance.missScore);
        }
    }

    void CheckGameOverBoundary()
    {
        float screenBottomY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float tileYSize = spriteRenderer.bounds.size.y;
        float gameOverY = screenBottomY - tileYSize / 2;

        if (transform.position.y < gameOverY && !hasBeenTapped)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
}
