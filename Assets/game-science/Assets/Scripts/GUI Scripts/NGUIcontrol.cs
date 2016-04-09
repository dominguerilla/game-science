using UnityEngine;
using System.Collections;
using FlyingCamera;

public class NGUIcontrol : MonoBehaviour {

    GameObject libraryWindow;
    GameObject flyingcam;
    bool libbool = false;
    bool active = false;
    int temp = 0;

	// Use this for initialization
	void Start () {
        libraryWindow = GameObject.Find("Library");
        flyingcam = GameObject.Find("FlyingPlayer");
        NGUITools.SetActive(libraryWindow, libbool);
	}
	
	// Update is called once per frame
	void Update () {
        //Found it easier to place objects when you can move. You can change this back if you want.
        if (active == true)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                flyingcam.GetComponent<FlyingCameraController>().enabled = !flyingcam.GetComponent<FlyingCameraController>().enabled;
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (temp == 0)
            {
                active = true;
                temp = 1;
            }
            else {
                active = false;
                temp = 0;
            }


            if (Cursor.lockState == CursorLockMode.None)
            {

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            //Debug.Log("worked");
            libbool = !libbool;
            NGUITools.SetActive(libraryWindow, libbool);
        }
    }
}
