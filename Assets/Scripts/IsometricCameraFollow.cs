using UnityEngine;

public class IsometricCameraFollow : MonoBehaviour
{
    [Header("Takip Ayarları")]
    public Transform target;                // Player referansı
    public Vector3 offset = new Vector3(0f, 20f, -15f); // Kamera konumu
    public float smoothSpeed = 10f;         // Takip yumuşaklığı
    public Vector3 lookAtOffset = new Vector3(0f, 1f, 0f); // Karakterin gövdesine bakması için

    private void LateUpdate()
    {
        if (target == null) return;

        // Hedef pozisyon (kamerayı karakterin pozisyonuna göre kaydır)
        Vector3 desiredPosition = target.position + offset;

        // Pozisyona yumuşak geçiş
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Kameranın bakış yönü (karakterin gövdesine bakar)
        transform.LookAt(target.position + lookAtOffset);
    }
}
