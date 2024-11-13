using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using TMPro;
using TMPro.Examples;
using UnityEngine.Rendering;
public class walljump : MonoBehaviour
{
    public LayerMask wall;
    public bool hitawallright;
    public bool hitawallleft;
    public bool hitawallfow;
    public bool hitawallback;
    public float playerheight;
    public bool walljumable;
    public bool walljumablel;
    public bool walljumables;
    public bool walljumablesl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hitawallright = Physics.Raycast(transform.position, Vector3.left, playerheight+0.3f, wall);
        hitawallleft = Physics.Raycast(transform.position, Vector3.right, playerheight + 0.3f, wall);
        hitawallback = Physics.Raycast(transform.position, Vector3.forward, playerheight + 0.3f, wall);
        hitawallfow = Physics.Raycast(transform.position, Vector3.back, playerheight + 0.3f, wall);

        if (hitawallright)
        {
             walljumable = true;
        }
        else if (!hitawallright)
        {
            walljumable = false;
        }
        if (hitawallleft)
        {
            walljumablel = true;
        }
        else if(!hitawallleft)
        {
            walljumablel = false;
        }
        if(hitawallback)
        {
            walljumables = true;
        }
        else if(!hitawallback)
        {
            walljumables= false;
        }
        if (hitawallfow) 
        {
            walljumablesl = true;
        }
        if(!hitawallfow)
        {
            walljumablesl= false;
        }
    }
}
