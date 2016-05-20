using UnityEngine;
using TreeSharpPlus;
using System.Collections.Generic;

public class Abuse : NeoAccessory
{
    //built on rose
    public GameObject equipModel;
    public float RotateSpeed = 100.0f;

    public override bool IsEquippable { get { return true; } }
    public override GameObject EquipModel { get { return equipModel; } }
    public override EquipSlots EquipSlot { get { return EquipSlots.LeftHand; } }

    private bool reject = false;

    void Update()
    {
        IdleRotate(transform, RotateSpeed);
    }

    int id = 0;

	public override void Initialize(){

	}
	/*
    public override void InitializePriorities()
    {
		hybridAccessory.SetPriorities(new int[4] { Random.Range(1, 100), Random.Range(1,100), Random.Range(1,100), Random.Range(1,100)});
    }

    public override void InitializeTargets()
    {
        hybridAccessory.GetTarget().Add(this.gameObject);
    }

    public override void InitializeAction()
    {
        GameObject[] Targets = hybridAccessory.GetTarget().ToArray();

        Node Action =
            new DecoratorLoop(
                new SequenceParallel(
                    new Sequence(
                        new LeafAssert(() => {
                            return Targets[0] != null;
                        }),
                        new WalkToToy(toy, Targets[0].GetComponent<Toy>())
                    ),
                    new Sequence(
                        new LeafAssert(() => {
                            return Targets[0] != null;
                        }),
                        new LeafAssert(() => {
                            return Utils.TargetIsInRange(toy, Targets[0]);
                        }),
                        new LeafInvoke(() => {
                            this.toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
                        }),
                        new LeafInvoke(() => {
                            hybridAccessory.ExecuteEffects();
                            id = 1;
                        }),
                        new LeafInvoke(() =>
                        {
                            if (reject)
                            {
                                toy.GetAnimator().SetTrigger("Attack");
                                hybridAccessory.ExecuteEffects();
                                id = 0;
                            }
                        })
                        ),
                    new LeafInvoke(() => {
                        this.toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                    })));
                    
        hybridAccessory.SetAction(Action, hybridAccessory.ReturnPriority(1));
    }

	public override void InitializeEffects(){
        HybridAccessory.AccessoryFunction function = () =>
        {
            Toy target = hybridAccessory.GetTarget()[0].GetComponent<Toy>();
            if (target)
            {
                if (id == 0)
                {
                    if (UnityEngine.Random.Range(0, 2) < 1)
                    {   // Accepted
                        target.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                    }
                    else
                    {   // Rejected
                        reject = true;
                        target.ShowEmoji(EmojiScript.EmojiTypes.Anger_Emoji);
                    }
                }
                else if (id == 1)
                {
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