using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 5f;
    public float aimSpeed = 2.5f;
    private Vector3 moveDirection;

    [Header("Roll Ayarları")]
    public float rollSpeed = 10f;
    public float rollDuration = 0.4f;
    private bool isRolling = false;
    private bool canMove = true;

    [Header("Aim & Shoot Sistemi")]
    public bool isAiming = false;
    public bool canShoot = false;
    public Camera mainCamera;
    public Image uiCrosshair;

    [Header("Ateş Ayarları")]
    public GameObject bulletPrefab;
    public Transform firePoint; // namlu veya karakterin el noktası
    public float fireRate = 0.25f;
    private float nextFireTime = 0f;

    [Header("Bileşenler")]
    private Rigidbody rb;
    private Animator anim;
    [Header("Efektler")]
    public ParticleSystem muzzleFlash;
    public AudioSource shootSound;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (uiCrosshair != null)
            uiCrosshair.gameObject.SetActive(false);
    }

    private void Update()
    {
        HandleInput();
        HandleAim();

        if (isAiming)
        {
            AimTowardsMouse();
            MoveCrosshairWithMouse();

            if (canShoot && Input.GetMouseButton(0))
                TryShoot();
        }
    }

    private void FixedUpdate()
    {
        if (canMove && !isRolling)
            Move();
    }

    void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(h, 0f, v).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && !isRolling)
            StartCoroutine(Roll());

        if (moveDirection.magnitude > 0 && !isAiming)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    void Move()
    {
        float currentSpeed = isAiming ? aimSpeed : moveSpeed;
        Vector3 move = moveDirection * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
        anim.SetFloat("Speed", moveDirection.magnitude);
    }

    IEnumerator Roll()
    {
        isRolling = true;
        canMove = false;
        anim.SetTrigger("Roll");

        float timer = 0f;
        Vector3 rollDir = moveDirection;

        if (rollDir == Vector3.zero)
            rollDir = transform.forward;

        while (timer < rollDuration)
        {
            rb.MovePosition(rb.position + rollDir * rollSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isRolling = false;
        canMove = true;
    }

    void HandleAim()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
            canShoot = true;
            anim.SetBool("isAiming", true);
            if (uiCrosshair != null)
                uiCrosshair.gameObject.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            canShoot = false;
            anim.SetBool("isAiming", false);
            if (uiCrosshair != null)
                uiCrosshair.gameObject.SetActive(false);
        }
    }

    void AimTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            Vector3 lookPos = hit.point - transform.position;
            lookPos.y = 0;
            Quaternion rot = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 15f * Time.deltaTime);
        }
    }

    void MoveCrosshairWithMouse()
    {
        if (uiCrosshair == null) return;
        uiCrosshair.rectTransform.position = Input.mousePosition;
    }
void TryShoot()
{
    if (Time.time < nextFireTime) return;

    nextFireTime = Time.time + fireRate;
    anim.SetTrigger("Shoot");

    if (bulletPrefab != null && firePoint != null)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Yönü doğrudan firePoint yönüne sabitle
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * 25f; // mermiyi firePoint yönünde it
        }

        // Mermi kendi yönünü düzelt
        bullet.transform.forward = firePoint.forward;

        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (shootSound != null)
            shootSound.Play();
    }
}





    }

