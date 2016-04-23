using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TreeSharpPlus;

/// <summary>
/// Unimplemented Playzone script
/// </summary>
public abstract class Playzone : MonoBehaviour {

    // The Data Logger in the scene
    private DataLogger logger;

    // The toys currently in this Playzone
    private List<Toy> ToysInZone = new List<Toy>();

    // The range of this Playzone
    public int range;

    // Iteration counter for Update
    private int numIter = 0;

    /// <summary>
    /// At a basic implementation level, each Playzone should return a Node
    /// representing a BehaviorTree for its Toys to act out
    /// </summary>
    /// <returns></returns>
    public abstract Node GetPlayzoneNode(params Toy[] toys);

    void Start () {
        // Use 10 as a default range if it wasn't assigned
	    if(range == default(int)) { range = 10; }

        // When accessory is placed in the scene, log it
        GameObject logObject = GameObject.FindGameObjectWithTag("Logger");
        logger = logObject.GetComponent(typeof(DataLogger)) as DataLogger;
        if (logger != null) { logger.LogNewItem(this); }
    }
	
	void Update () {
        // Only do this every 5 updates for performance
        if (++numIter > 4)
        {
            numIter = 0;
            UpdateToys();
        }
    }

    private void UpdateToys()
    {
        // Get all Toys in range of Playzone
        Collider[] objectsInRange =
            Physics.OverlapSphere(this.transform.position, range);
        List<Toy> tempToys = new List<Toy>();

        foreach (Collider c in objectsInRange)
        {
            Toy currentToy = c.GetComponent<Toy>();
            if (currentToy != null)
            {   // Found Toy in range
                tempToys.Add(currentToy);
            }
        }

        // Check for new Toys
        foreach (Toy foundToy in tempToys)
        {
            if (!ToysInZone.Contains(foundToy))
            {   // It's a new Toy
                RegisterToy(foundToy);
            }
        }

        // Check for missing Toys
        foreach (Toy foundToy in ToysInZone.ToArray())
        {
            if (!tempToys.Contains(foundToy))
            {   // This Toy has left the playzone
                UnregisterToy(foundToy);
            }
        }
    }

    private void RegisterToy(Toy currentToy)
    {
        ToysInZone.Add(currentToy);
        currentToy.OnPlayzoneEnter(this);
        print("Playzone.RegisterToy: "
            + currentToy + " entered Playzone " + this);
    }

    private void UnregisterToy(Toy currentToy)
    {
        ToysInZone.Remove(currentToy);
        currentToy.OnPlayzoneExit(this);
        print("Playzone.UnreisterToy: "
            + currentToy + " left Playzone " + this);
    }

    private void DEBUG_PrintToys(List<Toy> toys)
    {
        print("Playzone.DEBUG_PrintToys: ");
        foreach(Toy toy in toys)
        {
            print("\t" + toy);
        }
    }

    public virtual Toy GetRandomToyInZone()
    {
        return ToysInZone[Random.Range(0, ToysInZone.Count-1)];
    }

    public virtual Toy GetRandomOtherToyInZone(Toy toy)
    {
        if(ToysInZone.Count < 2) { return null; }
        List<Toy> tempList = new List<Toy>(ToysInZone.Count);

        // Copy over to temp list
        ToysInZone.ForEach((item) => { tempList.Add(item); });

        // Remove the Toy we don't want
        tempList.Remove(toy);

        // At least 1 toy in tempList
        return tempList[Random.Range(0, tempList.Count-1)];
    }

    public virtual Toy GetRandomEnemyToyInZone(Toy toy)
    {
        if (ToysInZone.Count < 2) { return null; }
        List<Toy> tempList = new List<Toy>(ToysInZone.Count);

        // Copy over to temp list
        ToysInZone.ForEach((item) => { tempList.Add(item); });

        // Remove this Toy
        tempList.Remove(toy);

        // Remove this Toy's teammates
        int team = toy.GetTeam();
        foreach(Toy t in tempList.ToArray())
        {
            if(t.GetTeam() == team)
            {
                tempList.Remove(t);
            }
        }

        if(tempList.Count < 1) { return null; }

        // Remainder will have at least 1 enemy
        return tempList[Random.Range(0, tempList.Count - 1)];
    }
}
