using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Camera playerCam;
    [SerializeField] private Transform playerDirection;
    private float moveInputx;
    private float moveInputz;

    private float mouseInputX;
    private float mouseInputY;
    private float rotateX;
    private float rotateY;
    const float MOVESPEED = 10;
    const float LOOKSPEED = 4;

    //Player camera and movement controls adapted from this tutorial
    //Player goes faster when going diagonal
    //https://www.youtube.com/watch?v=f473C43s8nE
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        MovePlayer();
    }

    void MoveCamera()
    {
        mouseInputX = Input.GetAxis("Mouse X") * LOOKSPEED;
        mouseInputY = Input.GetAxis("Mouse Y") * LOOKSPEED;

        rotateY += mouseInputX;
        rotateX -= mouseInputY;

        rotateX = Mathf.Clamp(rotateX, -90f, 90f);
        playerCam.transform.eulerAngles = new Vector3(rotateX, rotateY, 0);
        playerDirection.eulerAngles = new Vector3(0, rotateY, 0);
    }

    void MovePlayer()
    {
        //INPUT AXIS
        /*
        //either continue with Input.GetAxis
        //or we handle the pressed keys individually
        moveInputx = Input.GetAxis("Horizontal");
        moveInputz = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(-moveInputx, 0, -moveInputz);

        rb.AddForce((playerDirection.forward * moveInputz + playerDirection.right * moveInputx) * MOVESPEED);

        if ((moveInputx == 0) && (moveInputz == 0))
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
        */

        //KEY HANDLING
        if (Input.GetKey(KeyCode.W))
        {
            moveInputz = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveInputz = -1;
        }
        else
        {
            moveInputz = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveInputx = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInputx = 1;
        }
        else
        {
            moveInputx = 0;
        }

        Vector3 dir = new Vector3(moveInputx, 0, moveInputz);

        rb.AddForce((playerDirection.forward * moveInputz + playerDirection.right * moveInputx) * MOVESPEED);
        if ((moveInputx == 0) && (moveInputz == 0))
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }
}
