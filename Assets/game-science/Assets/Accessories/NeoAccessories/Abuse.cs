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
	/*
    public override void Effects()
    {
        // Target shows either heart or anger
        
		Toy target = Targets[0].GetComponent<Toy>();
        if (target)
        {
            if(id == 0)
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
            }else if (id == 1)
            {
                target.ShowEmoji(EmojiScript.EmojiTypes.Hurt_Emoji);
            }
        }
    }
    */

    public override void InitializePriorities()
    {
		/*
        TargetPriority = 9;
        ActionPriority = 56;
        EffectPriority = 11;
        */
		hybridAccessory.SetPriorities(new int[4] { Random.Range(1, 100), Random.Range(1,100), Random.Range(1,100), Random.Range(1,100)});
    }

    public override void InitializeTargets()
    {
        // Target is random other Toy in scene
        GameObject target = Utils.GetRandomOtherToyInSceneAsGameObject(toy);
        if (target)
        {
            //Targets.Add(target);
        }
        else
        {   // To avoid errors, add the Toy itself
            //Targets.Add(this.toy.gameObject);
        }
    }

    public override void InitializeAction()
    {
		/*
        Action =
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
                            Effects();
                            id = 1;
                        }),
                        new LeafInvoke(() =>
                        {
                            if (reject)
                            {
                                toy.GetAnimator().SetTrigger("Attack");
                                Effects();
                                id = 0;
                            }
                        })
                        ),
                    new LeafInvoke(() => {
                        this.toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                    })));
                    */
    }

	public override void InitializeEffects(){

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
        // Debug.Log("Rose.GetParameterizedAction entered");
        // If InputTargets doesn't contain a Toy, this won't work out well
        List<GameObject> InputTargets = targetAccessory.GetTargets();

        Debug.Log("Rose.GetParameterizedAction inputs: " + toy + ", "
            + targetAccessory + ", "
            + effectAccessory);

        targetAccessory.DEBUG_PrintTargets();

        return new DecoratorLoop(
                new SequenceParallel(
                    new Sequence(
                        new LeafAssert(() => {
                            return InputTargets[0] != null;
                        }),
                        new WalkToToy(toy, InputTargets[0].GetComponent<Toy>())
                    ),
                    new Sequence(
                        new LeafAssert(() => {
                            return InputTargets[0] != null;
                        }),
                        new LeafAssert(() => {
                            return Utils.TargetIsInRange(toy, InputTargets[0]);
                        }),
                        // The following 2 should only run if target is in range
                        new LeafInvoke(() => {
                            toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
                        }),
                        new LeafInvoke(() => { effectAccessory.Effects(); })
                        ),
                    new LeafInvoke(() => {
                        toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                    })));
    }
    */
}