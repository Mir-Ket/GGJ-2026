using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //ZAMANA BAGLI IVMELENME MEKANIGI EKLENMEDI. ONU YARIN EKLE VE KONTROL ET.

    Rigidbody rb;
    public Transform orientation;

    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float wallRunSpeed;
    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;
    public float desiredMoveSpeed;
    public float lastDesiredMoveSpeed;

    public float groundLinearDamp;
    public bool limitSpeed = true;

    

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool canJump= true;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Camera Effects")]
    public PlayerCamera cam;
    public float grappleFov = 95f;
    private float startFov;



    [Header("Inputs")]
    public InputActionReference moveInput;
    public InputActionReference jumpInput;
    public InputActionReference sprintInput;
    public InputActionReference crouchInput;
    float horizontalInput;
    float vertialInput;
    Vector2 inputVector;
    Vector3 moveDirection;

    public bool testSlope;
    public enum MovementStates
    {
        WALK, SPRINT, AIR, CROUCH, SLIDE, WALLRUN, FREEZE
    }

    public MovementStates state;
    public bool sliding;
    public bool wallRunning;
    public bool freeze;
    public bool activeGrapple;
    private bool enableMovementOnNextTouch;


    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canJump = true;

        startYScale = transform.localScale.y;
        startFov = cam.getFovValue();

    }

    private void Update()
    {
        testSlope = OnSlope();
        //Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        //For Debug
        Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f), Color.red);

        GetInput();
        if(limitSpeed)
        {
            ControlSpeed();
        }
        HandleState();

        //Handle Drag
        if (isGrounded && !activeGrapple)
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

    private void HandleState()
    {
        //Mode - Freeze
        if (freeze)
        {
            state = MovementStates.FREEZE;
            moveSpeed = 0;
            rb.linearVelocity = Vector3.zero;
        }
        else if (wallRunning)
        {
            state = MovementStates.WALLRUN;
            desiredMoveSpeed = wallRunSpeed;
        }
        //Crouching Mode
        else if (crouchInput.action.IsPressed())
        {
            state = MovementStates.CROUCH;
            desiredMoveSpeed = crouchSpeed;
        }
        else if (sliding)
        {
            state = MovementStates.SLIDE;

            if (OnSlope() && rb.linearVelocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }
        //Sprinting Mode
        else if (isGrounded && sprintInput.action.IsPressed())
        {
            state = MovementStates.SPRINT;
            desiredMoveSpeed = sprintSpeed;

        }
        //Walking Mode
        else if (isGrounded)
        {
            state = MovementStates.WALK;
            desiredMoveSpeed = walkSpeed;
        }

        //Air Mode
        else
        {
            state = MovementStates.AIR;


        }

        

        

        
        

        //Check if desiredMoveSpeed has changed drastically
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

            lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        //Smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while(time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if(OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
            {
                time += Time.deltaTime * speedIncreaseMultiplier;
            }
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void GetInput()
    {
        inputVector = moveInput.action.ReadValue<Vector2>();
        
        if(jumpInput.action.WasPerformedThisFrame() && canJump && isGrounded)
        {
            //Debug.Log("Jumped.");
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        else if(jumpInput.action.WasPerformedThisFrame() && canJump && OnSlope())
        {
            //Debug.Log("Jumped.");
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (crouchInput.action.WasPerformedThisFrame())
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if(crouchInput.action.WasReleasedThisFrame())
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

        }

    }

    private void MovePlayer()
    {
        //Calculate movement direction
        moveDirection = orientation.forward * inputVector.y + orientation.right * inputVector.x;

        //on Slope
        if(OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f,ForceMode.Force);

            if(rb.linearVelocity.y > 0)
            {
                //It was 80f before
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }


        //on ground
        if(isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!isGrounded)
        {
            //in air
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        }

        rb.useGravity = !OnSlope();
        

    }

    private void ControlSpeed()
    {
        if(activeGrapple)
        {
            return;
        }

        //Limit speed on slope
        if(OnSlope() && !exitingSlope)
        {
            if(rb.linearVelocity.magnitude > moveSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }

        //Limit speed on air or on ground
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            //limit velocity

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
        


    }

    private void Jump()
    {
        exitingSlope = true;

        //reset y linear velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
    }

    private void ResetJump()
    {
        exitingSlope = false;
        canJump = true;
    }

    private Vector3 velocityToSet;
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight )
    {
        activeGrapple = true;
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);

        Invoke(nameof(SetVelocity),0.1f);
        Invoke(nameof(ResetRestrictions), 3f);
        
    }


    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.linearVelocity = velocityToSet;

        cam.DoFov(grappleFov);
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
        cam.DoFov(85f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();
            GetComponent<Grappling>().StopGrapple();
        }
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

}
