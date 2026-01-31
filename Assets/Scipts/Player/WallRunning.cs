using UnityEngine;
using UnityEngine.InputSystem;
public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallClimbForce;
    public float wallClimbSpeed;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;
    public InputActionReference movement;
    public InputActionReference upwardRunInput;
    public InputActionReference downwardRunInput;
    public InputActionReference jump;
    private bool isUpwardsRunning;
    private bool isDownwardsRunning;
    private Vector2 movementDirection;


    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("References")]
    public Transform orientation;
    private PlayerMovement pm;
    private Rigidbody rb;
    public PlayerCamera cam;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if(pm.wallRunning)
        {
            WallRunningMovement();
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);

    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        //Getting Inputs
        horizontalInput = movement.action.ReadValue<Vector2>().x;
        verticalInput = movement.action.ReadValue<Vector2>().y;

        isUpwardsRunning = upwardRunInput.action.IsPressed();
        isDownwardsRunning = downwardRunInput.action.IsPressed();

        // State 1 - Wallrunning

        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
        {
            // start wallrun
            if(!pm.wallRunning)
            {
                StartWallRun();
            }

            //wallrun timer
            if(wallRunTimer > 0)
            {
                wallRunTimer -= exitWallTime;
            }

            if(wallRunTimer <= 0 && pm.wallRunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            if(jump.action.WasPressedThisFrame())
            {
                WallJump();
            }
            

        }

        //state 2 - Exiting
        else if (exitingWall)
        {
            if(pm.wallRunning)
            {
                StopWallRun();
            }
            if(exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }
            if(exitWallTimer <= 0)
            {
                exitingWall = false;
            }
        }

        //State 3 - None
        else
        {
            StopWallRun();
        }

    }

    private void StartWallRun()
    {
        pm.wallRunning = true;

        wallRunTimer = maxWallRunTime;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // apply camera effects
        if (cam == null) return;
        cam.DoFov(90f);
        if (wallLeft) cam.DoTilt(-5f);
        if (wallRight) cam.DoTilt(5f);

    }

    private void WallRunningMovement()
    {
        rb.useGravity = useGravity;
        //rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        // forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // upwards/downwards force
        if (isUpwardsRunning)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, wallClimbSpeed, rb.linearVelocity.z);
        if (isDownwardsRunning)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -wallClimbSpeed, rb.linearVelocity.z);


        //push to wall force (This part is buggy. Player sticks to wall. Needs fixed)
        /*
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);

        }
        */

        if(useGravity)
        {
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
        }

    }

    private void StopWallRun()
    {
        pm.wallRunning = false;
        rb.useGravity = true;

        if (cam == null) return;
        cam.DoFov(cam.getFovValue());
        cam.DoTilt(0f);

    }

    private void WallJump()
    {
        //enter exiting wall state
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        // reset y velocity and add force
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

    }
}
