﻿using UnityEngine;
using System.Collections.Generic;

namespace TreeSharpPlus{

    /// <summary>
    /// Author: Colin
    /// Walk to a vector
    /// </summary>
    public class WalkToNearestCharacter : Node {

        NavMeshAgent agent;

        public WalkToNearestCharacter(NavMeshAgent agent)
        {
            this.agent = agent;
        }

        public override IEnumerable<RunStatus> Execute()
        {
            Vector3 agentPos = agent.transform.position;
            Toy[] otherChars = agent.GetComponents<Toy>();

            if(otherChars.Length < 1) {
                Debug.Log("No other characters found");
                yield return RunStatus.Failure;
            }

            // Find the nearest character
            Toy nearestChar = otherChars[0];
            Vector3 vector = nearestChar.transform.position;
            float minDistance = Vector3.Distance(agentPos, vector);
            for (int i = 1; i < otherChars.Length; i++)
            {
                Toy currentChar = otherChars[i];
                float newDistance = Vector3.Distance(agentPos, currentChar.transform.position);
                if (newDistance < minDistance)
                {
                    minDistance = newDistance;
                    nearestChar = currentChar;
                    vector = currentChar.transform.position;
                    Debug.Log("Current nearest character: " + nearestChar);
                }
            }

            // Walk to the toy
            Debug.Log("Walking to nearest character: " + nearestChar);
            agent.SetDestination(vector);

            int maxIterations = 1000;
            while (true)
            { 
                float distance = Vector3.Distance(agent.transform.position, vector);

                if( distance <= 2f){
                    break;
                }

                maxIterations--;
                if(maxIterations % 10 == 0)
                {
                    // Recalculate vector
                    vector = nearestChar.transform.position;
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