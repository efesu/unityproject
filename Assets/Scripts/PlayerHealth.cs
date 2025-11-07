using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Bileşenler")]
    private Animator animator;
    private UIManager uiManager;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        // UIManager'ı sahnede bul
        uiManager = FindFirstObjectByType<UIManager>();

        // Can barını başlangıçta güncelle
        if (uiManager != null)
            uiManager.UpdateHealth(GetHealthPercent());
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        // UI'yi güncelle
        if (uiManager != null)
            uiManager.UpdateHealth(GetHealthPercent());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Ölüm animasyonu
        if (animator != null)
            animator.SetTrigger("Die");

        // Timer durdur
        if (uiManager != null)
            uiManager.StopTimer();

        // Oyuncu hareket edemesin
        MonoBehaviour movement = GetComponent<MonoBehaviour>();
        if (movement != null) movement.enabled = false;

        // Collider ve Rigidbody devre dışı
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Destroy(gameObject, 3f);
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        // UI'yi güncelle
        if (uiManager != null)
            uiManager.UpdateHealth(GetHealthPercent());
    }

    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
}
