using UnityEngine;
using System.Collections;

public class SpawnToy : MonoBehaviour {

    [SerializeField]
    public Toy toy;
    public Accessory accessory;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Spawn()
    {
        RaycastHit hit;
        Vector3 rayDir = Camera.main.transform.forward;
        if (Physics.Raycast(Camera.main.transform.position, rayDir, out hit))
        {
            Debug.Log(Camera.main.transform.position);
            Debug.Log(hit.distance);
            Instantiate(toy, hit.point, Quaternion.identity);
        }
    }

    public void SpawnA()
    {
        RaycastHit hit;
        Vector3 rayDir = Camera.main.transform.forward;
        if (Physics.Raycast(Camera.main.transform.position, rayDir, out hit))
        {
            Debug.Log(Camera.main.transform.position);
            Debug.Log(hit.distance);
            Instantiate(accessory, hit.point, Quaternion.identity);
        }
    }
}
