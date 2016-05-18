using UnityEngine;
using System.Collections.Generic;
using TreeSharpPlus;

public class Debug_HA_Text : NeoAccessory {

	public List<GameObject> DebugTargets;

	public string DebugActionText;
	public int TargetPriority;
	public int ActionPriority;
	public int EffectPriority;
	public int ExecutionPriority;
	public int RunCount = 0;

	public GameObject equipModel;

	public float RotateSpeed = 100.0f;

	public override bool IsEquippable { get { return true; } }
	public override GameObject EquipModel { get { return equipModel; } }
	public override EquipSlots EquipSlot { get { return EquipSlots.LeftHand; } }

	void Update()
	{
		IdleRotate(transform, RotateSpeed);
	}

	public override void InitializePriorities()
	{
		hybridAccessory.SetPriorities(new int[4] { TargetPriority, ActionPriority, EffectPriority, ExecutionPriority});
	}

	public override void InitializeTargets()
	{
		hybridAccessory.SetTarget (DebugTargets);
	}

	public override void InitializeAction()
	{
		DecoratorLoop root = new DecoratorLoop(
			new Sequence(
				new LeafInvoke(() => { hybridAccessory.ExecuteEffects();}),
				new LeafWait(5000)
			)
		);

		hybridAccessory.SetAction (root);
	}

	//recompile pls
	public override void InitializeEffects()
	{
		HybridAccessory.AccessoryFunction function = () => {
			Debug.Log(DebugActionText + this.RunCount);
			this.RunCount++;
		};
		hybridAccessory.SetEffect(function);
	}
		

	public override void InitializeCheckerFunction(){
		HybridAccessory.CheckerFunction function = () => {
			return true;
		};
		hybridAccessory.SetCheckerFunction (function);
	}
}
