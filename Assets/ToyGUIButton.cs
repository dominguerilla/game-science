using UnityEngine;
using System.Collections;

public class ToyGUIButton : MonoBehaviour {

	/// <summary>
	/// The prefab that is set to be spawned when this button is pressed.
	/// </summary>
	[SerializeField]
	GameObject SpawnPrefab;

	/// <summary>
	/// The ToyGUI that manages this button.
	/// </summary>
	[SerializeField]
	ToyGUI toyGUI;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Makes the ToyGUI selected switch to SPAWN_MODE() along with this button's selected Spawn prefab.
	/// </summary>
	public void CallSpawnMode(){
		toyGUI.EnterSPAWN_MODE (SpawnPrefab);
	}
}
