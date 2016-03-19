using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class GenericSword : Accessory {

    public GameObject equipModel;
    public float RotateSpeed = 100.0f;

    public override string Archetype { get { return "GenericSword"; } }
    public override bool IsEquippable { get { return true; } }
    public override GameObject EquipModel { get { return equipModel; } }
    public override EquipSlots EquipSlot { get { return EquipSlots.RightHand; } }

    void Update()
    {
        IdleRotate(transform, RotateSpeed);
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
        return IdleBehaviors.CountTo3();
    }
}
