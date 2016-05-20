using UnityEngine;
using System.Collections.Generic;
using TreeSharpPlus;

public class H_Bone : NeoAccessory {

	public GameObject equipModel;
	public GameObject summonPrefab;

	public float RotateSpeed = 100.0f;

	public override bool IsEquippable { get { return true; } }
	public override GameObject EquipModel { get { return equipModel; } }
	public override EquipSlots EquipSlot { get { return EquipSlots.LeftHand; } }

	void Update()
	{
		IdleRotate(transform, RotateSpeed);
		int x = 0;
	}
		
	//recompile u shitter
	//pls recompile

	public override void Initialize(){ 
		//priorities
		hybridAccessory.SetPriorities(new int[4] { 0, 5, 0, 5});

		//targets
		hybridAccessory.GetTarget ().Add (summonPrefab);

		//effects
		HybridAccessory.AccessoryFunction effectFunction = () => {
			Debug.Log("Something summoned.");
		};
		hybridAccessory.SetEffect(effectFunction);

		//treefunction and action
		//In this function, the first GameObject in targets (targets[0]) is the prefab to summon, while the default effect is just logging "Something summoned." in the console.
		HybridAccessory.TreeFunction function = (targets, effect) => {
			//Debug.Log("H_Bone CreateTree function run!");
			GivePresent summonNode;
			if (targets[0] != null)
				summonNode = new GivePresent (toy, targets[0]);
			else
				summonNode = new GivePresent (toy, toy.gameObject);

			LeafInvoke effectExecute = new LeafInvoke (() => {effect();});

			DecoratorLoop root = new DecoratorLoop(
				// Do this for 5 skellies
				5,
				new Sequence(
					// Walk to a nearby location
					new WalkToRandomRange(toy, 6f),
					// Summon a thing
					summonNode,
					effectExecute
				)
			);
			return root;
		};
		hybridAccessory.SetTreeFunction (function);
		hybridAccessory.SetAction (hybridAccessory.CreateTree(hybridAccessory.GetTarget(), hybridAccessory.GetEffect()), hybridAccessory.ReturnPriority (1));

		//CheckerFunction
		HybridAccessory.CheckerFunction checker = () => {
			return true;
		};
		hybridAccessory.SetCheckerFunction (checker);
	}


}
