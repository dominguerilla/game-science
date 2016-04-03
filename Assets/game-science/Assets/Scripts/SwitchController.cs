using UnityEngine;
using System.Collections;
using FlyingCamera;
using RTSCam;

public class SwitchController : MonoBehaviour
{

    Camera main;
    Camera RTS;
    int i;
    // Use this for initialization
    void Start()
    {
        i = 0;
        main = Camera.main;
        RTS = GameObject.Find("RTSCam").GetComponent<Camera>();
        enableMain();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        
        if (Input.GetKey(KeyCode.V))
        {
            if (i > 1)
            {
                i = 0;
            }
            else
            {
                i++;
            }
            
            if(i == 0)
            {
                enableMain();
            }
            else if(i==1)
            {
                enableRTS();
            }
        }
        
    }

    void enableMain()
    {

        main.enabled = true;
        RTS.enabled = false;
        GetComponent<FlyingCameraController>().enabled = true;
        GetComponent<RTSController>().enabled = false;
    }

    void enableRTS()
    {

        RTS.enabled = true;
        main.enabled = false;
        GetComponent<FlyingCameraController>().enabled = false;
        GetComponent<RTSController>().enabled = true;
    }
}

