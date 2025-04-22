using System.Collections;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 2f;

    private bool playerKilledABird = false;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        Debug.Log(playerKilledABird + "in SpawnLoop");
        while (!playerKilledABird)
        {
            SpawnBird();

            float elapsed = 0f;
            while (elapsed < spawnInterval)
            {
                if (playerKilledABird)
                {
                    Debug.Log("Stopped during wait — exiting loop.");
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        Debug.Log("Loop ended naturally.");
    }


    void SpawnBird()
    {
        Debug.Log("Spawning bird");
        Instantiate(birdPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void OnBirdKilledByPlayer()
    {
        playerKilledABird = true;
        Debug.Log("OnBirdKilledByPlayer CALLED");
    }
}