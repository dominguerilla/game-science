﻿using UnityEngine;
using System.Collections.Generic;

namespace TreeSharpPlus{

    public class WalkTo : Node {

        GameObject location;
        NavMeshAgent agent;

        public WalkTo(NavMeshAgent agent, GameObject location)
        {
            this.agent = agent;
            this.location = location;
        }

        public override IEnumerable<RunStatus> Execute()
        {
            
            agent.SetDestination(location.transform.position);
            //Debug.Log("Heading to " + location.name);

            while (true)
            {
                float distance = Vector3.Distance(agent.transform.position, location.transform.position);
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