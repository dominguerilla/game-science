using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class SmartAxe : Accessory {
    
    public override string Archetype { get { return "SmartAxe"; } }
    public float RotateSpeed = 100.0f;

    private bool isEquipped; //Whether or not this item is equipped

	void Start () {
        isEquipped = false;
	}

    void Update()
    {
        IdleRotate(transform, RotateSpeed);
    }

    public override Node OnUse()
    {
        return new Sequence(); //placeholder
    }
}
