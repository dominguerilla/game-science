using UnityEngine;
using System.Collections.Generic;

namespace TreeSharpPlus{

    /// <summary>
    /// Author: Colin
    /// Walk to a vector
    /// </summary>
    public class CheckForCharacterInRange : Node {

        NavMeshAgent agent;
        float range;

        public CheckForCharacterInRange(NavMeshAgent agent, float range)
        {
            this.agent = agent;
            this.range = range;
        }

        // NOTE pretty sure this Node doesn't do anything LOL
        public override IEnumerable<RunStatus> Execute()
        {
            Vector3 agentPos = agent.transform.position;
            Toy[] otherChars = agent.GetComponents<Toy>();

            if (otherChars.Length < 1)
            {
                //Debug.Log("No other characters found");
                yield return RunStatus.Failure;
            }

            // Is there a character in range?
            foreach(Toy toy in otherChars)
            {
                if(Vector3.Distance(agent.transform.position,
                    toy.transform.position) < range)
                {
                    //Debug.Log("Found toy " + toy + " in range");
                    yield return RunStatus.Success;
                }
            }

            yield return RunStatus.Failure;
        }
    }

}