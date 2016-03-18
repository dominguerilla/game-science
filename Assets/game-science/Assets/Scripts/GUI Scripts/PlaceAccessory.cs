using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlaceAccessory : MonoBehaviour {

    public GameObject axePrefab;
    public GameObject speedPotionPrefab;
    public GameObject woodBagPrefab;
    public GameObject santaHatPrefab;
    public Dropdown accessoryDropdown;

    int end = 3; //number of accessories
    int current = 0;

    public void onClick()
    {
        switch (accessoryDropdown.value)
        {
            case 0:
                Instantiate(axePrefab);
                break;
            case 1:
                Instantiate(speedPotionPrefab);
                break;
            case 2:
                Instantiate(woodBagPrefab);
                break;
            case 3:
                Instantiate(santaHatPrefab);
                break;
            default:
                Instantiate(axePrefab);
                break;
        }
    }

    void Update()
    {

        //switching accessories with scroll wheel
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(current > 0)
            {
                current--;
            }
            else
            {
                current = end;
            }
        }else if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(current < end)
            {
                current++;
            }
            else
            {
                current = 0;
            }
        }

        //System.Console.WriteLine(current);
        accessoryDropdown.value = current;

        if (Input.GetMouseButtonDown(1))
        {

            //gonna need a fix. We have to manually set the z according to the main camera. Need some sort of adaptive coordinate thingy
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 45f));
            switch (accessoryDropdown.value)
            {
                case 0:
                    Instantiate(axePrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 1:
                    Instantiate(speedPotionPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(woodBagPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(santaHatPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                default:
                    Instantiate(axePrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
            }
        }
    }

}
