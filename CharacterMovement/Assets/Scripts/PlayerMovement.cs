using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerControls controls;
    public CharacterController controller;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Transform cam;
    public float gravity = -9.81f;
    public float sprintSpeed = 12f;
    public float jumpforce = 4f;
    public float crouchSpeed = 3f;
    public float atkDmg = 50f;

    Animator anim;
    Vector2 move;
    Vector2 rotate;
    Vector3 velocity;
    Rigidbody rb;
    private bool isJump = false;
    private bool isSprint = false;
    private bool isCrouch = false;
    private float canJump;
    private float canJumpCD = -1f;

    void Awake()
    {
        anim = GetComponent<Animator>();    
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControls();
        canJump = jumpforce / 2.5f;//might need to change depending on jumpforce

        controls.Gameplay.Attack.performed += ctx => Attack();

        controls.Gameplay.Jump.performed += ctx => isJump = true;

        controls.Gameplay.Sprint.performed += ctx => isSprint = true;
        controls.Gameplay.Sprint.canceled += ctx => isSprint = false;

        controls.Gameplay.Crouch.performed += ctx => isCrouch = true;
        controls.Gameplay.Crouch.canceled += ctx => isCrouch = false;

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.performed += ctx => Walk();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Move.canceled += ctx => Idle();

        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;
    }

    void Update()
    {
        Vector3 direction = new Vector3(move.x, 0f, move.y).normalized;

        //if holding the move buttons
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);//rotate character

            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            float movSpd = 0f;

            if (isCrouch)
            {
                movSpd = crouchSpeed;
            }
            else if(isSprint)
            {
                movSpd = sprintSpeed;
            }
            else
            {
                movSpd = speed; 
            }

            controller.Move(moveDir.normalized * movSpd * Time.deltaTime);//move character
        }
        //gravity
        velocity.y += gravity * Time.deltaTime;

        if(isJump && Time.time > canJumpCD)
        {
            canJumpCD = Time.time + canJump;
            velocity.y = jumpforce;
            isJump = false;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void OnEnable() 
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Walk()
    {
        anim.SetTrigger("Walk");
    }

    void Idle() 
    {
        anim.SetBool("Walk", false);
    }

    void Atk()
    {
        Debug.Log("attack");
        anim.SetTrigger("Fight");
        Idle();    
    }

    private void Attack()
    {
        // Atk();
        BoxCollider box = new BoxCollider();
        gameObject.AddComponent<BoxCollider>();
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        gameObject.GetComponent<BoxCollider>().size = new Vector3(2f, 2f, 0.9f);
        gameObject.GetComponent<BoxCollider>().center = new Vector3(0f, 1.1f, 0.6f);
    }

    void OnTriggerEnter(Collider collider) 
    {        
        if (collider.transform.tag == "Enemy")
        {
            EnemyScript es = collider.GetComponent<EnemyScript>();

            if (es != null) {
                es.takeDmg(atkDmg);
            }
        }

        Destroy(gameObject.GetComponent<BoxCollider>());
    }

}
