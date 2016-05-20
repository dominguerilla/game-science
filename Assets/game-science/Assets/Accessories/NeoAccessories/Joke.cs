using UnityEngine;
using TreeSharpPlus;
using System.Collections.Generic;

public class Joke : NeoAccessory
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

	public override void Initialize(){

	}
	/*
    public override void InitializePriorities()
    {
		//hybridAccessory.SetPriorities(new int[4] { 0, 0, 1, 0});
		hybridAccessory.SetPriorities(new int[4] { Random.Range(1, 100), Random.Range(1,100), Random.Range(1,100), Random.Range(1,100)});
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
        {
            // To avoid errors, add the Toy itself so there's something in the Targets list
            hybridAccessory.GetTarget().Add(toy.gameObject);
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
                        new LeafAssert(() => {
                            return Targets[0] != null;
                        }),
                        // Wait a bit before walking
                        new LeafWait(500),
                        new WalkToToy(toy, Targets[0].GetComponent<Toy>() as Toy),

                        new LeafTrace("Joke: target in range"),
                        IdleBehaviors.TurnAndWave(this.toy, Targets[0].GetComponent<Toy>() as Toy),
                        new LeafInvoke(() => {
                            this.toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
                        }),
                        // Wait for the joke
                        new LeafWait(500),
                        new LeafInvoke(() => { hybridAccessory.ExecuteEffects(); })
                        ),
                    // Show a laugh at the beginning of the behavior
                    new LeafInvoke(() =>
                    {
                        this.toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
                    })));

        hybridAccessory.SetAction(Action, hybridAccessory.ReturnPriority(1));

    }

    public override void InitializeEffects()
    {
        HybridAccessory.AccessoryFunction function = () =>
        {
            Toy target = hybridAccessory.GetTarget()[0].GetComponent<Toy>();
            if (target)
            {
                if (UnityEngine.Random.Range(0, 2) < 1)
                {   // Accepted
                    target.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
                }
                else {
                    // The joke is so bad it hurts!
                    target.ShowEmoji(EmojiScript.EmojiTypes.Hurt_Emoji);
                }
            }
        };

        hybridAccessory.SetEffect(function, hybridAccessory.ReturnPriority(2));
    }


	public override void InitializeCheckerFunction(){
		HybridAccessory.CheckerFunction function = () => {
			return true;
		};
		hybridAccessory.SetCheckerFunction (function);
	}

*/
}