﻿using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class H_SantaHat : NeoAccessory {

	public GameObject equipModel;
	public GameObject present;

	public float RotateSpeed = 100.0f;

	public override bool IsEquippable { get { return true; } }
	public override GameObject EquipModel { get { return equipModel; } }
	public override EquipSlots EquipSlot { get { return EquipSlots.LeftHand; } }

	void Update()
	{
		IdleRotate(transform, RotateSpeed);
	}

	public override void Initialize(){
        // For showing behavior: use GenericSword effect with SantaHat's targets/action
        hybridAccessory.SetPriorities(new int[4] { 9, 9, 0, 9 });

        // Targets
        hybridAccessory.GetTarget().Add(this.gameObject);

        // Effects (just a debug for this one)
        HybridAccessory.AccessoryFunction effectFunction = () => {
            Debug.Log("Gave Present.");
        };
        hybridAccessory.SetEffect(effectFunction);

        // Treefunction and Action
        HybridAccessory.TreeFunction function = (targets, effect) =>
        {
            NavMeshAgent agent = toy.GetAgent();

            LeafInvoke effectExecute = new LeafInvoke(() => { effect(); });
            Sequence behaviorNode;
            if(Utils.GetAllOtherToysInSceneAsGameObjects(this.toy.gameObject) == null)
            {
                // Toy will randomly walk around, giving presents and executing effect
                behaviorNode = new Sequence(
                    new GivePresent(toy, present),
                    new LeafWait(200),
                    effectExecute);
            }
            else
            {
                behaviorNode = new Sequence(
                        // Go to other character
                        new WalkToNearestCharacter(agent),
                        // Give them a present!
                        new GivePresent(toy, present),
                        // Smile
                        new LeafInvoke(() =>
                        {
                            toy.ShowEmoji(EmojiScript.EmojiTypes.Smiley_Emoji);
                        }),
                        // Finally, execute effect
                        new LeafWait(200),
                        effectExecute
                    );
            }

            DecoratorLoop root = new DecoratorLoop(
                new Sequence(
                    // Walk somewhere
                    new WalkToRandomNewVector(toy),
                    // Execute behavior
                    behaviorNode,
                    // Wait a bit
                    new LeafWait(2000)));
            return root;
        };
        hybridAccessory.SetTreeFunction(function);
        hybridAccessory.SetAction(
            hybridAccessory.CreateTree(
                hybridAccessory.GetTarget(), hybridAccessory.GetEffect()),
            hybridAccessory.ReturnPriority(1));

        // Checkmate
        HybridAccessory.CheckerFunction checker = () => {
            return true;
        };
        hybridAccessory.SetCheckerFunction(checker);
    }
}
