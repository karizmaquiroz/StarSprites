using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [Tooltip("Reference to the BossTwoScript instance.")]
    public BossTwoScript boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boss.SetPlayerTriggered(true);
            Debug.Log("Player entered external boss trigger area.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boss.SetPlayerTriggered(false);
            Debug.Log("Player exited external boss trigger area.");
        }
    }
}
