using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class SheathedSword : Accessory
{

    public GameObject equipModel;
    public float RotateSpeed = 100.0f;

    public override string Archetype { get { return "SheathedSword"; } }
    public override bool IsEquippable { get { return true; } }
    public override GameObject EquipModel { get { return equipModel; } }
    public override EquipSlots EquipSlot { get { return EquipSlots.RightHand; } }

    void Update()
    {
        IdleRotate(transform, RotateSpeed);
    }

    public override void IdleRotate(Transform obj, float speed)
    {
        obj.Rotate(Vector3.forward * Time.deltaTime * speed);
    }

    public override Node OnUse(Toy toy)
    {
        return new Sequence(
            new LeafAssert(() => { return IsEquippable; }),
            new EquipAccessory(toy, this, EquipSlot),
            new LeafInvoke(() => { toy.ChangeAttack(10.0f); gameObject.SetActive(false); })
            );
    }

    public override Node ToyUse(Toy toy)
    {
        return IdleBehaviors.IdleStandDuringAction(IdleBehaviors.AttackUntilDead(toy, toy.targetToy.GetComponent<Toy>()));
    }
}
