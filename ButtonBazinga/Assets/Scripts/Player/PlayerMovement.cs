using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform mapRoot;
    [SerializeField] private float mapRotationSpeedDegreesPerSecond = 90f;

    [SerializeField] private bool lockPlayerHorizontalPosition = true;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 lockedPlayerPosition;

    public bool isGrounded;

    [SerializeField] private int jumpHeight;
    [SerializeField] private float groundCheckDistance = 0.35f;
    [SerializeField] private float maxGroundSlopeAngle = 55f;
    [SerializeField] private float fallGravityMultiplier = 20f;

    private CapsuleCollider capsuleCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        lockedPlayerPosition = transform.position;
    }

    private void FixedUpdate()
    {
        RotateMapOnZ();

        if (lockPlayerHorizontalPosition)
        {
            LockPlayerHorizontalPosition();
        }
        else
        {
            ZeroPlayerHorizontalVelocity();
        }

        if (rb.useGravity && rb.linearVelocity.y < 0f)
        {
            rb.AddForce(Physics.gravity * (fallGravityMultiplier - 1f), ForceMode.Acceleration);
        }
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

    private void RotateMapOnZ()
    {
        if (mapRoot == null)
        {
            return;
        }

        float angle = moveInput.x * mapRotationSpeedDegreesPerSecond * Time.fixedDeltaTime;
        if (Mathf.Approximately(angle, 0f))
        {
            return;
        }


        mapRoot.Rotate(0f, 0f, -angle, Space.Self);
    }

    private void LockPlayerHorizontalPosition()
    {
        Vector3 pos = rb.position;
        pos.x = lockedPlayerPosition.x;
        pos.z = lockedPlayerPosition.z;
        rb.MovePosition(pos);

        ZeroPlayerHorizontalVelocity();
    }

    private void ZeroPlayerHorizontalVelocity()
    {
        Vector3 v = rb.linearVelocity;
        v.x = 0f;
        v.z = 0f;
        rb.linearVelocity = v;
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