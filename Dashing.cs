using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using static PlayerMovement;
using System.Threading;

public class Dashing : MonoBehaviour
{
    public Transform orientation;
    public Transform playercam;
    public Rigidbody rb;
    public PlayerMovement pm;

    public float dashpower;
    public float dashuppower;
    public float dashduration;
    public float dashspeed;
    public float delay;

    public bool cooldown;
    public float dashtimer;
    public bool isdashing;

    public KeyCode dashkey = KeyCode.E;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown == false)
        {
            Dashcool();
        }
        if(Input.GetKeyUp(dashkey) && cooldown==true)
        {
            cooldown = false;
            Dash();
        }
    }
    public void Dash()
    {
        if (pm.running == true)
        {
            pm.desiredSpeed = dashspeed + pm.runSpeed;
            pm.cam.fieldOfView= 89f+pm.runSpeed;
        }
        else if (pm.walking == true)
        {
            pm.desiredSpeed = dashspeed + pm.walkSpeed;
            pm.cam.fieldOfView = 89f + pm.walkSpeed;
        }
        else if(pm.standing == true)
        {
           pm.desiredSpeed = dashspeed;
            pm.cam.fieldOfView = 89;
        }
        isdashing = true;
        Vector3 forcetoapply = orientation.forward * dashpower + orientation.up * dashuppower;
        rb.AddForce(forcetoapply, ForceMode.Impulse);
        Invoke(nameof(ResetDash), dashduration);    }
    public void ResetDash()
    {
        isdashing = false;
    }
    public void Dashcool()
    {
        delay += Time.deltaTime;
        if(delay >= dashtimer)
        {
            delay = 0;
            cooldown = true;
        }
    }
}
