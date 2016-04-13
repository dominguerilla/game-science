using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controller for player inputs
/// </summary>
public class InputController : MonoBehaviour {

    // The Toys in this scene
    Toy[] toysInScene;

	// Use this for initialization
	void Start () {
        UpdateToys();
	}
	
	// Update is called once per frame
	void Update () {
        CheckForInput();

        // Could do this if we don't care about performance
        // UpdateToys();	
    }

    /// <summary>
    /// Before doing something that will affect all Toys,
    /// update the list of Toys in the scene
    /// </summary>
    private void UpdateToys()
    {
        toysInScene = FindObjectsOfType<Toy>();
    }

    private void CheckForInput() { }

    #region Debug Methods
    // Play all Toys
    public void Play()
    {
        UpdateToys();
        foreach (Toy toy in toysInScene)
        {
            toy.OnPlay();
        }
    }

    // Pause all Toys
    public void Pause()
    {
        UpdateToys();
        foreach (Toy toy in toysInScene)
        {
            toy.OnPause();
        }

    }
    #endregion

}
