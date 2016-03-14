using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public abstract class Accessory : SmartObject {

    /**
     * A simple function to rotate the Accessory while it is on the ground unequipped.
     */ 
    public virtual void IdleRotate(Transform obj, float speed)
    {
        obj.Rotate(Vector3.up * Time.deltaTime * speed);
    }


    /**
     * The subtree that happens when a Toy decides to 'use' the Accessory.
     * For equippable items, this would entail making the object display on the Toy, changing the state of both the Toy and Accessory, and then destroying the Accessory from the game world.
     * Make the subtree execute successfully if equipping is successful, otherwise make it fail if it cannot be equipped at the time.
     * 
     */
    public abstract Node OnUse();
}
