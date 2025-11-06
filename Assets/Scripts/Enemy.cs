using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{
    [Header("Statlar")]
    public float maxHealth = 100f;
    public float moveSpeed = 3f;
    public float damage = 15f;
    public float attackCooldown = 1f;
    public float attackRange = 1.5f;

    [Header("Bileşenler")]
    private Transform player;
    private float currentHealth;
    private bool canAttack = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (player == null) return;

        // Oyuncuya doğru yönel
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // sadece yatay düzlemde dön
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10f * Time.deltaTime);

        // Oyuncuya yaklaş
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackRange)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else if (canAttack)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.TakeDamage(damage);

        StartCoroutine(AttackCooldown());
    }

    System.Collections.IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
