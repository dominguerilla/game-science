using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlaceAccessory : MonoBehaviour {

    public GameObject axePrefab;
    public GameObject speedPotionPrefab;
    public GameObject woodBagPrefab;
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
            default:
                Instantiate(axePrefab);
                break;
        }
    }

}
