using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class Heart : Accessory {

    public GameObject equipModel;
    public GameObject present;
    public float RotateSpeed = 100.0f;

    public override string Archetype { get { return "Heart"; } }
    public override bool IsEquippable { get { return true; } }
    public override GameObject EquipModel { get { return equipModel; } }
    public override EquipSlots EquipSlot { get { return EquipSlots.Head; } }

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

        NavMeshAgent agent = toy.GetAgent();

        return new DecoratorLoop(
            new Sequence(
                // Walk somewhere
                new WalkToRandomNewVector(toy),
                new Sequence(
                    // Check for a nearby character
                    new CheckForCharacterInRange(agent, 10f),
                    // Go to them, if they're nearby
                    new WalkToNearestCharacter(agent),
                    // Give them a present!
                    new GivePresent(toy, present)
                    ),
                // Wait a bit
                new LeafWait(2000)));
    }
}
