using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 5f;
    public float aimSpeed = 2.5f; // Aim modunda yavaşlama
    private Vector3 moveDirection;

    [Header("Roll Ayarları")]
    public float rollSpeed = 10f;
    public float rollDuration = 0.4f;
    private bool isRolling = false;
    private bool canMove = true;

    [Header("Aim Sistemi")]
    public bool isAiming = false;
    public bool canShoot = false;

    [Header("Bileşenler")]
    private Rigidbody rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInput();
        HandleAim();
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

    // Karakter yönü (aim modunda dönmez)
    if (moveDirection.magnitude > 0 && !isAiming)
    {
        // Eski: transform.forward = moveDirection;
        // Yeni: daha yumuşak dönüş
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }
}


    void Move()
    {
        float currentSpeed = isAiming ? aimSpeed : moveSpeed;

        // Doğru hız uygulaması — sadece 1 kez Time.fixedDeltaTime kullanılmalı!
        Vector3 move = moveDirection * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Animator parametresi
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
            rollDir = transform.forward; // Boştayken ileri doğru roll atar

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
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            canShoot = false;
            anim.SetBool("isAiming", false);
        }
    }
}
