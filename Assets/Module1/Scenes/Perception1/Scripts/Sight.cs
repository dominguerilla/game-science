using UnityEngine;
using System.Collections;

public class Sight : MonoBehaviour {

    public GameObject Seer;
    public GameObject eyePosition;
    public int ColorSelect = 0;
    public GameObject target;
    public string TargetName = "Target";

    private Color DebugLineColor;
    
  
	void Start () {
        switch (ColorSelect)
        {
            case 0:
                DebugLineColor = Color.cyan;
                break;
            case 1:
                DebugLineColor = Color.red;
                break;
            case 2:
                DebugLineColor = Color.green;
                break;
            case 3:
                DebugLineColor = Color.yellow;
                break;
            default:
                DebugLineColor = Color.black;
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnTriggerStay(Collider other)
    {
        if (isVisible(eyePosition.transform, other.transform))
        {
            Debug.DrawLine(eyePosition.transform.position, other.transform.position, DebugLineColor);
            if (!target && other.gameObject.name == TargetName) { target = other.gameObject; }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(target == other.gameObject){
            target = null;
        }
    }

    /**
 * Checks if the target is visible from eyes at eyePosition. Returns true if visible, false if otherwise.
 * */
    bool isVisible(Transform eyePosition, Transform target)
    {
        RaycastHit hit;
        if (Physics.Linecast(eyePosition.position, target.position, out hit))
        {
            if (hit.collider.gameObject != Seer && hit.collider.gameObject.tag == "Visible")
            {
                return true;
            }
        }
        return false;
    }

}
