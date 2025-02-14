using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public class CameraTrigger
    {
        public GameObject trigger;  // The trigger GameObject
        public GameObject cameraSpot; // The target camera spot for this trigger
    }

    public List<CameraTrigger> cameraTriggers = new List<CameraTrigger>(); // List of trigger and spot pairs
    public Camera MainCamera;
    public float moveSpeed = 2.0f;
    private Vector3 originalCameraPosition;
    private Vector3 targetPosition;
    private bool shouldMove = false;

    private void Start()
    {
        // Store the camera's initial position
        originalCameraPosition = MainCamera.transform.position;
        targetPosition = originalCameraPosition;
    }

    private void Update()
    {
        if (shouldMove)
        {
            // Smoothly move the camera towards the target position
            MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Stop moving if close enough to the target
            if (Vector3.Distance(MainCamera.transform.position, targetPosition) < 0.01f)
            {
                MainCamera.transform.position = targetPosition;
                shouldMove = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check each trigger in the list
        foreach (CameraTrigger cameraTrigger in cameraTriggers)
        {
            // If the player enters a trigger associated with this camera spot
            if (other.gameObject == cameraTrigger.trigger)
            {
                // If the camera is already at the target spot, move it back to the original position
                if (targetPosition == cameraTrigger.cameraSpot.transform.position)
                {
                    targetPosition = originalCameraPosition;
                }
                else
                {
                    // Otherwise, move the camera to the corresponding spot
                    targetPosition = cameraTrigger.cameraSpot.transform.position;
                }
                shouldMove = true;
                Debug.Log($"Camera is moving to: {targetPosition}");
                break;
            }
        }
    }
}