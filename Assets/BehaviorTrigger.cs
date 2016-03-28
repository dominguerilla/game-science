using UnityEngine;
using System.Collections;
using TreeSharpPlus;

/// <summary>
/// Sets all Toys specified in ToysToTrigger to Idle, but then starts their behaviors as soon as a GameObject tagged 'Player' enters its collider.
/// </summary>
public class BehaviorTrigger : MonoBehaviour {

	public Toy[] ToysToTrigger;

	// Use this for initialization
	void Start () {
		foreach (Toy toy in ToysToTrigger) {
			toy.DEBUG_StopBehavior ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			Debug.Log ("Behaviors triggered!");
			foreach (Toy toy in ToysToTrigger) {
				toy.DEBUG_StartBehavior ();
			}
			this.gameObject.SetActive (false);
		}
	}
}
