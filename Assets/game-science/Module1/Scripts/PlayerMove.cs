using UnityEngine;
using System;
using System.Collections;
using FlyingCamera;

/// <summary>
/// The script in charge of handling the third-person shooter control scheme for individual toys.
/// </summary>
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


    public bool clickMode = false;

	void Start () {
        
	}

	public void Initialize(){
		ModelToy = GetComponentInChildren<Toy>();
		if (ModelToy) {
			ModelToy.StopBehavior ();
			model = ModelToy.gameObject;
			anim = model.GetComponent<Animator>();
			NavMeshAgent agent = model.GetComponent<NavMeshAgent> ();
			if (agent) agent.enabled = false;
			CharacterController charController = GetComponent<CharacterController> ();
			charController.center = ModelToy.GetComponent<CapsuleCollider> ().center;
			//ModelToy.Equip (acc);
		} else {
			Debug.Log ("Error! No Toy Component found in any children of TPS Controller!");
		}
	}

	// Update is called once per frame
	void Update () {
        //For controlling the character directly
		if (!model.activeInHierarchy) {
			ToyIsAlive = false;
		}
        //Movement animation
		if (ToyIsAlive) {
			KeyboardInput ();
			MouseInput ();
		}
    }

	void KeyboardInput(){
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
		{
			anim.SetBool("Moving", true);
		}
		else anim.SetBool("Moving", false);

		if (Input.GetKeyUp (KeyCode.Escape)) {
            // Exit control, exit TPS mode
            ExitControl();
		}
		//Toggling the walking animation
		if (Input.GetKeyUp(KeyCode.CapsLock)) isWalk = !isWalk;
		anim.SetBool("isWalk", isWalk);

		//Rotating the model to look where it is moving
		float FWD = Input.GetAxis(FWDAxis);
		float SIDE = Input.GetAxis(SIDEAxis);
		anim.SetFloat("RunFWD", Math.Abs(FWD));
		anim.SetFloat("RunSIDE", Math.Abs(SIDE));

		//Vector3 direction = new Vector3(FWD * XInvert, 0, SIDE * ZInvert);
		//Vector3 newDir = Vector3.RotateTowards(model.transform.forward, direction, 1000, 0.0F);
		//model.transform.rotation = Quaternion.LookRotation(newDir);


		//Character Movement
		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded)
		{
			anim.SetBool("isMidair", false);
			/*if (flipAxis)
				moveDirection = new Vector3 (FWD, 0, SIDE);
			else
				moveDirection = new Vector3 (SIDE, 0, FWD);
			
			moveDirection = transform.TransformDirection(moveDirection);
			*/

			moveDirection = new Vector3 (FWD, 0, SIDE);
			moveDirection = camera.transform.TransformDirection(moveDirection);
			if(moveDirection != new Vector3(0, 0, 0)) model.transform.rotation = Quaternion.LookRotation (moveDirection);
			if (!isWalk) moveDirection *= RunSpeed;
			else moveDirection *= WalkSpeed;
			if (Input.GetButton ("Jump")) {
				moveDirection.y = jumpSpeed;
				anim.SetTrigger ("Jump");
				anim.SetBool ("isMidair", true);
			} 
		}
		model.transform.position = controller.transform.position;
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}

	void MouseInput(){
		//Attack
		if (Input.GetMouseButtonDown (0)) {
            // Toy should know which accessory it has equipped
            ModelToy.DEBUG_RunEquippedAccessoryCore();

            // Note: Should probably use ToyController rather than talk
            // to Toy directly? (See Below code)

            // Activate Core without any targets on Mouse Click
            // ToyController tc = ModelToy.GetComponent<ToyController>();
            // if(tc != null) { tc.ActivateCore(); }


            // if (acc)
            // acc.Core (ModelToy);
        }
			
		//Camera orbitiing
		camera.transform.RotateAround (model.transform.position, Vector3.up, Input.GetAxis("Mouse X") * 1.0f);
	}

	void ExitControl(){
		model.gameObject.transform.parent = null;
		GameObject.Destroy (this.gameObject);
        //TODO Have a better way of re-enabling the flying camera controller!
        // Note: currently flying camera controller is simply not deactivated

        // Reset Flying Camera Controller to its starting position
        GameObject flyingcam = GameObject.Find("FlyingPlayer");
        if (flyingcam)
        {
            FlyingCameraController fcc = flyingcam.GetComponent<FlyingCameraController>();
            if (fcc) { fcc.ResetToDefaultPosition(); }
        }

        // Once we exit TPS, tell the Toy that it's not in TPS anymore
        ModelToy.OnDeselect();
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


