using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 4f;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 movement;

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(inputX, inputY);
        movement = input.normalized;

        bool isWalking = input != Vector2.zero;
        animator.SetBool("isWalking", isWalking);



        if (isWalking)
        {
            animator.SetFloat("inputx", input.x);
            animator.SetFloat("inputy", input.y);

            animator.SetFloat("lastInputx", input.x);
            animator.SetFloat("lastInputy", input.y);
        }
    }


    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
