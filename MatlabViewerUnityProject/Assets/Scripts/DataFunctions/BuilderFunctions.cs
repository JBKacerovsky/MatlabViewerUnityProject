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
        meshInstance.AddComponent<MeshFilter>().sharedMesh = mesh;  //passing the mesh to the MeshFiltere so it can be displayed
        meshInstance.AddComponent<MeshCollider>().sharedMesh = mesh;  //adding _mesh to MeshCollider so it can be hit by raycasts (and do other collider physics)

        return meshInstance; 
    }

    public static GameObject InstantiateMesh(Vector3[] vertices, int[] faces, Transform targetTransform, int shootability, int id)
    {
        GameObject meshInstance = BuilderFunctions.FVmesh(vertices, faces);
        meshInstance.AddComponent<EmissionController>().SetShootability(shootability); 
        meshInstance.transform.parent = targetTransform;
        meshInstance.name = "target_" + id.ToString(); 
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
        public static void SpawnScatterSpheres(Vector3[] pts, int[] sz, Color color, Material mat, Transform transform, int[] shootability, int[] id)
    {
        GameObject scatterContainer = new GameObject("scatter3Container");
        scatterContainer.transform.parent = transform;

        // build the first sphere 
        // bby building once and instantiating after the execution time of this script is cut approximately in half. 
        GameObject sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sp.transform.position = pts[0];
        sp.transform.localScale = new Vector3(sz[0], sz[0], sz[0]);
        sp.GetComponent<Renderer>().material = mat;
        sp.GetComponent<Renderer>().material.SetColor("_color", color);
        sp.transform.parent = scatterContainer.transform;
        sp.AddComponent<EmissionController>().SetShootability(shootability[0]); 
        sp.name = "target_" + id[0].ToString(); 

        for (int i = 1; i < pts.GetLength(0); i++)
        {
            GameObject nsp = GameObject.Instantiate(sp); 
            nsp.transform.localScale = new Vector3(sz[i], sz[i], sz[i]);
            nsp.transform.position = pts[i];
            nsp.name = "target_" + id[i].ToString(); 
            nsp.transform.parent = scatterContainer.transform;
        }
    }
}