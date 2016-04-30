using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils : MonoBehaviour {

    public static Vector3 ORIGIN_VECTOR = new Vector3(0, 0, 0);

    // Given a vector, returns a new vector in a random range
    public static Vector3 GetNewRandomPosition(Vector3 pos)
    {
        Vector3 returnVector = pos;

        //Debug.Log("Choosing new position from: " + pos);

        returnVector.x = Random.Range(pos.x - 20, pos.x + 20);
        returnVector.z = Random.Range(pos.z - 20, pos.z + 20);

        //Debug.Log("New position chosen: " + returnVector);

        return returnVector;
    }

    // Given a vector, returns a new vector in a given input range
    public static Vector3 GetNewRandomPositionInRange(Vector3 pos, float range)
    {
        Vector3 returnVector = pos;

        //Debug.Log("Choosing new position from: " + pos);

        returnVector.x = Random.Range(pos.x - range, pos.x + range);
        returnVector.z = Random.Range(pos.z - range, pos.z + range);

        //Debug.Log("New position chosen: " + returnVector);

        return returnVector;
    }

    // Generates a random vector within 20 units of 0,0,0
    public static Vector3 GetRandomPositionFromOrigin()
    {
        Vector3 returnVector;

        returnVector.x = Random.Range(-20, 20);
        returnVector.z = Random.Range(-20, -20);
        returnVector.y = 0;

        return returnVector;
    }

    // Get a random Other Toy in the scene as GameObject
    public static GameObject GetRandomOtherToyInSceneAsGameObject(Toy toy)
    {
        if (!toy)
        {
            print("Utils.GetRandomOther: no Toy given as input");
            return null;
        }

        Object[] ToysInScene = FindObjectsOfType(typeof(Toy));
        List<GameObject> tempList;

        // Make sure there's at least one other Toy
        if (ToysInScene.Length > 1)
        {
            tempList = new List<GameObject>(ToysInScene.Length);
        }
        else
        {
            return null;
        }

        // Copy over
        foreach (Object ob in ToysInScene)
        {
            tempList.Add(ob as GameObject);
        }

        // Remove the Toy we don't want
        tempList.Remove(toy.gameObject);

        // At least 1 toy in tempList
        return tempList[Random.Range(0, tempList.Count - 1)];
    }
}
