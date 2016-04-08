using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Unimplemented Playzone script
/// </summary>
public class Playzone : MonoBehaviour {

    // The toys currently in this Playzone
    private List<Toy> ToysInZone = new List<Toy>();

    // The range of this Playzone (can set in editor)
    public int range;

	// Use this for initialization
	void Start () {
        // Use 10 as a default range if it wasn't assigned
	    if(range == 0) { range = 10; }
    }
	
	// Update is called once per frame
	void Update () {
        // Check for objects in range of Playzone
        Collider[] objectsInRange =
            Physics.OverlapSphere(this.transform.position, range);

        foreach (Collider c in objectsInRange)
        {
            Toy currentToy = c.GetComponent<Toy>();
            if (currentToy != null)
            {   // Found Toy in range
                if (!ToysInZone.Contains(currentToy))
                {   // It's a new Toy
                    ToysInZone.Add(currentToy);
                    currentToy.OnPlayzoneEnter(this);
                    print(currentToy + " entered Playzone " + this);
                }
            }
        }

    }

}
