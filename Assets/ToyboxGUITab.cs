using UnityEngine;
using System.Collections;

/// <summary>
/// A tab in the ToyBox TAB NGUI menu.
/// </summary>
public class ToyboxGUITab : MonoBehaviour {

	/// <summary>
	/// The enumerated Tab that this specific tab is.	
	/// </summary>
	[SerializeField]
	private ToyGUI.TabTypes tabEnum;

	/// <summary>
	/// The menu that will appear in the SELECTION when this tab is selected.
	/// </summary>
	[SerializeField]
	private GameObject SelectionMenu;

	/// <summary>
	/// The ToyGUIManager that is handling the switching between the different Tabs, and is responsible for dealing with this tab.
	/// </summary>
	[SerializeField]
	private ToyGUI ToyGUIManager;

	// Use this for initialization
	void Start () {
		if (!SelectionMenu) 
			Debug.Log ("No Selection Menu specified for tab!");
		else 
			NGUITools.SetActive (SelectionMenu, false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// When called, will disable the currently active tab specified in the ToyGUIManager and enable this one.
	/// </summary>
	public void SwitchTab(){
		ToyGUIManager.SwitchTabs (this.tabEnum);
		Debug.Log ("Disabling tab " + System.Enum.GetName(typeof(ToyGUI.TabTypes), this.tabEnum));
	}

	/// <summary>
	/// Gets the enum assigned to this specific tab.
	/// </summary>
	/// <returns>The tab enum.</returns>
	public ToyGUI.TabTypes GetTabEnum(){
		return this.tabEnum;
	}

	public GameObject GetSelectionMenu(){
		if (this.SelectionMenu)
			return this.SelectionMenu;
		else
			return null;
	}
}
