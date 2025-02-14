using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Collider2D playerCollider;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private float dropDuration = 0.2f;

    private void Start()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Check for the down arrow or 'S' key press
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(DropThroughPlatform());
        }
    }

    private System.Collections.IEnumerator DropThroughPlatform()
    {
        // Disable collision temporarily
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"), true);
        yield return new WaitForSeconds(dropDuration);
        // Re-enable collision
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"), false);
    }
}
