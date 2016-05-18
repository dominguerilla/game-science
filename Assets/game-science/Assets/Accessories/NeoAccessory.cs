using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TreeSharpPlus;

/// <summary>
/// The revamped definition for Accessory, consisting of the three components outlined:
/// 1. Targets
/// 2. Action
/// 3. Effects
/// 4. Execution Order!
/// Note that we don't need a core function for this--we're focusing directly on behaviors that the Toy
/// uses, not the player.
/// :(((((((
/// </summary>
public abstract class NeoAccessory : MonoBehaviour{


    private DataLogger logger;  // The Data Logger in the scene
    protected Toy toy;    // The Toy that's holding this accessory
	protected HybridAccessory hybridAccessory;

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
        Effect = 2,
		Execution = 3
    }

	void Start(){
		hybridAccessory = new HybridAccessory ();

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
	/// Sets the Effects function in the Hybrid Accessory.
	/// </summary>
	public abstract void InitializeEffects ();


	/// <summary>
	/// Initializes the checker function for this Accessory. The checker function is called when the Toy is scanning H_LIST, and if it is true, execution of this Accessory takes place.
	/// </summary>
	public abstract void InitializeCheckerFunction ();


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
    //public abstract Node GetParameterizedAction(Toy toy, NeoAccessory targetAccessory,
        //NeoAccessory effectAccessory);

    /// <summary>
    /// Called when a Toy equips this NeoAccessory.
    /// </summary>
    /// <param name="toy"></param>
    public virtual void OnEquip(Toy toy)
    {
		hybridAccessory.SetEquipper (toy);

        // NeoAccessory also needs to know which Toy equipped it
        this.toy = toy;
    }

    /// <summary>
    /// Get rid of this accessory when it gets picked up
    /// </summary>
    public void OnUse()
    {
        // Initialize all the stuff for this Accessory when Toy picks it up
        /*** NOTE: Please leave this so that InitializePriorities runs FIRST */
        InitializePriorities();
        InitializeTargets();
        InitializeAction();
        InitializeEffects();
		InitializeCheckerFunction ();

        //Debug.Log("NeoAccessory.OnUse for " + this);
        /*Debug.Log("\tPriorities: " + GetPriorities()[0] + ", "
            + GetPriorities()[1] + ", "
            + GetPriorities()[2]);
        Debug.Log("\tTargets Count: " + GetTargets().Count);*/

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Print the list of Targets for this NeoAccessory to Debug.Log
    /// </summary>
    public void DEBUG_PrintTargets()
    {
        Debug.Log("NeoAccessory.DEBUG_PrintTargets for " + this);
        /*Debug.Log("Count = " + Targets.Count);
        for(int i = 0; i < Targets.Count; i++)
        {
            Debug.Log("\tNull?" + Targets[i] == null
                + " Toy? " + Targets[i].GetComponent<Toy>());
        }*/
    }

	public HybridAccessory GetHybridAccessory(){
		return this.hybridAccessory;
	}
}
