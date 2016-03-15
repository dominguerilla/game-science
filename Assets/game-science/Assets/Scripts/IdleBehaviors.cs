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
                // TODO possibly implement this later:
                // toy.Simple_Node_Require(IsStanding),

                new WalkToRandomNewVector(agent, toy),
                new LeafWait(2000)
        ));
    }

    // TODO doesn't work
    //[Affordance]
    public static Node WalkBackAndForth(Toy toy)
    {
        NavMeshAgent agent = toy.GetAgent;
        GameObject position1 = toy.GetWayPoint1;
        GameObject position2 = toy.GetWayPoint2;

        return new DecoratorLoop(
            new Sequence(
                new WalkTo(agent, position1),
                new LeafWait(2000),
                new WalkTo(agent, position2),
                new LeafWait(2000)
                )
            );
    }
}
