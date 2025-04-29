using UnityEngine;

public class ToHouse : MonoBehaviour
{
    [Tooltip("House Scene Name")]
    public string houseSceneName = "HouseScene"; // Name of the house scene to load

    [Tooltip("Level Index")]
    public int levelIndex = 1; // Index of the level to load

    public void GoToHouse()
    {
        var progress = SaveManager.Instance.currentData.levelProgress.Find(x => x.levelIndex == levelIndex);
        if (progress != null)
        {
            progress = new GameSaveData.LevelProgress
            {
                levelIndex = levelIndex,
            };
        }
        SaveManager.Instance.SaveGame(); // Save the game before transitioning
        UnityEngine.SceneManagement.SceneManager.LoadScene(6); // Load the house scene
    }

    public void ReturnToLastLevel()
    {
        var progress = SaveManager.Instance.currentData.levelProgress.Find(x => x.levelIndex == levelIndex);
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
