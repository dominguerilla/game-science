using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class SmartHold : Accessory
{
   

    public GameObject equipModel;
    public float RotateSpeed = 50.0f;

    public override string Archetype { get { return "SmartHold"; } }
    public override bool IsEquippable { get { return true; } }
    public override GameObject EquipModel { get { return equipModel; } }
    public override EquipSlots EquipSlot { get { return EquipSlots.Head; } }

    void Update()
    {
        IdleRotate(transform, RotateSpeed);
    }

    public override Node OnUse(Toy toy)
    {
        return new Sequence(
            new LeafAssert(() => { return IsEquippable; }),
            new LeafAssert(() => { return toy.GetAvailableSlotCount() > 0; }),
            new EquipAccessory(toy, this, EquipSlot),
            new LeafInvoke(() => { GameObject.Destroy(this); })
            );
    }


    // Character should wander around and try to give presents to people
    public override Node ToyUse(Toy toy)
    {
        NavMeshAgent agent = toy.GetAgent;
        RaycastHit hit;
        GameObject[] dummies;

        return new Sequence(
           new LeafInvoke(() =>
           {

               dummies = GameObject.FindGameObjectsWithTag("dummy");
               Ray beam = new Ray(transform.position, toy.transform.forward);

               Vector3 rayDir = dummies[0].transform.position - transform.position;

               Vector3 fwd = transform.TransformDirection(Vector3.forward);
               if (Physics.Raycast(transform.position, rayDir, out hit))
               {
                   if (hit.collider.tag == "dummy")
                   {
                       hit.transform.gameObject.transform.position = transform.position;
                   }
               }
           }),
           new DecoratorLoop(
               new SequenceParallel(
                   new LeafInvoke(()=>
                   {
                       hit.transform.gameObject.transform.position = transform.position;
                   })
               )
           )
         );
    }

}
