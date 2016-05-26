using UnityEngine;
using TreeSharpPlus;
using System.Collections.Generic;

public class Rose : NeoAccessory
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

    bool startBehavior = true;

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
                if (targetToy != null)
                {
                    if (UnityEngine.Random.Range(0, 2) < 1)
                    {   // Accepted
                        targetToy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                        this.toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
                    }
                    else {   // Rejected :(
                        targetToy.ShowEmoji(EmojiScript.EmojiTypes.Neutral_Emoji);
                        this.toy.ShowEmoji(EmojiScript.EmojiTypes.BrokenHeart_Emoji);
                    }
                }
            }
            else
            {
                // Nobody to love...
                this.toy.ShowEmoji(EmojiScript.EmojiTypes.BrokenHeart_Emoji);
            }
        };
        hybridAccessory.SetEffect(effectFunction);

        // Treefunction and Such
        HybridAccessory.TreeFunction function = (targets, effect) =>
        {
            WalkToToy walkNode;
            Node turnAndWaveNode;
            if (targets[0] != null)
            {
                walkNode = new WalkToToy(this.toy, targets[0].GetComponent<Toy>() as Toy);
                turnAndWaveNode = IdleBehaviors.TurnAndWave(this.toy, targets[0].GetComponent<Toy>() as Toy);
            }
            else
            {   // This will be interesting...
                Debug.Log("Rose: targets[0] is null");
                walkNode = new WalkToToy(this.toy, this.toy);
                turnAndWaveNode = IdleBehaviors.TurnAndWave(this.toy, this.toy);
            }

            LeafInvoke effectExecute = new LeafInvoke(() => { effect(); });

            Node root = new DecoratorLoop(
                new Sequence(
                    new LeafInvoke(() =>
                    {
                        if (startBehavior)
                        {
                            this.toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                        }
                        startBehavior = false;
                    }),
                    // Wait a bit before walking
                    new LeafWait(500),
                    walkNode,
                    turnAndWaveNode,
                    new LeafInvoke(() => {
                        this.toy.ShowEmoji(EmojiScript.EmojiTypes.Smiley_Emoji);
                    }),
                    // Wait for the proposal...
                    new LeafWait(1000),
                    effectExecute)
                );
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
		//hybridAccessory.SetPriorities(new int[4] { 1, 1, 0, 1});
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
            hybridAccessory.GetTarget ().Add (toy.gameObject);
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
                            return Targets[0] != null; }),
                        // Wait a bit before walking
                        new LeafWait(500),
                        new WalkToToy(toy, Targets[0].GetComponent<Toy>() as Toy),

                        new LeafTrace("Rose: target in range"),
                        IdleBehaviors.TurnAndWave(this.toy, Targets[0].GetComponent<Toy>() as Toy),
                        new LeafInvoke(() => {
                            this.toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji); }),
                        // Wait for the proposal
                        new LeafWait(500),
                        new LeafInvoke(() => { hybridAccessory.ExecuteEffects(); })
                        ),
                    // Show a heart at the beginning of the behavior
                    new LeafInvoke(() =>
                    {
                        this.toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
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
                    target.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                }
                else {   // Rejected
                    target.ShowEmoji(EmojiScript.EmojiTypes.Anger_Emoji);
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