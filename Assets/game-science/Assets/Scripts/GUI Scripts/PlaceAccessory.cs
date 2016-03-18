using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlaceAccessory : MonoBehaviour {

    public GameObject axePrefab;
    public GameObject speedPotionPrefab;
    public GameObject woodBagPrefab;
    public GameObject santaHatPrefab;
    public Dropdown accessoryDropdown;

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
