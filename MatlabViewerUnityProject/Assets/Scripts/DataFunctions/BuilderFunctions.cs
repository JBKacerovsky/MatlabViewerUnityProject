using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuilderFunctions 
{
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

    public static Mesh FVmesh(Vector3[] vertices, int[] faces)
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = faces;
        mesh.RecalculateNormals();

        return mesh; 
    }
}
