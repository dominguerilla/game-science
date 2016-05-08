using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TreeSharpPlus;


/// <summary>
/// The object that actually holds the Targets, Actions, Effects, and their utility functions.
/// Different from NeoAccessory in that NeoAccessory is just an interface between the GameObject and this HybridAccessory.
/// </summary>
public class HybridAccessory {

	public delegate void EffectFunction();

	private List<GameObject> Targets;
	private Node Action;
	private EffectFunction Effects;
	private Toy Equipper;

	private int TargetPriority, ActionPriority, EffectPriority;

	/// <summary>
	/// Initializes a new instance of the <see cref="HybridAccessory"/> class.
	/// </summary>
	/// <param name="effects">Effect function. Pass a void() function to this.</param>
	/// <param name="priorities">Array of integers. [0] is Target priority, [1] is Action priority, [2] is Effect priority.</param>
	public HybridAccessory(List<GameObject> Targets, Node Action, EffectFunction effects, int[] priorities){
		this.Targets = Targets;
		this.Action = Action;
		this.Effects = effects;

		this.TargetPriority = priorities [0];
		this.ActionPriority = priorities [1];
		this.EffectPriority = priorities [2];
	}

	/// <summary>
	/// For creating a 'blank' hybrid Accessory.
	/// </summary>
	public HybridAccessory(){
		this.Targets = new List<GameObject> ();
		this.Action = new Sequence ();
		this.Effects = null;

		this.TargetPriority = -1;
		this.ActionPriority = -1;
		this.EffectPriority = -1;
	}

	/// <summary>
	/// Execute the stored effect function. If there is none, will display error message.
	/// </summary>
	public void ExecuteEffects(){
		if (Effects != null)
			Effects ();
		else
			Debug.Log ("No effect function set!");
	}

	//TODO If you guys know a faster way to do this, feel free to change it.
	/// <summary>
	/// Given a bunch of HybridAccessories, will return a HybridAccessory with the highest priorities found.
	/// This is should be the only way to combine HybridAccessories.
	/// </summary>
	/// <param name="Accessories">Accessories.</param>
	public static HybridAccessory HybridizeComponents(params HybridAccessory[] Accessories){
		int[] maxPriorities = new int[3];
		HybridAccessory hybrid = new HybridAccessory ();

		foreach (HybridAccessory acc in Accessories) {
			int[] priorities = acc.ReturnPriorities();
			if (priorities [0] > hybrid.ReturnPriority(0)) {
				hybrid.SetTarget (acc.GetTarget(), priorities[0]);
			}
			if (priorities [1] > hybrid.ReturnPriority(1)) {
				hybrid.SetAction (acc.GetAction(), priorities[1]);
			}
			if (priorities [2] > hybrid.ReturnPriority(2)) {
				hybrid.SetEffect (acc.GetEffect(), priorities[2]);
			}
		}

		return hybrid;
	}


	/// <summary>
	/// Prints the targets in this Accessory's target list.
	/// </summary>
	public void DisplayTargets(){
		Debug.Log ("Targets: ");
		foreach (GameObject target in Targets) {
			Debug.Log (target.name);
		}
	}


	/// <summary>
	/// Returns a priority, specified by an index. Returns -1 if invalid index specified.
	/// </summary>
	/// <param name="index">Index. '0' for Target, '1' for Action, '2' for Effects/</param>
	public int ReturnPriority(int index){
		if (index == 0) {
			return TargetPriority;
		} else if (index == 1) {
			return ActionPriority;
		} else if (index == 2) {
			return EffectPriority;
		} else {
			Debug.Log ("Invalid index requested: " + index);
			return -1;
		}
	}

	/// <summary>
	/// Returns all the priorities in an array.
	/// </summary>
	/// <returns>The priorities--[0] for Target, [1] for Action, [2] for Effect.</returns>
	public int[] ReturnPriorities(){
		int[] priorities = new int[3];
		priorities [0] = TargetPriority;
		priorities [1] = ActionPriority;
		priorities [2] = EffectPriority;
		return priorities;
	}


	//TODO PLEASE PLEASE PLEASE only use these for initializing HybridAccessories, NOT for changing other HybridAccessories!
	#region FOR PRIVATE USE/INITIALIZATION ONLY

	/// <summary>
	/// For PRIVATE USE/INITIALIZATION ONLY!
	/// </summary>
	public List<GameObject> GetTarget(){
		return this.Targets;
	}

	/// <summary>
	/// For PRIVATE USE/INITIALIZATION ONLY!
	/// </summary>
	public Node GetAction(){
		return this.Action;
	}

	/// <summary>
	/// For PRIVATE USE/INITIALIZATION ONLY!
	/// </summary>
	public EffectFunction GetEffect(){
		return this.Effects;
	}

	public Toy GetEquipper(){
		if (Equipper)
			return Equipper;
		else
			Debug.Log ("No Toy equipping this right now.");
		return null;
	}

	/// <summary>
	/// For PRIVATE USE/INITIALIZATION ONLY!
	/// </summary>
	public void SetTarget(List<GameObject> Targets, int priority){
		this.Targets = Targets;
		this.TargetPriority = priority;
		Debug.Log ("Target set, with priority " + priority);
	}

	/// <summary>
	/// For PRIVATE USE/INITIALIZATION ONLY!
	/// </summary>
	public void SetAction(Node Action, int priority){
		this.Action = Action;
		this.ActionPriority = priority;
		Debug.Log ("Action set, with priority " + priority);
	}

	/// <summary>
	/// For PRIVATE USE/INITIALIZATION ONLY!
	/// </summary>
	public void SetEffect(EffectFunction Effect, int priority){
		this.Effects = Effect;
		this.EffectPriority = priority;
		Debug.Log ("Effects set, with priority " + priority);
	}

	/// <summary>
	/// For PRIVATE USE/INITIALIZATION ONLY!
	/// </summary>
	public void SetPriorities(int[] priorities){
		if (priorities.Length != 3) {
			Debug.Log ("Invalid array size of " + priorities.Length);
			return;
		}
		TargetPriority = priorities [0];
		ActionPriority = priorities [1];
		EffectPriority = priorities [2];
	}

	public void SetEquipper(Toy toy){
		this.Equipper = toy;
	}
	#endregion
}
