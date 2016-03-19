﻿using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class HealthPotion : Accessory
{

    public GameObject equipModel;
    public float RotateSpeed = 100.0f;

    public override string Archetype { get { return "HealthPotion"; } }
    public override bool IsEquippable { get { return true; } }
    public override GameObject EquipModel { get { return equipModel; } }
    public override EquipSlots EquipSlot { get { return EquipSlots.LeftHand; } }

    void Update()
    {
        IdleRotate(transform, RotateSpeed);
    }

    public override Node OnUse(Toy toy)
    {
        return new Sequence(
            new LeafAssert(() => { return IsEquippable; }),
            new EquipAccessory(toy, this, EquipSlot),
            new LeafInvoke(() => { gameObject.SetActive(false); }),
            new LeafInvoke(() => { toy.Health = toy.Health + 25; if (toy.Health > 100) { toy.Health = 100; } })
            );
    }

    public override Node ToyUse(Toy toy)
    {
        return IdleBehaviors.CountTo3();
    }
}
