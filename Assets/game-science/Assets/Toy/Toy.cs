using RootMotion.FinalIK;
using TreeSharpPlus;
using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Toy class is our implementation of SmartCharacter
/// </summary>
public class Toy : SmartObject {

    //J. A pretty basic implementation of stats. 
	[SerializeField]
    private float forwardSpeed = 7f;

	[SerializeField]
    private float Health = 100f;

	[SerializeField]
    private float Attack = 10.0f;

    private NavMeshAgent agent;
    private BehaviorAgent bagent;
    private Animator anim;

    //This is used to specify where equipped Accessories will show up on the Toy.
	[SerializeField]
    private Transform[] AccessorySlots = new Transform[10];
    
    //The current number of available Accessory slots on the Toy.
    private int AvailableSlots;

    //Specifies if the player is currently in control of this Toy.
	[SerializeField]
    private bool playerInControl;

    //The node of the Behavior Tree that defines the Toy's idle behavior.
	[SerializeField]
    private Node IdleTreeRoot;

    #region Playzone Fields
    // The previous node of this Toy's idle behavior
    private Node PrevIdleTreeRoot;

    // The number of Playzones this Toy is in
    private int numPlayzones = 0;
    #endregion

    // The states for this toy
    private bool[] states = new bool[Enum.GetNames(typeof(SimpleStateDef)).Length];

    //The light that appears when this Toy is selected
    private GameObject light;

    //The current ability given by the Accessory equipped by this Toy.
	private Accessory equippedAccessory;

    //The current Accessory Archetype of the Toy. Currently, only one is able to be stored at a time.
    private string AccessoryArchetype;

    //A debug reference.
	[SerializeField]
	private GameObject targetAccessory;
	[SerializeField]
	private GameObject targetToy;

    // The Data Logger in the scene
    private DataLogger logger;

    // The team number for this Toy
    public int team;

    // The Emojis for this Toy
    public GameObject Hurt_Emoji;
    public GameObject Anger_Emoji;
    public GameObject Laugh_Emoji;
    public GameObject Heart_Emoji;

    #region setters
    // All states are initially set to true
    private void SetInitialStates()
    {
        // (1) Set everything to be true
        for(int i = 0; i < states.Length; i++)
        {
            states[i] = true;
        }

        // (2) Then set these states to be false
        SetStatesToFalse(SimpleStateDef.TPSMode,
            SimpleStateDef.IsInPlayzone);

        #region Notes On States
        /** Should set TPSMode *back* to false when we exit TPControl
        * This is currently done in PlayerMove.ExitControl

        * Should set IsInPlayzone *back* to false when Toy exits a
        * playzone.
        * This is currently unimplemented
        */
        #endregion
    }

    /// <summary>
    /// Makes this Toy equip an Accessory on the target GameObject.
    /// </summary>
    /// <param name="targetAccessory">Target accessory.</param>
    public void SetTargetAccessory(GameObject target)
    {
        Debug.Log("SetTargetAccessory: " + target);
        Accessory acc = target.GetComponent (typeof(Accessory)) as Accessory;
		if (acc) {
			this.targetAccessory = target;
            if (playerInControl)
            {   // Equip the accessory directly
                DEBUG_EquipAccessoryDirectly(acc);
            }
            else
            {   // New behavior: pick up the accessory
                SetIdleBehavior(IdleBehaviors.IdleStandDuringAction(IdleBehaviors.MoveAndEquipAccessory(this, acc)));
            }

            // NOTE: Depending on implementation, may want to do this too
            Equip(acc);
			targetAccessory = null;
		} else {
			Debug.Log ("No Accessory found in given Game Object!");
		}
    }

    #endregion

    #region getters
    public override string Archetype
    { get { return "Toy"; } }

    
    public NavMeshAgent GetAgent()
    {
        return this.agent;
    }

    public Animator GetAnimator()
    {
        return this.anim;
    }

    public float GetAttack()
    {
        return this.Attack;
    }

    public float GetHealth()
    {
        return this.Health;
    }

    public string GetAccessoryArchetype()
    {
        return this.AccessoryArchetype;
    }

	public Accessory GetEquippedAccessory()
	{
		return this.equippedAccessory;
	}

	public Node GetIdleTreeRoot(){
		return this.IdleTreeRoot;
	}

    public int GetTeam()
    {
        return this.team;
    }

	/// <summary>
	/// Gets the Transform equip slot stored in the AccessorySlots array. Returns null if the index is invalid or if there is no equip slot specified in that position.
	/// </summary>
	/// <returns>The accessory equip slot.</returns>
	/// <param name="index">Index.</param>
	public Transform GetAccessoryEquipSlot(int index){
		if (index >= AccessorySlots.Length || index < 0) {
			Debug.Log ("Invalid index!");
			return null;
		} else if (AccessorySlots [index] != null) {
			return AccessorySlots [index];
		} else {
			Debug.Log ("No equip slot found in that index.");
			return null;
		}
	}

