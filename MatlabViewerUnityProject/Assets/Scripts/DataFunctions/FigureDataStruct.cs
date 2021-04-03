using System.Collections.Generic;
using UnityEngine;
using Accord.IO;

public class FigureDataStruct
{
    public string Type;
    public Vector3[] Vertices;
    public int[] Faces;
    public float Opacity;
    public Color SingleColor;
    public int CamDistance;
    public int[] PointSize;
    public List<Vector2[]> GraphPointList; 
    public List<Color[]> VertColorList;
    public FigureDataStruct(MatNode matNode)
    {
       Type = new List<string>(matNode.Fields["type"].Fields.Keys)[0]; // type MUST be defined for every input object that is why it is the only one not in an if statement    
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
            double[,] colorArray = matNode.Fields["colors"].GetValue<double[,]>();
            VertColorList = ColorParser.GetVertexColorList(colorArray, matNode.Fields["map"].GetValue<double[,]>());
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