using UnityEngine;
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
            // Get agent's location
            Vector3 agentPos = agent.transform.position;

            // Get all other agents in scene
            NavMeshAgent[] otherChars =
               Object.FindObjectsOfType(typeof(NavMeshAgent)) as NavMeshAgent[];

            // Must be at least one other character
            if (otherChars.Length < 2)
            {
                // No chars besides this one
                Debug.Log("No other characters found");
                yield return RunStatus.Failure;
            }

            // Put them in a list
            List<NavMeshAgent> otherCharsList = new List<NavMeshAgent>(otherChars);

            // Remove this character from the list
            otherCharsList.Remove(agent);
            foreach (NavMeshAgent a in otherCharsList)
            {
                Debug.Log("Toy found: " + a);
            }

            // Back to array
            otherChars = otherCharsList.ToArray();

            // Find the nearest character
            NavMeshAgent nearestChar = otherChars[0];
            Vector3 vector = nearestChar.transform.position;
            float minDistance = Vector3.Distance(agentPos, vector);
            for (int i = 1; i < otherChars.Length; i++)
            {
                NavMeshAgent currentChar = otherChars[i];
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

            // Don't be rude and walk inside the other character
            vector = Utils.GetSlightVectorOffset(vector);
            agent.SetDestination(vector);

            int maxIterations = 1000;
            while (true)
            { 
                float distance = Vector3.Distance(agent.transform.position, vector);

                if( distance <= 2f){
                    break;
                }

                if(--maxIterations % 10 == 0)
                {
                    // Recalculate vector
                    vector = Utils.GetSlightVectorOffset(nearestChar.transform.position);
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