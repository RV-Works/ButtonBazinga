using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 1f;
    [SerializeField] private float turnSpeed = 45f;
    [SerializeField] private Transform graphicsRoot;
    private Rigidbody rb;
    private Vector2 moveInput;
    public bool isGrounded;
    [SerializeField] private int jumpHeight;
    [SerializeField] private float groundCheckDistance = 0.35f;
    [SerializeField] private float maxGroundSlopeAngle = 55f;
    [SerializeField] private float fallGravityMultiplier = 20f;

    private CapsuleCollider capsuleCollider;
    private Vector3 graphicsInitialScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        if (graphicsRoot == null)
        {
            graphicsRoot = transform;
        }

        graphicsInitialScale = graphicsRoot.localScale;
    }

    private void FixedUpdate()
    {
        UpdateFacingDirection();

        if (Mathf.Abs(moveInput.x) > 0.001f)
        {
            Quaternion deltaRotation = Quaternion.Euler(0f, moveInput.x * turnSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        Vector3 direction = moveInput.x * transform.right;
        direction.y = 0f;

        rb.AddForce(direction * playerSpeed, ForceMode.VelocityChange);

        if (rb.useGravity && rb.linearVelocity.y < 0f)
        {
            rb.AddForce(Physics.gravity * (fallGravityMultiplier - 1f), ForceMode.Acceleration);
        }
    }

    private void UpdateFacingDirection()
    {
        if (moveInput.x > 0.001f)
        {
            SetFacingSign(1f);
        }
        else if (moveInput.x < -0.001f)
        {
            SetFacingSign(-1f);
        }
    }

    private void SetFacingSign(float sign)
    {
        Vector3 scale = graphicsInitialScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(sign);
        graphicsRoot.localScale = scale;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        float x = context.ReadValue<Vector2>().x;
        moveInput = new Vector2(x, 0f);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded == true)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isGrounded = false;
        }
        else { return; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Water"))
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        SceneManager.LoadScene("YOUDIED");
    }
}