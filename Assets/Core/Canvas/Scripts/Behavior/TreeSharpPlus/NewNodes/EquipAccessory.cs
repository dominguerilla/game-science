using UnityEngine;
using System.Collections.Generic;

namespace TreeSharpPlus
{
    // This Node should only be called when it has been verified that:
    // 1. The Accessory is equippable by the Toy and
    // 2. The Toy has enough free equip slots to equip the Accessory.
	/// <summary>
	/// Makes the Toy equip the specified Accessory, but does NOT make it walk to it.
	/// After the Accessory is equipped, the Toy's behavior is changed to the one specified by the Accessory's ToyUse() function.
	/// </summary>
    public class EquipAccessory : Node
    {
        private Toy toy;
        private Accessory acc;
        private int EquipSlot;

        public EquipAccessory(Toy toy, Accessory acc, Accessory.EquipSlots EquipSlot)
        {
            this.toy = toy;
            this.acc = acc;
            this.EquipSlot = (int)EquipSlot;
        }

        public override IEnumerable<RunStatus> Execute()
        {
            Debug.Log("Executing EquipAccessory");

            //Trigger the animation for picking up Accessories, and returns running while it is animating
            Animator anim = toy.GetComponent<Animator>();
            anim.SetTrigger("IdlePickUp");
            
            
            //Spawn a model that will appear on the Toy picking up the Accessory, if the Accessory is MEANT to appear and if it has a specified EquipModel.
            if (acc.EquipModel && EquipSlot != (int)Accessory.EquipSlots.None)
            {
				GameObject accModel = (GameObject)GameObject.Instantiate(acc.EquipModel, toy.GetAccessoryEquipSlot(EquipSlot).transform.position, Quaternion.identity);

                //Make the model a child of the bones of the Toy model
				accModel.transform.parent = toy.GetAccessoryEquipSlot(EquipSlot); 
            }

            while (anim.GetCurrentAnimatorStateInfo(1).IsName("PickUp"))
            {
                yield return RunStatus.Running;
            }
            //Destroy the Accessory on the ground
            acc.gameObject.SetActive(false);
            toy.SetIdleBehavior(acc.ToyUse(toy));
            yield return RunStatus.Success;
        }
    }
}