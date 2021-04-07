using System.Collections.Generic;
using UnityEngine;

public static class ShootManagerScript
{
    public static Dictionary<string, string[]> ShootConnectionDict = new Dictionary<string, string[]>(); 
    public static void KeepShooting(string shot)
    {
        if (ShootConnectionDict.ContainsKey(shot))
        {
            // this all seems pretty inefficient but it does not have to run very often or very fast.
            // for now this should be good enough. Maybe I will come up with a cleaner solution later.....
            GameObject hit = GameObject.Find(shot); 
            bool shotState = hit.GetComponent<EmissionController>().emission; 

            foreach (string vic in ShootConnectionDict[shot])
            {
                GameObject go = GameObject.Find(vic); 
                go.GetComponent<EmissionController>().SetEmission(shotState); 
            }
        } 
    }
}
