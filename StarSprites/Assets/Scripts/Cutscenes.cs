using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cutscenes : MonoBehaviour
{
    [Header("Image Setup")]
    public List<Sprite> imageList = new List<Sprite>(); // Assign images in Inspector
    public Image displayImage; // UI Image to show the sprites

    [Header("Scene Setup")]
    public int nextSceneIndex; // Name of the scene to load

    private int currentIndex = 0;

    void Start()
    {
        if (imageList.Count > 0 && displayImage != null)
        {
            displayImage.sprite = imageList[0]; // Show the first image at start
        }
    }

    public void ShowNextImage()
    {
        if (imageList.Count == 0 || displayImage == null)
            return;

        currentIndex++;
        if (currentIndex >= imageList.Count)
        {
            currentIndex = 0; // Loop back to first image
        }

        displayImage.sprite = imageList[currentIndex];
    }

    public void LoadNextScene()
    {
        if (nextSceneIndex < 0)
        {
            Debug.LogError("Next scene name not set!");
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
