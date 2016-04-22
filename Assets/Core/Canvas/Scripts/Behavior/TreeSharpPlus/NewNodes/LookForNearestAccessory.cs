using UnityEngine;
using System.Collections.Generic;
using System;

namespace TreeSharpPlus{
    // OLD, BROKEN, DON'T USE IT

    /// <summary>
    /// Author: Colin
    /// Walk to a vector
    /// </summary>
    public class LookForNearestAccessory : Node {

        Toy toy;

        public LookForNearestAccessory(Toy toy)
        {
            this.toy = toy;

        }

        public override IEnumerable<RunStatus> Execute()
        {
            //GameObject[] accessoriesInScene =
            // UnityEngine.Object.FindObjectsOfType(typeof(Accessory)) as GameObject[];

            GameObject[] accessoriesInScene =
                GameObject.FindGameObjectsWithTag("Accessory");

            if (accessoriesInScene == null)
            {
                Debug.Log("No accessories in scene");
                yield return RunStatus.Failure;
            } else
            {
                foreach (GameObject o in accessoriesInScene)
                {
                    Debug.Log("Accessory found: " + o);
                }
            }

            // Choose a random accessory from the ones in the scene
            System.Random rand = new System.Random();
            GameObject chosenAccessory =
                accessoriesInScene[rand.Next(0, accessoriesInScene.Length)];

            // Have this accessory be this toy's target accessory
            toy.SetTargetAccessory(chosenAccessory);

            Debug.Log("In Node: Set " + toy + "'s accessory to " + chosenAccessory);

            yield return RunStatus.Success;

            /*
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
            */
        }
    }

}