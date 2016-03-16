using UnityEngine;
using System.Collections;

/** These enums are indexes in a boolean array stored by the Toy object.
* The boolean array represents whether or not the character is in that state.
* Can check these enums in a PBT using Toy.Simple_Node_Require
*/
public enum SimpleStateDef
{
    HasObject,
    IsStanding,

    // This one should go at the end
    NUMSTATES
}