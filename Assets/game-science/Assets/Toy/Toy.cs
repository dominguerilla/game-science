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

    //The current Accessory in range of use for this Toy.
    private GameObject AccessoryInRange;

    //This is used to specify where equipped Accessories will show up on the Toy.
    public List<Transform> AccessorySlots = new List<Transform>();
    
    //The current number of available Accessory slots on the Toy.
    private int AvailableSlots;

    public override string Archetype
    { get { return "Toy"; } }

    [Affordance]
    protected Node IdleTree()
    {
        return new DecoratorLoop(
            new Sequence(
                // TODO: Make this an actual behavior
                // Would be nice if it was a random wandering behavior
                // Would be even better if the character picked things up
                new LeafWait(2000)
                )
            );
    }

    //[Affordance]
    protected void PlayerInstructions() {
        // TODO
    }

    [Affordance]
    protected Node PickUpAxe(SmartCharacter user)
    {
        // TODO
        return new Sequence();
    }

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        schar = GetComponent<SmartCharacter>();
        anim = GetComponent<Animator>();
        bagent = new BehaviorAgent(IdleTree());
        AvailableSlots = AccessorySlots.Count;
        bagent.StartBehavior();
    }
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("Moving", agent.hasPath);
        if (Input.GetMouseButtonDown(0))
        {
            if (AccessoryInRange)
            {
                Accessory acc = AccessoryInRange.GetComponent<Accessory>();
                Node OnUse = acc.OnUse(this);
            }
        }
    }

    void OnCollisionEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Accessory>() != null)
        {
            AccessoryInRange = other.gameObject;
        }
    }

    void OnCollisionExit(Collider other)
    {
        if (other.gameObject == AccessoryInRange)
        {
            AccessoryInRange = null;
        }
    }

    public int GetAvailableSlotCount()
    {
        return AvailableSlots;
    }
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