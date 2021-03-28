using UnityEngine;
using System.Collections.Generic; 
using Accord.IO; 

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

    public static GameObject FVmesh(Vector3[] vertices, int[] faces)
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = faces;
        mesh.RecalculateNormals();

        GameObject meshInstance = new GameObject();
        meshInstance.AddComponent<MeshRenderer>();
        meshInstance.AddComponent<EmissionController>();
        meshInstance.AddComponent<MeshFilter>().sharedMesh = mesh;  //passing the mesh to the MeshFiltere so it can be displayed
        meshInstance.AddComponent<MeshCollider>().sharedMesh = mesh;  //adding _mesh to MeshCollider so it can be hit by raycasts (and do other collider physics)
    
        return meshInstance; 
    }

    public static GameObject InstantiateMesh(MatNode fv, Transform targetTransform)
    {
        Vector3[] vertices = DataParser.MatrixToVectorArray(fv.Fields["vertices"].GetValue<double[,]>());
        int[] faces = DataParser.MatrixTo1DArray(fv.Fields["faces"].GetValue<int[,]>());

        GameObject meshInstance = BuilderFunctions.FVmesh(vertices, faces);

        meshInstance.transform.parent = targetTransform;

        return meshInstance;
    }

    public static void AddColor(MatNode fv, GameObject meshInstance, Material transparentMat, Material opaqueMat)
    {
        double[,] temp = fv.Fields["opacity"].GetValue<double[,]>();
        float _opacity = (float)temp[0, 0];

        if (_opacity < 1f)
        {
            meshInstance.GetComponent<MeshRenderer>().material = transparentMat;
            meshInstance.GetComponent<MeshRenderer>().material.SetFloat("_opacity", _opacity);
        }
        else
        {
            meshInstance.GetComponent<MeshRenderer>().material = opaqueMat;
        }

        Color color = ColorParser.GetColor(fv.Fields["color"].GetValue<double[,]>());
        meshInstance.GetComponent<MeshRenderer>().material.SetColor("_color", color);
    }

    public static List<Color[]> BuildVertColorList(MatNode fv)
    {
        double[,] col = fv.Fields["colors"].GetValue<double[,]>();
        List<Color[]> vertexColorList = ColorParser.GetVertexColorList(col, fv.Fields["map"].GetValue<double[,]>());

        return vertexColorList;
    }
}