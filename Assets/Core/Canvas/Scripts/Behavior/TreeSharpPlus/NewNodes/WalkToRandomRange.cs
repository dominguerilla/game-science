using UnityEngine;
using System.Collections.Generic;

namespace TreeSharpPlus{

    /// <summary>
    /// Author: Colin
    /// Walk to a random new vector from toy's current location
    /// </summary>
    public class WalkToRandomRange : Node
    {
        Toy toy;
        float range;

        public WalkToRandomRange(Toy toy, float range)
        {
            this.toy = toy;
            this.range = range;
        }

        public override IEnumerable<RunStatus> Execute()
        {
            // Get a random new vector from current toy location
            Vector3 pos = toy.transform.position;
            Vector3 vector = Utils.GetNewRandomPositionInRange(pos, range);

            NavMeshAgent agent = toy.GetAgent();

            agent.SetDestination(vector);
            //Debug.Log("Heading to " + vector);


            // Hack: if character can't reach vector in 50 iterations, send them near the origin
            int maxIterations = 50;
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

                maxIterations--;
                if(maxIterations < 1)
                {
                    vector = Utils.GetNewRandomPosition(Utils.ORIGIN_VECTOR);
                    agent.SetDestination(vector);
                    maxIterations = 100;
                }

            }
            Debug.Log("Arrived at " + vector);
            yield return RunStatus.Success;
        }
    }

}