using UnityEngine;
using System.Collections;

/// <summary>
/// Logs data-related info into the log file with a special log tag
/// Objects that we want to log must call LogItem in their Start methods
/// </summary>
public class DataLogger : MonoBehaviour {

    private static string logTag = "\tDATALOGGER: ";
    private float time = 0;

	// Use this for initialization
	void Start () {
        LogMessage("Game Started");

        print("DataLogger.Start: turning off all audio listeners in DataLogger.Update");
    }

    // Update is called once per frame
    void Update ()
    {
        time += Time.deltaTime;

        // Bonus: make the console stop printing audio listener warnings
        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
        foreach(AudioListener al in listeners)
        {
            al.enabled = false;
        }
    }

    // Record final time
    void OnDestroy()
    {
        LogMessage("Game Ended");
    }

    // Write this toy to the log
    public void LogNewItem(Toy newItem)
    {
        LogMessage(newItem + " has entered the scene");
    }

    // Write this accessory to the log
    public void LogNewItem(Accessory newItem)
    {
        LogMessage(newItem + " has entered the scene");
    }

    // Write this playzone to the log
    public void LogNewItem(Playzone newItem)
    {
        LogMessage(newItem + " has entered the scene");
    }

    // Log data messages with a special tag in front
    private void LogMessage(string s)
    {
        Debug.Log(logTag + time + " " + s);
    }
}
