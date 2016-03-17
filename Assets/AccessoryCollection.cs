using UnityEngine;
using System.Collections;

public class AccessoryCollection : MonoBehaviour {

    public float radius = 1.0f;
    
    public void RandomMove()
    {
        Debug.Log("check move");
        transform.position += Random.insideUnitSphere * radius;
    }

    public GameObject explosion;

    public void RandomParticle()
    {
        Debug.Log("check particle");

    }
}
