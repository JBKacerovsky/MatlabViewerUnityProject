using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Accord.IO; 

public class MatFileReader_firstTest
{
    public string _hh = "preseent and accounted for:\n";
    public string _absent = "missing types:\n";
    private string _type = "none";

    private string[] _fieldsToRead = new string[]
        {
            "type",
            "vertices",
            "faces",
            "opacity",
            "colors",
            "color",
            "pts",
            "size",
            "camDistance"
        };

    public MatFileReader_firstTest(MatNode matNode)
    {
        // this is a bit of a silly workaround to get a string read in to be the key "_type", but using strings as keys for _matTypes is going to maake this whole script much more readable. 
        // Accord throws an error if _struct is a Matlab struct with both numeric arrays and strings
        // so type is a struct with one field. The field contains no data but the FieldName is the sting I am trying to pass. 
        _type = _type = new List<string>(matNode.Fields["type"].Fields.Keys)[0];

        foreach (string str in _fieldsToRead)
        {
            

            if (matNode.Fields.ContainsKey(str))
            {
                _hh += str + " here\n";
                continue; 
            } 

            _absent += str + " missing\n"; 
        }
    }

    public void DoSomethings()
    {
        Debug.Log("matreader here with a: \n" + _type + "\nfor you"); 
        Debug.Log(_hh);
        Debug.Log(_absent); 
    }
}
