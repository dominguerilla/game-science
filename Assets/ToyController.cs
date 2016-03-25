using UnityEngine;
using System.Collections;
using TreeSharpPlus;

/// <summary>
/// The interface the Player uses to control a Toy.
/// </summary>
public static class ToyController {

	public static Toy CurrentToy;
	private static Node CurrentToyTree;

	/// <summary>
	/// Selects the Toy given to be the focus of the controller.
	/// </summary>
	/// <param name="toy">Toy to focus on.</param>
	public static void SelectToy(Toy toy)
	{
		CurrentToy = toy;
		CurrentToyTree = toy.IdleTreeRoot;
		toy.OnSelect ();
	}

	/// <summary>
	/// Selects a Toy and deselects the old one.
	/// </summary>
	/// <param name="newToy">New Toy to focus on.</param>
	/// <param name="oldToy">Old Toy to deselect.</param>
	public static void SelectToy(Toy newToy, Toy oldToy)
	{
		CurrentToy = newToy;
		CurrentToyTree = newToy.IdleTreeRoot;
		oldToy.OnDeselect ();
		newToy.OnSelect ();
	}

	/// <summary>
	/// Deselects the currently selected toy.
	/// </summary>
	public static void DeselectToy()
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
	public static void EquipAccessory(Accessory acc)
	{
		if (CurrentToy) 
		{
			CurrentToyTree = CurrentToy.IdleTreeRoot;
			CurrentToy.SetIdleBehavior (IdleBehaviors.IdleStandDuringAction(IdleBehaviors.MoveAndEquipAccessory(CurrentToy,acc)));
		}
	}

	/// <summary>
	/// Makes the Toy walk to the specified location and then stand there.
	/// </summary>
	/// <param name="location">Location.</param>
	public static void MoveTo(Vector3 location)
	{
		if (CurrentToy) 
		{
			CurrentToyTree = CurrentToy.IdleTreeRoot;
			CurrentToy.SetIdleBehavior (IdleBehaviors.IdleStandDuringAction(new WalkTo(CurrentToy, location)));
		}
	}

	/// <summary>
	/// Activates the Core function of whatever Accessory the Toy has equipped.
	/// </summary>
	public static void ActivateCore(params Toy[] targets)
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
}
