﻿using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class SmartAxe : Accessory {

    public GameObject equipModel;
    public float RotateSpeed = 100.0f;

    public override string Archetype { get { return "SmartAxe"; } }
    public override bool IsEquippable { get { return true; } }
    public override GameObject EquipModel { get { return equipModel; } }

    

    private bool isEquipped; //Whether or not this item is equipped--this is a placeholder for state

	void Start () {
        isEquipped = false;
	}

    void Update()
    {
        IdleRotate(transform, RotateSpeed);
    }

    public override Node OnUse(Toy toy)
    {
        return new Sequence(
            new LeafAssert(() => { return IsEquippable; }),
            new LeafAssert(() => { return toy.GetAvailableSlotCount() > 0; }),
            new EquipAccessory(toy, this),
            new LeafInvoke(() => { GameObject.Destroy(this); })
            );
    }

    public override Node ToyUse(Toy toy)
    {
        return null;
    }

}
