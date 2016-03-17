using UnityEngine;
using System.Collections;

/// <summary>
/// Class that handle Inputs.
/// This class use custom inputs configured at Unity inspector.
/// </summary>
/// ######################################################
/// Author: Luigi Garcia
/// - Email: mr.garcialuigi@gmail.com
/// - Linkedin: http://br.linkedin.com/in/garcialuigi
/// - Github:  https://github.com/garcialuigi
/// - Facebook: https://www.facebook.com/mr.garcialuigi
/// ######################################################
public class InputManager : MonoBehaviour
{

    // this must be configured by inspector
    public KeyCode upArrow = KeyCode.W;
    public KeyCode downArrow = KeyCode.S;
    public KeyCode leftArrow = KeyCode.A;
    public KeyCode rightArrow = KeyCode.D;
    public KeyCode rotateAroundLeft = KeyCode.Q;
    public KeyCode rotateAroundRight = KeyCode.E;
    public KeyCode zoomIn = KeyCode.Z;
    public KeyCode zoomOut = KeyCode.X;
    public KeyCode jumpBackToPlayer = KeyCode.Space;
    public MyCamera camera;

    public static InputManager instance; // instance reference
    private Vector2 panAxis = Vector2.zero;
    private Toy currentToy;

    void Awake()
    {
        instance = this; // instance reference
    }

    void Update()
    {
        //Toy selection
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Toy newToy = hit.collider.gameObject.GetComponent<Toy>();
                if (newToy != null)
                {
                    //If we select a new Toy
                    if (currentToy && currentToy != newToy)
                    {
                        currentToy.OnDeselect();
                        newToy.OnSelect();
                        currentToy = newToy;
                    }
                    else if(currentToy != newToy)
                    {
                        newToy.OnSelect();
                        currentToy = newToy;
                        camera.thePlayer = currentToy.gameObject;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (currentToy)
            {
                currentToy.OnDeselect();
                camera.thePlayer = camera.focusPoint;
                currentToy = null;
            }
        }
        UpdatePanAxis();
    }

    private void UpdatePanAxis()
    {
        panAxis = Vector2.zero;

        if (Input.GetKey(upArrow))
        {
            panAxis.y = 1;
        }
        else if (Input.GetKey(downArrow))
        {
            panAxis.y = -1;
        }

        if (Input.GetKey(rightArrow))
        {
            panAxis.x = 1;
        }
        else if (Input.GetKey(leftArrow))
        {
            panAxis.x = -1;
        }
    }

    public Vector2 GetPanAxis()
    {
        return panAxis;
    }

    public bool GetRotateAroundLeft()
    {
        return Input.GetKey(rotateAroundLeft);
    }

    public bool GetRotateAroundRight()
    {
        return Input.GetKey(rotateAroundRight);
    }

    public float GetZoomInputAxis()
    {
        float value = 0;

        if (Input.GetKey(zoomOut))
        {
            value = -0.3f;
        }
        else if (Input.GetKey(zoomIn))
        {
            value = 0.3f;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            value = -1;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            value = 1;
        }

        return value;
    }

    public bool GetJumpBackToPlayer()
    {
        return Input.GetKey(jumpBackToPlayer);
    }
}