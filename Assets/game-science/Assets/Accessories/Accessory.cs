using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public abstract class Accessory : SmartObject {
    /**
    * Data Logging Functionality
    */

    // The Data Logger in the scene
    private DataLogger logger;

    void Start()
    {
        // When accessory is placed in the scene, log it
        GameObject logObject = GameObject.FindGameObjectWithTag("Logger");
        logger = logObject.GetComponent(typeof(DataLogger)) as DataLogger;
        if (logger != null) { logger.LogNewItem(this); }
    }

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
    /**
     * A simple function to rotate the Accessory while it is on the ground unequipped.
     */ 
    public virtual void IdleRotate(Transform obj, float speed)
    {
        obj.Rotate(Vector3.up * Time.deltaTime * speed);
    }

    /**
     * Determines whether or not an Accessory can be picked up and equipped by a Toy.
     */ 
    public abstract bool IsEquippable{ get; }

    /**
     * The model that will show up on the Toy who equips this Accessory, if any.
     */
    public abstract GameObject EquipModel { get; }

    /**
     * Specifies which equip slot this Accessory will appear if equippable.
     */ 
    public abstract EquipSlots EquipSlot { get; }

    /**
     * The subtree that happens when a Toy decides to 'use' the Accessory. Should only be called when the Toy is within 'use' range.
     * For equippable items, this would entail making the object display on the Toy, changing the state of both the Toy and Accessory, and then destroying the Accessory from the game world.
     * ex: Picking up a weapon from the ground would 1. Display the weapon on the Toy 2. The Toy now has a weapon equipped, and the weapon is now equipped and 3. The weapon should be no longer on the ground, but in the Toy's hand
     * Make the subtree return Success if the Toy can use this Accessory, otherwise make it return Fail if it cannot be used at the time.
     * 
     */
    public abstract Node OnUse(Toy toy);

    /**
     * The action available to the Toy who equips the Accessory, and then uses it.
     * ex: When a Toy who equips the weapon from OnUse() uses the weapon, whether it fires or swings or stabs is specified by ToyUse()
     */ 
    public abstract Node ToyUse(Toy toy);

	/// <summary>
	/// The Core function that this Accessory makes available to the player.
	/// </summary>
	public virtual void Core(Toy toy, params Toy[] targets)
	{
		Debug.Log ("Core function executed.");
		return;
	}

	public virtual bool Equals(Accessory acc1, Accessory acc2){
		return acc1.Archetype.Equals (acc2.Archetype);
	}

}
