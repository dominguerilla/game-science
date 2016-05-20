using UnityEngine;
using TreeSharpPlus;
using System.Collections.Generic;

public class Mic : NeoAccessory {

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
		//hybridAccessory.SetPriorities(new int[4] { 3, 0, 0, 0});
		hybridAccessory.SetPriorities(new int[4] { Random.Range(1, 100), Random.Range(1,100), Random.Range(1,100), Random.Range(1,100)});
    }

    public override void InitializeTargets()
    {
        // Target is all other Toys in scene at time of initialization
        List<GameObject> tempList = Utils.GetAllOtherToysInSceneAsGameObjects(this.toy.gameObject);
        hybridAccessory.SetTarget(new List<GameObject>(), hybridAccessory.ReturnPriority(0));

        if (tempList != null)
        {
            foreach (GameObject o in tempList)
            {
                hybridAccessory.GetTarget().Add(o);
            }
        }
        else
        {
            // Add the Toy itself to ensure there's something in the Target list
            hybridAccessory.GetTarget().Add(this.toy.gameObject);
        }
    }

    public override void InitializeAction()
    {
        GameObject[] Targets = hybridAccessory.GetTarget().ToArray();
        Node Action =
            new DecoratorLoop(
                new Sequence(
                    // Show burst of 5 laughs
                    new DecoratorLoop(5,
                        new Sequence(
                            new LeafInvoke(() => {
                                this.toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
                            }),
                            new LeafWait(200)
                            )
                        ),
                    new DecoratorLoop(
                        new Sequence(
                            new LeafTrace("Target is: " + (Targets[0].GetComponent<Toy>() as Toy)),
                            new WalkToToy(this.toy, Targets[0].GetComponent<Toy>() as Toy),
                            // Have the Toy turn to face the target and wave
                            IdleBehaviors.TurnAndWave(this.toy, Targets[0].GetComponent<Toy>() as Toy),
                            new LeafInvoke(() => {
                                hybridAccessory.ExecuteEffects();
                            }),
                            new LeafWait(1000)
                            )
                        )
                    )
                );
    }

	public override void InitializeEffects(){
        HybridAccessory.AccessoryFunction function = () =>
        {
            for (int i = 0; i < 3; i++)
            {
                toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
            }
        };

        hybridAccessory.SetEffect(function, hybridAccessory.ReturnPriority(2));
    }
	/*
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
    *//*



	public override void InitializeCheckerFunction(){
		HybridAccessory.CheckerFunction function = () => {
			return true;
		};
		hybridAccessory.SetCheckerFunction (function);
	}
	*/
}
