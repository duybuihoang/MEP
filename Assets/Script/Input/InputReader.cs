using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputReader : MonoBehaviour
{
    [Header("Touch Settings")]
    public LayerMask tileLayer = -1;

    private PlayerInput playerInput;
    private Camera mainCamera;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
    }

    public void OnTap(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = playerInput.actions["Position"].ReadValue<Vector2>();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, mainCamera.nearClipPlane));
        CheckTileHit(worldPosition);
    }

    private void CheckTileHit(Vector3 tapPosition)
    {
        var hit = Physics2D.Raycast(tapPosition, Vector2.zero, Mathf.Infinity, tileLayer);

        if(hit.collider != null)
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            tile?.TapTile(tapPosition);
        }
    }
}
