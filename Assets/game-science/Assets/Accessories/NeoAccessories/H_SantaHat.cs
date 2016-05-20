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
