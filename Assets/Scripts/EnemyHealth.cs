using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 50;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log(gameObject.name + " HP: " + health);
        if (health <= 0)
            Destroy(gameObject);
    }
}
