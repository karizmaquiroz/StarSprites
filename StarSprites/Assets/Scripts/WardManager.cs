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
            DontDestroyOnLoad(gameObject); // Optional: Keep across scenes
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

        // Mark the level as completed
        switch (levelIndex)
        {
            case 1:
                level1Complete = true;
                break;
            case 2:
                level2Complete = true;
                break;
            case 3:
                level3Complete = true;
                break;
        }

        // Unlock corresponding ward piece
        int pieceIndex = levelIndex - 1;
        if (!wardPieces[pieceIndex])
        {
            wardPieces[pieceIndex] = true;
            Debug.Log($"Ward piece {pieceIndex + 1} collected!");
        }

        // Check if the full ward is assembled
        if (IsWardComplete)
        {
            Debug.Log("All ward pieces collected! The ward is complete.");
            // Trigger whatever happens when the full ward is built
        }
    }

    // Optional helper to check current progress
    public int GetWardProgress()
    {
        int count = 0;
        foreach (bool piece in wardPieces)
        {
            if (piece) count++;
        }
        return count;
    }
}
