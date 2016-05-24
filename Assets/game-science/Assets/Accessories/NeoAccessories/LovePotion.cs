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

	public override void Initialize(){
        // For showing behavior: use LovePotion target & action, use Joke effect
        hybridAccessory.SetPriorities(new int[4] { 9, 9, 0, 9 });

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

        // Effect
        HybridAccessory.AccessoryFunction effectFunction = () =>
        {
            // This Toy is in so much love
            toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
        };
        hybridAccessory.SetEffect(effectFunction);

        // TreeFunction & Action
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
                Debug.Log("LovePotion: targets[0] is null");
                walkNode = new WalkToToy(this.toy, this.toy);
                turnAndWaveNode = IdleBehaviors.TurnAndWave(this.toy, this.toy);
            }

            LeafInvoke effectExecute = new LeafInvoke(() => { effect(); });

            DecoratorLoop root = new DecoratorLoop(
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
                            walkNode,
                            turnAndWaveNode,
                            // Show 3 hearts, or execute some other effect 3 times in a row
                            new DecoratorLoop(3,
                                new Sequence(
                                    effectExecute,
                                    new LeafWait(200)
                                )),
                            new LeafWait(1000))
                        )
                    )
                );
            return root;
        };
        hybridAccessory.SetTreeFunction(function);
        hybridAccessory.SetAction(
            hybridAccessory.CreateTree(
                hybridAccessory.GetTarget(), hybridAccessory.GetEffect()),
            hybridAccessory.ReturnPriority(1));

        // CheckerFunction
        HybridAccessory.CheckerFunction checker = () => {
            return true;
        };
        hybridAccessory.SetCheckerFunction(checker);
    }
       
}