using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public string saveFileName = "gamesave.json";
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    public GameSaveData currentData = new GameSaveData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(currentData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Game saved to: " + SavePath);
    }

    public void LoadGame()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            currentData = JsonUtility.FromJson<GameSaveData>(json);
            Debug.Log("Game loaded from: " + SavePath);
        }
        else
        {
            Debug.LogWarning("No save file found. Creating new save data.");
            currentData = new GameSaveData();
        }
    }

    public void ClearSave()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
        currentData = new GameSaveData();
        Debug.Log("Save cleared.");
    }
}