    #endregion

    void Start () {
        // Log this Toy for our data logging
        GameObject logObject = GameObject.FindGameObjectWithTag("Logger");
        logger = logObject.GetComponent(typeof(DataLogger)) as DataLogger;
        if (logger != null) { logger.LogNewItem(this); }

        // Set state booleans
        this.SetInitialStates();

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.SetBool("isWalk", false);
        playerInControl = false;
        AvailableSlots = AccessorySlots.Length;

        // Option to start a toy with a target accessory
        if (targetAccessory != null)
        {
            Debug.Log("Starting with target accessory: " + targetAccessory);
            Accessory acc = targetAccessory.GetComponent(typeof(Accessory)) as Accessory;
			if (acc != null) {
				Debug.Log ("Accessory found in " + targetAccessory);																														
				IdleTreeRoot = IdleBehaviors.IdleStandDuringAction(IdleBehaviors.MoveAndEquipAccessory (this, acc));

                // Adding this so Toys with targetAccessories pick them up
                SetIdleBehavior(IdleTreeRoot);
			}
            else IdleTreeRoot = IdleBehaviors.IdleStand();
        }
		else // Otherwise, look for a random accessory
        {
            // Look for accessory
            //Debug.Log("Looking for accessory...");
            /*
            GameObject[] accessoriesInScene =
                GameObject.FindGameObjectsWithTag("Accessory");

			if (accessoriesInScene.Length == 0)
            {
                Debug.Log("No accessories in scene");
				IdleTreeRoot = IdleBehaviors.IdleStand ();
            }
            else
            {
                // Choose a random accessory from the ones in the scene
                System.Random rand = new System.Random();
                GameObject chosenAccessory =
                    accessoriesInScene[rand.Next(0, accessoriesInScene.Length)];

                // Have this accessory be this toy's target accessory
                SetAccessory(chosenAccessory);
            }*/

        }
    }
	
	// Update is called once per frame
	void Update () {
        if(agent == null)
        {
            Debug.Log("null");
        }

		/*if (targetAccessory) {
			SetTargetAccessory (targetAccessory);
		}*/
            
        anim.SetBool("Moving", agent.hasPath);

        // If Toy is in range of an accessory, is in the "wantsAccessory" state,
        // and is in TPSMode, then have it automatically equip the accessory
        // Note: possible that we don't want to call this every frame
        if (CheckStates(SimpleStateDef.WantsAccessories,
            SimpleStateDef.TPSMode))
        {
            EquipAccessoriesInRange();
        }

        // If Toy is inside of a Playzone, we may want to do something to it here
        // if(CheckStates(SimpleStateDef.IsInPlayzone)){ }
    }

    #region Public interface functions
    /// <summary>
    /// Called when user Pauses
    /// </summary>
    public void OnPause()
    {
        DEBUG_StopBehavior();
    }
		
    /// <summary>
    /// Called when user hits Play
    /// </summary>
    public void OnPlay()
    {
        DEBUG_StartBehavior();
    }

    /// <summary>
    /// Called when user hits Stop
    /// </summary>
    public void OnStop()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// The function called when this Toy is selected.
    /// </summary>
    public void OnSelect()
    {
        SetStatesToTrue(SimpleStateDef.TPSMode);
        //playerInControl = true;

        light = new GameObject("Spotlight");
        Light lightComp = light.AddComponent<Light>();
        light.transform.parent = this.gameObject.transform;
        lightComp.color = Color.green;
        lightComp.type = LightType.Spot;
        lightComp.intensity = 10;
        light.transform.position = this.transform.position + new Vector3 (0, 2, 0);
        light.transform.Rotate(90, 0, 0);

        Debug.Log(gameObject.name + " is selected.");
    }

    /// <summary>
    /// The function called when this Toy is deselected.
    /// </summary>
    public void OnDeselect()
    {
        SetStatesToFalse(SimpleStateDef.TPSMode);
        playerInControl = false;
        //Debug.Log(gameObject.name + " is unselected.");

        GameObject.Destroy(light);

        // When we exit TPS mode, tell the Toy to perform its idle behavior
        SetIdleBehavior(IdleTreeRoot);
    }

