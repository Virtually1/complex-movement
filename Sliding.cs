using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    public Transform orientation;
    public Transform playerObj;
    public Rigidbody rb;
    public PlayerMovement pm;
    [Header("Sliding")]
    public float maxSlidingTime;
    public float slideForce;
    private float slideTimer;
    public float slideYScale;
    public float startyScale;
    public KeyCode slideKey = KeyCode.X;
    private float horizontalInput;
    private float verticalInput;
    public LayerMask whatIsGround;
    public float slidercooldown;
    public float slidercooldownr;
    public int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        startyScale = playerObj.localScale.y;
        slidercooldown = 1;
    }
    async void Update()
    {
        if (i < slidercooldownr && slidercooldown == 0)
        {
            Task.Delay(10);
            i++;
        }
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0) && slidercooldown == 1)
            StartSlide();
        if (Input.GetKeyUp(slideKey) && (horizontalInput != 0 || verticalInput != 0))
            StopSlide();

        if (i == slidercooldownr && slidercooldown==0)
        {
            slidercooldown = 1;
            i = 0;
        }
    }
    private void FixedUpdate()
    {
        if (pm.sliding)
            SlidingMovement();
    }
    private void StartSlide()
    {
        pm.sliding = true;
        playerObj.localScale = new Vector3 (playerObj.localScale.x,slideYScale,playerObj.localScale.z);
        rb.AddForce(Vector3.down * 10f, ForceMode.Force);
        slideTimer = maxSlidingTime;
    }
    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward *verticalInput +orientation.right *horizontalInput;
        if(!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Impulse);
            slideTimer -= Time.deltaTime;
        }
        else
        {
            rb.AddForce(pm.GetSlopeDirection(inputDirection) * slideForce, ForceMode.Impulse);
            rb.AddForce(Vector3.down * 18888f, ForceMode.Force);
        }
        if (slideTimer <= 0)
            StopSlide();

    }
    private void StopSlide()
    {
        pm.sliding =false;
        playerObj.localScale = new Vector3(playerObj.localScale.x,startyScale, playerObj.localScale.z);
        slidercooldown = 0;
    }
}
