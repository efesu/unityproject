using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Player referansı
    public Vector3 offset = new Vector3(0f, 20f, -15f);
    public float smoothTime = 0.2f; // takip yumuşaklığı (0.1–0.3 arası iyi)
    
    private Vector3 velocity = Vector3.zero; // SmoothDamp'in kendi hız hesabı

    void LateUpdate()
    {
        if (target == null) return;

        // Hedef pozisyonu belirle
        Vector3 desiredPosition = target.position + offset;

        // SmoothDamp: kamerayı sarsıntısız şekilde hedefe taşır
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        // Kameranın rotasyonu sabit kalır (isometrik açı)
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }
}
