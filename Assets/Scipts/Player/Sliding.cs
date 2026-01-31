using UnityEngine;
using UnityEngine.InputSystem;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Sliding Parameters")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;
    bool isSliding;

    [Header("Input")]
    public InputActionReference slideInput;
    public InputActionReference moveInput;
    Vector2 moveVector;
    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        pm = this.gameObject.GetComponent<PlayerMovement>();

        startYScale = playerObj.localScale.y;
    }

    private void Update()
    {
        moveVector = moveInput.action.ReadValue<Vector2>();

        if(slideInput.action.WasPerformedThisFrame() && (moveInput.action.ReadValue<Vector2>().x != 0 || moveInput.action.ReadValue<Vector2>().y != 0))
        {
            StartSlide();
        }
        if(slideInput.action.WasReleasedThisFrame())
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if(isSliding)
        {
            SlidingMovement();
        }
    }

    private void StartSlide()
    {
        isSliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        //Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //Be careful down here
        //BUG: player bumps when sliding and cancelling sliding. LOOK FIX
        Vector3 inputDirection = orientation.forward * moveVector.y + orientation.right * moveVector.x;

        //Slide normal
        if (!pm.OnSlope() || rb.linearVelocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }

        //slide down a slope

        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }



        if (slideTimer <= 0)
        {
            StopSlide();
        }

    }

    private void StopSlide()
    {
        isSliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }




}
