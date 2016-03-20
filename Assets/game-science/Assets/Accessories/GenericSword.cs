using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class GenericSword : Accessory {

    public GameObject equipModel;
    public float RotateSpeed = 100.0f;

    public override string Archetype { get { return "GenericSword"; } }
    public override bool IsEquippable { get { return true; } }
    public override GameObject EquipModel { get { return equipModel; } }
    public override EquipSlots EquipSlot { get { return EquipSlots.RightHand; } }

    void Update()
    {
        IdleRotate(transform, RotateSpeed);
    }

    public override Node OnUse(Toy toy)
    {
        return new Sequence(
            new LeafAssert(() => { return IsEquippable; }),
            new EquipAccessory(toy, this, EquipSlot),
            new LeafInvoke(() => { toy.ChangeAttack(30); }),
            new LeafInvoke(() => { gameObject.SetActive(false); })
            );
    }

    public override Node ToyUse(Toy toy)
    {
        NavMeshAgent agent = toy.GetAgent();

        return new DecoratorLoop(
            new Sequence(
                new Sequence(
                    // Check for a nearby character
                    new CheckForCharacterInRange(agent, 100f),
                    // Go to them, if they're nearby
                    new WalkToNearestCharacter(agent),
                    new LeafWait(2000),

                    //assuming walktonearestcharacter will have the accessory face the target
                    //face and attack, use Core()
                    new LeafInvoke(() => { Core(toy); }),
                    new LeafWait(2000)
                    ),
                new WalkToRandomNewVector(toy),
                // Wait a bit
                new LeafWait(2000)));
    }

    public void Core(Toy toy) {
        //play animation 
        //check for collision 
        //if collide, damage
        RaycastHit hit;

        Animator anim = toy.GetAnimator();
        Vector3 rayDir = toy.transform.forward ;

        anim.SetTrigger("Attack");
        Debug.Log("attack!");

        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(toy.transform.position, rayDir, out hit, 3.0f))
        {
            anim.SetTrigger("Attack");
            if (hit.transform.gameObject.GetComponent<Toy>() != null)
            {
                Debug.Log("trigger");
                (hit.transform.gameObject).GetComponent<Toy>().ChangeHealth(-toy.GetAttack());
                //apply damage to hit.transform.gameObject
            }
        }
    }

}
