using RootMotion.FinalIK;
using TreeSharpPlus;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Toy class is our implementation of SmartCharacter
/// </summary>
public class Toy : SmartObject {

    //J. A pretty basic implementation of stats. Only for speed now. Prob can add other stats like this too
    public float forwardSpeed = 7f;
    public float Health = 100;
    public float Attack = 10.0f;

    private NavMeshAgent agent;
    private BehaviorAgent bagent;
    private Animator anim;

    //The current Accessory in range of use for this Toy.
    private GameObject AccessoryInRange;

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
    private List<Accessory> Inventory;

    //A debug reference.
    public GameObject targetAccessory;

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

    #endregion

    void Start () {
        // Set all states to true for now
        this.SetInitialStates();

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.SetBool("isWalk", false);
        playerInControl = true;
        AvailableSlots = AccessorySlots.Length;
        Inventory = new List<Accessory>();

        //TEMPORARY DEBUGGING SECTION   
        if (targetAccessory != null)
        {
            Accessory acc = targetAccessory.GetComponent(typeof(Accessory)) as Accessory;
            if (acc != null) IdleTreeRoot = IdleBehaviors.IdleStandDuringAction(IdleBehaviors.MoveAndEquipAccessory(this, acc));
            else IdleTreeRoot = IdleBehaviors.IdleStand();
        }
        else
        {
            IdleTreeRoot = IdleBehaviors.IdleStand();
        }

        bagent = new BehaviorAgent(IdleTreeRoot);
        bagent.StartBehavior();
        //END TEMPORARY DEBUGGING SECTION
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

        Debug.Log(gameObject.name + " is selected.");
    }

    /// <summary>
    /// The function called when this Toy is deselected.
    /// </summary>
    public void OnDeselect()
    {
        playerInControl = false;
        Debug.Log(gameObject.name + " is unselected.");
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
        Inventory.Add(acc);
        Debug.Log("Added " + acc.Archetype + " to " + this.Archetype + "'s inventory.");
    }

    /// <summary>
    /// Removes the specified Accessory from the Inventory of this Toy.
    /// </summary>
    /// <param name="acc"></param>
    public void Unequip(Accessory acc)
    {
        Inventory.Remove(acc);
        Debug.Log("Removed " + acc.Archetype + " from " + this.Archetype + "'s inventory.");
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
        if (Health < 0)
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

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Accessory>() != null)
        {
            AccessoryInRange = other.gameObject;
        }

        //J. If it f's up anyone elses behaviors commit out this section and let me know on slack
        if (other.gameObject.CompareTag("Speed"))
        {
            other.gameObject.SetActive(false);
            forwardSpeed = 12f;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == AccessoryInRange)
        {
            AccessoryInRange = null;
        }
    }
    #endregion



    #region Debugging Functions, that may or may not be called by our GUI
    //Assuming that the Behavior Agent hasn't been initialized or started yet, will attempt to equip an Accessory within range.
    bool DEBUG_EquipAccessory()
    {
        if (AccessoryInRange != null && AccessoryInRange.activeInHierarchy)
        {
            Accessory acc = AccessoryInRange.GetComponent<Accessory>();
            Node OnUse = acc.OnUse(this);
            bagent = new BehaviorAgent(IdleBehaviors.IdleStandDuringAction(OnUse));
            //Node AccessoryAbility = acc.ToyUse(this);
            bagent.StartBehavior();
            return true;
        }
        else
        {
            Debug.Log("No Accessory within range.");
            return false;
        }
    }
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