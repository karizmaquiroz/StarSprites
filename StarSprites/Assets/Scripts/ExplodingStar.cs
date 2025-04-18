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
            Destroy(gameObject);           // Destroy star
        }
    }
}
