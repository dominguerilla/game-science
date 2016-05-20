using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class Skelefier : NeoAccessory {

	public override bool IsEquippable{ get { return true;}}
	public override GameObject EquipModel { get { return EquipmentModel;}}
	public override EquipSlots EquipSlot{get {return EquipSlots.RightHand; }}

	public GameObject EquipmentModel;

	public override void Initialize(){

	}

	public  void InitializePriorities(){
		hybridAccessory.SetPriorities(new int[4] { Random.Range(1, 100), Random.Range(1,100), Random.Range(1,100), Random.Range(1,100)});
	}

	public  void InitializeTargets(){
		GameObject randomToy = Utils.GetRandomOtherToyInSceneAsGameObject (toy);
		Debug.Log ("Added " + randomToy.name + " as a Skelefier target.");
	}

	public void InitializeAction(){
		
	}

	public void InitializeEffects(){

	}


	public void InitializeCheckerFunction(){
		HybridAccessory.CheckerFunction function = () => {
			return true;
		};
		hybridAccessory.SetCheckerFunction (function);
	}



}
