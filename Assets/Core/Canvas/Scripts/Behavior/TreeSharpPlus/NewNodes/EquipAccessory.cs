﻿using UnityEngine;
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
        private int EquipSlot;

        public EquipAccessory(Toy toy, Accessory acc, Accessory.EquipSlots EquipSlot)
        {
            this.toy = toy;
            this.acc = acc;
            this.EquipSlot = (int)EquipSlot;
        }

        public override IEnumerable<RunStatus> Execute()
        {
            //Trigger the animation for picking up Accessories
            toy.GetComponent<Animator>().SetTrigger("IdlePickUp");

            //Spawn a model that will appear on the Toy picking up the Accessory, if the Accessory is MEANT to appear and if it has a specified EquipModel.
            if (acc.EquipModel && EquipSlot != (int)Accessory.EquipSlots.None)
            {
                GameObject accModel = (GameObject)GameObject.Instantiate(acc.EquipModel, toy.AccessorySlots[EquipSlot].transform.position, Quaternion.identity);

                //Make the model a child of the bones of the Toy model
                accModel.transform.parent = toy.AccessorySlots[EquipSlot]; 
            }

            //Destroy the Accessory on the ground
            acc.gameObject.SetActive(false);
            yield return RunStatus.Success;
        }
    }
}