using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controller for player inputs
/// </summary>
public class InputController : MonoBehaviour {

    // Objects in scene
    Toy[] toysInScene;
    Accessory[] accessoriesInScene;

    // Current scene state (playing, paused, or stopped)
    SceneState currentState = SceneState.paused;

    private enum SceneState
    {
        playing,
        paused,
        stopped
    }

    void Start () {
        UpdateToys();
	}
	
	void Update () {
        CheckForInput();
    }

    private void CheckForInput() { }

    #region GUI Methods
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
