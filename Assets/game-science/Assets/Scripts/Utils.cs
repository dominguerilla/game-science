using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils : MonoBehaviour
{

    public static Vector3 ORIGIN_VECTOR = new Vector3(0, 0, 0);

    // Use this so Toys don't act rude and step inside other Toys
    public static Vector3 GetSlightVectorOffset(Vector3 pos)
    {
        Vector3 returnVector = pos;

        int maxIter = 50;
        while (--maxIter > 0 && Vector3.Distance(pos, returnVector) < 1)
        {
            returnVector.x = Random.Range(pos.x - 1.5f, pos.x + 1.5f);
            returnVector.z = Random.Range(pos.z - 1.5f, pos.z + 1.5f);
        }

        return returnVector;
    }

    // Use this so Toys don't act rude and step inside other Toys
    public static Vector3 GetSlightVectorOffset(Vector3 pos, float offset)
    {
        Vector3 returnVector = pos;

        int maxIter = 50;
        while (--maxIter > 0 && Vector3.Distance(pos, returnVector) < 1)
        {
            returnVector.x = Random.Range(pos.x - offset, pos.x + offset);
            returnVector.z = Random.Range(pos.z - offset, pos.z + offset);
        }

        return returnVector;
    }

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

    internal static List<GameObject> GetAllOtherToysInSceneAsGameObjects(GameObject thisToy)
    {
        if (!thisToy)
        {
            print("Utils.GetAllOtherToys: no Toy given as input");
            return null;
        }

        GameObject[] ToysInScene = GameObject.FindGameObjectsWithTag("Toy");
        //GameObject[] ToysInScene = FindObjectsOfType(typeof(Toy)) as GameObject[];

        List<GameObject> tempList;

        // Make sure there's at least one other Toy
        if (ToysInScene.Length > 1)
        {
            tempList = new List<GameObject>();
        }
        else
        {
            Debug.Log("Utils.GetOtherToys: no other Toys in scene");
            return null;
        }


        // Copy over every Toy but this one
        foreach (GameObject ob in ToysInScene)
        {
            if (ob != thisToy)
            {
                tempList.Add(ob);
            }
            else
            {
                Debug.Log("Utils.GetOtherToys: not copying over this Toy: " + thisToy);
            }
        }
        return tempList;
    }

    // Get a random Other Toy in the scene as GameObject
    public static GameObject GetRandomOtherToyInSceneAsGameObject(Toy toy)
    {
        if (!toy)
        {
            print("Utils.GetRandomOther: no Toy given as input");
            return null;
        }

        // Object[] ToysInScene = FindObjectsOfType(typeof(Toy));
        // GameObject[] ToysInScene = FindObjectsOfType(typeof(Toy)) as GameObject[];
        GameObject[] ToysInScene = GameObject.FindGameObjectsWithTag("Toy");

        List<GameObject> tempList;

        // Make sure there's at least one other Toy
        if (ToysInScene.Length > 1)
        {
            tempList = new List<GameObject>();
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
        // TODO: check this
        tempList.Remove(toy.GetComponent<GameObject>());
        //tempList.Remove(toy.gameObject);

        // At least 1 toy in tempList
        return tempList[Random.Range(0, tempList.Count)];
    }

    /// <summary>
    /// Return true if target is within 3f of Toy
    /// </summary>
    /// <param name="toy"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool TargetIsInRange(Toy toy, GameObject target)
    {
        return Vector3.Distance(toy.transform.position, target.transform.position) < 3f;
    }

    // Same thing, for multiple targets (List version)
    public static bool TargetIsInRange(Toy toy, List<GameObject> targets)
    {
        foreach (GameObject ob in targets)
        {
            if (Vector3.Distance(toy.transform.position, ob.transform.position) < 3f)
                return true;
        }
        return false;
    }

    // Same thing, for multiple targets (Array version)
    public static bool TargetIsInRange(Toy toy, GameObject[] targets)
    {
        foreach (GameObject ob in targets)
        {
            if (Vector3.Distance(toy.transform.position, ob.transform.position) < 3f)
                return true;
        }
        return false;
    }

    // Return the Toy that's in range, if there is one
    public static Toy GetToyInRange(Toy toy, GameObject[] targets, float range)
    {
        foreach (GameObject ob in targets)
        {
            if (Vector3.Distance(toy.transform.position, ob.transform.position) < range)
            {
                if(ob.GetComponent<Toy>() != null)
                {
                    Debug.Log("Utils.GetToyInRange: found Toy " + ob.GetComponent<Toy>()
                        + "at distance = "
                        + Vector3.Distance(toy.transform.position, ob.transform.position));
                    return ob.GetComponent<Toy>() as Toy;
                }
            }
        }
        Debug.Log("Utils.GetToyInRange: no Toy found");
        return null;
    }

}