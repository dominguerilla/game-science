using UnityEngine;
using System.Collections;
using System;
using TreeSharpPlus;

public class Warzone : Playzone {

    // This Toy will attack a random Toy in the Toyzone
    public override Node GetPlayzoneNode(params Toy[] toys)
    {
        if(toys.Length == 0)
        {
            print("Warzone.GetPlayzoneNode: No Toy given as param");
            return IdleBehaviors.IdleStand();
        }

        // Try to find an enemy Toy in the Toyzone
        Toy defender = GetRandomEnemyToyInZone(toys[0]);

        /*
        if (!defender)
        {   // Try to find ANY other Toy
            defender = GetRandomOtherToyInZone(toys[0]);
        }*/

        if (!defender)
        {
            print("Warzone.GetPlayzoneNode: No defender found");
            return IdleBehaviors.IdleWander(toys[0]);
        }

        print("Warzone.GetPlayzoneNode: " + toys[0] + " will attack " + defender);
        return new SequenceParallel(
            IdleBehaviors.GiveOffEmojis(toys[0], EmojiScript.EmojiTypes.Anger_Emoji, 5000),
            IdleBehaviors.AttackUntilDead(toys[0], defender)
            );
    }
}
