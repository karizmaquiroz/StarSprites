using UnityEngine;

public class BirdRowCollision : MonoBehaviour
{
    [Tooltip("Reference to the BossTwoScript instance managing the bird rows.")]
    public BossTwoScript boss;

    [Tooltip("This bird's row index in the boss's array.")]
    public int rowIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if (boss != null)
            {
                // Call the boss's HitRow method to register damage for this row.
                Debug.Log($"Hit detected on row {rowIndex}.");
                boss.HitRow(rowIndex);
            }
            else
            {
                Debug.LogWarning("Boss reference is not set on BirdRowCollision.");
            }
        }
    }
}
