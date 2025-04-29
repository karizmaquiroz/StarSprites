using System.Collections;
using UnityEngine;

public class Boss3Behavior : MonoBehaviour
{
    [Header("Attack Prefabs")]
    public GameObject birdPrefab;
    public GameObject bearPrefab;
    public GameObject foxPrefab;

    [Header("Bird Attack")]
    public int birdsToSpawn = 1;
    public Transform[] birdSpawnPoints; // <-- NEW: Assign multiple points manually

    [Header("Bear & Fox Spawns")]
    public Transform[] bearSpawnPoints;
    public Transform[] foxSpawnPoints;

    [Header("Timings")]
    public float idleDuration = 2f;
    public float attackCooldown = 2f;

    private Animator animator;
    private bool isAttacking = false;
    private Transform player;
    public int hitPoints = 500;

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
            // TODO: animator.SetTrigger("Idle");
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
        Debug.Log("Bird Attack!");
        animator.SetTrigger("RaiseHands");
        yield return new WaitForSeconds(0.5f);

        // SCREEN SHAKE!
        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake(0.5f, 0.4f);

        // Spawn birds
        for (int i = 0; i < birdsToSpawn; i++)
        {
            if (birdSpawnPoints.Length > 0)
            {
                // Pick a random spawn point from array
                Transform spawnPoint = birdSpawnPoints[Random.Range(0, birdSpawnPoints.Length)];
                GameObject bird = Instantiate(birdPrefab, spawnPoint.position, Quaternion.identity);

                // Target the player
                Enemy2Behavior birdScript = bird.GetComponent<Enemy2Behavior>();
                if (birdScript != null && player != null)
                    birdScript.player = player;
            }
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator BearAttack()
    {
        Debug.Log("Bear Attack!");
        animator.SetTrigger("RaiseHands");
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 3; i++)
        {
            Instantiate(bearPrefab, bearSpawnPoints[i].position, bearSpawnPoints[i].rotation);
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator FoxAttack()
    {
        Debug.Log("Fox Attack!");
        animator.SetTrigger("RaiseHands");
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 3; i++)
        {
            Instantiate(foxPrefab, foxSpawnPoints[i].position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Debug.Log("Hit by bullet!");
            hitPoints--;
            Debug.Log("Hit points: " + hitPoints);
            if (hitPoints <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Debug.Log("Enemy defeated!");
        Destroy(gameObject);
    }
}
