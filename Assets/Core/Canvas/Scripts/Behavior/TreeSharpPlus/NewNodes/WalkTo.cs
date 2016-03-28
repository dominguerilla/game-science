using UnityEngine;
using System.Collections.Generic;

namespace TreeSharpPlus{

    public class WalkTo : Node {


		Toy toy;
		Toy target;
        Vector3 location;

		/// <summary>
		/// Makes the Toy walk to a specified destination.
		/// </summary>
		/// <param name="toy">Toy that will walk to the destination.</param>
		/// <param name="location">The destination the Toy will walk to.</param>
		public WalkTo(Toy toy, Vector3 location)
        {
			this.toy = toy;
            this.location = location;
        }

		/// <summary>
		/// Makes the Toy go to some other Toy, even if the other is moving.
		/// </summary>
		/// <param name="walker">The Toy that will walk.</param>
		/// <param name="target">The Toy that the other will walk to.</param>
		public WalkTo(Toy walker, Toy target){
			this.toy = walker;
			this.target = target;
		}

        public override IEnumerable<RunStatus> Execute()
        {
			if (target) {
				while (true) {
					toy.GetAgent ().SetDestination (target.transform.position);
					float distance = Vector3.Distance (toy.transform.position, target.transform.position);
					if (distance <= toy.GetAgent ().stoppingDistance) {
						break;
					}

					yield return RunStatus.Running;
				}
			} else {
				while (true)
				{
					toy.GetAgent().SetDestination(location);
					float distance = Vector3.Distance(toy.transform.position, location);
					if( distance <= toy.GetAgent().stoppingDistance){
						break;
					}

					yield return RunStatus.Running;
				}
			}

            yield return RunStatus.Success;
        }
    }

}