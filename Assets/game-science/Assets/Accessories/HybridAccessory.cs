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
	public delegate Node TreeFunction (List<GameObject> targets, AccessoryFunction effects);

	private List<GameObject> Targets;
	private Node Action;
	private AccessoryFunction Effects;
	private CheckerFunction CheckerFunc;
	private TreeFunction TreeCreator;
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
        if(Accessories.Length < 1) { return null; }

		HybridAccessory newHybrid = new HybridAccessory ();

		foreach (HybridAccessory existingAcc in Accessories) {
			int[] existingPriorities = existingAcc.ReturnPriorities();

			//For targets
			if (existingPriorities [0] > newHybrid.ReturnPriority(0)) {
				newHybrid.SetTarget (existingAcc.GetTarget(), existingPriorities[0]);
			}

			//For actions and tree functions
			if (existingPriorities [1] > newHybrid.ReturnPriority(1)) {
				Node newRoot = existingAcc.CreateTree (newHybrid.GetTarget(), newHybrid.GetEffect());
				newHybrid.SetTreeFunction (existingAcc.GetTreeFunction());
				if (newRoot != null)
					newHybrid.SetAction (newRoot, existingPriorities [1]);
				else
					newHybrid.SetAction (existingAcc.GetAction(), existingPriorities[1]);
			}

			//For effects--note we have to create a new tree, just in case the effect function is called in the tree
			if (existingPriorities [2] > newHybrid.ReturnPriority(2)) {
				Node newRoot = newHybrid.CreateTree (newHybrid.GetTarget(), existingAcc.GetEffect());
				if (newRoot != null) {
					newHybrid.SetAction (newRoot, newHybrid.ReturnPriority(1));
				} else {
					newHybrid.SetAction (existingAcc.GetAction (), existingAcc.ReturnPriority(1));
				}
				newHybrid.SetEffect (existingAcc.GetEffect (), existingPriorities[2]);
			}

			//For execution priorities
			if (existingPriorities [3] > newHybrid.ReturnPriority (3)) {
				newHybrid.SetExecutionPriority (existingPriorities[3]);
			}
		}

		//This is just so that the new hybrid has a biggest priority
		newHybrid.SetExecutionPriority (newHybrid.ReturnPriority (3) + 1);


		HybridAccessory.CheckerFunction function = () => {
			return true;
		};
		newHybrid.SetCheckerFunction (function);

		newHybrid.PrintPriorities ();

        return newHybrid;
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

	/// <summary>
	/// Creates a new Action PBT with the input target list and effect function. If the TreeFunction TreeCreator is not set, will return null.
	/// </summary>
	/// <returns>The tree.</returns>
	/// <param name="Targets">Targets.</param>
	/// <param name="Effect">Effect.</param>
	public Node CreateTree(List<GameObject> Targets, AccessoryFunction Effect){
		if (TreeCreator == null) {
			Debug.Log ("No TreeFunction set for this HybridAccessory!");
			return null;
		} else {
			return TreeCreator (Targets, Effect);
		}
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
		Debug.Log ("Target set, initialized with priority " + priority);
	}

	public void SetTarget(List<GameObject> Targets){
		this.Targets = Targets;
		Debug.Log ("Targets set, with priority " + TargetPriority);
	}

	/// <summary>
	/// For PRIVATE USE/INITIALIZATION ONLY!
	/// </summary>
	public void SetAction(Node Action, int priority){ //this is obsolete
		this.Action = Action;
		this.ActionPriority = priority;
		Debug.Log ("Action set, initialized with priority " + priority);
	}

	public void SetAction(Node Action){
		this.Action = Action;
		Debug.Log ("Action set, with priority " + ActionPriority);
	}

	public void SetTreeFunction(TreeFunction function){
		this.TreeCreator = function;
	}

	public TreeFunction GetTreeFunction(){
		return this.TreeCreator;
	}

	/// <summary>
	/// For PRIVATE USE/INITIALIZATION ONLY!
	/// </summary>
	public void SetEffect(AccessoryFunction Effect, int priority){
		this.Effects = Effect;
		this.EffectPriority = priority;
		Debug.Log ("Effects set, initialized with priority " + priority);
	}

	public void SetEffect(AccessoryFunction Effect){
		this.Effects = Effect;
		Debug.Log ("Effects set, with priority " + EffectPriority);
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
		Debug.Log ("Checker function set, with priority " + ExecutionPriority);
	}

	public void SetEquipper(Toy toy){
		this.Equipper = toy;
	}
	#endregion
}
