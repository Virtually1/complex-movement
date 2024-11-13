using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercam : MonoBehaviour
{
    public float senY;
    public float senX;
    public float mouseX;
    public float mouseXlast;
    public float mouseY;
    public Transform orientation;
    public Transform playerobj;
    float xRotate;
    float yRotate;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * senX * Time.deltaTime;
        mouseY = Input.GetAxisRaw("Mouse Y") * senY * Time.deltaTime;
        if(mouseX!=0)
        {
            mouseXlast=mouseX;
        }
        yRotate += mouseX;
        xRotate -= mouseY;
        xRotate = Mathf.Clamp(xRotate, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRotate, yRotate, 0);
        orientation.rotation = Quaternion.Euler(0, yRotate, 0);
        playerobj.rotation = Quaternion.Euler(0, yRotate-90f, 0);

    }
}
