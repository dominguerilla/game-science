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
        // Priorities
        hybridAccessory.SetPriorities(new int[4] { Random.Range(1, 100), Random.Range(1, 100), Random.Range(1, 100), Random.Range(1, 100) });

        // Target is one random other Toy in scene
        GameObject target = Utils.GetRandomOtherToyInSceneAsGameObject(toy);
        hybridAccessory.SetTarget(new List<GameObject>());
        if (target)
        {
            hybridAccessory.GetTarget().Add(target);
        }
        else
        {
            // To avoid errors, add the Toy itself so there's something in the Targets list
            hybridAccessory.GetTarget().Add(toy.gameObject);
        }

        // Effects
        HybridAccessory.AccessoryFunction effectFunction = () =>
        {
            if (hybridAccessory.GetTarget()[0] != null)
            {
                Toy targetToy = hybridAccessory.GetTarget()[0].GetComponent<Toy>();
                if (targetToy)
                {
                    if (id == 0)
                    {
                        if (UnityEngine.Random.Range(0, 2) < 1)
                        {   // Accepted
                            targetToy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                        }
                        else
                        {   // Rejected
                            reject = true;
                            targetToy.ShowEmoji(EmojiScript.EmojiTypes.Anger_Emoji);
                        }
                    }
                    else if (id == 1)
                    {
                        targetToy.ShowEmoji(EmojiScript.EmojiTypes.Hurt_Emoji);
                    }
                }
            }
            else
            {
                Debug.Log("Abuse effect has no target");
            }
        };
        hybridAccessory.SetEffect(effectFunction);


        // TreeFunction and Action
        HybridAccessory.TreeFunction function = (targets, effect) =>
        {
            WalkToToy walkNode;
            Node turnAndWaveNode;
            if (targets[0] != null)
            {
                if (targets[0].GetComponent<Toy>() != null)
                {
                    walkNode = new WalkToToy(this.toy, targets[0].GetComponent<Toy>() as Toy);
                    turnAndWaveNode = IdleBehaviors.TurnAndWave(this.toy, targets[0].GetComponent<Toy>() as Toy);
                }
                else
                {
                    walkNode = new WalkToToy(this.toy, this.toy);
                    turnAndWaveNode = IdleBehaviors.TurnAndWave(this.toy, this.toy);
                }
            }
            else
            {   // This will be interesting...
                Debug.Log("Abuse: targets[0] is null");
                walkNode = new WalkToToy(this.toy, this.toy);
                turnAndWaveNode = IdleBehaviors.TurnAndWave(this.toy, this.toy);
            }

            LeafInvoke effectExecute = new LeafInvoke(() => { effect(); });

            // Not sure if I ported this behavior over correctly
            Node root = new DecoratorLoop(
                new Sequence(
                    walkNode,
                    turnAndWaveNode,
                    new LeafInvoke(() =>
                    {
                        this.toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
                    }),
                    effectExecute,
                    new LeafInvoke(() =>
                    {
                        id = 1;
                        if (reject)
                        {
                            toy.GetAnimator().SetTrigger("Attack");
                            hybridAccessory.ExecuteEffects();
                            id = 0;
                        }
                    })));
            return root;
        };

        hybridAccessory.SetTreeFunction(function);
        hybridAccessory.SetAction(hybridAccessory.CreateTree(hybridAccessory.GetTarget(), hybridAccessory.GetEffect()), hybridAccessory.ReturnPriority(1));

        //CheckerFunction
        HybridAccessory.CheckerFunction checker = () => {
            return true;
        };
        hybridAccessory.SetCheckerFunction(checker);
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