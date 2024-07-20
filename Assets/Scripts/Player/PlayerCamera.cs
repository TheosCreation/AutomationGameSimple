using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float smoothSpeed = 5.0f; // Speed at which the camera smooths its movement
    public Vector3 offset; // Offset from the player
    public float startScreenSize = 5f;
    public float minimumScreenSize = 4f;
    public float maximumScreenSize = 10f;

    private float currentScreenSize = 0f;

    private Camera thisCamera;

    private Vector2 mouseScroll = Vector2.zero;

    private void Start()
    {
        thisCamera = GetComponent<Camera>();
        currentScreenSize = startScreenSize;
        thisCamera.orthographicSize = startScreenSize;

        InputManager.Instance.playerInputActions.Game.Zoom.started += OnMouseZoomPerformed;
    }
    private void OnMouseZoomPerformed(InputAction.CallbackContext ctx)
    {
        mouseScroll = ctx.ReadValue<Vector2>();
        currentScreenSize -= mouseScroll.y;

        currentScreenSize = Mathf.Clamp(currentScreenSize, minimumScreenSize, maximumScreenSize);

        thisCamera.orthographicSize = currentScreenSize;
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 desiredPosition = player.position + offset; // Desired position is the player's position plus the offset
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime); // Smoothly interpolate to the desired position using Time.deltaTime
        transform.position = smoothedPosition; // Set the camera's position to the smoothed position
    }
}