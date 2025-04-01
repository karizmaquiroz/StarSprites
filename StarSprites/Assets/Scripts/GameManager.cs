using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    public GameObject GameScreen0; //guardian sprites house
    public GameObject GameScreen1; //level 1
    public GameObject GameScreen2;
    public GameObject GameScreen3;
    public GameObject MainMenuUI; //game start screen
    public GameObject CreditUI;
    public GameObject GameOverUI; //maybe??

    public void Awake()
    {
        MainMenuUI.SetActive(true);

    }

    public void GameOver()
    {
        GameOverUI.SetActive(true);
    }

   

    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        //Debug.Log("In the MainMenu scene");
    }

    public void quit() //inside gameOver when pressed application quits
    {
        SceneManager.LoadScene("Credits");

        Application.Quit();
    }


    public void Exit()
    {
        SceneManager.LoadScene("Main Menu");

    }


    public void startGame()
    {
        SceneManager.LoadScene("Level1"); 

        
    }

    public void creditButton()
    {
        SceneManager.LoadScene("Credits");
        //Debug.Log("In the credits scene");




    }

    public void creditBackButton()
    {
        SceneManager.LoadScene("Main Menu");
        //Debug.Log("Back in MainMenu scene");

    }
}
