using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed = 10f;
    public float laneChangeSpeed = 10f; // Ταχύτητα αλλαγής λωρίδας

    public float jumpForce = 10f; // Δύναμη άλματος
    public float gravity = -20f; // Βαρύτητα

    private int desiredLane = 1; // 0: αριστερά, 1: κέντρο, 2: δεξιά
    public float laneDistance = 4; // Η απόσταση ανάμεσα στις λωρίδες

    // Προσθήκη περιορισμών εδάφους
    public float leftBoundary = -4f;
    public float rightBoundary = 4f;

    private Vector3 targetPosition;
    private float verticalVelocity = 0; // Ταχύτητα για το άλμα

    public Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Προσθήκη αυτού εδώ για να αποκτήσουμε τον Animator component
    }

    void Update()
    {
        if (!PlayerManager.isGameStarted)
        {
            animator.SetBool("isGameStarted", false);
            return;
        }

        animator.SetBool("isGameStarted", true); // Ενεργοποίηση της κατάστασης isGameStarted στον animator
        
        direction.z = forwardSpeed;

        // Έλεγχος αν ο χαρακτήρας είναι στο έδαφος
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
            {
                verticalVelocity = -1; // Κρατάμε τον χαρακτήρα προσκολλημένο στο έδαφος
            }

            // Άλμα
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            // Εφαρμογή βαρύτητας
            verticalVelocity += gravity * Time.deltaTime;
        }

        direction.y = verticalVelocity;

        // Μετακίνηση ανάμεσα στις λωρίδες
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if (desiredLane > 2)
                desiredLane = 2;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if (desiredLane < 0)
                desiredLane = 0;
        }

        // Υπολογισμός της νέας θέσης λωρίδας χωρίς το forward direction
        targetPosition = transform.position;
        targetPosition.x = (desiredLane - 1) * laneDistance; // Υπολογίζει τη θέση x ανάλογα με την επιθυμητή λωρίδα

        // Εφαρμογή περιορισμών εδάφους
        targetPosition.x = Mathf.Clamp(targetPosition.x, leftBoundary, rightBoundary);
    }

    void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
            return;

        // Μετακίνηση προς τα εμπρός και αλλαγή λωρίδας στο FixedUpdate για συνέπεια
        Vector3 moveVector = direction * Time.fixedDeltaTime;

        // Ομαλή μετάβαση προς τη θέση στόχο της λωρίδας
        Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, laneChangeSpeed * Time.fixedDeltaTime);
        moveVector.x = newPosition.x - transform.position.x;

        // Εφαρμογή της μετακίνησης μέσω του CharacterController
        controller.Move(moveVector);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Obstacle")) // Αλλαγή από tag σε CompareTag για σύγκριση με ετικέτες
        {
            PlayerManager.gameOver = true;
        }
    }
}
