using UnityEngine;
using TreeSharpPlus;
using System;
using System.Collections.Generic;

public class Rose : NeoAccessory {

    public GameObject equipModel;
    public float RotateSpeed = 100.0f;

    public override bool IsEquippable { get { return true; } }
    public override GameObject EquipModel { get { return equipModel; } }
    public override EquipSlots EquipSlot { get { return EquipSlots.LeftHand; } }

    // The Toy holding this accessory
    private Toy toy;

    private List<GameObject> Targets;
    private Node Action;

    private int TargetPriority, ActionPriority, EffectPriority;


    void Update()
    {
        IdleRotate(transform, RotateSpeed);
    }

    public override void Effects()
    {
        // Target shows heart for now
        Toy target = Targets[0].GetComponent<Toy>();
        if (target)
        {
            target.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
        }
    }

    public override void InitializePriorities()
    {
        TargetPriority = 9;
        ActionPriority = 56;
        EffectPriority = 11;
    }

    public override void InitializeTargets()
    {
        // Target is random other Toy in scene
        Targets.Add(Utils.GetRandomOtherToyInSceneAsGameObject(toy));
    }

    public override void InitializeAction()
    {
        Action =
            new DecoratorLoop(
                new SequenceParallel(
                    // IdleBehaviors.WalkTo(Targets.get(0)),
                    new Sequence(
                        // IdleBehaviors.CheckForTargetInRange(Targets.get(0))),
                        new LeafInvoke(() => {
                    // toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
                })),
                    new LeafInvoke(() => {
                // toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
            })));
    }
}
