using UnityEngine;
using System.Collections;
using FlyingCamera;

public class NGUIcontrol : MonoBehaviour {

    GameObject libraryWindow;
    GameObject flyingcam;
    bool libbool = false;

	// Use this for initialization
	void Start () {
        libraryWindow = GameObject.Find("Library");
        flyingcam = GameObject.Find("FlyingPlayer");
        NGUITools.SetActive(libraryWindow, libbool);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.G))
        {
            flyingcam.GetComponent<FlyingCameraController>().enabled = !flyingcam.GetComponent<FlyingCameraController>().enabled;


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
