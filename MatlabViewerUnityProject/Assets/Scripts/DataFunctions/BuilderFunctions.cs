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

    public static GameObject FVmesh(Vector3[] vertices, int[] faces)
    {
        GameObject meshInstance = new GameObject();
        MeshCollider mc = meshInstance.AddComponent<MeshCollider>();
        MeshFilter mf = meshInstance.AddComponent<MeshFilter>();
        meshInstance.AddComponent<MeshRenderer>();
        meshInstance.AddComponent<EmissionController>();

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = faces;
        mesh.RecalculateNormals();

        mf.sharedMesh = mesh; //passing the mesh to the MeshFiltere so it can be displayed
        mc.sharedMesh = mesh; //adding _mesh to MeshCollider so it can be hit by raycasts (and do other collider physics)

        return meshInstance; 
    }
}
