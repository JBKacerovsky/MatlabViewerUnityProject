using UnityEngine;
using System.Collections.Generic;

public static class DataParser
{
    // the DataParser static class contains all methods to onvert int[,] and double[,] matrices 
    // which are read in from matlab files (using Accord.IO in FigureDataStruct) into the 
    // appropriate data structures expected by BuilderFunctions and Unity for building the specified 3D objects. 
    public static int[] MatrixTo1DArray(int[,] matrix) 
    {
        int[] arr = new int[matrix.Length];
        int nrows = matrix.GetLength(0); 
        int ncols = matrix.GetLength(1); 

        for (int i = 0; i < nrows; i++)
        {
            for (int j = 0; j < ncols; j ++)
            {
                int tempInt = (int)matrix[i, j]; 
                arr[i * ncols + j] = tempInt;
            }
        }
        return (arr);
    }

    public static Vector3[] MatrixToVectorArray(double[,] matrix)
    {
        int rowCount = matrix.GetLength(0);
        Vector3[] arr = new Vector3[rowCount];
        Vector3 temp = new Vector3(0, 0, 0);
        for (int i = 0; i < rowCount; i++)
        {
            temp.x = -1 * (float)matrix[i, 0];    //matlab and Unity are using different axis handedness
            temp.y = (float)matrix[i, 1];
            temp.z = (float)matrix[i, 2];
            arr[i] = temp;
        }
        return (arr);
    }

    public static double[,] getColumnDouble(double[,] arr, int idx)
    {
        int rLength = arr.GetLength(0);
        double[,] column = new double[rLength, 1]; 
        for (int i = 0; i < rLength; i++)
        {
            column[i, 0] = arr[i, idx]; 
        }
        return column; 
    }

    public static List<Vector2[]> buildPointsList(double[,] x, double[,] y)
    {
        List<Vector2[]> pointsList = new List<Vector2[]>(); 
        for (int i = 0; i < y.GetLength(1); i++)
        {
            Vector2[] pointsArray = new Vector2[y.GetLength(0)];
            for (int j = 0; j < y.GetLength(0); j++)
            {
                pointsArray[j].x = (float)x[j, 0];
                pointsArray[j].y = (float)y[j, i];
            }
            pointsList.Add(pointsArray); 
        }
        return pointsList;
    }
public static Color GetColor(double[,] col)
    {
        Color _color = new Color(0, 0, 0);
        _color.r = (float)col[0, 0];
        _color.g = (float)col[0, 1];
        _color.b = (float)col[0, 2];
        return _color;
    }
    public static Color[] BuildColorArray(double[,] vertCol, Gradient colMap)
    {
        Color[] colors = new Color[vertCol.GetLength(0)];

        for (int i = 0; i < vertCol.GetLength(0); i++)
        {
            colors[i] = colMap.Evaluate((float)vertCol[i, 0]);
        }
        return (colors);
    }
    public static List<Color[]> GetVertexColorList(double[,] colors, double[,] ColMapMatrix)
    {
        Gradient colMap = MatrixToColormap(ColMapMatrix);

        List < Color[]> ColorList = new List<Color[]>();

        for (int i = 0; i < colors.GetLength(1); i++)
        {
            ColorList.Add(BuildColorArray(DataParser.getColumnDouble(colors, i), colMap));
        }
        return ColorList;
    }
    public static Gradient MatrixToColormap(double[,] matrix)
    {
        Gradient map = new Gradient();
        int rowCount = matrix.GetLength(0);
        GradientColorKey[] colorKey = new GradientColorKey[rowCount];
        Color temp = new Color();

        for (int i = 0; i < rowCount; i++)
        {
            temp.r = (float)matrix[i, 0];
            temp.g = (float)matrix[i, 1];
            temp.b = (float)matrix[i, 2];
            colorKey[i].color = temp;
            colorKey[i].time = (float)i / rowCount;
        }

        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        map.SetKeys(colorKey, alphaKey);
        return (map);
    }
    public static string[] IntMatrixTo1DStringArray(int[,] matrix, string prefix = "", string suffix = "")
    {
        return (System.Array.ConvertAll(MatrixTo1DArray(matrix), x => prefix + x.ToString() + suffix)); 
    }
}