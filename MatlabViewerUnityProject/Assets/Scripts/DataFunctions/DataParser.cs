using UnityEngine;

public static class DataParser
{
    public static int[] MatrixTo1DArray(int[,] matrix)
    {
        int[] arr = new int[matrix.Length];
        Vector3 temp = new Vector3(0, 0, 0);
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            arr[i * 3] = matrix[i, 0];
            arr[i * 3 + 1] = matrix[i, 1];
            arr[i * 3 + 2] = matrix[i, 2];
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

    public static Vector2[] buildPointsArray(double[,] x, double[,] y, int idx)
    {
        Vector2[] pointsArray = new Vector2[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            pointsArray[i].x = (float)x[i, 0];
            pointsArray[i].y = (float)y[i, idx];
        }
        return pointsArray;
    }
}