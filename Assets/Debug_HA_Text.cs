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

	public override void Initialize(){

		//priorities
		hybridAccessory.SetPriorities(new int[4] { TargetPriority, ActionPriority, EffectPriority, ExecutionPriority});

		//Targets
		hybridAccessory.SetTarget (DebugTargets);

		//Effects
		HybridAccessory.AccessoryFunction effectFunc= () => {
			Debug.Log(DebugActionText + this.RunCount);
			this.RunCount++;
		};
		hybridAccessory.SetEffect(effectFunc);

		//Action and TreeCreator
		HybridAccessory.TreeFunction treeFunc = (targets, effect) => {
			return new DecoratorLoop(
				new Sequence(
					new LeafInvoke(() => { effect();}),
					new LeafWait(5000)
				)
			);
		};
		hybridAccessory.SetTreeFunction(treeFunc);
		hybridAccessory.SetAction (hybridAccessory.CreateTree(null, hybridAccessory.GetEffect()));

		//CheckerFunction
		HybridAccessory.CheckerFunction function = () => {
			return true;
		};
		hybridAccessory.SetCheckerFunction (function);
	}
}
