﻿using UnityEngine;
using System.Collections.Generic;

namespace TreeSharpPlus
{

    /// <summary>
    /// Walk to a GameObject
    /// </summary>
    public class WalkToGameObject : Node
    {
        NavMeshAgent agent;
        GameObject target;

        public WalkToGameObject(Toy toy, GameObject target)
        {
            this.agent = toy.GetAgent();
            this.target = target;
        }

        public override IEnumerable<RunStatus> Execute()
        {
            if (!agent || !target)
            {
                // No chars besides this one
                Debug.Log("WalkToToy Node failed");
                yield return RunStatus.Failure;
            }

            // Get agent's location
            Vector3 agentPos = agent.transform.position;

            // Get target's location
            // Vector3 vector = target.transform.position;

            // Get slight offset of target's location
            Vector3 vector = Utils.GetSlightVectorOffset(target.transform.position);

            // Walk to the toy
            //Debug.Log("Walking to Toy " + target);
            agent.SetDestination(vector);

            int maxIterations = 1000;
            while (true)
            {
                float distance = Vector3.Distance(agent.transform.position, vector);

                // Close enough
                if (distance <= 1f) { break; }

                if (--maxIterations % 5 == 0)
                {
                    // Recalculate vector
                    vector = Utils.GetSlightVectorOffset(target.transform.position);
                    agent.SetDestination(vector);
                }
                else if (maxIterations < 1)
                {
                    //Debug.Log("Couldn't get to the character in time");
                    yield return RunStatus.Failure;
                }
                //Debug.Log("Still en route to " + vector + " with a distance of " + distance);
                yield return RunStatus.Running;
            }
            //Debug.Log("Arrived at " + vector);
            yield return RunStatus.Success;
        }
    }

}