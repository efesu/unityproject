using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    public float speed = 30f;  // Hareket hýzý

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()   // Fizik tabanlý hareket FixedUpdate ile yapýlýr
    {
        float x = Input.GetAxis("Horizontal"); // A/D veya Ok tuþlarý
        float z = Input.GetAxis("Vertical");   // W/S veya Ok tuþlarý

        Vector3 move = new Vector3(x, 0, z) * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }
}



