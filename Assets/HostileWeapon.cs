using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class HostileWeapon : Accessory {

	public GameObject equipModel;
	public Toy target;
	public float RotateSpeed = 100.0f;

	public override string Archetype { get { return "HostileWeapon"; } }
	public override bool IsEquippable { get { return true; } }
	public override GameObject EquipModel { get { return equipModel; } }
	public override EquipSlots EquipSlot { get { return EquipSlots.RightHand; } }

	void Update()
	{
		IdleRotate(transform, RotateSpeed);
	}

	public override void IdleRotate(Transform obj, float speed)
	{
		obj.Rotate(Vector3.down * Time.deltaTime * speed);
	}


	public override Node OnUse(Toy toy)
	{
		return new Sequence(
			new LeafAssert(() => { return IsEquippable; }),
			new EquipAccessory(toy, this, EquipSlot),
			new LeafInvoke(() => { gameObject.SetActive(false); })
		);
	}

	public override Node ToyUse(Toy toy)
	{
		if (target == null) {
			Debug.Log ("No target Toy specified!");
			return IdleBehaviors.IdleStand ();
		}
		return IdleBehaviors.IdleStandDuringAction(IdleBehaviors.AttackUntilDead(toy, target));
	}

}