    /// <summary>
    /// Set health directly
    /// </summary>
    /// <param name="newHealth"></param>
    public void SetHealth(float newHealth)
    {
        Health = newHealth;
        print(this + " Toy.SetHealth: Health set to " + Health);
        if (Health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Set speed directly
    /// </summary>
    /// <param name="newSpeed"></param>
    public void SetSpeed(float newSpeed)
    {
        forwardSpeed = newSpeed;
        agent.speed = forwardSpeed;

        print(this + " Toy.SetSpeed: Speed set to " + forwardSpeed);
    }

    /// <summary>
    /// Tells the Toy to move to this specific location.
    /// </summary>
    /// <param name="other"></param>
    public void SetDestination(Vector3 position)
    {
        agent.SetDestination(position);
    }

    /// <summary>
    /// Adds the given accessory to the Inventory of this Toy.
    /// FUTURE WORK: Allow the Toy access to all the abilities of the Accessories equipped.
    /// </summary>
    /// <param name="acc"></param>
    public void Equip(Accessory acc)
    {
		equippedAccessory = acc;
        AccessoryArchetype = acc.Archetype;
        Debug.Log("Added " + acc.Archetype + " to " + this.Archetype + "'s inventory.");

        // Set the Accessory's OnUse to be this Toy's IdleRoot
        // IdleTreeRoot = acc.OnUse(this);
    }

    /// <summary>
    /// Removes the specified Accessory from the Inventory of this Toy.
    /// </summary>
    /// <param name="acc"></param>
    public void Unequip()
    {
		Debug.Log("Removing " + equippedAccessory.Archetype + " from inventory.");
		equippedAccessory = null;
		AccessoryArchetype = "";
    }


    /// <summary>
    /// Adds a flat float value bonus to this Toy's Attack stat. Can be positive or negative.
    /// </summary>
    /// <param name="attackBonus"></param>
    public void ChangeAttack(float attackBonus)
    {
        if(this.Attack + attackBonus > 0) this.Attack += attackBonus;
    }

    /// <summary>
    /// Change the Health of this Toy by the specified amount. Can be positive or negative.
    /// If the new Health of the Toy is less than zero, calls the Die() function.
    /// </summary>
    /// <param name="healthChange"></param>
    public void ChangeHealth(float healthChange)
    {
        Health += healthChange;
        if (Health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Called when this Toy's health becomes less than zero.
    /// </summary>
    private void Die()
    {
        Debug.Log(gameObject.name + " has died.");

        // ShowEmoji(Hurt_Emoji);

		if (bagent != null) bagent.StopBehavior();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the idle behavior of this Toy to the given root node.
    /// </summary>
    /// <param name="root"></param>
    public void SetIdleBehavior(Node root) //recompile
    {
		if (root != null) {
			Debug.Log ("Idle Behavior root given NOT null");
			IdleTreeRoot = root;
            if (bagent != null) { bagent.StopBehavior(); }
			bagent = new BehaviorAgent(IdleTreeRoot);
			bagent.StartBehavior();
		} else {
			Debug.Log ("Toy.SetIdleBehavior given null input");
		}
    }

    /// <summary>
    /// Tell the Toy it's in a Playzone
    /// Currently called by Playzone.cs when it detects this Toy
    /// </summary>
    /// <param name="zone"></param>
    public void OnPlayzoneEnter(Playzone zone)
    {
        SetStatesToTrue(SimpleStateDef.IsInPlayzone);
        numPlayzones++;

        if (numPlayzones == 1)
        {
            // Change this Toy's Idle Behavior to the Playzone Node
            PrevIdleTreeRoot = IdleTreeRoot;
            SetIdleBehavior(zone.GetPlayzoneNode(this));
        }

        // TODO: Need to think about possibility of multiple Playzones
    }

    /// <summary>
    /// Tell the Toy it's no longer in a Playzone
    /// Called by Playzone.cs when this Toy exits
    /// </summary>
    /// <param name="zone"></param>
    public void OnPlayzoneExit(Playzone zone)
    {
        numPlayzones--;

        if(numPlayzones < 1)
        {
            SetStatesToFalse(SimpleStateDef.IsInPlayzone);
            SetIdleBehavior(PrevIdleTreeRoot);
        }
    }

    /// <summary>
    /// Tell the Toy it's on this team
    /// </summary>
    /// <param name="newTeam"></param>
    public void OnTeamSet(int newTeam)
    {
        team = newTeam;
        print(this + " Toy.OnTeamChange: team is now " + team);

    }

    /// <summary>
    /// Run Core function of current Toy accessory
    /// </summary>
    public void DEBUG_RunEquippedAccessoryCore()
    {
        if(equippedAccessory == null)
        {
            print("Toy.ActivateCore: no accessory equipped");
        }
        else
        {
            equippedAccessory.Core(this);
        }
    }

    /// <summary>
    /// Show an emoji
    /// </summary>
    /// <param name="emoji"></param>
    public void ShowEmoji(GameObject emoji)
    {
        if (!emoji)
        {
            print("Toy.ShowEmoji: Emoji is null");
        }

        Instantiate(emoji,
            this.transform.position + new Vector3(0,4,0),
            this.transform.rotation);

        // TODO: Give Emojis a timer script
    }
    #endregion

    #region Private utility functions

    /// <summary>
    /// If Toy "WantsAccessories," automatically equip an accessory if it's in range
    /// </summary>
    private void EquipAccessoriesInRange()
    {
        // Attempt to improve performance: not sure if this works
        new WaitForSeconds(1);

        // Check objects in range 2 or less
        Collider[] objectsInRange =
            Physics.OverlapSphere(this.transform.position, 2);

        foreach(Collider c in objectsInRange)
        {
            if (c.tag == "Accessory")
            {
                // Have this accessory be this toy's target accessory
                SetTargetAccessory(c.gameObject);
            }
        }
        
    }

    /// <summary>
    /// Equip this accessory to the Toy directly, without animation
    /// Used in TPS mode if player gets within range of accessory
    /// </summary>
    /// <param name="acc"></param>
    private void DEBUG_EquipAccessoryDirectly(Accessory acc)
    {
        // Code copied and modified from "EquipAccessory"
        int EquipSlot = (int)acc.EquipSlot;

        if (acc.EquipModel &&
            EquipSlot != (int)Accessory.EquipSlots.None)
        {
            GameObject accModel = (GameObject)GameObject.Instantiate(acc.EquipModel,
                this.GetAccessoryEquipSlot(EquipSlot).transform.position,
                Quaternion.identity);

            // Make the model a child of the bones of the Toy model
            accModel.transform.parent = this.GetAccessoryEquipSlot(EquipSlot);
        }
        else
        {
            // Otherwise, just exit
            Debug.Log("Toy.DEBUG_EquipAccessoryDirectly: couldn't equip " + acc);
            return;
        }

        // Destroy the Accessory on the ground
        acc.gameObject.SetActive(false);

        // Set this Toy's idle behavior to be accessory.ToyUse
        Debug.Log("Toy.DEBUG_EquipAccessoryDirectly: setting idle behavior to "
            + acc
            + " ToyUse method");
        SetIdleBehavior(acc.ToyUse(this));
    }


    #endregion

    #region Debugging Functions, that may or may not be called by our GUI

    public void DEBUG_SetIdleRootAsIdleStand()
    {
        IdleTreeRoot = IdleBehaviors.IdleStand();
    }
    public void DEBUG_SetIdleRootAsWander()
    {
        IdleTreeRoot = IdleBehaviors.IdleWander(this);
    }
    public void DEBUG_StartBehavior()
    {
        if (IdleTreeRoot != null)
        {
            bagent = new BehaviorAgent(IdleTreeRoot);
            bagent.StartBehavior();
        }
        else
        {
            print("Toy.DEBUG_StartBehavior: IdleTreeRoot is null");
        }
    }
	public void DEBUG_StopBehavior()
	{
        if (bagent != null)
        {
            bagent.StopBehavior();

            // Hack: To stop behavior right now, just flicker the Toy
            this.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
        }
        else
        {
            print("Toy.DEBUG_StopBehavior: bagent is null");
        }
    }
    #endregion

    #region Node Functions
    /// <summary>
    /// Requires a collection of states be the given values
    /// </summary>
    public Node Simple_Node_Require(params SimpleStateDef[] states)
    {
        return new LeafAssert(() => CheckStates(states));
    }

    public Node Simple_Node_SetTrue(params SimpleStateDef[] states)
    {
        return new LeafInvoke(() => SetStatesToTrue(states));
    }

    public Node Simple_Node_SetFalse(params SimpleStateDef[] states)
    {
        return new LeafInvoke(() => SetStatesToFalse(states));
    }

    public void SetStatesToTrue(params SimpleStateDef[] states)
    {
        foreach (SimpleStateDef i in states)
        {
            this.states[(int)i] = true;
        }
    }

    public void SetStatesToFalse(params SimpleStateDef[] states)
    {
        foreach (SimpleStateDef i in states)
        {
            this.states[(int)i] = false;
        }
    }

    // Utility function: checks that all states are true for this toy
    public bool CheckStates(params SimpleStateDef[] states)
    {
        foreach (SimpleStateDef i in states)
        {
            if(this.states[(int)i] != true)
            {
                //Debug.Log("In Toy.CheckStates. State '" + i + "' is false");
                return false;
            }
        }
        return true;
    }
    #endregion

}