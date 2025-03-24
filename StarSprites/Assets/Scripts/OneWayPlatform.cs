using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Collider2D playerCollider;
    private int platformLayer;
    public Transform GroundCheck;
    [SerializeField] private float dropDuration = 1f;

    private void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        platformLayer = LayerMask.NameToLayer("Platform");
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
        Debug.Log(gameObject.layer);
        Debug.Log(platformLayer);
        Debug.Log("Falling through");
        // Disable collision temporarily
        RaycastHit2D hit = Physics2D.Raycast(GroundCheck.position, Vector2.down, 1f, 1 << platformLayer);
        Collider2D platformCollider = hit.collider;
        if (platformCollider == null)
        {
            yield break;
        }
        platformCollider.enabled = false;
        yield return new WaitForSeconds(dropDuration);
        // Re-enable collision
        platformCollider.enabled = true;
    }
}
