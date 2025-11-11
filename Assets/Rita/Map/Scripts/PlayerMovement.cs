using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 14f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float originalScaleX;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScaleX = Mathf.Abs(transform.localScale.x);
    }

    void Update()
    {
        isGrounded = IsGrounded(); 
        HandleMovement();
        HandleJumpInput();
        UpdateAnimatorStates();
        // Debug.Log($"isGrounded = {isGrounded}");
    }

    private bool IsGrounded()
    {
        // Check a small circle under the player to see if it overlaps with ground
        return Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);
    }
    private void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Flip player facing direction
        if (move != 0)
            transform.localScale = new Vector3(-Mathf.Sign(move) * originalScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("isJumping", true);
            Debug.Log("Jump triggered!");
        }
    }

    private void UpdateAnimatorStates()
    {
        float move = Mathf.Abs(rb.velocity.x);

        // Walk only when grounded and moving
        bool isWalking = move > 0.01f && isGrounded;
        animator.SetBool("isWalking", isWalking);

        // Jump state: true when in air, false when grounded
        animator.SetBool("isJumping", !isGrounded);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, 0.15f);
        }
    }
}
