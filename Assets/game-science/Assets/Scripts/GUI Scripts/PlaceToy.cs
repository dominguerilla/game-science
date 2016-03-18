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


    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //gonna need a fix. We have to manually set the z according to the main camera. Need some sort of adaptive coordinate thingy
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 45f));
            switch (toyDropdown.value)
            {
                case 0:
                    Instantiate(lumberjackPrefab, new Vector3(point.x,point.y,point.z),Quaternion.identity);
                    break;
                case 1:
                    Instantiate(skeletonPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(unityChanPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(trollPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                default:
                    Instantiate(lumberjackPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
            }
        }
    }



}
