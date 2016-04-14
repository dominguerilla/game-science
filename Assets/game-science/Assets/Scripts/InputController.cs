using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controller for player inputs
/// </summary>
[RequireComponent (typeof(ToyController))]
public class InputController : MonoBehaviour {

    // GUI fields
    public int Team_Number;
    public float Health_Value;
    public float Speed_Value;
    public Accessory Accessory_ToEquip;

    // The ToyController in the scene
    public ToyController toyController;

    // Objects in scene
    private Toy[] toysInScene;
    private Accessory[] accessoriesInScene;

    // The currently selected Toy
    private Toy currentToy;

    // Current scene state
    private enum SceneState { playing, paused, stopped }

    // For now, assume it starts out as paused
    private SceneState currentState = SceneState.paused;

    void Start ()
    {
        UpdateToys();
	}

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

    /* NOTE: The functions below use ToyController to figure out which 
    * Toy is currently selected.
    * They need to grab values from the GUI elements */

    // Assign current Toy to a team
    public void AssignTeam()
    {
        currentToy = GetCurrentToy();
        if (currentToy) { currentToy.OnTeamSet(Team_Number); }
    }

    // Assign an accessory to current Toy
    public void AssignAccessory()
    {
        currentToy = GetCurrentToy();
        if (currentToy) { currentToy.Equip(Accessory_ToEquip); }
    }

    // Change health of current Toy
    public void AssignHealth()
    {
        currentToy = GetCurrentToy();
        if (currentToy) { currentToy.SetHealth(Health_Value); }
    }

    // Change speed of current Toy
    public void AssignSpeed()
    {
        currentToy = GetCurrentToy();
        if (currentToy) { currentToy.SetSpeed(Speed_Value); }
    }
    #endregion

    #region Private Utility Functions
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

    /// <summary>
    /// Gets the Toy Controller's Current Toy
    /// </summary>
    /// <returns></returns>
    private Toy GetCurrentToy()
    {
        return toyController.CurrentToy;
    }
    #endregion

}
