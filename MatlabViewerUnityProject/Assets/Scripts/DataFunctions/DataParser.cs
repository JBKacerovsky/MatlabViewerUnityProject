using UnityEngine;
using System.Collections.Generic;

public static class DataParser
{
    public static int[] MatrixTo1DArray(int[,] matrix) 
    {
        int[] arr = new int[matrix.Length];
        Vector3 temp = new Vector3(0, 0, 0);

        int nrows = matrix.GetLength(0); 
        int ncols = matrix.GetLength(1); 

        for (int i = 0; i < nrows; i++)
        {
            for (int j = 0; j < ncols; j ++)
            {
                arr[i * ncols + j] = matrix[i, j]; 
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

}