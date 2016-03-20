using System;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

namespace TreeSharpPlus
{
    public class Attack : Node
    {
        Toy Attacker;
        Toy Defender;
        protected Stopwatch stopwatch;
        protected long timeBetweenAttacks;

        public Attack(Toy Attacker, Toy Defender)
        {
            this.Attacker = Attacker;
            this.Defender = Defender;
            this.timeBetweenAttacks = 1333;
            this.stopwatch = new Stopwatch();
        }

        /// <summary>
        ///    Resets the wait timer
        /// </summary>
        /// <param name="context"></param>
        public override void Start()
        {
            base.Start();
            this.stopwatch.Reset();
            this.stopwatch.Start();
        }

        public override void Stop()
        {
            base.Stop();
            this.stopwatch.Stop();
        }

        public override IEnumerable<RunStatus> Execute()
        {
            Attacker.GetAnimator().SetTrigger("Attack");
            while (true)
            {
                // Count down the wait timer
                // If we've waited long enough, succeed
                if (this.stopwatch.ElapsedMilliseconds >= this.timeBetweenAttacks)
                {
                    //Defender.GetAnimator().SetTrigger("Hurt");
                    UnityEngine.Debug.Log(Attacker.gameObject.name + " attacks " + Defender.gameObject.name + " for " + Attacker.GetAttack() + " damage!");
                    Defender.ChangeHealth(Attacker.GetAttack() * -1);
                    UnityEngine.Debug.Log(Defender.gameObject.name + " now has " + Defender.GetHealth() + " HP.");
                    yield return RunStatus.Success;
                    yield break;
                }
                // Otherwise, we're still waiting
                yield return RunStatus.Running;
            }
        }
    }
}
