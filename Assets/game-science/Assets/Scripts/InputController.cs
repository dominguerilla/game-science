using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controller for player inputs
/// </summary>
public class InputController : MonoBehaviour {

    Toy[] toysInScene;
    Accessory[] accessoriesInScene;

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
