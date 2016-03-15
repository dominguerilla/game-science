using UnityEngine;
using System.Collections.Generic;

namespace TreeSharpPlus{

    /// <summary>
    /// Author: Colin
    /// Walk to a random new vector from toy's current location
    /// </summary>
    public class WalkToRandomNewVector : Node
    {

        NavMeshAgent agent;
        Toy toy;

        public WalkToRandomNewVector(NavMeshAgent agent, Toy toy)
        {
            this.agent = agent;
            this.toy = toy;
        }

        public override IEnumerable<RunStatus> Execute()
        {
            // Get a random new vector from current toy location
            Vector3 pos = toy.transform.position;
            Vector3 vector = Utils.GetNewRandomPosition(pos);

            agent.SetDestination(vector);
            //Debug.Log("Heading to " + vector);

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