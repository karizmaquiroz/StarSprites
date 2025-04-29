using System.Collections;
using UnityEngine;

public class Boss3Behavior : MonoBehaviour
{
    [Header("Attack Prefabs")]
    public GameObject birdPrefab;
    public GameObject bearPrefab;
    public GameObject foxPrefab;

    [Header("Bird Attack")]
    public int birdsToSpawn = 5;
    public float birdSpawnY = -4.5f; // Y position for birds at bottom
    public float birdSpawnXMin = -8f;
    public float birdSpawnXMax = 8f;

    [Header("Bear & Fox Spawns")]
    public Transform[] bearSpawnPoints; // Set 3 in inspector
    public Transform[] foxSpawnPoints;  // Set 3 in inspector

    [Header("Timings")]
    public float idleDuration = 2f;
    public float attackCooldown = 5f;

    private Animator animator;
    private bool isAttacking = false;
    private Transform player;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(BossRoutine());
    }

    IEnumerator BossRoutine()
    {
        while (true)
        {
            // IDLE PHASE
            // TODO: animator.SetTrigger("Idle"); // Your idle trigger
            yield return new WaitForSeconds(idleDuration);

            // ATTACK PHASE
            if (!isAttacking)
            {
                isAttacking = true;
                int attackType = Random.Range(0, 3);

                switch (attackType)
                {
                    case 0:
                        yield return StartCoroutine(BirdAttack());
                        break;
                    case 1:
                        yield return StartCoroutine(BearAttack());
                        break;
                    case 2:
                        yield return StartCoroutine(FoxAttack());
                        break;
                }

                yield return new WaitForSeconds(attackCooldown);
                isAttacking = false;
            }
        }
    }

    IEnumerator BirdAttack()
    {
        animator.SetTrigger("RaiseHands"); // Boss raises hands
        yield return new WaitForSeconds(0.5f); // short windup

        // SCREEN SHAKE!
        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake(0.5f, 0.4f);

        for (int i = 0; i < birdsToSpawn; i++)
        {
            float t = (float)i / (birdsToSpawn - 1);
            float x = Mathf.Lerp(birdSpawnXMin, birdSpawnXMax, t);
            Vector3 spawnPos = new Vector3(x, birdSpawnY, 0);

            GameObject bird = Instantiate(birdPrefab, spawnPos, Quaternion.identity);

            // Target the player
            Enemy2Behavior birdScript = bird.GetComponent<Enemy2Behavior>();
            if (birdScript != null && player != null)
                birdScript.player = player;
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator BearAttack()
    {
        animator.SetTrigger("RaiseHands");
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < bearSpawnPoints.Length && i < 3; i++)
        {
            Instantiate(bearPrefab, bearSpawnPoints[i].position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator FoxAttack()
    {
        TODO: animator.SetTrigger("RaiseHands");
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < foxSpawnPoints.Length && i < 3; i++)
        {
            Instantiate(foxPrefab, foxSpawnPoints[i].position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);
    }
}
