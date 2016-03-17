using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using System.Collections.Generic;
using UnityEngine.Events;

[RequireComponent (typeof(SphereCollider))]
public class AccessoryPlus : Accessory
{

    public GameObject equipModel;
    public float RotateSpeed = 100.0f;
    public string type;

    public override string Archetype { get { return type; } }
    public override bool IsEquippable { get { return true; } }
    public override GameObject EquipModel { get { return equipModel; } }
    public override EquipSlots EquipSlot { get { return EquipSlots.RightHand; } }

    public UnityEvent callme;

    void Update()
    {
        IdleRotate(transform, RotateSpeed);
    }

    public override Node OnUse(Toy toy)
    {
        return new Sequence(
            new LeafAssert(() => { return IsEquippable; }),
            new LeafAssert(() => { return toy.GetAvailableSlotCount() > 0; }),
            new EquipAccessory(toy, this, EquipSlot),
            new LeafInvoke(() => { GameObject.Destroy(this); })
            );
    }

    public override Node ToyUse(Toy toy)
    {

        return null;
    }
    
}