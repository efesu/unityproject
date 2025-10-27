using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 5f;
    public float aimSpeed = 2.5f; // aim modunda yavaşlama
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

        if (moveDirection.magnitude > 0 && !isAiming)
            transform.forward = moveDirection;
    }

    void Move()
    {
        float currentSpeed = isAiming ? aimSpeed : moveSpeed;
        Vector3 move = moveDirection * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        anim.SetFloat("Speed", moveDirection.magnitude);
    }

    System.Collections.IEnumerator Roll()
    {
        isRolling = true;
        canMove = false;

        anim.SetTrigger("Roll");
        float timer = 0f;

        Vector3 rollDir = moveDirection;
        if (rollDir == Vector3.zero)
            rollDir = transform.forward; // boştayken yönüne göre roll atar

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
