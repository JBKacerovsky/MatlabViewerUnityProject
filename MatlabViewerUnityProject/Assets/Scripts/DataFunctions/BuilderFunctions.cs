using UnityEngine;

public static class BuilderFunctions 
{
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

    public static GameObject InstantiateMesh(Vector3[] vertices, int[] faces, Transform targetTransform)
    {
        GameObject meshInstance = BuilderFunctions.FVmesh(vertices, faces);

        meshInstance.transform.parent = targetTransform;

        return meshInstance;
    }

    public static void AddMat(float opacity, GameObject meshInstance, Material transparentMat, Material opaqueMat)
    {
        if (opacity < 1f)
        {
            meshInstance.GetComponent<MeshRenderer>().material = transparentMat;
            meshInstance.GetComponent<MeshRenderer>().material.SetFloat("_opacity", opacity);
        }
        else
        {
            meshInstance.GetComponent<MeshRenderer>().material = opaqueMat;
        }
    }
}