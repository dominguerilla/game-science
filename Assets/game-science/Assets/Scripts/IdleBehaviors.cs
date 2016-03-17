using TreeSharpPlus;
using UnityEngine;

public static class IdleBehaviors {

    //[Affordance]
    public static Node TestBehavior(Toy toy)
    {
        NavMeshAgent agent = toy.GetAgent;

        return new DecoratorLoop(
            new Sequence(
                new WalkToRandom(agent)
                ));
    }

    //[Affordance]
    public static Node IdleWander(Toy toy)
    {
        NavMeshAgent agent = toy.GetAgent;

        //Debug.Log("Inside IdleWander");

        return new DecoratorLoop(
            new Sequence(
                toy.Simple_Node_Require(SimpleStateDef.IsStanding),

                new WalkToRandomNewVector(agent, toy),
                new LeafWait(2000)
        ));
    }

    //[Affordance]
    public static Node WalkBackAndForth(Toy toy, GameObject position1, GameObject position2)
    {
        NavMeshAgent agent = toy.GetAgent;

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
        return new DecoratorLoop(
                        new SequenceParallel(
                            subtree,
                            new DecoratorLoop(
                                new LeafWait(2000)    
                                )
                        )
                    );
    }

    public static Node StopBehaviorTest()
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
}
