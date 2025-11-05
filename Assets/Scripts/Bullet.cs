using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 3f;
    public float damage = 25f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);
        }

        if (!other.CompareTag("Player"))
            Destroy(gameObject);
    }
}
