using RootMotion.FinalIK;
using TreeSharpPlus;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Toy class is our implementation of SmartCharacter
/// </summary>
public class Toy : SmartObject {

    //J. A pretty basic implementation of stats. 
    public float forwardSpeed = 7f;
    public float Health = 100f;
    public float Attack = 10.0f;

    private NavMeshAgent agent;
    private BehaviorAgent bagent;
    private Animator anim;

    //This is used to specify where equipped Accessories will show up on the Toy.
    public Transform[] AccessorySlots = new Transform[10];
    
    //The current number of available Accessory slots on the Toy.
    private int AvailableSlots;

    //Specifies if the player is currently in control of this Toy.
    public bool playerInControl;

    //The node of the Behavior Tree that defines the Toy's idle behavior.
    public Node IdleTreeRoot;

    // The states for this toy
    private bool[] states;

    //The light that appears when this Toy is selected
    private GameObject light;

    //The current ability given by the Accessory equipped by this Toy.
	private Accessory equippedAccessory;

    //The current Accessory Archetype of the Toy. Currently, only one is able to be stored at a time.
    private string AccessoryArchetype;

    //A debug reference.
    public GameObject targetAccessory;
    public GameObject targetToy;


    #region setters
    // Right now, all states are initially set to true
    // Eventually, we should define which states are true inititially
    private void SetInitialStates()
    {
        states = new bool[(int)SimpleStateDef.NUMSTATES];

        for(int i = 0; i < states.Length; i++)
        {
            states[i] = true;
        }
    }

    // Set this Toy's target accessory
    public void SetAccessory(GameObject targetAccessory)
    {
        this.targetAccessory = targetAccessory;
    }
    #endregion

    #region getters
    public override string Archetype
    { get { return "Toy"; } }

    // Getter for behavior trees
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

    #endregion

    void Start () {
        // Set all states to true for now
        this.SetInitialStates();

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.SetBool("isWalk", false);
        //playerInControl = true;
        AvailableSlots = AccessorySlots.Length;


        // Option to start a toy with a target accessory
        if (targetAccessory != null)
        {
            Debug.Log("Starting with target accessory: " + targetAccessory);
            Accessory acc = targetAccessory.GetComponent(typeof(Accessory)) as Accessory;
			if (acc != null) {
				Debug.Log ("Accessory found in " + targetAccessory);																														
				IdleTreeRoot = IdleBehaviors.IdleStandDuringAction(IdleBehaviors.MoveAndEquipAccessory (this, acc));

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
			IdleTreeRoot = IdleBehaviors.IdleStand ();
        }
    }
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("Moving", agent.hasPath);
    }

    #region Public interface functions

    /// <summary>
    /// The function called when this Toy is selected.
    /// </summary>
    public void OnSelect()
    {
        playerInControl = true;
        light = new GameObject("Spotlight");
        Light lightComp = light.AddComponent<Light>();
        light.transform.parent = this.gameObject.transform;
        lightComp.color = Color.green;
        lightComp.type = LightType.Spot;
        lightComp.intensity = 10;
        light.transform.position = this.transform.position + new Vector3 (0, 2, 0);
        light.transform.Rotate(90, 0, 0);

        //Debug.Log(gameObject.name + " is selected.");
    }

    /// <summary>
    /// The function called when this Toy is deselected.
    /// </summary>
    public void OnDeselect()
    {
        playerInControl = false;
        //Debug.Log(gameObject.name + " is unselected.");
        GameObject.Destroy(light);
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
		if (bagent != null) bagent.StopBehavior();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the idle behavior of this Toy to the given root node.
    /// </summary>
    /// <param name="root"></param>
    public void SetIdleBehavior(Node root)
    {
        IdleTreeRoot = root;
        bagent.StopBehavior();
        bagent = new BehaviorAgent(IdleTreeRoot);
        bagent.StartBehavior();
    }
   
    

    #endregion

    #region Private utility functions


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
    }
	public void DEBUG_StopBehavior()
	{
		if (bagent != null) {
			bagent.StopBehavior ();
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
            Debug.Log("State '" + i + "' set to true");
            this.states[(int)i] = true;
        }
    }

    public void SetStatesToFalse(params SimpleStateDef[] states)
    {
        foreach (SimpleStateDef i in states)
        {
            //Debug.Log("State '" + i + "' set to false");
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

#region additional comments
/*
See https://docs.google.com/document/d/11rvpzzHWuOlMyPGsaNj5fZhYVhoGgwgKTClkwYhpMic/edit
SmartCharacters have:

State: condition a SmartObject is in at a specific time
    -Ex: HoldingWeapon, HoldingBall
    -See StateDefs.cs
Affordance: a tuple:
    <[Participants], State Requirements, State Effects>
    -Changes the state of its participants according to State Effects
    -Can do this iff participants match State Requirements
Idle Behavior Tree:
    -Standard idle behavior for this character
    (-Would be nice, for now, if it wandered around looking for items to pick up)

*/
#endregion