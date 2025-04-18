using UnityEngine;

public class ExplodingStar : MonoBehaviour
{
    public GameObject explosionEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (explosionEffect != null)
                Instantiate(explosionEffect, transform.position, Quaternion.identity);

            Destroy(collision.gameObject); // Optional: destroy enemy
            Destroy(gameObject);           // Destroy star
        }
    }
}
