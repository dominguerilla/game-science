﻿using TreeSharpPlus;
using UnityEngine;

public static class IdleBehaviors {

    public static Node TurnTowardsTarget(Toy toy, Toy target)
    {
        return new LeafInvoke(() =>
        {
            // For now, just turn instantly
            toy.transform.LookAt(target.transform);
        });
    }

    public static Node TurnAndWave(Toy toy, Toy target)
    {
        Animator anim = toy.GetAnimator();
        if (!anim) { return null; }

        return new Sequence(
            // Turn to the target
            TurnTowardsTarget(toy, target),
            // Wave at the target
            new LeafInvoke(() => {
                Debug.Log("TODO: Implement waving");
            })
            );
    }

    /// <summary>
    /// Tell the Toy to give off Emojis at a given time interval
    /// </summary>
    /// <param name="toy"></param>
    /// <param name="intervalInMillis"></param>
    /// <returns></returns>
    public static Node GiveOffEmojis(Toy toy, EmojiScript.EmojiTypes type, int intervalInMillis)
    {
        return new DecoratorLoop(
            new Sequence(
                new LeafInvoke(() => {
                    toy.ShowEmoji(type);
                }),
                new LeafWait(intervalInMillis)
            ));
    }

    /// <summary>
    /// Tell the Toy to show all the Emojis
    /// </summary>
    /// <param name="toy"></param>
    /// <returns></returns>
    public static Node GiveOffAllEmojis(Toy toy)
    {
        return new DecoratorLoop(
            new Sequence(
                new LeafInvoke(() => {
                    toy.ShowEmoji(EmojiScript.EmojiTypes.Anger_Emoji);
                }),
                new LeafWait(1000),

                new LeafInvoke(() => {
                    toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                }),
                new LeafWait(1000),

                new LeafInvoke(() => {
                    toy.ShowEmoji(EmojiScript.EmojiTypes.Hurt_Emoji);
                }),
                new LeafWait(1000),

                new LeafInvoke(() => {
                    toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
                }),
                new LeafWait(1000)
            ));
    }

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

        return new DecoratorLoop(
            new Sequence(
                new WalkToRandomNewVector(toy),
                new LeafWait(2000)
        ));
    }

    //[Affordance]
    public static Node WalkBackAndForth(Toy toy, GameObject position1, GameObject position2)
    {
        return new DecoratorLoop(
            new Sequence(
				new WalkTo(toy, position1.transform.position),
                new LeafWait(2000),
				new WalkTo(toy, position2.transform.position),
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
			new WalkTo(toy, acc.gameObject.transform.position),
                    new LeafAssert(() => { return acc.gameObject.activeInHierarchy; }), 
                    new LeafInvoke(() => { toy.Equip(acc); }),
                    acc.OnUse(toy)
        );
    }

    /// <summary>
    /// Moves the Toy to the position of the NeoAccessory and then attempts to equip it.
    /// </summary>
    /// <param name="toy"></param>
    /// <param name="acc"></param>
    /// <returns></returns>
    public static Node MoveAndEquipAccessory(Toy toy, NeoAccessory acc)
    {
        return new Sequence(
            new WalkTo(toy, acc.gameObject.transform.position),
            new LeafAssert(() => { return acc.gameObject.activeInHierarchy; }),
            new LeafInvoke(() => { toy.Equip(acc); })
        );
    }

    /// <summary>
    /// Makes the Attacker move to the Defender and attack repeatedly until the defender is dead.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    /// <returns> IdleStand() if either parameter is null.</returns>
    public static Node AttackUntilDead(Toy attacker, Toy defender)
    {
        if (attacker == null)
        {
            Debug.Log("Invalid attacker.");
            return IdleStand();
        }

        if (defender == null)
        {
            Debug.Log("Invalid or no target specified.");
            return IdleStand();
        }
        
        /*return new DecoratorInvert(
                new SequenceParallel(
                            new DecoratorLoop(
                                new DecoratorInvert(
                                    new LeafAssert(() => { return defender.GetHealth() <= 0; })
                                    )
                                ),
                                new DecoratorLoop(
                                    new Sequence(
                                        new WalkTo(attacker.GetAgent(), defender.gameObject),
                                        new Attack(attacker, defender)
                                        )
                                    )
                    )
            );*/
        return new DecoratorInvert(
                new SequenceParallel(
                    new DecoratorLoop(
                        new LeafAssert(() => { return defender.GetHealth() > 0; })
                    ),
                    new DecoratorLoop(
                            new DecoratorForceStatus(RunStatus.Success,
                                new Sequence(
                                    new LeafAssert(() => { return Vector3.Distance(attacker.transform.position, defender.transform.position) > attacker.GetAgent().stoppingDistance; }),
									new WalkTo(attacker, defender)
                                    )
                                )
                    ),
                    new DecoratorLoop(
                        new DecoratorForceStatus(RunStatus.Success,
                            new Sequence(
                                new LeafAssert(() => { return Vector3.Distance(attacker.transform.position, defender.transform.position) <= attacker.GetAgent().stoppingDistance; }),
                                new Attack(attacker, defender)
                                )
                            )
                    )

                )
            );
    }

}
