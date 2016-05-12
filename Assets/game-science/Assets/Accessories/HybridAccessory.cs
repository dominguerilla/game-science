using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TreeSharpPlus;


/// <summary>
/// The object that actually holds the Targets, Actions, Effects, and their utility functions.
/// Different from NeoAccessory in that NeoAccessory is just an interface between the GameObject and this HybridAccessory.
/// </summary>
public class HybridAccessory {

	public delegate void AccessoryFunction();
	public delegate bool CheckerFunction();

	private List<GameObject> Targets;
	private Node Action;
	private AccessoryFunction Effects;
	private CheckerFunction CheckerFunc;
	private Toy Equipper;

	private int TargetPriority, ActionPriority, EffectPriority, ExecutionPriority;

	/// <summary>
	/// Initializes a new instance of the <see cref="HybridAccessory"/> class.
	/// </summary>
	/// <param name="effects">Effect function. Pass a void() function to this.</param>
	/// <param name="priorities">Array of integers. [0] is Target priority, [1] is Action priority, [2] is Effect priority.</param>
	public HybridAccessory(List<GameObject> Targets, Node Action, AccessoryFunction effects, CheckerFunction checker, int[] priorities){
		this.Targets = Targets;
		this.Action = Action;
		this.Effects = effects;
		this.CheckerFunc = checker;

		if (priorities.Length != 4) {
			Debug.Log ("Incorrect length of input priorities!");
			this.TargetPriority = -1;
			this.ActionPriority = -1;
			this.EffectPriority = -1;
			this.ExecutionPriority = -1;
		} else {
			this.TargetPriority = priorities [0];
			this.ActionPriority = priorities [1];
			this.EffectPriority = priorities [2];
			this.ExecutionPriority = priorities [3];
		}

	}

	/// <summary>
	/// For creating a 'blank' hybrid Accessory.
	/// </summary>
	public HybridAccessory(){
		this.Targets = new List<GameObject> ();
		this.Action = new Sequence ();
		this.Effects = null;
		this.CheckerFunc = null;

		this.TargetPriority = -1;
		this.ActionPriority = -1;
		this.EffectPriority = -1;
		this.ExecutionPriority = -1;
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
		List<int> ExecutionPriorities = new List<int> ();

        if(Accessories.Length < 1) { return null; }

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
			ExecutionPriorities.Add (priorities[3]);
		}
		int max = ExecutionPriorities [0];
		for(int i = 0; i < ExecutionPriorities.Count; i++){
			max = Mathf.Max (max, ExecutionPriorities[i]);
		}
		hybrid.SetExecutionPriority (max);
		HybridAccessory.CheckerFunction function = () => {
			return true;
		};
		hybrid.SetCheckerFunction (function);

		hybrid.PrintPriorities ();

        return hybrid;
	}

	public void PrintPriorities(){
		Debug.Log ("Hybrid Priorities:");
		Debug.Log("\t" + this.ReturnPriority(0) + ", "
			+ this.ReturnPriority(1) + ", "
			+ this.ReturnPriority(2) + ", "
			+ this.ReturnPriority(3));
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
	/// <param name="index">Index. '0' for Target, '1' for Action, '2' for Effects, 3 for Execution Order/</param>
	public int ReturnPriority(int index){
		if (index == 0) {
			return TargetPriority;
		} else if (index == 1) {
			return ActionPriority;
		} else if (index == 2) {
			return EffectPriority;
		} else if (index == 3) {
			return ExecutionPriority;
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
		int[] priorities = new int[4];
		priorities [0] = TargetPriority;
		priorities [1] = ActionPriority;
		priorities [2] = EffectPriority;
		priorities [3] = ExecutionPriority;
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
	public AccessoryFunction GetEffect(){
		return this.Effects;
	}

	public Toy GetEquipper(){
		if (Equipper)
			return Equipper;
		else
			Debug.Log ("No Toy equipping this right now.");
		return null;
	}

	public CheckerFunction GetCheckerFunction(){
		return CheckerFunc;
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
	public void SetEffect(AccessoryFunction Effect, int priority){
		this.Effects = Effect;
		this.EffectPriority = priority;
		Debug.Log ("Effects set, with priority " + priority);
	}

	/// <summary>
	/// For PRIVATE USE/INITIALIZATION ONLY!
	/// </summary>
	public void SetPriorities(int[] priorities){
		if (priorities.Length != 4) {
			Debug.Log ("Invalid array size of " + priorities.Length);
			return;
		}
		TargetPriority = priorities [0];
		ActionPriority = priorities [1];
		EffectPriority = priorities [2];
		ExecutionPriority = priorities [3];
	}

	public void SetExecutionPriority(int priority){
		this.ExecutionPriority = priority;
		Debug.Log ("Execution priority set as " + priority);
	}

	public void SetCheckerFunction(CheckerFunction function){
		this.CheckerFunc = function;
	}

	public void SetEquipper(Toy toy){
		this.Equipper = toy;
	}
	#endregion
}
