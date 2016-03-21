using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnToy : MonoBehaviour {


    private Toy toy;
    private Accessory accessory;

    public void setToy(Toy toy)
    {
        this.toy = toy;
    }

    public void setAccessory(Accessory acc)
    {
        this.accessory = acc;
    }

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
