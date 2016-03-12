using UnityEngine;
using System;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PlayerMove : MonoBehaviour {

    //model NEEDS to have an Animator Controller component!
    public GameObject model;
    public float RunSpeed = 6.0F;
    public float WalkSpeed = 2.0f;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;

    private Vector3 moveDirection = Vector3.zero;
    private Animator anim;
    private bool isWalk = false;

    public GameObject Player;
    public bool clickMode = false;

	void Start () {
        anim = model.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //For controlling the character directly
        
            //Movement animation
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                anim.SetBool("Moving", true);
            }
            else anim.SetBool("Moving", false);

            //Toggling the walking animation
            if (Input.GetKeyUp(KeyCode.CapsLock)) isWalk = !isWalk;
            anim.SetBool("isWalk", isWalk);

            //Rotating the model to look where it is moving
            float FWD = Input.GetAxis("Vertical");
            float SIDE = Input.GetAxis("Horizontal");
            anim.SetFloat("RunFWD", Math.Abs(FWD));
            anim.SetFloat("RunSIDE", Math.Abs(SIDE));
            Vector3 direction = new Vector3(FWD, 0, SIDE * -1);
            Vector3 newDir = Vector3.RotateTowards(model.transform.forward, direction, 1000, 0.0F);
            model.transform.rotation = Quaternion.LookRotation(newDir);

            //Character Movement
            CharacterController controller = GetComponent<CharacterController>();
            if (controller.isGrounded)
            {
                anim.SetBool("isMidair", false);
                moveDirection = new Vector3(SIDE, 0, FWD);
                moveDirection = transform.TransformDirection(moveDirection);
                if (!isWalk) moveDirection *= RunSpeed;
                else moveDirection *= WalkSpeed;
                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                    anim.SetTrigger("Jump");
                    anim.SetBool("isMidair", true);
                }
            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
        
    }


