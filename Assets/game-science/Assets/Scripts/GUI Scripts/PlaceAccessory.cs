using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlaceAccessory : MonoBehaviour {

    public GameObject genericSwordPrefab;
    public GameObject speedPotionPrefab;
    public GameObject summoningSkullPrefab;
    public GameObject santaHatPrefab;
    public GameObject bonePrefab;
    public GameObject heroSwordPrefab;
    public GameObject sheathedSwordPrefab;
    public GameObject breadPrefab;
    public GameObject cheesePrefab;
    public GameObject applePrefab;
    public GameObject healthPotionPrefab;

    public Dropdown accessoryDropdown;

    int end = 10; //number of accessories
    int current = 0;

    public void onClick()
    {
        switch (accessoryDropdown.value)
        {
            case 0:
                Instantiate(genericSwordPrefab);
                break;
            case 1:
                Instantiate(speedPotionPrefab);
                break;
            case 2:
                Instantiate(summoningSkullPrefab);
                break;
            case 3:
                Instantiate(santaHatPrefab);
                break;
            case 4:
                Instantiate(bonePrefab);
                break;
            case 5:
                Instantiate(heroSwordPrefab);
                break;
            case 6:
                Instantiate(sheathedSwordPrefab);
                break;
            case 7:
                Instantiate(breadPrefab);
                break;
            case 8:
                Instantiate(cheesePrefab);
                break;
            case 9:
                Instantiate(applePrefab);
                break;
            case 10:
                Instantiate(healthPotionPrefab);
                break;
            default:
                Instantiate(genericSwordPrefab);
                break;
        }
    }


    // REMOVING CLICK-TO-PLACE FUNCTIONALITY (BELOW)
    // for demo, since it doesn't work properly right now

    /*void Update()
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
                    Instantiate(genericSwordPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 1:
                    Instantiate(speedPotionPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(summoningSkullPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(santaHatPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 4:
                    Instantiate(bonePrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 5:
                    Instantiate(heroSwordPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 6:
                    Instantiate(sheathedSwordPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 7:
                    Instantiate(breadPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 8:
                    Instantiate(cheesePrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 9:
                    Instantiate(applePrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                case 10:
                    Instantiate(healthPotionPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
                default:
                    Instantiate(genericSwordPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                    break;
            }
        }
    }*/

}
