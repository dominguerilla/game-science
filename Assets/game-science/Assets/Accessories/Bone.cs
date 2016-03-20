using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class Bone : Accessory
{

    public GameObject equipModel;
    public float RotateSpeed = 100.0f;

    public override string Archetype { get { return "Bone"; } }
    public override bool IsEquippable { get { return true; } }
    public override GameObject EquipModel { get { return equipModel; } }
    public override EquipSlots EquipSlot { get { return EquipSlots.LeftHand; } }

    void Update()
    {
        IdleRotate(transform, RotateSpeed);
    }

    public override Node OnUse(Toy toy)
    {
        return new Sequence(
            new LeafAssert(() => { return IsEquippable; }),
            new EquipAccessory(toy, this, EquipSlot),
            new LeafInvoke(() => { gameObject.SetActive(false); })
            );
    }

    public override Node ToyUse(Toy toy)
    {
        foreach(var gameObj in GameObject.FindGameObjectsWithTag("Skelly"))
        {
            Debug.Log("We in");
            if(toy.tag == "Skelly")
            {
                Debug.Log("Dream inside a Dream");
                Debug.Log(gameObj);

                //this return statement is the issue
                //I need it to come back to Bone.cs to kill off the rest of the Skelly
                return IdleBehaviors.IdleStandDuringAction(IdleBehaviors.AttackUntilDead(toy, gameObj.GetComponent<Toy>()));
                Debug.Log("Another one");
            }
        }

        Debug.Log("Why you wake up?");
        return IdleBehaviors.CountTo3();

    }
}
