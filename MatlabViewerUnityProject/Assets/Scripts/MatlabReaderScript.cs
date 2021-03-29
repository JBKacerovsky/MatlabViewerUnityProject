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
    private Dictionary<string, System.Action<MatNode>> _matTypes;

    void Start()
    {
        _matTypes = new Dictionary<string, System.Action<MatNode>>
        {
            {"VertexColorMesh", FVmeshVertexColor},
            {"Scatter3D", scatter3},
            {"MultiVertexColorMesh", FVmeshMultiVertColor},
            {"Graph", drawGraph},
            {"SingleColorMesh", FVmeshSingleColor},
            {"SetCamDistance", CamDistSetter}
        };
    }

    public void UpdateMatlabFigure()
    {
        string _path = Application.streamingAssetsPath + Path.DirectorySeparatorChar + fileSelectionDropDown.Text; 
        MatReader matFileReader = new MatReader(_path);

        MatNode matFile = matFileReader.Fields[matFileReader.FieldNames[0]];

        foreach (var field in matFile.Fields)
        { 
            MatNode _struct = matFile.Fields[field.Key];
                // this is a bit of a silly workaround to get a string read in to be the key "_type", but using strings as keys for _matTypes is going to maake this whole script much more readable. 
                // Accord throws an error if _struct is a Matlab struct with both numeric arrays and strings
                // so type is a struct with one field. The field contains no data but the FieldName is the sting I am trying to pass. 
            string _type = new List<string>(_struct.Fields["type"].Fields.Keys)[0]; 
            _matTypes[_type](_struct);

            //MatFileReader hmm = new MatFileReader(_struct);
        }
    }

    // matType Functions
    private void FVmeshSingleColor(MatNode fv)
    {
        Vector3[] vertices = DataParser.MatrixToVectorArray(fv.Fields["vertices"].GetValue<double[,]>());
        int[] faces = DataParser.MatrixTo1DArray(fv.Fields["faces"].GetValue<int[,]>());

        GameObject meshInstance = BuilderFunctions.InstantiateMesh(vertices, faces, transform);

        Color color = ColorParser.GetColor(fv.Fields["color"].GetValue<double[,]>());

        double[,] temp = fv.Fields["opacity"].GetValue<double[,]>();
        float opacity = (float)temp[0, 0];

        BuilderFunctions.AddMat(opacity, meshInstance, _singleColor, _opaqueSingleColor);
        meshInstance.GetComponent<MeshRenderer>().material.SetColor("_color", color);
    }

    private void FVmeshVertexColor(MatNode fv)
    {
        Vector3[] vertices = DataParser.MatrixToVectorArray(fv.Fields["vertices"].GetValue<double[,]>());
        int[] faces = DataParser.MatrixTo1DArray(fv.Fields["faces"].GetValue<int[,]>());

        GameObject meshInstance = BuilderFunctions.InstantiateMesh(vertices, faces, transform);

        double[,] temp = fv.Fields["opacity"].GetValue<double[,]>();
        float opacity = (float)temp[0, 0];

        BuilderFunctions.AddMat(opacity, meshInstance, _vertexColors, _opaqueVertexColors);

        double[,] col = fv.Fields["colors"].GetValue<double[,]>();

        List<Color[]> vertexColorList = ColorParser.GetVertexColorList(col, fv.Fields["map"].GetValue<double[,]>());
        meshInstance.GetComponent<MeshFilter>().mesh.colors = vertexColorList[0]; 
    }

    private void FVmeshMultiVertColor(MatNode fv)
    {
        Vector3[] vertices = DataParser.MatrixToVectorArray(fv.Fields["vertices"].GetValue<double[,]>());
        int[] faces = DataParser.MatrixTo1DArray(fv.Fields["faces"].GetValue<int[,]>());

        GameObject meshInstance = BuilderFunctions.InstantiateMesh(vertices, faces, multiMeshContainer);

        double[,] col = fv.Fields["colors"].GetValue<double[,]>();
        List<Color[]> _multiVertColorList = ColorParser.GetVertexColorList(col, fv.Fields["map"].GetValue<double[,]>());

        meshInstance.GetComponent<MeshFilter>().mesh.colors = _multiVertColorList[0];

        double[,] temp = fv.Fields["opacity"].GetValue<double[,]>();
        float opacity = (float)temp[0, 0];

        BuilderFunctions.AddMat(opacity, meshInstance, _vertexColors, _opaqueVertexColors);

        multiMeshContainer.GetComponent<MultiVertColUpdater>().SetStuff(_multiVertColorList, meshInstance); 
    }

    private void scatter3(MatNode sc)
    {
        GameObject scatterInstance = new GameObject("scatter3Container");
        scatterInstance.transform.parent = transform;

        Vector3[] pts = DataParser.MatrixToVectorArray(sc.Fields["vertices"].GetValue<double[,]>());
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

        List<Vector2[]> points = DataParser.buildPointsList(x, y); 

        graphContainer.GetComponent<GraphController>().BuildGraph(points, color); 
    }

    private void CamDistSetter(MatNode d)
    {
        int[,] _dist = d.Fields["camDistance"].GetValue<int[,]>();
        CamOrbit.functions.SetCamDistance(_dist[0,0]);
    }
}