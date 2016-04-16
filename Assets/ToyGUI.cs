using UnityEngine;
using System.Collections;

/// <summary>
/// The MonoBehaviour in charge of controlling the NGUI Toy interface.
/// </summary>
public class ToyGUI : MonoBehaviour {

    /// <summary>
    /// The Tabs available in the NGUI, used to switch selections of prefabs based on category in the Spawn Menu area.
    /// </summary>
    public enum TabTypes{
		TOYS,
		ACCESSORIES,
		PLAYZONES
	}

	/// <summary>
	/// The states that the GUI is in, each with different capabilities.
	/// </summary>
	public enum GUIStates{
		/// <summary>
		/// The default mode. Should allow only for camera movement.
		/// </summary>
		NO_MODE,

		/// <summary>
		/// Activates when a Toy, Accessory, or Playzone is clicked on. Allows the editing of specific parameters in the object.
		/// </summary>
		EDIT_MODE,

		/// <summary>
		/// Activates when a prefab button is selected from the Spawn Menu in the NGUI. Left click on the play area to spawn that prefab.
		/// </summary>
		SPAWN_MODE
	}
		
	TabTypes CurrentActiveTab;
	GUIStates CurrentGUIState;
	Toy SelectedToy;
	Accessory SelectedAccessory;

	/// <summary>
	/// The GameObject holding all ToyBoxGUITabs.
	/// </summary>
	[SerializeField]
	GameObject TabObject;

	GameObject[] Tabs;

	// Use this for initialization
	void Start () {

		//Initiating tabs
		Tabs = new GameObject[System.Enum.GetNames(typeof(TabTypes)).Length];
		foreach(Transform tab in TabObject.transform){
			ToyboxGUITab button = tab.GetComponent<ToyboxGUITab> ();
			if (button == null) {
				//Debug.Log ("No ToyBoxGUITab component found in " + button.gameObject.name);
				return;
			} else if (Tabs [(int)button.GetTabEnum()] != null) {
				Debug.Log ("Multiple copies of same tab found in " + button.gameObject.name + "! Returning.");
				return;
			} else {
				Tabs [(int)button.GetTabEnum()] = button.gameObject;
			}
		}
		SwitchTabs (Tabs [0].GetComponent<ToyboxGUITab> ().GetTabEnum ());

		//Initiating GUIState
		CurrentGUIState = GUIStates.NO_MODE;
		Debug.Log ("Starting at NO_MODE.");
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				Toy toy = hit.collider.gameObject.GetComponent<Toy> ();
				Accessory acc = hit.collider.gameObject.GetComponent (typeof(Accessory)) as Accessory;
                if (toy) {
                    SelectedToy = toy;
					SelectedAccessory = null;
					CurrentGUIState = GUIStates.EDIT_MODE;
					Debug.Log ("Entered EDIT_MODE.");
				} else if (acc) {
					SelectedToy = null;
					SelectedAccessory = acc;
					CurrentGUIState = GUIStates.EDIT_MODE;
					Debug.Log ("Entered EDIT_MODE.");
				}
			}
		} else if (Input.GetMouseButtonDown (1)) {
			SelectedToy = null;
			SelectedAccessory = null;
			CurrentGUIState = GUIStates.NO_MODE;
			Debug.Log ("Entered NO_MODE.");
		}


		switch (CurrentGUIState) {
		case GUIStates.NO_MODE:
			NO_MODE ();
			break;
		case GUIStates.EDIT_MODE:
			EDIT_MODE ();
			break;
		case GUIStates.SPAWN_MODE:
			SPAWN_MODE ();
			break;
		}
	}

	/// <summary>
	/// The default ToyGUI mode--allows for basic camera panning and scrolling.
	/// </summary>
	void NO_MODE(){
		
	}

	/// <summary>
	/// Allows the player to edit the parameters of a given Accessory or Toy.
	/// Called when a Toy or Accessory is clicked on in NO_MODE.
	/// Exited when right clicking a non-Toy or non-Accessory.
	/// </summary>
	void EDIT_MODE(){

	}


	/// <summary>
	/// Enables Spawn Mode, overriding the current GUI State.
	/// </summary>
	public void EnterSPAWN_MODE(){
		SelectedToy = null;
		SelectedAccessory = null;
		CurrentGUIState = GUIStates.SPAWN_MODE;
        Debug.Log("Entered SPAWN_MODE");
    }

	/// <summary>
	/// Allows the player to spawn a selected Toy or Accessory on the spot clicked on.
	/// Is unique in that it is meant to be called from the GUI, overriding the CurrentGUIState.
	/// Exited when right clicking on a non-Toy or non-Accessory.
	/// </summary>
	void SPAWN_MODE(){
        Debug.Log("We in");
    }


    public GameObject selected;


    public void Spawn()
    {
        Debug.Log("We in deeper");
        RaycastHit hit;
        Vector3 rayDir = Camera.main.transform.forward;
        if (Physics.Raycast(Camera.main.transform.position, rayDir, out hit))
        {
            Debug.Log("We in the deepest");
            Debug.Log(Camera.main.transform.position);
            Debug.Log(hit.distance);
            Instantiate(selected, hit.point, Quaternion.identity);
        }
    }

    /// <summary>
    /// Switches from the currently active tab to the tab that calls this method.
    /// </summary>
    /// <param name="NextTab">The next tab to switch to.</param>
    public void SwitchTabs(TabTypes NextTab){
		if (Tabs [(int)CurrentActiveTab])
			NGUITools.SetActive (Tabs [(int)CurrentActiveTab].GetComponent<ToyboxGUITab> ().GetSelectionMenu (), false);
		CurrentActiveTab = NextTab;
		NGUITools.SetActive (Tabs [(int)CurrentActiveTab].GetComponent<ToyboxGUITab>().GetSelectionMenu(), true);
		Debug.Log ("Enabling tab " + System.Enum.GetName(typeof(ToyGUI.TabTypes), NextTab));
        EnterSPAWN_MODE();
    }
}
