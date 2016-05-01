using UnityEngine;
using TreeSharpPlus;
using System;
using System.Collections.Generic;

public class LovePotion : NeoAccessory {

    public GameObject equipModel;
    public float RotateSpeed = 100.0f;

    public override bool IsEquippable { get { return true; } }
    public override GameObject EquipModel { get { return equipModel; } }
    public override EquipSlots EquipSlot { get { return EquipSlots.LeftHand; } }

    void Update()
    {
        IdleRotate(transform, RotateSpeed);
    }

    public override void Effects()
    {
        // This Toy is happy and in love
        for (int i = 0; i < 5; i++)
        {
            toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
            toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
        }
    }

    public override void InitializePriorities()
    {
        TargetPriority = 15;
        ActionPriority = 20;
        EffectPriority = 65;
    }

    public override void InitializeTargets()
    {
        // Target is all other Toys in scene at time of initialization
        Targets.AddRange(Utils.GetAllOtherToysInSceneAsGameObjects(this.toy));
    }

    public override void InitializeAction()
    {
        Action =
            new DecoratorLoop(
                new Sequence(
                    // Show burst of 5 hearts
                    new DecoratorLoop(5,
                        new Sequence(
                            new LeafInvoke(() => { this.toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji); }),
                            new LeafWait(200)
                            )
                        ),
                    new DecoratorLoop(
                        new Sequence(
                            new LeafAssert(() => {
                                return Utils.TargetIsInRange(this.toy, Targets);
                            }),
                            // Below should only run if some target is in range
                            new LeafInvoke(() => {
                                Effects();
                            }),
                            new LeafWait(1000)
                            )
                        )
                    )
                );
    }

    public override Node GetParameterizedAction(Toy toy, NeoAccessory targetAccessory,
        NeoAccessory effectAccessory)
    {
        // If InputTargets doesn't contain a Toy, this won't work out well
        List<GameObject> InputTargets = targetAccessory.GetTargets();

        return new DecoratorLoop(
            new Sequence(
                // Show burst of 5 hearts
                new DecoratorLoop(5,
                    new Sequence(
                        new LeafInvoke(() => { toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji); }),
                        new LeafWait(200)
                        )
                    ),
                new DecoratorLoop(
                    new Sequence(
                        new LeafAssert(() => {
                            return Utils.TargetIsInRange(toy, InputTargets);
                        }),
                        // Below should only run if some target is in range
                        new LeafInvoke(() => {
                            effectAccessory.Effects();
                        }),
                        new LeafWait(1000)
                        )
                    )
                )
            );
    }
}
