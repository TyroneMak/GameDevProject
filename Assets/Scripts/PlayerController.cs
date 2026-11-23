using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public InputActionReference playerMove;
    public InputActionReference playerLook;
    public InputActionReference playerSprint;
    public InputActionReference playerJump;
    public InputActionReference playerInteract;

    private Vector2 mouseInput;
    public Camera viewCam;
    public Rigidbody theRB;
    public float moveSpeed = 3f;
    public float mouseSensitivity = 0.1f;

    public float buffer = 0.5f;
    private float last = 0f;
    public bool isGrounded;
    public bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        isGrounded = false;
        isSprinting = false;
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();

        //player view
        mouseInput = new Vector2(playerLook.action.ReadValue<Vector2>().x, playerLook.action.ReadValue<Vector2>().y) * mouseSensitivity;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        viewCam.transform.localRotation = Quaternion.Euler(viewCam.transform.localRotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f)); 

        Debug.Log(theRB.velocity.y);
    }


    // fixed rate (50 per sec) 
    // for physics so isnt tied to framerate (e.g. fallout 4)
    void FixedUpdate()
    {
        //player movement
        Vector2 move = playerMove.action.ReadValue<Vector2>();
        Vector3 direction = (transform.forward * move.y + transform.right * move.x) * moveSpeed;
        Vector3 tempMove = theRB.velocity; 
        tempMove.x = direction.x;
        tempMove.z = direction.z;
        theRB.velocity = tempMove;

        Jump();
    }

    public void Sprint(){
        if (playerSprint.action.IsPressed() /*&& PlayerInformation.instance.stamina > 0*/){
            moveSpeed = 5f;
            isSprinting = true;
            // PlayerInformation.instance.stamina
        }
        else{
            moveSpeed = 3f;
            isSprinting = false;
        }
    }

    public void Jump(){
        if (!isGrounded && Math.Abs(theRB.velocity.y) < 0.01f && Time.time - last > buffer){
            isGrounded = true;
        }
        else if (isGrounded && playerJump.action.WasPressedThisFrame()){
            Vector3 tempJump = theRB.velocity;
            tempJump.y = 5f;
            theRB.velocity = tempJump;
            isGrounded = false;
            last = Time.time;
            // PlayerInformation.instance.stamina -= 3;
            // PlayerInformation.instance.oxygen --;
        }
    }
}
