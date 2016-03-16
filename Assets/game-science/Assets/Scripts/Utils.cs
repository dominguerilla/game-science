using UnityEngine;
using System.Collections;

public class Utils {

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

    // Generates a random vector within 20 units of 0,0,0
    public static Vector3 GetRandomPositionFromOrigin()
    {
        Vector3 returnVector;

        returnVector.x = Random.Range(-20, 20);
        returnVector.z = Random.Range(-20, -20);
        returnVector.y = 0;

        return returnVector;
    }

}
