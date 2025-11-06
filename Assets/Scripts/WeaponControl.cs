using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Ateş Ayarları")]
    public GameObject bulletPrefab;   // Bullet prefab referansı
    public Transform firePoint;       // Merminin çıkış noktası
    public float bulletSpeed = 20f;   // Merminin hızı
    public float fireRate = 0.2f;     // Ateş etme aralığı (saniye)
    private float nextFireTime = 0f;  // Bir sonraki atış zamanı

    [Header("Aim Ayarları")]
    public bool isAiming = false;     // Aim durumu

    void Update()
    {
        // Sağ tık (Aim)
        if (Input.GetMouseButton(1))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }

        // Aim alındıysa ve sol tık basıldıysa ateş et
        if (isAiming && Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Fire()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("BulletPrefab veya FirePoint atanmadı!");
            return;
        }

        // Mermiyi oluştur
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Mermiye ileri yönlü kuvvet uygula
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }
    }
}
