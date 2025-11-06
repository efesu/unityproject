using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Transform player;       // Oyuncunun transform'u (Inspector’dan atanýr)
    public float speed = 3f;       // Düþmanýn hareket hýzý
    public float stopDistance = 1.5f; // Oyuncuya ne kadar yaklaþabileceði

    void Update()
    {
        if (player == null)
            return;

        // Oyuncuya olan mesafeyi hesapla
        float distance = Vector3.Distance(transform.position, player.position);

        // Eðer yeterince yakýn deðilse, oyuncuya doðru hareket et
        if (distance > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Oyuncuya doðru bakmasý için rotasyon
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.1f);
        }
    }
}
