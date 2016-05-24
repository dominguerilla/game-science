using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class H_GenericSword : NeoAccessory {

	public GameObject equipModel;

	public float RotateSpeed = 100.0f;
    public float AttackDamage = 10.0f;

    public override bool IsEquippable { get { return true; } }
	public override GameObject EquipModel { get { return equipModel; } }
	public override EquipSlots EquipSlot { get { return EquipSlots.LeftHand; } }

	void Update()
	{
		IdleRotate(transform, RotateSpeed);
	}

    public override void Initialize()
    {
        // For showing behavior: use GenericSword effect with SantaHat's targets/action
        hybridAccessory.SetPriorities(new int[4] { 0, 0, 9, 0 });

        // Targets
        hybridAccessory.GetTarget().Add(this.gameObject);

        // Effects (it's the old Core function for Generic Sword)
        HybridAccessory.AccessoryFunction effectFunction = () =>
        {
            Core(this.toy);
        };
        hybridAccessory.SetEffect(effectFunction, hybridAccessory.ReturnPriority(2));

        // Treefunction and Action. Don't care about targets for this one.
        HybridAccessory.TreeFunction function = (targets, effect) =>
        {
            NavMeshAgent agent = toy.GetAgent();
            LeafInvoke effectExecute = new LeafInvoke(() => { effect(); });

            DecoratorLoop root = new DecoratorLoop(
                new Sequence(
                    // Check for a nearby character
                    new CheckForCharacterInRange(agent, 100f),
                    // Go to them, if they're nearby
                    new WalkToNearestCharacter(agent, 0.2f),
                    new LeafWait(2000),

                    //face and attack, use Core()
                    // TODO: make the Toy face the nearest character
                    effectExecute,

                    new LeafWait(1000),
                    new WalkToRandomRange(toy, 1f),
                    // Wait a bit
                    new LeafWait(1000)));
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

    public void Core(Toy toy)
    {
        //play animation 
        //check for collision 
        //if collide, damage
        RaycastHit hit;

        Animator anim = toy.GetAnimator();
        Vector3 rayDir = toy.transform.forward;

        anim.SetTrigger("Attack");
        //Debug.Log("attack!");

        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(toy.transform.position, rayDir, out hit, 3.0f))
        {
            anim.SetTrigger("Attack");
            if (hit.transform.gameObject.GetComponent<Toy>() != null)
            {
                Debug.Log("Found hit");
                (hit.transform.gameObject).GetComponent<Toy>().ChangeHealth(-AttackDamage);
                //apply damage to hit.transform.gameObject
            }
        }
    }
}
