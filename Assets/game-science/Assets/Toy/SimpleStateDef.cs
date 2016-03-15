using UnityEngine;
using System.Collections;

public enum SimpleStateDef : long
{
        HasObject   = 1L << 0,
        HasAxe      = 1L << 1,
        IsStanding  = 1L << 2,
}
