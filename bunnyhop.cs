using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovement;

public class bunnyhop : MonoBehaviour
{
    private Rigidbody rb;
    public PlayerMovement pm;
    public float bunspeed;
    public float bun2speed;
    public float mouseX;
    public bool isbunny;
    public float cooldown;
    public float time;
    public int times=0;
    bool jumpedonce;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        bunspeed = pm.runSpeed2;
        bun2speed = pm.walkSpeed2;
    }

    // Update is called once per frame
    void Update()
    {
        
        mouseX = Input.GetAxisRaw("Mouse X");
        if (pm.grounded && pm.jumping && mouseX !=0f && time>0.02f)
        {
            time = 0;
            times++;
            if(times >=4)
            {
                pm.state = MovementState.bunnyjumping;
                isbunny = true;
            }
            if (pm.running)
            {
                pm.runSpeed = pm.runSpeed+(pm.runSpeed2 * 0.1f);
            }
            else
            {
                pm.walkSpeed = pm.walkSpeed+(pm.walkSpeed2 * 0.1f);
            }
        }
        else if(!pm.grounded)
        {
            isbunny = false;
            if (pm.walkSpeed>bun2speed)
            {
                pm.walkSpeed = pm.walkSpeed-0.01f;
            }
            else if (pm.walkSpeed<bun2speed+1f&&pm.walkSpeed>bun2speed)
            {
                pm.walkSpeed = bun2speed;
            }
            if (pm.runSpeed > bunspeed)
            {
                pm.runSpeed = pm.runSpeed - 0.01f;
            }
            else if (pm.runSpeed < bunspeed + 1f && pm.runSpeed > bunspeed)
            {
                pm.runSpeed2 = bunspeed;
            }
        }
        else if (!pm.jumping)
        {
            pm.walkSpeed = pm.walkSpeed2;
            pm.runSpeed = pm.runSpeed2;

        }
        delays();
    }
    public void delays()
    {
        time += Time.deltaTime;
        if(time > 0.5f)
        {
        }
    }
        
}
