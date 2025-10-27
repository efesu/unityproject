using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 2f;   // Merminin sahnede kalma süresi
    public int damage = 10;       // Vereceği hasar

    private void Start()
    {
        // Belirli süre sonra yok olsun
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Eğer Enemy tag'lı objeye çarparsa
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Çarptıktan sonra yok ol
        Destroy(gameObject);
    }
}
