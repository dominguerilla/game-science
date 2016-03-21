using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class SummoningSkull : Accessory
{

    public GameObject equipModel;
    public GameObject summonPrefab;
    public float RotateSpeed = 100.0f;

    public override string Archetype { get { return "SummoningSkull"; } }
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
            new LeafInvoke(() => { gameObject.SetActive(false); })
            );
    }

    public override Node ToyUse(Toy toy)
    {
        Debug.Log("Inside summon skull toyuse");

        DecoratorLoop root = new DecoratorLoop(
            // Do this for 5 skellies
            5,
            new Sequence(
                // Walk to a nearby location
                new WalkToRandomRange(toy, 6f),
                // Summon a skelly warrior
                new GivePresent(toy, summonPrefab)
            )
        );

        return IdleBehaviors.IdleStandDuringAction(root);
    }
}
