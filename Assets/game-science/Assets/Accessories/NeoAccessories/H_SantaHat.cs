using UnityEngine;
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
        hybridAccessory.SetEffect(effectFunction, hybridAccessory.ReturnPriority(2));

        // Treefunction and Action
        HybridAccessory.TreeFunction function = (targets, effect) =>
        {
            GivePresent presentNode = new GivePresent(toy, present);

            LeafInvoke effectExecute = new LeafInvoke(() => { effect(); });

            NavMeshAgent agent = toy.GetAgent();
            DecoratorLoop root = new DecoratorLoop(
                new Sequence(
                    // Walk somewhere
                    new WalkToRandomNewVector(toy),
                    new Sequence(
                        // Go to other character
                        new WalkToNearestCharacter(agent),
                        // Give them a present!
                        new GivePresent(toy, present),
                        // Show a heart
                        new LeafInvoke(() => {
                            toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                        }),
                        // Finally, execute effect
                        effectExecute
                    ),
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
	/*

	public override void InitializePriorities()
	{
        // For showing behavior: use GenericSword effect with SantaHat's targets/action
        hybridAccessory.SetPriorities(new int[4] { 9, 9, 0, 9 });

        // hybridAccessory.SetPriorities(new int[4] { Random.Range(1, 100), Random.Range(1,100), Random.Range(1,100), Random.Range(1,100)});
	}

	public override void InitializeTargets()
	{
		hybridAccessory.GetTarget().Add(this.gameObject);
	}

	public override void InitializeAction()
	{
        NavMeshAgent agent = toy.GetAgent();

        DecoratorLoop root = new DecoratorLoop(
            new Sequence(
                // Walk somewhere
                new WalkToRandomNewVector(toy),
                new Sequence(
                    // Check for a nearby character
                    new CheckForCharacterInRange(agent, 10f),
                    // Go to them, if they're nearby
                    new WalkToNearestCharacter(agent),
                    // Give them a present!
                    new GivePresent(toy, present),
                    // Show a heart
                    new LeafInvoke(() => {
                        toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
                    }),
                    new LeafInvoke(() => { hybridAccessory.ExecuteEffects(); })
                ),
                // Wait a bit
                new LeafWait(2000)));

		hybridAccessory.SetAction (root, hybridAccessory.ReturnPriority (1));
	}

	public override void InitializeEffects()
	{
		HybridAccessory.AccessoryFunction function = () => {
			Debug.Log("Gave Present.");
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
