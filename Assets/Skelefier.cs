using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class Skelefier : NeoAccessory {

	public override bool IsEquippable{ get { return true;}}
	public override GameObject EquipModel { get { return EquipmentModel;}}
	public override EquipSlots EquipSlot{get {return EquipSlots.RightHand; }}

	public GameObject EquipmentModel;



	public override void InitializePriorities(){
		this.TargetPriority = Random.Range (0, 100);
		this.ActionPriority = Random.Range (0, 100);
		this.EffectPriority = Random.Range (0, 100);
	}

	public override void InitializeTargets(){
		GameObject randomToy = Utils.GetRandomOtherToyInSceneAsGameObject (toy);
		Targets.Add (randomToy);
		Debug.Log ("Added " + randomToy.name + " as a Skelefier target.");
	}

	public override void InitializeAction(){
		Action = new SequenceParallel (
			new Sequence(
				new LeafAssert(()=>{return Targets[0] != null;}),
				new WalkToToy(toy, Targets[0].GetComponent<Toy>())
			),
			new Sequence(
				new LeafAssert(() => {
					return Targets[0] != null; }),
				new LeafAssert(() => {
					return Utils.TargetIsInRange(toy, Targets[0]); }),
				new LeafInvoke(() => {
					this.toy.ShowEmoji(EmojiScript.EmojiTypes.Anger_Emoji); }),
				new LeafInvoke(() => { Effects(); })
			),
			new DecoratorLoop(
				IdleBehaviors.IdleStand()
			)
		);
	}

	public override void Effects(){
		Vector3 position = Targets [0].transform.position;
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.transform.position = position;
	}

	public override Node GetParameterizedAction(Toy toy, NeoAccessory targetAccessory, NeoAccessory effectAccessory){
		return null;
	}
}
