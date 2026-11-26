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

    public Camera viewCam;
    public Rigidbody theRB;

    private Vector2 mouseInput;
    public float mouseSensitivity = 0.1f;
    private float moveSpeed;
    public float walkSpeed = 3f;
    public float sprintSpeed = 5f;

    public float jumpBuffer = 0.5f;
    public float sprintTickRate = 0.2f;

    private bool jumpPressed;
    public bool isGrounded;
    public bool usedStamina;
    private float absMin;

    private float lastJump;
    private float lastSprint;

    public float jumpVelocity = 5f;
    public float staminaSprint = -1f;
    public float staminaJump = -3f;
    public float oxygenJump = -1f;


    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isGrounded = false;
        usedStamina = false;
        absMin = 0.01f;
        lastJump = 0f;
        lastSprint = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();

        //player view
        mouseInput = new Vector2(playerLook.action.ReadValue<Vector2>().x, playerLook.action.ReadValue<Vector2>().y) * mouseSensitivity;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        viewCam.transform.localRotation = Quaternion.Euler(viewCam.transform.localRotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f)); 

        //Debug.Log(theRB.velocity.y);

        if (playerJump.action.WasPressedThisFrame()){
            jumpPressed = true;
        }
        
        if (!isGrounded){
            usedStamina = true;
        }

        // for interact raycast used 
        // if distance < reach && tag == interactable && interactable object mass < Maxlift && interact.action.waspressedthisframe
            // player can interact
        // else 
            // player can only push 
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
        if (playerSprint.action.IsPressed() && PlayerInformation.instance.currentStamina > 0){
            moveSpeed = sprintSpeed;
            if (Math.Abs(theRB.velocity.x) > absMin || Math.Abs(theRB.velocity.z) > absMin){
                usedStamina = true;
                if (Time.time - lastSprint > sprintTickRate){
                    PlayerInformation.instance.StaminaChange(staminaSprint);
                    lastSprint = Time.time;
                }
            }
            else{
                usedStamina = false;
            }
        }
        else{
            moveSpeed = walkSpeed;
            usedStamina = false;
        }
    }

    public void Jump(){
        if (!isGrounded && Math.Abs(theRB.velocity.y) < absMin  && Time.time - lastJump > jumpBuffer){
            isGrounded = true;
        }
        else if (isGrounded && jumpPressed){
            Vector3 tempJump = theRB.velocity;
            tempJump.y = 5f;
            theRB.velocity = tempJump;
            isGrounded = false;
            lastJump = Time.time;
            PlayerInformation.instance.StaminaChange(staminaJump);
            PlayerInformation.instance.OxygenChange(oxygenJump);
        }
        jumpPressed = false;
    }
}
