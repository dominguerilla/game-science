using UnityEngine;
using System;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PlayerMove : MonoBehaviour {

    //model NEEDS to have an Animator Controller component!
    public GameObject model;
	public Camera camera;
    public float RunSpeed = 6.0F;
    public float WalkSpeed = 2.0f;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
	public Accessory acc;

	public int XInvert = 1;
	public int ZInvert = 1;
	public string FWDAxis = "Vertical";
	public string SIDEAxis = "Horizontal";
	public bool flipAxis = false;

    private Vector3 moveDirection = Vector3.zero;
    private Animator anim;
    private bool isWalk = false;
	private Toy ModelToy;
	private bool ToyIsAlive = true;
	private Accessory AccessoryInRange;


    public GameObject Player;
    public bool clickMode = false;

	void Start () {
        anim = model.GetComponent<Animator>();
		if (!clickMode) {
			NavMeshAgent agent = model.GetComponent<NavMeshAgent> ();
			if (agent)
				agent.enabled = false;
		}
		ModelToy = model.GetComponent<Toy> ();
		ModelToy.Equip (acc);
	}
	
	// Update is called once per frame
	void Update () {
        //For controlling the character directly
		if (!Player.activeInHierarchy) {
			ToyIsAlive = false;
		}
        //Movement animation
		if (ToyIsAlive) {
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
			{
				anim.SetBool("Moving", true);
			}
			else anim.SetBool("Moving", false);

			//Toggling the walking animation
			if (Input.GetKeyUp(KeyCode.CapsLock)) isWalk = !isWalk;
			anim.SetBool("isWalk", isWalk);

			//Attack
			if (Input.GetMouseButtonDown (0)) {
				if (acc)
					acc.Core (ModelToy);
			}
			//Rotating the model to look where it is moving
			float FWD = Input.GetAxis(FWDAxis);
			float SIDE = Input.GetAxis(SIDEAxis);
			anim.SetFloat("RunFWD", Math.Abs(FWD));
			anim.SetFloat("RunSIDE", Math.Abs(SIDE));
			Vector3 direction = new Vector3(FWD * XInvert, 0, SIDE * ZInvert);
			Vector3 newDir = Vector3.RotateTowards(model.transform.forward, direction, 1000, 0.0F);
			model.transform.rotation = Quaternion.LookRotation(newDir);

			//Character Movement
			CharacterController controller = GetComponent<CharacterController>();
			if (controller.isGrounded)
			{
				anim.SetBool("isMidair", false);
				if (flipAxis)
					moveDirection = new Vector3 (FWD, 0, SIDE);
				else
					moveDirection = new Vector3 (SIDE, 0, FWD);
				moveDirection = transform.TransformDirection(moveDirection);
				if (!isWalk) moveDirection *= RunSpeed;
				else moveDirection *= WalkSpeed;
				if (Input.GetButton ("Jump")) {
					moveDirection.y = jumpSpeed;
					anim.SetTrigger ("Jump");
					anim.SetBool ("isMidair", true);
				} 
			}
			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move(moveDirection * Time.deltaTime);
		}
        }

	void OnTriggerEnter(Collider other){
		Accessory acc = other.GetComponent (typeof(Accessory)) as Accessory;
		if (acc && !acc.Equals(AccessoryInRange)) {
			AccessoryInRange = acc;
		}
	}

	void OnTriggerExit(Collider other){
		Accessory acc = other.GetComponent (typeof(Accessory)) as Accessory;
		if (acc != null && acc.Equals(AccessoryInRange)) {
			AccessoryInRange = null;
		}
	}
        
    }


