using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using static PlayerMovement;
using System.Threading;

public class reversedash : MonoBehaviour
{
    public Transform orientationr;
    public Transform playercamr;
    public Rigidbody rbr;
    public PlayerMovement pmr;
    public bool isdashingr;

    public float dashpowerr;
    public float dashuppowerr;
    public float dashdurationr;
    public float dashspeedr;
    public float delayr;

    public bool cooldownr;
    public float dashtimerr;

    public KeyCode dashkey = KeyCode.E;
    // Start is called before the first frame update
    void Start()
    {
        rbr = GetComponent<Rigidbody>();
        pmr = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownr == false)
        {
            Dashcoolr();
        }
        if (Input.GetKeyUp(dashkey) && cooldownr == true)
        {
            cooldownr = false;
            Dashr();
        }
    }
    public void Dashr()
    {
        if (pmr.running == true)
        {
            pmr.desiredSpeed = dashspeedr + pmr.runSpeed;
        }
        else if (pmr.walking == true)
        {
            pmr.desiredSpeed = dashspeedr + pmr.walkSpeed;
        }
        else if (pmr.standing == true)
        {
            pmr.desiredSpeed = dashspeedr;
        }
        isdashingr = true;
        pmr.desiredSpeed = dashspeedr;
        Vector3 forcetoapply = orientationr.forward * -dashpowerr + orientationr.up * dashuppowerr;
        rbr.AddForce(forcetoapply, ForceMode.Impulse);
        Invoke(nameof(ResetDashr), dashdurationr);
    }
    public void ResetDashr()
    {
        isdashingr = false;
    }
    public void Dashcoolr()
    {
        delayr += Time.deltaTime;
        if (delayr >= dashtimerr)
        {
            delayr = 0;
            cooldownr = true;
        }
    }
}

