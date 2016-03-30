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
	private Toy CurrentFocusedToy;

    void Awake()
    {
        instance = this; // instance reference
    }

    void Update()
    {
        //Upon Left Click
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				Toy newToy = hit.collider.gameObject.GetComponent<Toy> ();
				//If there was a Toy in the GameObject hit
				if (newToy != null) {
					if (CurrentFocusedToy != newToy) {
						//If we select a new Toy after already having selected some other Toy
						if (CurrentFocusedToy) {
							CurrentFocusedToy = newToy;
							camera.thePlayer = CurrentFocusedToy.gameObject;
							//ToyController.SelectToy (newToy, CurrentFocusedToy);
						}
						//If we select a different Toy from the one currently equipped
						else {
							CurrentFocusedToy = newToy;
							camera.thePlayer = CurrentFocusedToy.gameObject;
							//ToyController.SelectToy (newToy);
						}
					}
				} else {
					//Deselects the CurrentFocusedToy, if there is any.
					if (CurrentFocusedToy) {
						CurrentFocusedToy = null;
						camera.thePlayer = camera.focusPoint;
						//ToyController.DeselectToy ();
					}
				}
			}
		}//Upon Right Click
        else if (Input.GetMouseButtonDown (1)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				if (CurrentFocusedToy) {
					Accessory acc = hit.collider.gameObject.GetComponent (typeof(Accessory)) as Accessory;
					Toy other = hit.collider.gameObject.GetComponent<Toy> ();
					if (acc) {
						//ToyController.EquipAccessory (acc);
					} else {
						if (CurrentFocusedToy.GetEquippedAccessory () != null && other) {
						}
							//ToyController.ActivateCore (other);
						//else ToyController.MoveTo (hit.point);
					}
				}
			}
            
		} 
        //UpdatePanAxis();
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