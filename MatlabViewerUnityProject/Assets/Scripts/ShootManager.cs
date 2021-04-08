using System.Collections.Generic;
using UnityEngine;

public static class ShootManager
{
    public static Dictionary<string, string[]> DirectConnectionDictionary = new Dictionary<string, string[]>();
    public static Dictionary<string, string[]> InDirectConnectionDictionary = new Dictionary<string, string[]>();
    public static void ShootConnections(string shot)
    {
        if (DirectConnectionDictionary.ContainsKey(shot))
        {
            // this all seems a bit inefficient but it does not have to run very often or very fast.
            // for now this should be good enough. Maybe I will come up with a cleaner solution later.....
            bool shotState = GameObject.Find(shot).GetComponent<EmissionController>().emission;
            foreach (string vic in DirectConnectionDictionary[shot])
            {
                GameObject.Find(vic).GetComponent<EmissionController>().SetEmission(shotState);
            }
        } 
    }

    public static void ShootInDirectConnections(string shot)
    {
        if (DirectConnectionDictionary.ContainsKey(shot))
        {
            bool shotState = GameObject.Find(shot).GetComponent<EmissionController>().emission;
            foreach (string vic in InDirectConnectionDictionary[shot])
            {
                GameObject.Find(vic).GetComponent<EmissionController>().SetEmission(shotState);
            }
        } 
    }

    public static void EmptyDictionaries()
    {
        DirectConnectionDictionary.Clear();
        InDirectConnectionDictionary.Clear(); 
    }
}
