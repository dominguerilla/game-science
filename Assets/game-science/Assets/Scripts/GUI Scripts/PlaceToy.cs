using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlaceToy : MonoBehaviour {

    public GameObject lumberjackPrefab;
    public GameObject skeletonPrefab;
    public GameObject unityChanPrefab;
    public GameObject trollPrefab;
    public GameObject samuraiPrefab;
    public Dropdown toyDropdown;

    int toycurrent = 0;
    int totalToys = 4; //total-1 since we start at 0

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
            case 4:
                Instantiate(samuraiPrefab);
                break;
            default:
                Instantiate(lumberjackPrefab);
                break;
        }

    }


    // REMOVING CLICK-TO-PLACE FUNCTIONALITY (BELOW)
    // for demo, since it doesn't work properly right now

    /*
    void Update()
    {

        if (Input.GetKeyDown("1"))
        {
            if (toycurrent > 0)
            {
                toycurrent--;
            }
            else
            {
                toycurrent = totalToys;
            }
        }
        else if (Input.GetKeyDown("2"))
        {
            if (toycurrent < totalToys)
            {
                toycurrent++;
            }
            else
            {
                toycurrent = 0;
            }
        }

        // NOTE: THIS WAS COMMENTED OUT IN ORGINAL
        /*if (Input.GetKeyDown("1"))
        {
            toycurrent = 0;
        }
        else if (Input.GetKeyDown("2"))
        {
            toycurrent = 1;
        }
        else if (Input.GetKeyDown("3"))
        {
            toycurrent = 2;
        }
        else if (Input.GetKeyDown("4"))
        {
            toycurrent = 3;
        }***** /


        toyDropdown.value = toycurrent;


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
                case 4:
                    Instantiate(samuraiPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                default:
                    Instantiate(lumberjackPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
            }
        }
    }*/



}
