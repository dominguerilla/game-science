using RootMotion.FinalIK;
using TreeSharpPlus;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Toy class is our implementation of SmartCharacter
/// </summary>
public class Toy : SmartObject {
    #region to do
    /*
    TODO
    -Ability to pick up and equip Accessories (SmartObject)
    -Switch between Idle Tree and Player control
        -ie: When player clicks on character
    -Implement a real Idle Tree
    */
    #endregion

    private NavMeshAgent agent;
    private BehaviorAgent bagent;
    private SmartCharacter schar;
    private Animator anim;

    

    // Testing: waypoints for the character, used for IdleBehavior
    private GameObject waypoint1, waypoint2;

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


    #region getters
    public override string Archetype
    { get { return "Toy"; } }

    // Getter for behavior trees
    public NavMeshAgent GetAgent
    { get { return agent; } }

    //Got rid of these--better to just pass the positions to the Node function itself
    // Getter for behavior trees
    //public GameObject GetWayPoint1
    //{ get { return waypoint1; } }

    // Getter for behavior trees
    //public GameObject GetWayPoint2
    //{ get { return waypoint2; } }

    public int GetAvailableSlotCount()
    {
        return AvailableSlots;
    }
    #endregion

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        schar = GetComponent<SmartCharacter>();
        anim = GetComponent<Animator>();
        anim.SetBool("isWalk", true);
        playerInControl = true;
        AvailableSlots = AccessorySlots.Length;
    }
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("Moving", agent.hasPath);
        if (playerInControl)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Attempt to equip an Accessory
                if (bagent != null)
                {
                    bagent.StopBehavior();
                }
                DEBUG_EquipAccessory();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                if (bagent != null)
                {
                    bagent.StopBehavior();
                }
                bagent = new BehaviorAgent(IdleBehaviors.IdleWander(this));
                bagent.StartBehavior();
            }
        }
    }

    

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Accessory>() != null)
        {
            AccessoryInRange = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == AccessoryInRange)
        {
            AccessoryInRange = null;
        }
    }
    

   

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