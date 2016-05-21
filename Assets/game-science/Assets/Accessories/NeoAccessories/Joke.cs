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

    // UNFINISHED -- NEED TO TEST
	public override void Initialize(){
        // For showing behavior: use Joke effect with LovePotion's targets/action
        hybridAccessory.SetPriorities(new int[4] { 0, 0, 9, 0 });

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
            hybridAccessory.GetTarget().Add(this.toy.gameObject);
        }

        // Effect
        HybridAccessory.AccessoryFunction effectFunction = () =>
        {
            if(hybridAccessory.GetTarget() == null)
            {
                Debug.Log("Joke: No target to perform effect on");
            }
            if (hybridAccessory.GetTarget()[0] != null)
            {
                Toy targetToy = hybridAccessory.GetTarget()[0].GetComponent<Toy>() as Toy;
                // Toy should just tell jokes to itself
                if(targetToy == null) { targetToy = this.toy; }

                if (Random.Range(0, 2) < 1) // Joke accepted
                    targetToy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji);
                else    // The joke is so bad it hurts!
                    targetToy.ShowEmoji(EmojiScript.EmojiTypes.Hurt_Emoji);
            }
            else
            {
                Debug.Log("Joke2: No target to perform effect on");
            }
        };
        hybridAccessory.SetEffect(effectFunction);

        // TreeFunction and Action
        HybridAccessory.TreeFunction function = (targets, effect) =>
        {
            WalkToToy walkNode;
            Node turnAndWaveNode;
            if(targets[0] != null && targets[0].GetComponent<Toy>() != null)
            {
                walkNode = new WalkToToy(toy, targets[0].GetComponent<Toy>() as Toy);
                turnAndWaveNode = IdleBehaviors.TurnAndWave(toy, targets[0].GetComponent<Toy>() as Toy);
            }
            else
            {   // Oh dear...
                Debug.Log("Joke: no targets found");
                walkNode = new WalkToToy(toy, toy);
                turnAndWaveNode = IdleBehaviors.TurnAndWave(toy, toy);
            }

            LeafInvoke effectExecute = new LeafInvoke(() => { effect(); });

            Node root = new DecoratorLoop(
                new SequenceParallel(
                    new Sequence(
                        // Wait a bit before walking
                        new LeafWait(500),
                        walkNode,
                        turnAndWaveNode,
                        new LeafInvoke(() => {
                            this.toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji); }),
                        // Wait for the joke
                        new LeafWait(500),
                        effectExecute
                        ),
                    // Show a laugh at the beginning of the behavior
                    new LeafInvoke(() => {
                        this.toy.ShowEmoji(EmojiScript.EmojiTypes.Laugh_Emoji); })));
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