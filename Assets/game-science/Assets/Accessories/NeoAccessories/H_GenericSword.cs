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

	public override void Initialize(){

	}

	/*
	public override void InitializePriorities()
	{
        // For showing behavior: use GenericSword effect with SantaHat's targets/action
        hybridAccessory.SetPriorities(new int[4] { 0, 0, 9, 0 });

        //hybridAccessory.SetPriorities(new int[4] { Random.Range(1, 100), Random.Range(1,100), Random.Range(1,100), Random.Range(1,100)});
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
                // Check for a nearby character
                new CheckForCharacterInRange(agent, 100f),
                // Go to them, if they're nearby
                new WalkToNearestCharacter(agent),
                new LeafWait(2000),

                //assuming walktonearestcharacter will have the accessory face the target
                //face and attack, use Core()
                new LeafInvoke(() => { hybridAccessory.ExecuteEffects(); }),
                new LeafWait(2000),
                new WalkToRandomNewVector(toy),
                // Wait a bit
                new LeafWait(2000)));

		hybridAccessory.SetAction (root, hybridAccessory.ReturnPriority (1));
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

    public override void InitializeEffects()
	{
        HybridAccessory.AccessoryFunction function = () => {
            Core(this.toy);
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
