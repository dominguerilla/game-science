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

}
