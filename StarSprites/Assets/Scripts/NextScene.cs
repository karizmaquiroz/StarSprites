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

    private bool bossCleared = false;
    private bool bossActivated = false;
    private bool bossDefeated = false;

    void Start()
    {
        // Ensure boss and ward are inactive, and trigger disabled at start
        if (boss != null)
            boss.SetActive(false);
        if (ward != null)
            ward.SetActive(false);
        if (bossTriggerZone != null)
            bossTriggerZone.enabled = false;
    }

    void Update()
    {
        if (!bossCleared)
        {
            // Check if all regular enemies are inactive or dead
            bool allEnemiesInactive = true;
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
