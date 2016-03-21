using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandleGUIList : MonoBehaviour {
    
    [SerializeField]
    public List<Toy> toys;
    [SerializeField]
    public List<Accessory> accessories;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
