using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandleGUIList : MonoBehaviour {
    
    [SerializeField]
    public List<Toy> toys;
    [SerializeField]
    public List<Accessory> accessories;

    /*private int current = 0;
    private int end = 4; //need to manually enter this for now
    */

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (current > 0)
            {
                current--;
            }
            else
            {
                current = end;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (current < end)
            {
                current++;
            }
            else
            {
                //Debug.LogError("we in");
                //Debug.LogError(end);
                current = 0;
            }
        }

        Debug.LogError(current);
        */
    }

    public void setToy()
    {

        GameObject.Find("Toy Button").GetComponent<SpawnToy>().setToy(toys[UIPopupList.current.items.IndexOf(UIPopupList.current.value)]);

        /*
        if (UIPopupList.current.value.Equals("Samurai")){
            GameObject.Find("Toy Button").GetComponent<SpawnToy>().setToy(toys[0]);
        }else if (UIPopupList.current.value.Equals("Skeleton")){
            GameObject.Find("Toy Button").GetComponent<SpawnToy>().setToy(toys[1]);
        }*/
    }

    public void setAcc()
    {

        GameObject.Find("Accessory Button").GetComponent<SpawnToy>().setAccessory(accessories[UIPopupList.current.items.IndexOf(UIPopupList.current.value)]);
        /*
        if (UIPopupList.current.value.Equals("Hero Sword")){
            GameObject.Find("Accessory Button").GetComponent<SpawnToy>().setAccessory(accessories[0]);
        }*/
    }
}
