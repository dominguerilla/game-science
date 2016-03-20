using TreeSharpPlus;
using UnityEngine;

public static class IdleBehaviors {

    //[Affordance]
    public static Node TestBehavior(Toy toy)
    {
        NavMeshAgent agent = toy.GetAgent();

        return new DecoratorLoop(
            new Sequence(
                new WalkToRandom(agent)
                ));
    }

    //[Affordance]
    public static Node IdleWander(Toy toy)
    {
        NavMeshAgent agent = toy.GetAgent();

        //Debug.Log("Inside IdleWander");

        return new DecoratorLoop(
            new Sequence(
                toy.Simple_Node_Require(SimpleStateDef.IsStanding),

                new WalkToRandomNewVector(toy),
                new LeafWait(2000)
        ));
    }

    //[Affordance]
    public static Node WalkBackAndForth(Toy toy, GameObject position1, GameObject position2)
    {
        NavMeshAgent agent = toy.GetAgent();

        return new DecoratorLoop(
            new Sequence(
                new WalkTo(agent, position1),
                new LeafWait(2000),
                new WalkTo(agent, position2),
                new LeafWait(2000)
                )
            );
    }


    public static Node IdleStand()
    {
        return new DecoratorLoop(
                new LeafWait(2000)
            );
    }

    /// <summary>
    /// Returns a PBT that will perform an action, and then make the character return to standing idly.
    /// </summary>
    /// <param name="subtree">The action that will be performed before standing idly.</param>
    /// <returns>The root node of the overall PBT.</returns>
    public static Node IdleStandDuringAction(Node subtree)
    {
        return new SequenceParallel(
                            subtree,
                            IdleStand()
                        );  
    }

    /// <summary>
    /// Counts from 1 to 3 forever, with 3 second intervals between each count.
    /// </summary>
    /// <returns></returns>
    public static Node CountTo3()
    {
        return new DecoratorLoop(
                new Sequence(
                        new LeafTrace("Leaf 1"),
                        new LeafWait(3000),
                        new LeafTrace("Leaf 2"),
                        new LeafWait(3000),
                        new LeafTrace("Leaf 3"),
                        new LeafWait(3000)
                    )
            );
    }

    /// <summary>
    /// Moves the Toy to the position of the Accessory and then attempts to equip it.
    /// </summary>
    /// <param name="toy"></param>
    /// <param name="acc"></param>
    /// <returns></returns>
    public static Node MoveAndEquipAccessory(Toy toy, Accessory acc)
    {
        return new Sequence(
                    new WalkTo(toy.GetAgent(), acc.gameObject),
                    new LeafAssert(() => { return acc.gameObject.activeInHierarchy; }), 
                    new LeafInvoke(() => { toy.Equip(acc); }),
                    new LeafWait(1000),
                    acc.OnUse(toy)
        );
    }

    public static Node AttackUntilDead(Toy attacker, Toy defender)
    {
        return new DecoratorInvert(
            new Sequence(
                new WalkTo(attacker.GetAgent(), defender.gameObject),
                new SequenceParallel(
                            new DecoratorLoop(
                                new DecoratorInvert(
                                    new LeafAssert(() => { return defender.GetHealth() <= 0; })
                                    )
                                ),
                                new DecoratorLoop(
                                    new Attack(attacker, defender)
                                    )
                    )
            ));
    }
}
