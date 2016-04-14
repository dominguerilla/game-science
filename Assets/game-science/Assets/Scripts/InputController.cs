using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controller for player inputs
/// </summary>
[RequireComponent (typeof(ToyController))]
public class InputController : MonoBehaviour {

    // The ToyController in the scene
    public ToyController toyController;

    // Objects in scene
    Toy[] toysInScene;
    Accessory[] accessoriesInScene;

    // Current scene state
    private enum SceneState { playing, paused, stopped }

    // For now, assume it starts out as paused
    SceneState currentState = SceneState.paused;

    void Start () {
        UpdateToys();
	}
	
	void Update () {
        // CheckForInput();
    }

    private void CheckForInput() { }

    #region GUI Functions
    // Play all Toys
    public void Play()
    {
        // Alreading playing
        if(currentState == SceneState.playing) { return; }

        UpdateToys();
        foreach (Toy toy in toysInScene)
        {
            toy.OnPlay();
        }

        currentState = SceneState.playing;
    }

    // Pause all Toys
    public void Pause()
    {
        // Already paused
        if (currentState == SceneState.paused) { return; }

        UpdateToys();
        foreach (Toy toy in toysInScene)
        {
            toy.OnPause();
        }

        currentState = SceneState.paused;
    }

    // Stop the scene (kill all Toys & Accessories)
    public void Stop()
    {
        UpdateToys();
        UpdateAccessories();
        foreach (Toy toy in toysInScene)
        {
            toy.OnStop();
        }
        foreach (Accessory acc in accessoriesInScene)
        {
            acc.OnStop();
        }

        currentState = SceneState.stopped;
    }

    // Assign current Toy to a team
    public void AssignTeam()
    {
        // TODO: Get team number from GUI element
        int teamToAssign = -1;

        /* NOTE: Currently uses ToyController to figure out which 
        * Toy is currently selected. There may be a better way to do this
        * which doesn't involve going through so many layers.
        */
        toyController.SetTeam(teamToAssign);
    }
    #endregion

    #region Utility Functions
    /// <summary>
    /// Before doing something that will affect all Toys,
    /// update the list of Toys in the scene
    /// </summary>
    private void UpdateToys()
    {
        toysInScene = FindObjectsOfType<Toy>();
    }

    /// <summary>
    /// Before doing something that will affect all Accessories,
    /// update the list of Accessories in the scene
    /// </summary>
    private void UpdateAccessories()
    {
        accessoriesInScene = FindObjectsOfType<Accessory>();
    }
    #endregion

}
