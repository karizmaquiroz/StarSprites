using System.Collections.Generic;
using UnityEngine;

public class LorebookManager : MonoBehaviour
{
    public static LorebookManager Instance;

    // Maps page numbers to unlocked status and text
    private Dictionary<int, string> unlockedLorePages = new Dictionary<int, string>();
    private const string PlayerPrefsKey = "LorePage_";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLorePages();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnlockLorePage(int pageNumber, string loreText)
    {
        if (!unlockedLorePages.ContainsKey(pageNumber))
        {
            unlockedLorePages[pageNumber] = loreText;
            PlayerPrefs.SetString(PlayerPrefsKey + pageNumber, loreText);
            PlayerPrefs.Save();
        }
    }

    public bool IsPageUnlocked(int pageNumber)
    {
        return unlockedLorePages.ContainsKey(pageNumber);
    }

    public string GetLoreText(int pageNumber)
    {
        if (unlockedLorePages.TryGetValue(pageNumber, out var text))
            return text;
        return "";
    }

    private void LoadLorePages()
    {
        // Assume you know max page count (e.g., 10 levels)
        for (int i = 1; i <= 10; i++)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKey + i))
            {
                unlockedLorePages[i] = PlayerPrefs.GetString(PlayerPrefsKey + i);
            }
        }
    }
}
