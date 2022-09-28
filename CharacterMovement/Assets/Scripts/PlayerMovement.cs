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

    Animator anim;
    Vector2 move;
    Vector2 rotate;

    void Awake()
    {
        anim = GetComponent<Animator>();    

        controls = new PlayerControls();

        // controls.Gameplay.Grow.performed += ctx => Grow();

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.performed += ctx => Walk();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Move.canceled += ctx => Idle();

        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;

    }

    // void Grow() 
    // {
    //     transform.localScale *= 1.1f;
    // }

    void Update()
    {
        // var keyboard = Keyboard.current;
        // if (keyboard != null)
        // {
        //     if (keyboard.fKey.wasPressedThisFrame)
        //     {
        //         anim.SetTrigger("Fight");
        //     }

        //     if (keyboard.iKey.wasPressedThisFrame)
        //     {
        //         anim.SetTrigger("Idle");
        //     }
        // }

        Vector3 direction = new Vector3(move.x, 0f, move.y).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
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
        anim.SetTrigger("Idle");
    }
}
