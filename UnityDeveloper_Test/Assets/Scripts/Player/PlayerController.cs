using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 8f;

    [Header("References")]
    [SerializeField] private Animator animator;

    private Rigidbody rb;
    private PlayerGroundCheck groundCheck;
    private Vector3 moveInput;
    private bool wasFalling= false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = GetComponent<PlayerGroundCheck>();
        rb.freezeRotation = true;
    }
    void Update()
    {
        if (!GameManager.instance.isGameActive) return;
        HandleInput();
        HandelAnimaiton();
    }
    void FixedUpdate()
    {
        if (!GameManager.instance.isGameActive) return;
        Move();
    }
    void HandleInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveInput = transform.right * x + transform.forward * z;
        // Debug.Log("Move Input is " + moveInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 currentVelocity = rb.linearVelocity;
            Vector3 velocityProjectOnUp = Vector3.Project(currentVelocity, transform.up);
            rb.linearVelocity = (currentVelocity - velocityProjectOnUp) + (transform.up * jumpForce);
            if (animator) animator.SetTrigger("Jump");
        }
    }
    void Move()
    {
        Vector3 verticalVelocity = Vector3.Project(rb.linearVelocity, transform.up);
        Vector3 targetMoveVelocity = moveInput * moveSpeed;
        rb.linearVelocity = targetMoveVelocity + verticalVelocity;
    }
    void HandelAnimaiton()
    {
        if (animator == null) return;
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);

        float speed = new Vector3(localVel.x, 0, localVel.z).magnitude;

        animator.SetFloat("Speed", speed);

        animator.SetBool("IsGrounded", groundCheck.isGrounded);
        bool isFalling = !groundCheck.isGrounded && localVel.y<-0.1f;
        if (isFalling && !wasFalling)
        {
            animator.SetTrigger("Falling");
        }
        wasFalling = isFalling;
    }

}
