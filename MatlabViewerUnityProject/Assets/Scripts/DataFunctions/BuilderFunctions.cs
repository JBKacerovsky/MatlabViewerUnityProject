using UnityEngine;

public static class BuilderFunctions 
{
    // BuilderFunctions contians the methods for building the specifed 3D objects from data stored in a Xfigure .mat file. 
    // after the data has been read and stored in a FigureDataStruct object (using data format conversions in the DataParser class)
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
        public static void SpawnScatterSpheres(Vector3[] pts, int[] sz, Color color, Material material, Transform transform)
    {
        GameObject scatterInstance = new GameObject("scatter3Container");
        scatterInstance.transform.parent = transform;

        for (int i = 0; i < pts.GetLength(0); i++)
        {
            GameObject sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sp.transform.position = pts[i];
            sp.transform.localScale = new Vector3(sz[i], sz[i], sz[i]);
            sp.GetComponent<Renderer>().material = material;
            sp.GetComponent<Renderer>().material.SetColor("_BaseColor", color);
            sp.transform.parent = scatterInstance.transform;
        }
    }
}