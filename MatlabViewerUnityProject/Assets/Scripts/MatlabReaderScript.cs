using UnityEngine;
using UnityEngine.UI.Extensions;
using System.Collections.Generic;
using System.IO;
using Accord.IO;

public class MatlabReaderScript : MonoBehaviour
{
    // UI components
    public AutoCompleteComboBox fileSelectionDropDown;

    // materials
    [SerializeField] private Material _opaqueSingleColor;
    [SerializeField] private Material _singleColor;
    [SerializeField] private Material _opaqueVertexColors;
    [SerializeField] private Material _vertexColors;
    [SerializeField] private Material _scatterMat;

    // containers for created objects
    [SerializeField] private Transform multiMeshContainer;
    [SerializeField] GameObject graphContainer;

    // Dictionary of matlab types to be displayed
    private Dictionary<int, System.Action<MatNode>> _matTypes;

    void Start()
    {
        _matTypes = new Dictionary<int, System.Action<MatNode>>
        {
            {1, FVmeshVertexColor},
            {2, scatter3},
            {3, FVmeshMultiVertColor},
            {4, drawGraph},
            {5, FVmeshSingleColor},
            {6, CamDistSetter}
        };
    }

    public void UpdateMatlabFigure()
    {
        MatReader matFileReader = new MatReader(Application.streamingAssetsPath + Path.DirectorySeparatorChar + fileSelectionDropDown.Text);

        MatNode matFile = matFileReader.Fields[matFileReader.FieldNames[0]];

        foreach (var field in matFile.Fields)
        {
            MatNode _struct = matFile.Fields[field.Key];
            int _type = _struct.Fields["type"].GetValue<int[,]>()[0, 0];

            _matTypes[_type](_struct); 
        }
    }

    // matType Functions
    private void FVmeshSingleColor(MatNode fv)
    {
        GameObject meshInstance = InstantiateMesh(fv, transform);

        AddColor(fv, meshInstance);
    }

    private void FVmeshVertexColor(MatNode fv)
    {
        GameObject meshInstance = InstantiateMesh(fv, transform);

        AddVertexColors(fv, meshInstance);
    }

    private void FVmeshMultiVertColor(MatNode fv)
    {
        GameObject _multicolorFV = InstantiateMesh(fv, multiMeshContainer);

        List<Color[]> _multiVertColorList = AddVertexColors(fv, _multicolorFV);

        multiMeshContainer.GetComponent<MultiVertColUpdater>().SetStuff(_multiVertColorList, _multicolorFV); 
    }

    private void scatter3(MatNode sc)
    {
        GameObject scatterInstance = new GameObject("scatter3Container");
        scatterInstance.transform.parent = transform;

        Vector3[] pts = DataParser.MatrixToVectorArray(sc.Fields["pts"].GetValue<double[,]>());
        int[,] sz = sc.Fields["size"].GetValue<int[,]>();

        Color color = ColorParser.GetColor(sc.Fields["color"].GetValue<double[,]>());

        for (int i = 0; i < pts.GetLength(0); i++)
        {
            GameObject sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sp.transform.position = pts[i];
            sp.transform.localScale = new Vector3(sz[0, i], sz[0, i], sz[0, i]);
            sp.GetComponent<Renderer>().material = _scatterMat;
            sp.GetComponent<Renderer>().material.SetColor("_BaseColor", color);
            sp.transform.parent = scatterInstance.transform;
        }
    }

    private void drawGraph(MatNode gr)
    {
        Color color = ColorParser.GetColor(gr.Fields["color"].GetValue<double[,]>());

        double[,] x = gr.Fields["x"].GetValue<double[,]>();
        double[,] y = gr.Fields["y"].GetValue<double[,]>();

        graphContainer.GetComponent<GraphController>().BuildGraph(x, y, color); 
    }

    private void CamDistSetter(MatNode d)
    {
        int[,] _dist = d.Fields["CamDistance"].GetValue<int[,]>();
        CamOrbit.functions.SetCamDistance(_dist[0,0]);
    }

    // build and destroy functions
    private GameObject InstantiateMesh(MatNode fv, Transform targetTransform)
    {
        Vector3[] vertices = DataParser.MatrixToVectorArray(fv.Fields["vertices"].GetValue<double[,]>());
        int[] faces = DataParser.MatrixTo1DArray(fv.Fields["faces"].GetValue<int[,]>());

        GameObject meshInstance = BuilderFunctions.FVmesh(vertices, faces);

        meshInstance.transform.parent = targetTransform;

        return meshInstance; 
    }

    private void AddColor(MatNode fv, GameObject meshInstance)
    {
        double[,] temp = fv.Fields["opacity"].GetValue<double[,]>();
        float _opacity = (float)temp[0, 0];

        if (_opacity < 1f)
        {
            meshInstance.GetComponent<MeshRenderer>().material = _singleColor;
            meshInstance.GetComponent<MeshRenderer>().material.SetFloat("_opacity", _opacity);
        }
        else
        {
            meshInstance.GetComponent<MeshRenderer>().material = _opaqueSingleColor;
        }

        Color color = ColorParser.GetColor(fv.Fields["color"].GetValue<double[,]>());
        meshInstance.GetComponent<MeshRenderer>().material.SetColor("_color", color);
    }
        
    private List<Color[]> AddVertexColors(MatNode fv, GameObject meshInstance)
    {
        double[,] temp = fv.Fields["opacity"].GetValue<double[,]>();
        float _opacity = (float)temp[0, 0];

        if (_opacity < 1)
        {
            meshInstance.GetComponent<MeshRenderer>().material = _vertexColors;
            meshInstance.GetComponent<MeshRenderer>().material.SetFloat("_opacity", _opacity);
        }
        else
        {
            meshInstance.GetComponent<MeshRenderer>().material = _opaqueVertexColors;
        }

        double[,] col = fv.Fields["colors"].GetValue<double[,]>();
        List<Color[]> vertexColorList = ColorParser.GetVertexColorList(col, fv.Fields["map"].GetValue<double[,]>());

        meshInstance.GetComponent<MeshFilter>().mesh.colors = vertexColorList[0];

        return vertexColorList;
    }
}