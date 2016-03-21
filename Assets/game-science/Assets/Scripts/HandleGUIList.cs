using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandleGUIList : MonoBehaviour {

    List<Toy> toys;
    List<Accessory> accs;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setToy()
    {
        if (UIPopupList.current.value.Equals("Samurai")){
            GameObject.Find("Toy Button").GetComponent<SpawnToy>().setToy(GameObject.Find("Toy Button").GetComponent<SpawnToy>().toys[0]);
        }else if (UIPopupList.current.value.Equals("Skeleton")){
            GameObject.Find("Toy Button").GetComponent<SpawnToy>().setToy(GameObject.Find("Toy Button").GetComponent<SpawnToy>().toys[1]);
        }
    }

    public void setAcc()
    {
        if (UIPopupList.current.value.Equals("Hero Sword")){
            GameObject.Find("Accessory Button").GetComponent<SpawnToy>().setAccessory(GameObject.Find("Accessory Button").GetComponent<SpawnToy>().accessories[0]);
        }
    }
}
