using UnityEngine;
using System.Collections.Generic;

namespace TreeSharpPlus{

    /// <summary>
    /// Author: Colin
    /// Walk to a random location
    /// </summary>
    public class WalkToRandom : Node {

        NavMeshAgent agent;

        public WalkToRandom(NavMeshAgent agent)
        {
            this.agent = agent;
        }

        public override IEnumerable<RunStatus> Execute()
        {
            Vector3 pos = Utils.GetRandomPositionFromOrigin();

            agent.SetDestination(pos);
            //Debug.Log("Heading to " + location.name);

            while (true)
            {
                float distance = Vector3.Distance(agent.transform.position, pos);
                if( distance <= 0.2f){
                    break;
                }
                //Debug.Log("Still en route to " + location.name + " with a distance of " + distance);
                yield return RunStatus.Running;
            }
            //Debug.Log("Arrived at " + location.name);
            yield return RunStatus.Success;
        }
    }

}