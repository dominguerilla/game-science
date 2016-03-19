using System;
using System.Collections.Generic;
using UnityEngine;

namespace TreeSharpPlus
{
    public class GivePresent : Node
    {
        Toy gifter;
        GameObject present;

        public GivePresent(Toy gifter, GameObject present)
        {
            this.gifter = gifter;
            this.present = present;
        }

        public override IEnumerable<RunStatus> Execute()
        {
            // Play attack animation
            gifter.GetAnimator().SetTrigger("Attack");
            // Give present
            UnityEngine.Object.Instantiate(present,
                gifter.transform.position,
                new Quaternion(0,0,0,0));
            yield return RunStatus.Success;
        }
    }
}
