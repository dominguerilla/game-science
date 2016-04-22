using UnityEngine;
using System.Collections;

/// <summary>
/// The MonoBehaviour in charge of controlling the NGUI Toy interface. Handles ONLY the NGUI Toy interface.
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
		
	/// <summary>
	/// The current active tab that is being displayed in the Spawn Menu.
	/// </summary>
	TabTypes CurrentActiveTab;

	/// <summary>
	/// The state of the current GUI.
	/// </summary>
	GUIStates CurrentGUIState;

	/// <summary>
	/// The Toy that has been selected by the player during scene setup. Will be null if not in EDIT_MODE.
	/// </summary>
	Toy SelectedToy;

	/// <summary>
	/// The Accessory that has been selected by the player during scene setup. Will be null if not in EDIT_MODE.
	/// </summary>
	Accessory SelectedAccessory;

	GameObject prefabToSpawn;


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


	// Update is called once per frame, and should only be used to switch between the different modes
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				Toy toy = hit.collider.gameObject.GetComponent<Toy>();
				Accessory acc = hit.collider.gameObject.GetComponent(typeof(Accessory)) as Accessory;
				if (toy || acc) {
					EDIT_MODE (toy, acc);
				} 
			}
			switch (CurrentGUIState) {
			case GUIStates.NO_MODE:
				//NO_MODE ();
				break;
			case GUIStates.EDIT_MODE:
				//EDIT_MODE (ray);
				break;
			case GUIStates.SPAWN_MODE:
				SPAWN_MODE (ray);
				break;
			}
        } else if (Input.GetMouseButtonDown (1)) {
			switch (CurrentGUIState) {
			case GUIStates.NO_MODE:
				//NO_MODE ();
				break;
			case GUIStates.EDIT_MODE:
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				//If a Toy has been selected and the user right clicks on an Accessory, the toy will go to equip it.
				if (SelectedToy && Physics.Raycast (ray, out hit)) {
					Accessory targetAcc = hit.collider.gameObject.GetComponent (typeof(Accessory)) as Accessory;
					if (targetAcc) {
						SelectedToy.SetTargetAccessory (hit.collider.gameObject);
					} else {
						NO_MODE ();
					}
				}
				break;
			case GUIStates.SPAWN_MODE:
				NO_MODE ();
				break;
			}
		}

	}

	/// <summary>
	/// The default ToyGUI mode--allows for basic camera panning and scrolling.
	/// </summary>
	void NO_MODE(){
		if (SelectedToy)
			SelectedToy.OnDeselect (); 
		SelectedToy = null;
		SelectedAccessory = null;
		CurrentGUIState = GUIStates.NO_MODE;
		Debug.Log ("Entered NO_MODE.");
    }


    /// <summary>
    /// Allows the player to edit the parameters of a given Accessory or Toy.
    /// Called when a Toy or Accessory is clicked on in NO_MODE.
    /// Exited when right clicking a non-Toy or non-Accessory.
    /// </summary>
	void EDIT_MODE(Toy toy, Accessory acc){
		if (toy)
		{
			SelectToyOrAccessory (toy);
		}
		else if (acc)
		{
			SelectToyOrAccessory (acc);
		}
	}

	/// <summary>
	/// Selects a Toy.
	/// </summary>
	/// <param name="toy">Toy to select</param>
	void SelectToyOrAccessory(Toy toy){
		if (SelectedToy)
			SelectedToy.OnDeselect ();
		if (SelectedAccessory)
			SelectedAccessory = null;
		toy.OnSelect ();
		SelectedToy = toy;
		SelectedAccessory = null;
		CurrentGUIState = GUIStates.EDIT_MODE;
		Debug.Log("Entered EDIT_MODE.");
	}

	/// <summary>
	/// Selects the given Accessory.
	/// </summary>
	/// <param name="acc">Accessory to select.</param>
	void SelectToyOrAccessory(Accessory acc){
		if (SelectedToy)
			SelectedToy.OnDeselect ();
		if (SelectedAccessory)
			SelectedAccessory = null;
		SelectedToy = null;
		SelectedAccessory = acc;
		CurrentGUIState = GUIStates.EDIT_MODE;
		Debug.Log("Entered EDIT_MODE.");
	}

	/// <summary>
	/// Enables Spawn Mode, overriding the current GUI State.
	/// </summary>
	public void EnterSPAWN_MODE(GameObject prefab){
		if (SelectedToy)
			SelectedToy.OnDeselect ();
		SelectedToy = null;
		SelectedAccessory = null;
		CurrentGUIState = GUIStates.SPAWN_MODE;
        Debug.Log("Entered SPAWN_MODE");
		prefabToSpawn = prefab;
    }

	/// <summary>
	/// Allows the player to spawn a selected Toy or Accessory on the spot clicked on.
	/// Is unique in that it is meant to be called from the GUI, overriding the CurrentGUIState.
	/// Exited when right clicking on a non-Toy or non-Accessory.
	/// </summary>
	void SPAWN_MODE(Ray ray){
		if (!prefabToSpawn) {
			Debug.Log ("No prefab to spawn specified in Inspector!");
			return;
		}
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit))
		{
			Instantiate(prefabToSpawn, hit.point, Quaternion.identity);
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
        //EnterSPAWN_MODE();
    }
}
