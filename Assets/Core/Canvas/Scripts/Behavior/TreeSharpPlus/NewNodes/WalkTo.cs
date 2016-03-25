using UnityEngine;
using System.Collections.Generic;

namespace TreeSharpPlus{

    public class WalkTo : Node {


		Toy toy;
        Vector3 location;

		public WalkTo(Toy toy, Vector3 location)
        {
			this.toy = toy;
            this.location = location;
        }

        public override IEnumerable<RunStatus> Execute()
        {
            

            while (true)
            {
				toy.GetAgent().SetDestination(location);
                float distance = Vector3.Distance(toy.transform.position, location);
				if( distance <= toy.GetAgent().stoppingDistance){
                    break;
                }

                yield return RunStatus.Running;
            }

            yield return RunStatus.Success;
        }
    }

}