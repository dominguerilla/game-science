using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class Skelefier : NeoAccessory {

	public override bool IsEquippable{ get { return true;}}
	public override GameObject EquipModel { get { return EquipmentModel;}}
	public override EquipSlots EquipSlot{get {return EquipSlots.RightHand; }}

	public GameObject EquipmentModel;

	//The toy equipping this Accessory.
	private Toy Equipper;

	public override void InitializePriorities(){
		this.TargetPriority = Random.Range (0, 100);
		this.ActionPriority = Random.Range (0, 100);
		this.EffectPriority = Random.Range (0, 100);
	}

	public override void InitializeTargets(){
		GameObject randomToy = Utils.GetRandomOtherToyInSceneAsGameObject ();
		Targets.Add (randomToy.GetComponent<Toy>());
		Debug.Log ("Added " + randomToy.name + " as a Skelefier target.");
	}

	public override void InitializeAction(){
		Action = new SequenceParallel (
			new Sequence(
				new LeafAssert(()=>{return Targets[0] != null;}),
				new WalkToToy(Equipper, Targets[0].GetComponent<Toy>())
			),
			new Sequence(
				new LeafAssert(() => {
					return Targets[0] != null; }),
				new LeafAssert(() => {
					return Utils.TargetIsInRange(Equipper, Targets[0]); }),
				new LeafInvoke(() => {
					this.Equipper.ShowEmoji(EmojiScript.EmojiTypes.Anger_Emoji); }),
				new LeafInvoke(() => { Effects(); })
			),
			new DecoratorLoop(
				IdleBehaviors.IdleStand()
			)
		);
	}
}
