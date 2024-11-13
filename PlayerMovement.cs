using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using TMPro;
using TMPro.Examples;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float walkSpeed2;
    public float runSpeed;
    public float runSpeed2;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public Transform orientation;
    public Dashing dash;
    public reversedash dashr;
    public bunnyhop bunnyhop;
    public walljump wall;
    public GameObject obj2;
    float horizontalaxis;
    float verticalaxis;
    Vector3 moveDirection;
    Rigidbody rb;
    public float groundDrag;
    [Header("GroundCheck")]
    public float playerheight;
    public LayerMask whatIsGround;
    public LayerMask whatIsRestartable;
    public bool grounded;
    public bool restartable;
    [Header("Jump")]
    public float timer;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public KeyCode jumpKey = KeyCode.Space;
    public MovementState state;
    [Header("Crouching")]
    public float crouchSpeed;
    public float YCScale;
    public float SCScale;
    public KeyCode crouchKey = KeyCode.LeftControl;
    [Header("Slope Control")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    [Header("SlopeAcceleration")]
    public float desiredSpeed;
    public float lastdesiredSpeed;
    public float slideSpeed;
    public bool sliding;
    public float speedIncrease;
    public float slopeIncrease;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI mode;
    [Header("Bools")]
    public bool running;
    public bool walking;
    public bool jumping;
    public bool bunnyjump;
    public bool standing;
    [Header("RestartTeleport")]
    public Transform teleportpos;
    public GameObject obj;
    [Header("FOVChange")]
    public Camera cam;
    public float cameratransi;
    public float cameratransiSpeed;
    public float forta;
    public float forta2;
    public float forta3;
    public float forta4;

    public enum MovementState
    {
        walking,
        running,
        crouching,
        sliding,
        air,
        dashing,
        standing,
        bunnyjumping
    }
    // Start is called before the first frame update

    void Start()
    {
        walkSpeed = walkSpeed2;
        runSpeed = runSpeed2;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        SCScale = transform.localScale.y;
        dash=GetComponent<Dashing>();
        dashr=GetComponent<reversedash>();
        bunnyhop=GetComponent<bunnyhop>();
        wall=GetComponent<walljump>();
        cam.fieldOfView = 80f;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position,Vector3.down,playerheight*0.5f+0.05f,whatIsGround);
        restartable = Physics.Raycast(transform.position, Vector3.down, playerheight * 0.5f + 0.05f,whatIsRestartable);
        if (restartable == true)
        {
            desiredSpeed = 0;
            obj.transform.position = teleportpos.transform.position;
        }
        speed.SetText(desiredSpeed.ToString());
        mode.SetText(state.ToString());
        MyInput();
        SpeedLimiter();
        StateHandler();
        FOVChange();
        if(grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MyInput()
    {
        horizontalaxis = Input.GetAxisRaw("Horizontal");
        verticalaxis = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            bunnyjump = true;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        else if (grounded)
        {
            delay();

            if (bunnyjump == false)
            {
                jumping = false;
            }
        }
        else if (!grounded) 
        {
            timer = 0;
        }
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x,YCScale,transform.localScale.z);
            if(grounded)
            rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
        }
        if(Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, SCScale, transform.localScale.z);
        }
        if (Input.GetKeyDown(jumpKey)&&readyToJump&& (wall.walljumable||wall.walljumablesl))
        {
            readyToJump = false;
            walljumpr();
            Invoke(nameof(ResetJump),0.1f);
        }
        if (Input.GetKeyDown(jumpKey) && readyToJump &&( wall.walljumablel||wall.walljumables))
        {
            readyToJump = false;
            walljumpl();
            Invoke(nameof(ResetJump), 0.1f);
        }
    }
    private void StateHandler()
    {
        if (sliding)
        {
            state = MovementState.sliding;
            if (OnSlope() && rb.velocity.y < 0.1f)
                desiredSpeed = slideSpeed;
            else
                desiredSpeed = runSpeed;
            cam.fieldOfView = 87f;
        }
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredSpeed = crouchSpeed;
            running = false;
            walking = false;
            standing = false;
        }
        else if (grounded && Input.GetKey(sprintKey) && (horizontalaxis != 0 || verticalaxis != 0))
        {
            state = MovementState.running;
            desiredSpeed = runSpeed;
            cam.fieldOfView = 89f;
            running = true;
            walking = false;
            standing = false;
        }
        else if (grounded&&(horizontalaxis!=0||verticalaxis!=0))
        {
            state = MovementState.walking;
            desiredSpeed = walkSpeed;
            if (cam.fieldOfView > 81f)
                cam.fieldOfView = cam.fieldOfView - 0.2f;
            if (cam.fieldOfView > 80f && cam.fieldOfView < 81)
                cam.fieldOfView = 80f;
            
            running = false;
            walking = true;
            standing = false;

        }
        else if (!grounded&&dash.isdashing==false&&!bunnyhop.isbunny)
        {
            state = MovementState.air;
            rb.AddForce(Vector3.down * 1f, ForceMode.Force);
        }
        else if(grounded && (horizontalaxis == 0 || verticalaxis == 0))
        {
            if (cam.fieldOfView > 81f)
              cam.fieldOfView = cam.fieldOfView - 0.2f;
            if (cam.fieldOfView > 80f && cam.fieldOfView < 81)
                cam.fieldOfView = 80f;
            state = MovementState.standing;
            desiredSpeed = walkSpeed;
            standing = true;
            running = false;
            walking = false;
        }
        if (dash.isdashing||dashr.isdashingr)
        state = MovementState.dashing;
    }
    private IEnumerator SmoothlyLerpSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredSpeed - moveSpeed);
        float startValue = moveSpeed;
        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredSpeed, time / difference);
            if(OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up,slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);
                time += Time.deltaTime * speedIncrease * slopeIncrease*slopeAngleIncrease;

            }
            else
                time += Time.deltaTime*speedIncrease;

            yield return null;
        }
        moveSpeed = desiredSpeed;
    }
    //
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalaxis + orientation.right * horizontalaxis;

        if(grounded||wall.walljumablel||wall.walljumable)
            rb.AddForce(moveDirection.normalized * desiredSpeed * 10f, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * desiredSpeed * 10f * airMultiplier ,ForceMode.Force);
        
    }
    //
    private void SpeedLimiter()
    {
        if (OnSlope())
        {
            if(rb.velocity.magnitude>desiredSpeed)
             rb.velocity = rb.velocity.normalized*moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVel.magnitude > desiredSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * desiredSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up*jumpForce,ForceMode.Impulse);
        jumping = true;
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerheight * 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up,slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;   
    }
    public void delay()
    {
        timer += Time.deltaTime;
        if (timer > 0.15f) 
        {
            timer = 0;
            bunnyjump = false;
        }
    }
    public Vector3 GetSlopeDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
    public void FOVChange()
    {
        if(cam.fieldOfView>81f&&grounded)
        {
            cam.fieldOfView = cam.fieldOfView - 0.1f;
        }
    }
    public void walljumpr()
    {
            rb.AddForce(transform.up * jumpForce*1.7f, ForceMode.Impulse);
            rb.AddForce(100f, 0, 0, ForceMode.Impulse);
            desiredSpeed = 10f;
            jumping = true;
    }
    public void walljumpl()
    {
            rb.AddForce(transform.up * jumpForce*1.7f, ForceMode.Impulse);
            rb.AddForce(-100f, 0, 0, ForceMode.Impulse);
            desiredSpeed = 10f;
            jumping = true;
    }
}
