using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<GameObject> cameraSpots = new List<GameObject>();// Position to move to when triggered
    public Camera MainCamera;
    public float moveSpeed = 2.0f; // Speed of the camera movement
    private bool shouldMove = false;
    private Vector3 originalCameraPosition;

    private void Start()
    {
        originalCameraPosition = MainCamera.transform.position - transform.position;
    }

    void Update()
    {
        if (shouldMove)
        {
            MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, cameraSpots[0].transform.position, moveSpeed * Time.deltaTime);

            // Stop moving if close enough to the target
            if (Vector3.Distance(MainCamera.transform.position, cameraSpots[0].transform.position) < 0.01f)
            {
                MainCamera.transform.position = cameraSpots[0].transform.position;
                shouldMove = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CameraTrigger")) // Make sure the trigger has this tag
        {
            Debug.Log("camera has moved");
            shouldMove = true;
        }
    }
}
