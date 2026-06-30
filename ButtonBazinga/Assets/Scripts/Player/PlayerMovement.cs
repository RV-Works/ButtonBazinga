using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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

    [SerializeField] private Transform playerVisual;
    public bool isGrappling = false;

    [SerializeField] private Animator animator;

    private float x_Movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        lockedPlayerPosition = transform.position;
    }

    private void Start()
    {
        GameManager.instance.getPlayerObject(gameObject);
    }

    private void Update()
    {
        OnKeyDown();
    }

    private void FixedUpdate()
    {
        RotateMapOnZ();

// rotating the world changes the contact angles under the player,
        // and they can get tiny sideways velocities from collisions.
             //yes i fucking wrote this myself before you think my comments are Ai generated. >:/
        if (!isGrappling)
        {
            if (lockPlayerHorizontalPosition)
            {
                LockPlayerHorizontalPosition();
            }
            else
            {
                ZeroPlayerHorizontalVelocity();
            }
        }

        if (isGrappling && rb.linearVelocity.y < 0f)
        {
            Vector3 v = rb.linearVelocity;
            v.y = 0f;
            rb.linearVelocity = v;
        }
        else if (rb.useGravity && rb.linearVelocity.y < 0f && !isGrappling)
        {
            rb.AddForce(Physics.gravity * (fallGravityMultiplier - 1f), ForceMode.Acceleration);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        x_Movement = context.ReadValue<Vector2>().x;
        moveInput = new Vector2(x_Movement, 0f);

        if(x_Movement != 0 && isGrounded)
        {
            animator.SetFloat("AnimationSpeed", 2);
        }
        else if(isGrounded && x_Movement == 0)
        {
            animator.SetFloat("AnimationSpeed", 1);
        }
        animator.SetFloat("X Walk", x_Movement);
        animator.SetFloat("X 2nd Walk", x_Movement);
        Debug.Log(x_Movement);
    }

    public System.Action onJumpEvent;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onJumpEvent?.Invoke();


            if (isGrounded == true)
            {
                rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                Debug.Log("Test");
                animator.SetFloat("AnimationSpeed", 3);
                animator.SetBool("Grounded", false);
                isGrounded = false;
            }
        }
    }

    public void RotateMap(float angle)
    {
        if (mapRoot != null)
        {
            mapRoot.Rotate(0f, 0f, angle, Space.Self);
        }
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
        mapRoot.Rotate(0f, 0f, angle, Space.Self);
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
            animator.SetBool("Grounded", true);
            if(x_Movement != 0)
            {
                animator.SetFloat("AnimationSpeed", 2);
            }
            else
            {
                animator.SetFloat("AnimationSpeed", 1);
            }
        }

        if (collision.gameObject.CompareTag("Water"))
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        SceneManager.LoadScene("You lost");
    }

    private void OnKeyDown()
    { 
        if (playerVisual == null) return;

        if (Keyboard.current.dKey.isPressed)
        {
            playerVisual.localRotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            playerVisual.localRotation = Quaternion.Euler(0f, -90f, 0f);
        }
        else
        {
            playerVisual.localRotation = Quaternion.Euler(0f, 180f, 0f);
            animator.SetFloat("Animation Speed", 1);
        }
    }
}