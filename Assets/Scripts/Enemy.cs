using UnityEngine;
using System.Collections;

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
    private PlayerHealth playerHealth;
    private float currentHealth;
    private bool canAttack = true;
    private bool isDead = false;

    private Animator animator;
    private Rigidbody rb;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }

        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation; // düşmesin, devrilmesin
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Oyuncuya yönel
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10f * Time.deltaTime);

        // Oyuncuya yaklaş
        if (distance > attackRange)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
            if (animator != null)
                animator.SetBool("isRunning", true);
        }
        else
        {
            if (animator != null)
                animator.SetBool("isRunning", false);
        }
    }

    // 👇 Çarpışma hasarı (Trigger değil)
    private void OnCollisionStay(Collision collision)
    {
        if (isDead || !canAttack) return;

        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth ph = collision.collider.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
                StartCoroutine(AttackCooldown());
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void SetDifficulty(float multiplier)
    {
        maxHealth *= multiplier;
        moveSpeed *= 0.8f + multiplier * 0.2f;
        damage *= multiplier;
        currentHealth = maxHealth;
    }

    void Die()
    {
        isDead = true;
        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetTrigger("Die");
        }

        Destroy(gameObject, 2f);
    }
}
