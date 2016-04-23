using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class Food : Accessory
{

    public GameObject equipModel;
    public float RotateSpeed = 100.0f;

    public override string Archetype { get { return "Food"; } }
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
            
            new LeafInvoke(() => {
                toy.ShowEmoji(EmojiScript.EmojiTypes.Heart_Emoji);
            }),

            new LeafInvoke(() => { gameObject.SetActive(false); }),
			new LeafInvoke(() => { toy.ChangeHealth(10); })
            );
    }

    public override Node ToyUse(Toy toy)
    {
        return IdleBehaviors.CountTo3();
    }
}
