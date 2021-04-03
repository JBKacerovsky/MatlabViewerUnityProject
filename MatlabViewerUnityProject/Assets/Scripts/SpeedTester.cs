using UnityEngine;
using System.IO; 

public class SpeedTester : MonoBehaviour
{

    public int testCycles = 10; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("s pressed"); 
            v(); 
        }
    }   

    void v()
    {
        string _path = Application.streamingAssetsPath + Path.DirectorySeparatorChar + "shipAndPlanet.mat";

        var script = this.GetComponent<MatlabReaderScript>();
        var script_old = this.GetComponent<MatlabReaderScript_old>();

        var stopwatch = new System.Diagnostics.Stopwatch(); 

        stopwatch.Start(); 
        for (int i = 0; i < testCycles; i++)
        {
            script.UpdateMatlabFigure(); 
        }
        stopwatch.Stop(); 
        int t1 = (int)stopwatch.ElapsedMilliseconds; 
        
        stopwatch.Start(); 
        for (int i = 0; i < testCycles; i++)
        {
            script_old.UpdateMatlabFigure(); 
        }
        stopwatch.Stop(); 
        int t2 = (int)stopwatch.ElapsedMilliseconds; 

        Debug.Log("Cleaned Script ran" + testCycles + " times in\t" + t1 + "ms"); 
        Debug.Log("avg\t" + t1/testCycles + "\tms per cycle"); 

        Debug.Log("Old script ran" + testCycles + " times in\t" + t2 + "ms"); 
        Debug.Log("avg\t" + t2/testCycles + "\tms per cycle"); 
    }

}
