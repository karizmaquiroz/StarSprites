using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

        if (progress != null)
        {
            int lastLevelSceneIndex = 0;

            switch (progress.levelIndex)
            {
                case 1:
                    lastLevelSceneIndex = 2;
                    break;
                case 2:
                    lastLevelSceneIndex = 3;
                    break;
                case 3:
                    lastLevelSceneIndex = 4;
                    break;
                default:
                    Debug.LogWarning("Invalid level index: " + progress.levelIndex);
                    return;
            }

            StartCoroutine(LoadSceneAndReactivatePlayer(lastLevelSceneIndex));
        }
        else
        {
            Debug.LogWarning("No matching progress found.");
        }
    }

    private IEnumerator LoadSceneAndReactivatePlayer(int sceneIndex)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        ReactivatePlayer();
    }

    private void ReactivatePlayer()
    {
        // Find inactive Player manually
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("Player") && !obj.activeInHierarchy)
            {
                obj.SetActive(true);
                Debug.Log("Player reactivated after scene load!");
                return;
            }
        }

        Debug.LogWarning("No inactive Player found after scene load!");
    }

}
