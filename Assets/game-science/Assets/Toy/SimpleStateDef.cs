using UnityEngine;
using System.Collections;

/** These enums are indexes in a boolean array stored by the Toy object.
* The boolean array represents whether or not the character is in that state.
* Can check these enums in a PBT using Toy.Simple_Node_Require
* Can assign a set of states to true/false using Toy.Simple_Node_SetTrue/False
* States are initialized in Toy.SetInitialStates, which is called in Toy.Start
*/
public enum SimpleStateDef
{
    // Toy will automatically pick up accessories in range using
    // Toy.EquipAccessoriesInRange in Toy.Update
    WantsAccessories,

    // Toy is being controlled by the player
    TPSMode,

    // Toy is inside a Playzone
    IsInPlayzone,
}