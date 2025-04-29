using UnityEngine;
using UnityEngine.SceneManagement;

public class ToHouse : MonoBehaviour
{
    [Tooltip("House Scene Name")]
    public string houseSceneName = "HouseScene"; // Name of the house scene to load

    [Tooltip("Level Index")]
    public int levelIndex = 1; // Index of the level to load

    public void GoToHouse()
    {
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            ReturnToLastLevel();
            return; // Very important! Otherwise, code continues!
        }

        // Check if progress already exists
        var progress = SaveManager.Instance.currentData.levelProgress.Find(x => x.levelIndex == levelIndex);
        if (progress == null)
        {
            // If it doesn't exist, create and add it properly
            progress = new GameSaveData.LevelProgress
            {
                levelIndex = levelIndex
            };
            SaveManager.Instance.currentData.levelProgress.Add(progress); // <- ADD to the list!
        }
        else
        {
            // If it exists, you could update it here if needed
            progress.levelIndex = levelIndex;
        }

        SaveManager.Instance.SaveGame(); // Save the game after making sure progress is correct
        SceneManager.LoadScene(6); // Load the house scene
    }


    public void ReturnToLastLevel()
    {
        var progress = SaveManager.Instance.currentData.levelProgress.Find(x => x.levelIndex == levelIndex);
        Debug.Log(progress);
        // Try to find the fade overlay if it still exists
        var fadeOverlay = GameObject.Find("dust"); // replace with your fade object's real name
        if (fadeOverlay != null)
        {
            fadeOverlay.SetActive(true);
        }
        if (progress != null)
        {
            int lastLevel = progress.levelIndex;
            if (lastLevel == 1)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            }
            else if (lastLevel == 2)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(3);
            }
            else if (lastLevel == 3)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(4);
            }
            else
            {
                Debug.LogWarning("Invalid level index: " + lastLevel);
            }
        }
        else
        {
            // Handle the case where no matching progress is found
        }
    }
}
