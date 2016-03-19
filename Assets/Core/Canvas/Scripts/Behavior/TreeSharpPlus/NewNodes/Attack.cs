using System;
using System.Collections.Generic;
using UnityEngine;

namespace TreeSharpPlus
{
    public class Attack : Node
    {
        Toy Attacker;
        Toy Defender;

        public Attack(Toy Attacker, Toy Defender)
        {
            this.Attacker = Attacker;
            this.Defender = Defender;
        }

        public override IEnumerable<RunStatus> Execute()
        {
            Attacker.GetAnimator().SetTrigger("Attack");
            //Defender.GetAnimator().SetTrigger("Hurt");
            Defender.ChangeHealth(Attacker.GetAttack());
            yield return RunStatus.Success;
        } //RECOMPILE
    }
}
