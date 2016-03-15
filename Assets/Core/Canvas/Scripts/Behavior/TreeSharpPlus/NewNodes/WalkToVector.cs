using UnityEngine;
using System.Collections.Generic;

namespace TreeSharpPlus{

    /// <summary>
    /// Author: Colin
    /// Walk to a vector
    /// </summary>
    public class WalkToVector : Node {

        NavMeshAgent agent;
        Vector3 vector;

        public WalkToVector(NavMeshAgent agent, Vector3 vector)
        {
            this.agent = agent;
            this.vector = vector;
        }

        public override IEnumerable<RunStatus> Execute()
        {

            agent.SetDestination(vector);
            //Debug.Log("Heading to " + location.name);

            while (true)
            {
                float distance = Vector3.Distance(agent.transform.position, vector);

                // TEST: Changed from 0.2f to 3f
                // For some reason, troll doesn't want to get much closer than that...
                if( distance <= 3f){
                    break;
                }
                //Debug.Log("Still en route to " + vector + " with a distance of " + distance);
                yield return RunStatus.Running;
            }
            //Debug.Log("Arrived at " + vector);
            yield return RunStatus.Success;
        }
    }

}