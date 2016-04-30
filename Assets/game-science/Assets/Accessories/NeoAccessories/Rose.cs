﻿using UnityEngine;
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
        // Target shows either heart or anger
        Toy target = Targets[0].GetComponent<Toy>();
        if (target)
        {
            if (UnityEngine.Random.Range(0, 2) < 1)
            {   // Accepted
                target.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
            }
            else
            {   // Rejected
                target.ShowEmoji(EmojiScript.EmojiTypes.Anger_Emoji);
            }
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
                    new WalkToToy(toy, Targets[0].GetComponent<Toy>()),
                    new Sequence(
                        new LeafAssert(() => {
                            return Utils.TargetIsInRange(toy, Targets[0]);
                        }),
                        new LeafInvoke(() => {
                            // TODO: need to initialize Toy
                            // toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
                        }),
                        new LeafInvoke(() => { Effects(); })
                        ),
                    new LeafInvoke(() => {
                        // TODO: need to initialize Toy
                        // toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                    })));
    }

    public override Node GetParameterizedAction(Toy toy, NeoAccessory targetAccessory,
        NeoAccessory effectAccessory)
    {
        // If InputTargets doesn't contain a Toy, this won't work out well
        List<GameObject> InputTargets = targetAccessory.GetTargets();

        return new DecoratorLoop(
                new SequenceParallel(
                    new WalkToToy(toy, InputTargets[0].GetComponent<Toy>()),
                    new Sequence(
                        new LeafAssert(() => {
                            return Utils.TargetIsInRange(toy, InputTargets[0]); }),
                        // The following 2 should only run if target is in range
                        new LeafInvoke(() => {
                            toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji); }),
                        new LeafInvoke(() => { effectAccessory.Effects(); })
                        ),
                    new LeafInvoke(() => {
                        toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                    })));
    }
}
