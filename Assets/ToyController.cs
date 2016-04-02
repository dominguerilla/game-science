﻿using UnityEngine;
using System.Collections;
using TreeSharpPlus;

/// <summary>
/// An interface for controlling a Toy, meant to be used by keyboard and mouse input handlers.
/// </summary>
public class ToyController : MonoBehaviour{

	/// <summary>
	/// The current toy selected by the player. Null if no Toy is selected.
	/// </summary>
	public Toy CurrentToy;

	/// <summary>
	/// The GameObject prefab that handles the third person control of Toys.
	/// </summary>
	public GameObject TPSController;

	/// <summary>
	/// The current PBT associated with the current Toy. Used for pausing and resuming a behavior tree.
	/// </summary>
	private Node CurrentToyTree;

	void Start(){

	}

	void Update(){

	}

	/// <summary>
	/// Selects the Toy given to be the focus of the controller.
	/// </summary>
	/// <param name="toy">Toy to focus on.</param>
	public void SelectToy(Toy toy)
	{
		CurrentToy = toy;
		CurrentToyTree = toy.GetIdleTreeRoot();
		toy.OnSelect ();
	}

	/// <summary>
	/// Selects a Toy and deselects the old one.
	/// </summary>
	/// <param name="newToy">New Toy to focus on.</param>
	/// <param name="oldToy">Old Toy to deselect.</param>
	public void SelectToy(Toy newToy, Toy oldToy)
	{
		CurrentToy = newToy;
		CurrentToyTree = newToy.GetIdleTreeRoot();
		oldToy.OnDeselect ();
		newToy.OnSelect ();
	}

	/// <summary>
	/// Deselects the currently selected toy.
	/// </summary>
	public void DeselectToy()
	{
		if (CurrentToy != null) 
		{
			CurrentToy.OnDeselect ();
			CurrentToy.SetIdleBehavior (IdleBehaviors.IdleStandDuringAction(CurrentToyTree));
			CurrentToy = null;
			CurrentToyTree = null;
		}
	}

	/// <summary>
	/// Makes the CurrentToy (if any) stop its current behavior, equip the given Accessory, and then execute whatever behavior that Accessory dictates.
	/// </summary>
	/// <param name="acc">Acc.</param>
	public void EquipAccessory(Accessory acc)
	{
		if (CurrentToy) 
		{
			CurrentToyTree = CurrentToy.GetIdleTreeRoot();
			CurrentToy.SetIdleBehavior (IdleBehaviors.IdleStandDuringAction(IdleBehaviors.MoveAndEquipAccessory(CurrentToy,acc)));
		}
	}

	/// <summary>
	/// Makes the Toy walk to the specified location and then stand there.
	/// </summary>
	/// <param name="location">Location.</param>
	public void MoveTo(Vector3 location)
	{
		if (CurrentToy) 
		{
			CurrentToyTree = CurrentToy.GetIdleTreeRoot();
			CurrentToy.SetIdleBehavior (IdleBehaviors.IdleStandDuringAction(new WalkTo(CurrentToy, location)));
		}
	}

	/// <summary>
	/// Activates the Core function of whatever Accessory the Toy has equipped.
	/// </summary>
	public void ActivateCore(params Toy[] targets)
	{
		if (CurrentToy) 
		{
			Accessory acc = CurrentToy.GetEquippedAccessory ();
			if (acc != null) 
			{
				acc.Core (CurrentToy, targets);
			}
		}
	}

	/// <summary>
	/// Enter third-person control scheme on the toy specified, if a third person controller has been specified.
	/// Makes the 
	/// </summary>
	public void EnterTPControl(Toy toy){
		if (TPSController) {
			GameObject newObject = (GameObject)Instantiate (TPSController, toy.transform.position, toy.transform.localRotation);
			toy.gameObject.transform.parent = newObject.transform;
			newObject.GetComponent<PlayerMove> ().Initialize ();
		} else {
			Debug.Log ("Error! No TPSController specified.");
		}
	}
}
