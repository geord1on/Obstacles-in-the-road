using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;  // Ταχύτητα του χαρακτήρα
    public float jumpForce = 5f;  // Δύναμη άλματος
    public Animator animator; // Αναφορά στον Animator
    public float rotationSpeed = 700f; // Ταχύτητα περιστροφής

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        // Πάγωμα των περιστροφών στον άξονα Χ και Ζ
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        if (PlayerManager.isGameStarted)
        {
            MovePlayer();
            Jump();
        }
    }

    void MovePlayer()
    {
        // Κίνηση μόνο μπροστά με σταθερή ταχύτητα
        Vector3 forwardMove = transform.forward * speed * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMove);

        // Ορίζει την ταχύτητα στον animator για το τρέξιμο
        animator.SetFloat("Speed", speed);

        // Ο χαρακτήρας πρέπει να βλέπει προς τη σωστή κατεύθυνση
        Vector3 direction = rb.velocity;
        direction.y = 0; // Εξαλείφουμε το y component
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime));
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("IsJumping", true);
        }
        else if (rb.velocity.y == 0)
        {
            animator.SetBool("IsJumping", false);
        }
    }

    bool IsGrounded()
    {
        // Επιστρέφει true αν ο χαρακτήρας είναι στο έδαφος
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }
}
