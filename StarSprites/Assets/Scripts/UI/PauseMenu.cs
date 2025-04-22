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
        PausePanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            if (GameIsPaused) { Resume(); }
            else { Pause(); }
        }
       
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        //Debug.Log("Playing game / inside game);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
        //Debug.Log("In the MainMenu scene");
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        PausePanel.SetActive(true);
        GameIsPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
        GameIsPaused = true; //changed
    }
}
