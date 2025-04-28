using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;

    public static bool GameIsPaused = false;

    private void Awake()
    {
        if (PausePanel != null)
            PausePanel.SetActive(false);
    }

    public void Pause()
    {
        Debug.Log("Pause button pressed!");
        Time.timeScale = 0f; // Freeze time
        if (PausePanel != null)
            PausePanel.SetActive(true);
        GameIsPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f; // Unfreeze time
        if (PausePanel != null)
            PausePanel.SetActive(false);
        GameIsPaused = false;
    }
}
