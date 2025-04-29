using UnityEngine;

public class HouseSceneCameraFix : MonoBehaviour
{
    void Start()
    {
        Camera mainCam = Camera.main;

        if (mainCam != null)
        {
            // If you want a Skybox background
            mainCam.clearFlags = CameraClearFlags.Skybox;

            // OR if you want a solid color background, uncomment this instead:
            // mainCam.clearFlags = CameraClearFlags.SolidColor;
            // mainCam.backgroundColor = Color.gray; // or whatever color you want

            Debug.Log("Camera clear flags and background reset on House scene load.");
        }
        else
        {
            Debug.LogWarning("No Main Camera found to fix in House scene.");
        }

        Time.timeScale = 1f; // Always unpause just in case
    }
}
