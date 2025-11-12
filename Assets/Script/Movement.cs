using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Komponen")]
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Stats")]
    public float moveSpeed = 5f;
    public float rotateDuration = 0.4f;
    public float jumpForce = 7f;
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;
    public float jumpDelay = 0.2f;
    public float landDelay = 0.2f;

    private bool isRotating = false;
    private bool facingRight = true;
    private bool isGrounded = false;
    private bool wasGrounded = false;
    private bool isSitting = false;
    private bool isJumping = false;
    private float jumpGroundIgnoreTime = 1f;
    private float jumpTimer = 0f;
    private bool isFalling = false;

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }

        wasGrounded = isGrounded;

        if (jumpTimer <= 0)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        }

        if (Input.GetKeyDown(KeyCode.S) && isGrounded && !isSitting)
        {
            SitDown();
        }

        if (isRotating)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            animator.SetBool("IsMoving", false);
            return;
        }

        if (isSitting)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);

            if (horizontal != 0 || Input.GetKeyDown(KeyCode.W))
            {
                StandUp();
            }

            return;
        }

        if (!isGrounded)
        {
            if ((facingRight && horizontal < 0) || (!facingRight && horizontal > 0))
            {
                horizontal = 0;
            }
        }

        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocityY);

        if (isGrounded)
        {
            if (horizontal > 0 && !facingRight)
            {
                facingRight = true;
                RotatePlayer(true);
            }
            else if (horizontal < 0 && facingRight)
            {
                facingRight = false;
                RotatePlayer(false);
            }
        }

        animator.SetBool("IsMoving", horizontal != 0 && isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isSitting && !isJumping)
        {
            Jump();
        }

        if (isJumping && rb.linearVelocityY < 0 && !isFalling)
        {
            isFalling = true;
            animator.ResetTrigger("Jump");
            animator.SetTrigger("Fall");
        }

        if (isFalling && isGrounded && !wasGrounded)
        {
            isFalling = false;
            isJumping = false;
            animator.SetTrigger("Land");
            StartCoroutine(DelayedLanding(landDelay));
        }

    }

    private void SitDown()
    {
        isSitting = true;
        animator.SetBool("IsSitIdle", true);
        rb.linearVelocity = Vector2.zero;
    }

    private void StandUp()
    {
        isSitting = false;
        animator.SetBool("IsSitIdle", false);
        animator.SetTrigger("StandUp");
    }

    private void Jump()
    {
        if (!isJumping)
        {
            animator.ResetTrigger("Land"); //jaga-jaga klo langsung loncat lagi, gatau fungsi apa ngga ya
            animator.SetTrigger("Jump");
            
            isJumping = true;
            isGrounded = false;
            jumpTimer = jumpGroundIgnoreTime;
            StartCoroutine(DelayedJumpForce(jumpDelay));
        }
    }

    private System.Collections.IEnumerator DelayedJumpForce(float delay)
    {
        yield return new WaitForSeconds(delay);

        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
    }

    private System.Collections.IEnumerator DelayedLanding(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.ResetTrigger("Jump");
    }
    private void RotatePlayer(bool turnRight)
    {
        isRotating = true;

        rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        animator.SetBool("IsMoving", false);

        Vector3 scale = transform.localScale;
        scale.x = turnRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;

        isRotating = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }

}
