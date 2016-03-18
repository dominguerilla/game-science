using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlaceToy : MonoBehaviour {

    public GameObject lumberjackPrefab;
    public GameObject skeletonPrefab;
    public GameObject unityChanPrefab;
    public GameObject trollPrefab;
    public Dropdown toyDropdown;

    public void onClick()
    {
        switch (toyDropdown.value)
        {
            case 0:
                Instantiate(lumberjackPrefab);
                break;
            case 1:
                Instantiate(skeletonPrefab);
                break;
            case 2:
                Instantiate(unityChanPrefab);
                break;
            case 3:
                Instantiate(trollPrefab);
                break;
            default:
                Instantiate(lumberjackPrefab);
                break;
        }
    }

}
