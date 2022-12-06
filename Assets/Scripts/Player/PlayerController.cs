using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using DeveloperConsole;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Basic movement settings")]
    public float walkSpeed;
    [SerializeField] float dashSpeed = 10;
    public float noclipSpeed = 5;
    [SerializeField] float jumpPower;
    [SerializeField] float wallJumpPower;
    [SerializeField] float airAcceleration;
    [SerializeField] float friction;
    [SerializeField] int MaxWalljumps = 3;
    [SerializeField] int WallJumpCount;

    [Header("Jumping")]
    [SerializeField] bool autoJump;
    [SerializeField] bool jump;
    [SerializeField] bool jumpQueued;

    [Header("ground detection")]
    public bool isGrounded;
    [SerializeField] float groundDistance;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;

    [Header("Slope Detection")]
   [SerializeField] float slopeHeight;
   [SerializeField] float slopeAngle;
    RaycastHit slopeHit;

    [Header("Dash")]
    [SerializeField] Vector3 DashVelocity;
    [SerializeField] float DashAmount;



    private Rigidbody rb;
    private CapsuleCollider collider;

    [SerializeField] Transform orientation;

    Vector3 moveVelocity;
    Vector3 savedVelocity;
    Vector3 moveVelocity2;
    Vector3 airVelocity;

    float horzontal;
    float vertical;

    bool crouching;
    bool sprinting;
    bool dashing;
    bool dead;

    //for wall detection
    CheckWall checkwall;
    

    Vector3 originalPos;
    Vector3 Poc;

    public float clingFade;

   [SerializeField] LayerMask wallMask;

    public static PlayerController instance;
    public bool onConsole;

    public bool noclip;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();

        checkwall = GetComponentInChildren<CheckWall>();

        originalPos = transform.position;
        jumpQueued = true;
    }

    // Update is called once per frame
    void Update()
    {



        if(!onConsole)
        {
            GetInput();
        }
        //QueueJump();


        //GroundCheck
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Resets player if they get too low
        if (transform.position.y < -170)
        {
            transform.position = originalPos;
        }
        //WallJump
        if(checkwall.onWall && !isGrounded && !noclip)
        {
 
            RaycastHit testHit;
            if (Physics.Raycast(transform.position, moveVelocity, out testHit, 1f, wallMask))
            {
                
                if (rb.velocity.y < -1f)
                {
                    rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -1f, 1f), -2f * clingFade, Mathf.Clamp(rb.velocity.z, -1f, 1f));
                }
                if (Input.GetButtonDown("Jump") && WallJumpCount < MaxWalljumps)
                {
                    WallJumpCount++;
                    jumpQueued = true;
                    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                    //Vector3 wallJumpPos = base.transform.position - checkwall.GetClosestPoint();

                    //Vector3 vector = new Vector3(wallJumpPos.normalized.x, 1f, wallJumpPos.normalized.z);
                    //rb.AddForce(vector * wallJumpPower * 60f);

                    Vector3 tempVec;
                    tempVec = new Vector3(testHit.normal.x, 1.5f, testHit.normal.z);
                    rb.AddForce(tempVec * 5, ForceMode.Impulse);
                    Invoke("ResetJump", 0.2f);
                }
            }
    

        }
        //Dash
        if (dashing && !noclip)
        {
            DashVelocity = moveVelocity;
            if(moveVelocity.x == 0 && moveVelocity.z == 0)
            {
                DashVelocity = orientation.transform.forward;
            }
            Dash();
        }
    }

    private void FixedUpdate()
    {
        if(!dead && !noclip)
        {
            Movement();
        }
        else if(!dead && noclip)
        {
            NoclipMovement();
        }
    }


    void Movement()
    {

        //movement for being grounded or in air
        if (isGrounded)
        {
            WallJumpCount = 0;
            moveVelocity2 = new Vector3(moveVelocity.x * walkSpeed, rb.velocity.y, moveVelocity.z * walkSpeed);

            if (crouching)
            {
                transform.localScale = new Vector3(transform.localScale.x, .5f, transform.localScale.z);
            }

            else
            {
                transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
            }

            if (jump && jumpQueued)
            {
                Jump();
            }

            if(!OnSlope())
            {
                rb.velocity = Vector3.Lerp(rb.velocity, moveVelocity2, 0.25f);
            }
            else
            {
                print("Onslope");
                rb.velocity = Vector3.Lerp(rb.velocity, SlopeMoveDirection() * walkSpeed, 0.25f);
            }

            //float speed = rb.velocity.magnitude;
            //if (speed != 0) // To avoid divide by zero errors
            //{
            //    float drop = speed * friction * Time.fixedDeltaTime;
            //    rb.velocity *= Mathf.Max(speed - drop, 0) / speed; // Scale the velocity based on friction.
            //}


            //rb.AddForce(moveVelocity2 * walkSpeed);
        }
        else
        {
            moveVelocity2 = new Vector3(moveVelocity.x * walkSpeed, rb.velocity.y, moveVelocity.z * walkSpeed);
            airVelocity.y = 0f;
            if ((moveVelocity2.x > 0f && rb.velocity.x < moveVelocity2.x) || (moveVelocity2.x < 0f && rb.velocity.x > moveVelocity2.x))
            {
                airVelocity.x = moveVelocity2.x;
            }
            else
            {
                airVelocity.x = 0f;
            }
            if ((moveVelocity2.z > 0f && rb.velocity.z < moveVelocity2.z) || (moveVelocity2.z < 0f && rb.velocity.z > moveVelocity2.z))
            { 
                airVelocity.z = moveVelocity2.z;
            }
            else
            {
                airVelocity.z = 0f;
            }
            rb.AddForce(airVelocity.normalized * airAcceleration * Time.deltaTime);
        }


    }

    void Dash()
    {
        moveVelocity2 = new Vector3(DashVelocity.x * dashSpeed, 0, DashVelocity.z * dashSpeed);
        rb.velocity = moveVelocity2 * 1.2f;
    }


    void Jump()
    {
        rb.AddForce(Vector3.up * jumpPower * 50);
        jumpQueued = false;
        if(!autoJump)
        {
            Invoke("ResetJump", 0.2f);
        }
        else
        {
            Invoke("ResetJump", 0.0f);
        }
    }

    void ResetJump()
    {

        jumpQueued = true;
    }


    void GetInput()
    {
        horzontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if(!noclip)
        {
            moveVelocity = new Vector3(horzontal, 0, vertical).normalized;
            moveVelocity = orientation.transform.TransformDirection(moveVelocity);
        }


        //moveVelocity = transform.TransformDirection(moveVelocity);

        crouching = Input.GetButton("Crouch");

        dashing = Input.GetButtonDown("Dash");

        jump = Input.GetButton("Jump");
    }

    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 2 * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            return angle < slopeAngle && angle != 0;
        }
        return false;
    }



  
    public void NoclipMovement()
    {
        orientation.transform.localRotation = Quaternion.Euler(CameraController.instance.rotX, CameraController.instance.rotY, 0);
        Vector3 noclipVelocity = new Vector3(horzontal, 0, vertical);
        noclipVelocity = orientation.transform.TransformDirection(noclipVelocity);
        rb.velocity = noclipVelocity * Time.fixedDeltaTime * noclipSpeed * 10 ;
    }

    private Vector3 SlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveVelocity, slopeHit.normal);
    }

    [ConCommand("set_speed", "Set speed of player")]
    public void SetSpeed(float speed = 9 )
    {
        instance.walkSpeed = speed;
        
    }

    [ConCommand("set_jump", "Set jump height of player")]
    public static void SetJump(float jumpPower = 8)
    {
        instance.jumpPower = jumpPower;
    }

    public  void EnableGravity(bool enable)
    {
        rb.useGravity = enable;
        Debug.Log(enable);
    }

    public  void EnableCollision(bool enable)
    {
        collider.enabled = enable;
        Debug.Log(enable);
    }
    
    
    
    


    //private void QueueJump()d
    //{
    //    if (autoJump)
    //    {
    //        jumpQueued = Input.GetButton("Jump");
    //        return;
    //    }

    //    if (Input.GetButtonDown("Jump") && !jumpQueued)
    //    {
    //        jumpQueued = true;
    //    }

    //    if (Input.GetButtonUp("Jump"))
    //    {
    //        jumpQueued = false;
    //    }
    //}
}
