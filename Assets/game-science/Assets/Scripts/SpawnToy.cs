using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

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

        //if anything goes wrong completely delete everything from update
        //Same issue as before. Even on GUI mouse clicks register. Need to fix
            //fix is not to use mouse. seems like a problem with unity in general. THere are fixes for unity's gui but can't find one for NGUI
        //find a way to scroll through lists with a key press or scroll wheel

        if (Input.GetKeyDown("1"))
        {
            //if (!EventSystem.current.IsPointerOverGameObject())
                Spawn();
        }

        if (Input.GetKeyDown("2"))
        {
            //if (!EventSystem.current.IsPointerOverGameObject())
                SpawnA();
        }
	    
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
