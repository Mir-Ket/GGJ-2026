using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public Transform orientation;

    [Header("Movement")]
    public float moveSpeed;
    public float groundLinearDamp;
    public bool limitSpeed = true;

    //Jumping
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool canJump= true;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Inputs")]
    public InputActionReference moveInput;
    public InputActionReference jump;
    float horizontalInput;
    float vertialInput;
    Vector2 inputVector;
    Vector3 moveDirection;


    


    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        //Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        //For Debug
        Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f), Color.red);

        GetInput();
        if(limitSpeed)
        {
            ControlSpeed();
        }

        //Handle Drag
        if (isGrounded)
        {
            rb.linearDamping = groundLinearDamp;

        }
        else
        {
            rb.linearDamping = 0;

        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        inputVector = moveInput.action.ReadValue<Vector2>();
        
        if(jump.action.WasPerformedThisFrame() && canJump && isGrounded)
        {
            Debug.Log("Jumped.");
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);

        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * inputVector.y + orientation.right * inputVector.x;

        //on ground
        if(isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        }

    }

    private void ControlSpeed()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //limit velocity

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }


    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }

}
