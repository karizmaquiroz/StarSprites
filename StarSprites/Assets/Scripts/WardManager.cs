using UnityEngine;

public class WardManager : MonoBehaviour
{
    public static WardManager Instance;

    [Header("Level Completion Flags")]
    public bool level1Complete = false;
    public bool level2Complete = false;
    public bool level3Complete = false;

    [Header("Ward Progress")]
    [SerializeField] private bool[] wardPieces = new bool[3];
    public bool IsWardComplete => wardPieces[0] && wardPieces[1] && wardPieces[2];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadWardProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CompleteLevel(int levelIndex)
    {
        if (levelIndex < 1 || levelIndex > 3)
        {
            Debug.LogWarning("Invalid level index: " + levelIndex);
            return;
        }

        switch (levelIndex)
        {
            case 1:
                level1Complete = true;
                if (!SaveManager.Instance.currentData.levelsCompleted.Contains(1))
                {
                    SaveManager.Instance.currentData.levelsCompleted.Add(1);
                    var progress = SaveManager.Instance.currentData.levelProgress.Find(x => x.levelIndex == 1);
                    if (progress != null)
                    {
                        progress = new GameSaveData.LevelProgress
                        {
                            levelIndex = 1,
                            allEnemiesKilled = true,
                            bossKilled = true,
                            wardCollected = true
                        };
                    }
                        Debug.Log("Level 1 completed and saved.");
                }
                SaveManager.Instance.SaveGame();
                break;
            case 2:
                level2Complete = true;
                if (!SaveManager.Instance.currentData.levelsCompleted.Contains(2))
                {
                    SaveManager.Instance.currentData.levelsCompleted.Add(2);
                    var progress = SaveManager.Instance.currentData.levelProgress.Find(x => x.levelIndex == 2);
                    if (progress != null)
                    {
                        progress = new GameSaveData.LevelProgress
                        {
                            levelIndex = 2,
                            allEnemiesKilled = true,
                            bossKilled = true,
                            wardCollected = true
                        };
                    }
                    Debug.Log("Level 2 completed and saved.");
                }
                SaveManager.Instance.SaveGame();
                break;
            case 3:
                level3Complete = true;
                if (!SaveManager.Instance.currentData.levelsCompleted.Contains(3))
                {
                    SaveManager.Instance.currentData.levelsCompleted.Add(3);
                    var progress = SaveManager.Instance.currentData.levelProgress.Find(x => x.levelIndex == 3);
                    if (progress != null)
                    {
                        progress = new GameSaveData.LevelProgress
                        {
                            levelIndex = 3,
                            allEnemiesKilled = true,
                            bossKilled = true,
                            wardCollected = true
                        };
                    }
                    Debug.Log("Level 3 completed and saved.");
                }
                SaveManager.Instance.SaveGame();
                break;
        }

        int pieceIndex = levelIndex - 1;
        if (!wardPieces[pieceIndex])
        {
            wardPieces[pieceIndex] = true;
            Debug.Log($"Ward piece {pieceIndex + 1} collected!");
        }

        SaveWardProgress();

        if (IsWardComplete)
        {
            Debug.Log("All ward pieces collected! The ward is complete.");
            // Add logic for what happens when the ward is complete
        }
    }

    public int GetWardProgress()
    {
        int count = 0;
        foreach (bool piece in wardPieces)
        {
            if (piece) count++;
        }
        return count;
    }

    private void SaveWardProgress()
    {
        PlayerPrefs.SetInt("Level1Complete", level1Complete ? 1 : 0);
        PlayerPrefs.SetInt("Level2Complete", level2Complete ? 1 : 0);
        PlayerPrefs.SetInt("Level3Complete", level3Complete ? 1 : 0);

        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt($"WardPiece{i}", wardPieces[i] ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    private void LoadWardProgress()
    {
        level1Complete = PlayerPrefs.GetInt("Level1Complete", 0) == 1;
        level2Complete = PlayerPrefs.GetInt("Level2Complete", 0) == 1;
        level3Complete = PlayerPrefs.GetInt("Level3Complete", 0) == 1;

        for (int i = 0; i < 3; i++)
        {
            wardPieces[i] = PlayerPrefs.GetInt($"WardPiece{i}", 0) == 1;
        }
    }

    public void ResetWardProgress()
    {
        PlayerPrefs.DeleteKey("Level1Complete");
        PlayerPrefs.DeleteKey("Level2Complete");
        PlayerPrefs.DeleteKey("Level3Complete");
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.DeleteKey($"WardPiece{i}");
        }

        level1Complete = false;
        level2Complete = false;
        level3Complete = false;
        for (int i = 0; i < 3; i++) wardPieces[i] = false;

        PlayerPrefs.Save();
        Debug.Log("Ward progress reset.");
    }
}
