using System.Collections;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 2f;

    private bool playerKilledABird = false;
    private Coroutine spawnCoroutine;

    void Start()
    {
        spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            //Check BEFORE spawning
            if (playerKilledABird)
            {
                Debug.Log("Stopping bird spawner...");
                yield break; // Stops the coroutine immediately
            }

            SpawnBird();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnBird()
    {
        Instantiate(birdPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void OnBirdKilledByPlayer()
    {
        playerKilledABird = true;

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        gameObject.SetActive(false);
    }
}
