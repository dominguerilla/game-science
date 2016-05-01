using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TreeSharpPlus;

/// <summary>
/// The revamped definition for Accessory, consisting of the three components outlined:
/// 1. Targets
/// 2. Action
/// 3. Effects
/// Note that we don't need a core function for this--we're focusing directly on behaviors that the Toy
/// uses, not the player.
/// :(((((((
/// </summary>
public abstract class NeoAccessory : MonoBehaviour{

	protected List<GameObject> Targets;
	protected Node Action;
	public abstract void Effects();

	protected int TargetPriority, ActionPriority, EffectPriority;

    private DataLogger logger;  // The Data Logger in the scene
    protected Toy toy;    // The Toy that's holding this accessory

    ///<summary>
    /// Determines whether or not an Accessory can be picked up and equipped by a Toy.
    ///</summary>
    public abstract bool IsEquippable{ get; }

	/// <summary>
	/// The model that is shown on the Toy.
	/// </summary>
	/// <value>The equip model.</value>
	public abstract GameObject EquipModel { get; }

	/// <summary>
	/// Specifies what equipment slot this Accessory will appear in.
	/// </summary>
	/// <value>The equip slot.</value>
	public abstract EquipSlots EquipSlot { get; }


	/// <summary>
	/// The enumerations for the equipment slots. These are passed to the EquipAccessory node to specify where the Accessory will appear on the Toy when equipped.
	/// In the Inspector, under the Accessory Slots section of the Toy component, make sure that the proper bones of the model are assigned to their respective indices as outlined here.
	/// </summary>
	public enum EquipSlots
	{
		RightHand = 0,
		LeftHand = 1,
		TorsoBack = 2,
		Head = 3,
		None = 9
	}

    /// <summary>
    /// Enum to ensure priorities are indexed correctly
    /// </summary>
    public enum PriorityIndex
    {
        Target = 0,
        Action = 1,
        Effect = 2
    }

	void Start(){
		Targets = new List<GameObject> ();
        InitializePriorities();
        InitializeTargets();
        InitializeAction();

        // Data logging
        GameObject logObject = GameObject.FindGameObjectWithTag("Logger");
        logger = logObject.GetComponent(typeof(DataLogger)) as DataLogger;
        if (logger != null) { logger.LogNewItem(this); }
    }

	/// <summary>
	/// Initializes the priorities of the three components. Should set the values for TargetPriority, ActionPriority, and EffectPriority.
	/// </summary>
	public abstract void InitializePriorities ();

	/// <summary>
	/// This function sets the Targets list. 
	/// Note that you can either find a way to choose a specific toy, or find a way to select Toys and Accessories
	/// that fit a criteria. 
	/// </summary>
	public abstract void InitializeTargets();

	/// <summary>
	/// This function constructs the tree that specifies the behavior of the Toy that equips the Accessory.
	/// It should SET the Action field to the root of the node.
	/// </summary>
	public abstract void InitializeAction();

	/// <summary>
	/// Returns the Action for this Accessory.
	/// </summary>
	/// <returns>The action.</returns>
	public virtual Node GetAction(){
		return Action;
	}

	/// <summary>
	/// Returns the Targets of this Accessory.
	/// </summary>
	/// <returns>The targets.</returns>
	public virtual List<GameObject> GetTargets(){
		return Targets;
	}

	/// <summary>
	/// Returns the priorities of the Accessory in an Array.
	/// [0] is the Target priority, [1] is the Action priority, [2] is the Effect priority.
	/// </summary>
	/// <returns>The priorities.</returns>
	public virtual int[] GetPriorities(){
		int[] priorities = new int[3];
		priorities [(int)PriorityIndex.Target] = TargetPriority;
		priorities [(int)PriorityIndex.Action] = ActionPriority;
		priorities [(int)PriorityIndex.Effect] = EffectPriority;
		return priorities;
	}



	/// <summary>
	/// Rotates the Accessory.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="speed">Speed.</param>
	public virtual void IdleRotate(Transform obj, float speed)
	{
		obj.Rotate(Vector3.up * Time.deltaTime * speed);
	}

	/// <summary>
	/// Called when User hits the Stop button
	/// </summary>
	public virtual void OnStop()
	{
		this.gameObject.SetActive(false);
		Destroy(this.gameObject);
	}

    /// <summary>
    /// Get a parameterized behavior for this NeoAccessory
    /// Target(s) provided by targetAccessory, effects by effectAccessory
    /// Action is provided by this accessory
    /// </summary>
    /// <param name="toy">The Toy that performs the behavior</param>
    /// <param name="targetAccessory">The target(s)</param>
    /// <param name="effectAccessory">The effect(s)</param>
    /// <returns></returns>
    public abstract Node GetParameterizedAction(Toy toy, NeoAccessory targetAccessory,
        NeoAccessory effectAccessory);

    /// <summary>
    /// Register a Toy for this accessory.
    /// </summary>
    /// <param name="toy"></param>
    public void SetToy(Toy toy)
    {
        this.toy = toy;
    }

    /// <summary>
    /// Get rid of this accessoy when it gets picked up
    /// </summary>
    public void OnUse()
    {
        this.gameObject.SetActive(false);
    }
}
