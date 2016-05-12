using UnityEngine;
using System.Collections;
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
	}

	public override void InitializePriorities()
	{
		hybridAccessory.SetPriorities(new int[4] { Random.Range(1, 100), Random.Range(1,100), Random.Range(1,100), Random.Range(1,100)});
	}

	public override void InitializeTargets()
	{
		hybridAccessory.GetTarget().Add(this.gameObject);
	}

	public override void InitializeAction()
	{
		DecoratorLoop root = new DecoratorLoop(
			// Do this for 5 skellies
			5,
			new Sequence(
				// Walk to a nearby location
				new WalkToRandomRange(toy, 6f),
				// Summon a skelly warrior
				new GivePresent(toy, summonPrefab),
				new LeafInvoke(()=>{ hybridAccessory.ExecuteEffects();})
			)
		);

		hybridAccessory.SetAction (root, hybridAccessory.ReturnPriority (1));
	}

	public override void InitializeEffects()
	{
		HybridAccessory.AccessoryFunction function = () => {
			Debug.Log("Skeleton summoned.");
		};
		hybridAccessory.SetEffect(function, hybridAccessory.ReturnPriority(2));
	}

	public override void InitializeExecutionOrder(){
		hybridAccessory.SetExecutionPriority (Random.Range(1,100));
	}

	public override void InitializeCheckerFunction(){
		HybridAccessory.CheckerFunction function = () => {
			return true;
		};
		hybridAccessory.SetCheckerFunction (function);
	}


}
