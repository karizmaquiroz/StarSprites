using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    
    [Header("Enemies in Scene")]
    public List<GameObject> enemyList = new List<GameObject>();

    [Header("Boss")]
    public GameObject boss;

    [Header("Boss Trigger Zone")]
    [Tooltip("Assign a Collider2D set as Trigger for the boss entrance area")]
    public Collider2D bossTriggerZone;

    [Header("Ward")]
    public GameObject ward;

    [Header("Current Level")]
    [Tooltip("Set the current level index (1, 2, or 3)")]
    public int currentLevelIndex = 1;

    public bool bossCleared = false;
    public bool bossActivated = false;
    public bool bossDefeated = false;
    public bool allEnemiesInactive = false;

    void Start()
    {
        // Ensure boss and ward are inactive, and trigger disabled at start
        if (boss != null)
            boss.SetActive(false);
        if (ward != null)
            ward.SetActive(false);
        if (bossTriggerZone != null)
            bossTriggerZone.enabled = false;
        SaveManager.Instance.LoadGame(); // Load the game data at the start
    }

    void Update()
    {
        if (!bossCleared)
        {
            // Check if all regular enemies are inactive or dead
            allEnemiesInactive = true;
            foreach (GameObject enemy in enemyList)
            {
                if (enemy != null && enemy.activeInHierarchy)
                {
                    allEnemiesInactive = false;
                    break;
                }
            }
            Debug.Log(allEnemiesInactive);
            if (allEnemiesInactive)
            {
                var progress = SaveManager.Instance.currentData.levelProgress.Find(x => x.levelIndex == currentLevelIndex);
                if (progress != null)
                {
                    progress = new GameSaveData.LevelProgress {
                        levelIndex = currentLevelIndex,
                        allEnemiesKilled = true,
                        bossKilled = false,
                        wardCollected = false
                    };
                    SaveManager.Instance.currentData.playerHealth = HealthBarManager.Instance.health;
                    SaveManager.Instance.currentData.playerHearts = HealthBarManager.Instance.heartsRemaining;
                    SaveManager.Instance.currentData.playerLevel = XPBarBehavior.Instance.level;
                    SaveManager.Instance.currentData.playerXP = XPBarBehavior.Instance.currentXP;
                    SaveManager.Instance.SaveGame();
                }
                bossCleared = true;
                if (bossTriggerZone != null)
                {
                    // Enable the trigger so the player can enter the boss area
                    bossTriggerZone.enabled = true;
                    Debug.Log("Boss trigger zone enabled. Walk into it to start the boss fight.");
                }
                else
                {
                    // Fallback: if no trigger assigned, activate boss immediately
                    Debug.Log("Boss activated");
                    ActivateBoss();
                }
            }
        }
        else if (bossActivated && !bossDefeated)
        {
            // Check if boss has been defeated (destroyed or inactive)
            if (boss == null || !boss.activeInHierarchy)
            {
                var progress = SaveManager.Instance.currentData.levelProgress.Find(x => x.levelIndex == currentLevelIndex);
                if (progress != null)
                {
                    progress.bossKilled = true;
                    progress.wardCollected = false;
                    SaveManager.Instance.SaveGame();
                }
                SaveManager.Instance.currentData.playerHealth = HealthBarManager.Instance.health;
                SaveManager.Instance.currentData.playerHearts = HealthBarManager.Instance.heartsRemaining;
                SaveManager.Instance.currentData.playerLevel = XPBarBehavior.Instance.level;
                SaveManager.Instance.currentData.playerXP = XPBarBehavior.Instance.currentXP;
                TriggerWard();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only respond if this is the boss trigger, player steps in, and boss not yet activated
        if (!bossActivated && bossCleared && bossTriggerZone != null && bossTriggerZone.enabled)
        {
            if (other.CompareTag("Player"))
            {
                // Prevent retriggering and disable the trigger
                bossTriggerZone.enabled = false;
                ActivateBoss();
            }
        }
    }

    private void ActivateBoss()
    {
        bossActivated = true;
        if (boss != null)
        {
            boss.SetActive(true);
            Debug.Log("Boss Activated!");
        }
    }

    private void TriggerWard()
    {
        bossDefeated = true;
        if (ward != null)
        {
            ward.SetActive(true);
            Debug.Log("Ward activated!");
        }
    }
}
