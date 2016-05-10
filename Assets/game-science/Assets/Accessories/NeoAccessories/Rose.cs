using UnityEngine;
using TreeSharpPlus;
using System;
using System.Collections.Generic;

public class Rose : NeoAccessory
{

    public GameObject equipModel;
    public float RotateSpeed = 100.0f;

    public override bool IsEquippable { get { return true; } }
    public override GameObject EquipModel { get { return equipModel; } }
    public override EquipSlots EquipSlot { get { return EquipSlots.LeftHand; } }

    void Update()
    {
        IdleRotate(transform, RotateSpeed);
    }


    public override void InitializePriorities()
    {
        hybridAccessory.SetPriorities(new int[3] { 99, 1, 99 });
    }

    public override void InitializeTargets()
    {
        // Target is one random other Toy in scene
        GameObject target = Utils.GetRandomOtherToyInSceneAsGameObject(toy);
        if (target)
        {
            hybridAccessory.SetTarget(new List<GameObject>(), hybridAccessory.ReturnPriority(0));
            hybridAccessory.GetTarget().Add(target);
        }
        else
        {   // To avoid errors, add the Toy itself
            //hybridAccessory.GetTarget ().Add (toy.gameObject);
        }
    }

    public override void InitializeAction()
    {
        GameObject[] Targets = hybridAccessory.GetTarget().ToArray();
        Node Action =
            new DecoratorLoop(
                new SequenceParallel(
                    new Sequence(
                        // Need a target
                        new LeafAssert(() =>
                        {
                            return Targets[0] != null;
                        }),
                        // Wait a bit before walking
                        new LeafWait(500),
                        new WalkToToy(toy, Targets[0].GetComponent<Toy>())
                    ),
                    new Sequence(
                        // Need a target
                        new LeafAssert(() =>
                        {
                            return Targets[0] != null;
                        }),
                        new LeafTrace("Rose: target not null"),
                        // Need target to be in range, and not null
                        new LeafAssert(() =>
                        {
                            return Utils.TargetIsInRange(toy, Targets[0]);
                        }),
                        new LeafTrace("Rose: target in range"),
                        // Once it's in range, laugh and execute effect
                        new LeafInvoke(() =>
                        {
                            this.toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
                        }),
                        new LeafInvoke(() => { hybridAccessory.ExecuteEffects(); })
                        ),
                    // Show a heart at the beginning of the behavior
                    new LeafInvoke(() =>
                    {
                        this.toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                    })));

        hybridAccessory.SetAction(Action, hybridAccessory.ReturnPriority(1));

    }

    public override void InitializeEffects()
    {
        HybridAccessory.EffectFunction function = () =>
        {
            Toy target = hybridAccessory.GetTarget()[0].GetComponent<Toy>();
            if (target)
            {
                if (UnityEngine.Random.Range(0, 2) < 1)
                {   // Accepted
                    target.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                }
                else {   // Rejected
                    target.ShowEmoji(EmojiScript.EmojiTypes.Anger_Emoji);
                }
            }
        };

        hybridAccessory.SetEffect(function, hybridAccessory.ReturnPriority(2));
    }
}