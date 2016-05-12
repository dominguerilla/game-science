using UnityEngine;
using TreeSharpPlus;
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

    public override void InitializePriorities()
    {
		hybridAccessory.SetPriorities(new int[4] { 0, 1, 0, 1});
		//hybridAccessory.SetPriorities(new int[4] { Random.Range(1, 100), Random.Range(1,100), Random.Range(1,100), Random.Range(1,100)});
    }

    public override void InitializeTargets()
    {
        // Target is all other Toys in scene at time of initialization
        List<GameObject> tempList = Utils.GetAllOtherToysInSceneAsGameObjects(this.toy.gameObject);
        hybridAccessory.SetTarget(new List<GameObject>(), hybridAccessory.ReturnPriority(0));

        foreach (GameObject o in tempList)
        {
            hybridAccessory.GetTarget().Add(o);
        }
    }

    public override void InitializeAction()
    {
        GameObject[] Targets = hybridAccessory.GetTarget().ToArray();
        Node Action =
            new DecoratorLoop(
                new Sequence(
                    // Show burst of 5 hearts
                    new DecoratorLoop(5,
                        new Sequence(
                            new LeafInvoke(() => {
                                this.toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji); }),
                            new LeafWait(200)
                            )
                        ),
                    new DecoratorLoop(
                        new Sequence(
                            // Old stuff: Failed attempt to get a random Toy in list as target
                            /*new DecoratorForceStatus(RunStatus.Success,
                            new DecoratorLoop(100,
                                new Sequence(
                                    new LeafTrace("Looking for Toy..."),
                                    // First, walk to a random location within 5 units of Toy
                                    new WalkToRandomRange(this.toy, 5f),
                                    // Check that a target is in range before proceeding
                                    new LeafInvoke(() => {
                                        SetTarget(Utils.GetToyInRange(this.toy, Targets, 8f));
                                    }),
                                    new LeafTrace("Current target == null? " + (currentTarget == null)),
                                    new LeafAssert(() => {
                                        return currentTarget == null;
                                    }))
                            )),*/
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
        hybridAccessory.SetAction(Action, hybridAccessory.ReturnPriority(1));
    }

	public override void InitializeEffects(){
        HybridAccessory.AccessoryFunction function = () =>
        {
            // This Toy is happy and in love
            for (int i = 0; i < 3; i++)
            {
                toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
                toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
            }
        };

        hybridAccessory.SetEffect(function, hybridAccessory.ReturnPriority(2));
    }

	public override void InitializeExecutionOrder(){
		hybridAccessory.SetExecutionPriority (Random.Range(1,100));
	}

	public override void InitializeCheckerFunction(){
		HybridAccessory.CheckerFunction function = () => {
			return true;
		};
		hybridAccessory.SetCheckerFunction (function);
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
    */
}