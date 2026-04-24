using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 1f;
    private Rigidbody rb;
    private Vector2 moveInput;
    public bool isGrounded;
    [SerializeField] private int jumpHeight;
    [SerializeField] private float groundCheckDistance = 0.35f;
    [SerializeField] private LayerMask groundMask = ~0;
    [SerializeField] private float maxGroundSlopeAngle = 55f;

    [SerializeField] private float fallGravityMultiplier = 20f;

    private CapsuleCollider capsuleCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = moveInput.x * transform.right + moveInput.y * transform.forward;
        rb.AddForce(direction * playerSpeed, ForceMode.VelocityChange);
    }

    private void Update()
    {
        OnDeath();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
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
    }

    private void OnDeath() 
    {
    if (gameObject.CompareTag("Water"))
        {
            SceneManager.LoadScene("YOUDIED");
        }
    }
}