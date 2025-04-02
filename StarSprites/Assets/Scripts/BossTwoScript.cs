using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTwoScript : MonoBehaviour
{
    [System.Serializable]
    public class BirdRow
    {
        [Tooltip("Assign all bird GameObjects in this row.")]
        public List<GameObject> birds;
        
        [Tooltip("Each row takes three hits before dying.")]
        public int hitsRemaining = 3;
        
        [Tooltip("Set to true when this row is permanently dead.")]
        public bool isDead = false;
        
        [HideInInspector]
        public List<Vector3> initialPositions = new List<Vector3>();
        
        [HideInInspector]
        public bool isAttacking = false;
    }

    [Header("Bird Rows Settings")]
    [Tooltip("Assign exactly three rows of birds.")]
    public BirdRow[] birdRows;

    [Header("Dive Settings")]
    [Tooltip("Speed multiplier for the dive movement.")]
    public float diveSpeed = 5f;
    
    [Tooltip("Time (in seconds) for a dive to complete.")]
    public float diveDuration = 1.0f;
    
    [Tooltip("Delay (in seconds) before birds respawn at their original positions after a dive.")]
    public float respawnDelay = 1f;

    [Header("Player Settings")]
    [Tooltip("Reference to the player transform.")]
    public Transform player;

    private void Start()
    {
        // Save the initial positions of the birds in each row for respawn.
        foreach (var row in birdRows)
        {
            row.initialPositions.Clear();
            foreach (var bird in row.birds)
            {
                if (bird != null)
                    row.initialPositions.Add(bird.transform.position);
            }
        }
    }

    private void Update()
    {
        // Determine which row should attack based on the player's platform.
        int platformIndex = GetPlayerPlatformIndex();
        if (platformIndex >= 0 && platformIndex < birdRows.Length)
        {
            BirdRow row = birdRows[platformIndex];
            // Only initiate a dive if the row is not dead and not already attacking.
            if (!row.isDead && !row.isAttacking)
            {
                StartCoroutine(DiveRow(row));
            }
        }
    }

    // Placeholder method to determine which platform the player is on.
    // Replace this logic with your own criteria (e.g., based on position, state, etc.).
    int GetPlayerPlatformIndex()
    {
        // Example logic: use the player's y-position to determine platform.
        float y = player.position.y;
        if (y < 0)
            return 0;
        else if (y < 5)
            return 1;
        else
            return 2;
    }

    // Coroutine to animate the dive of a row of birds.
    IEnumerator DiveRow(BirdRow row)
    {
        row.isAttacking = true;
        Vector3 playerPos = player.position;

        // Calculate target positions for each bird:
        // They will move from their start positions to just past the player.
        List<Vector3> startPositions = new List<Vector3>(row.initialPositions);
        List<Vector3> diveTargets = new List<Vector3>();
        foreach (var startPos in startPositions)
        {
            // Compute the normalized direction from the bird's start to the player.
            Vector3 direction = (playerPos - startPos).normalized;
            // The target is set to a point slightly past the player.
            Vector3 diveTarget = playerPos + direction * 2.0f;
            diveTargets.Add(diveTarget);
        }

        // Animate the dive over the set duration.
        float elapsedTime = 0f;
        while (elapsedTime < diveDuration)
        {
            for (int i = 0; i < row.birds.Count; i++)
            {
                if (row.birds[i] != null)
                {
                    row.birds[i].transform.position = Vector3.Lerp(startPositions[i], diveTargets[i], elapsedTime / diveDuration);
                }
            }
            elapsedTime += Time.deltaTime * diveSpeed;
            yield return null;
        }
        // Ensure birds reach the target positions.
        for (int i = 0; i < row.birds.Count; i++)
        {
            if (row.birds[i] != null)
                row.birds[i].transform.position = diveTargets[i];
        }
        
        // At this point the birds have rushed past the player.
        // TODO: Insert code to damage the player here.
        // For example: DamagePlayer();

        // Wait for a short delay before respawning the birds.
        yield return new WaitForSeconds(respawnDelay);
        
        // Respawn birds at their original positions if the row is still alive.
        if (!row.isDead)
        {
            for (int i = 0; i < row.birds.Count; i++)
            {
                if (row.birds[i] != null)
                    row.birds[i].transform.position = row.initialPositions[i];
            }
        }
        row.isAttacking = false;
    }

    // Call this function when the player successfully hits a row of birds.
    // The rowIndex should correspond to the appropriate row (0, 1, or 2).
    public void HitRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= birdRows.Length)
            return;

        BirdRow row = birdRows[rowIndex];
        if (row.isDead)
            return;

        row.hitsRemaining--;
        if (row.hitsRemaining <= 0)
        {
            // The row is now dead. Disable all birds in this row.
            row.isDead = true;
            foreach (var bird in row.birds)
            {
                if (bird != null)
                    bird.SetActive(false);
            }
        }
    }

    // Placeholder function for damaging the player.
    // Fill in with your own damage logic.
    void DamagePlayer()
    {
        // Your damage logic here.
    }
}