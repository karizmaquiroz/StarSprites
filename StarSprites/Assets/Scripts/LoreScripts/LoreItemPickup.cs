using UnityEngine;

public class LoreItemPickup : MonoBehaviour
{
    [Tooltip("The level number where this lore is found (used as the page number in the lorebook)")]
    public int lorePageNumber;

    [Tooltip("The lore text for this page")]
    [TextArea]
    public string loreText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Unlock the corresponding lore page
            LorebookManager.Instance.UnlockLorePage(lorePageNumber, loreText);

            // Optionally destroy or deactivate the item
            Destroy(gameObject);
        }
    }
}
