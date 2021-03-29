using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Accord.IO;
using System;

public class MatFileReader
{
    public string _hh = "preseent and accounted for:\n";
    public string _absent = "missing types:\n";
    private string _type = "none";

    public string Type;
    public Vector3[] Vertices;
    public int[] Faces;
    public float Opacity;
    public Color SingleColor;
    public int CamDistance;
    public int[] PointSize;

    public List<Vector2[]> GraphPointList; 
    public List<Color[]> VertColorList;

    private string[] _fieldsToRead = new string[]
        {
            "type",
            "vertices",
            "faces",
            "opacity",
            "colors",
            "color",
            "size",
            "camDistance"
        };

    public MatFileReader(MatNode matNode)
    {
        // type MUST be defined for every input object
        // that is why it is the only one not in an if statement    

        // this is a bit of a silly workaround to get a string read in to be the key "_type", but using strings as keys for _matTypes is going to maake this whole script much more readable. 
        // Accord throws an error if _struct is a Matlab struct with both numeric arrays and strings
        // so type is a struct with one field. The field contains no data but the FieldName is the sting I am trying to pass. 
        _type = _type = new List<string>(matNode.Fields["type"].Fields.Keys)[0];


        if (matNode.Fields.ContainsKey("vertices"))
        {
            Vertices = DataParser.MatrixToVectorArray(matNode.Fields["vertices"].GetValue<double[,]>());
        }

        if (matNode.Fields.ContainsKey("faces"))
        {
            Faces = DataParser.MatrixTo1DArray(matNode.Fields["faces"].GetValue<int[,]>());
        }

        if (matNode.Fields.ContainsKey("opacity"))
        {
            Opacity = (float)matNode.Fields["opacity"].GetValue<double[,]>()[0, 0];
        }

        if (matNode.Fields.ContainsKey("colors"))
        {

        }

        if (matNode.Fields.ContainsKey("color"))
        {
            SingleColor = ColorParser.GetColor(matNode.Fields["color"].GetValue<double[,]>());
        }

        if (matNode.Fields.ContainsKey("size"))
        {
            PointSize = DataParser.MatrixTo1DArray(matNode.Fields["size"].GetValue<int[,]>());
        }

        if (matNode.Fields.ContainsKey("camDistance"))
        {
            CamDistance = matNode.Fields["camDistance"].GetValue<int[,]>()[0, 0];
        }

        if (matNode.Fields.ContainsKey("x"))
        {
            if (matNode.Fields.ContainsKey("y"))  // y should always be defined if x is... but just to check
            {
                GraphPointList = DataParser.buildPointsList(matNode.Fields["x"].GetValue<double[,]>(), matNode.Fields["y"].GetValue<double[,]>()); 
            }
        }
    }
}