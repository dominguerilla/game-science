using UnityEngine;
using System.Collections.Generic;

namespace TreeSharpPlus
{
    // This Node should only be called when it has been verified that:
    // 1. The Accessory is equippable by the Toy and
    // 2. The Toy has enough free equip slots to equip the Accessory.
    public class EquipAccessory : Node
    {
        private Toy toy;
        private Accessory acc;

        public EquipAccessory(Toy toy, Accessory acc)
        {
            this.toy = toy;
            this.acc = acc;
        }

        public override IEnumerable<RunStatus> Execute()
        {
            //toy.GetComponent<Animator>().SetTrigger("PickUp");
            //GameObject accModel = GameObject.Instantiate(acc.GetComponent<Toy>().GetEquipModel());
            //accModel.parent = toy.AccessorySlots.Get(0); //currently gets the first slot only
            //GameObject.Destroy(Accessory);
            yield return RunStatus.Success;
        }
    }
}