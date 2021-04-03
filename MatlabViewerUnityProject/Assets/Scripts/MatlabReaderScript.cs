using UnityEngine;
using UnityEngine.UI.Extensions;
using System.Collections.Generic;
using System.IO;

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
    private Dictionary<string, System.Action<MatFileReader>> _matTypes;

    void Start()
    {
        _matTypes = new Dictionary<string, System.Action<MatFileReader>>
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

        List<MatFileReader> h2 = new MatStruct(_path).MatFileList;

        for (int i = 0; i < h2.Count; i ++)
        {
            MatFileReader temp = h2[i]; 
            _matTypes[temp.Type](temp); 
        }
    }

    // matType Functions
    private void FVmeshSingleColor(MatFileReader fv)
    {
        GameObject meshInstance = BuilderFunctions.InstantiateMesh(fv.Vertices, fv.Faces, transform);
        BuilderFunctions.AddMat(fv.Opacity, meshInstance, _singleColor, _opaqueSingleColor);
        meshInstance.GetComponent<MeshRenderer>().material.SetColor("_color", fv.SingleColor);
    }

    private void FVmeshVertexColor(MatFileReader fv)
    {
        GameObject meshInstance = BuilderFunctions.InstantiateMesh(fv.Vertices, fv.Faces, transform);
        BuilderFunctions.AddMat(fv.Opacity, meshInstance, _vertexColors, _opaqueVertexColors);
        meshInstance.GetComponent<MeshFilter>().mesh.colors = fv.VertColorList[0]; 
    }

    private void FVmeshMultiVertColor(MatFileReader fv)
    {
        GameObject meshInstance = BuilderFunctions.InstantiateMesh(fv.Vertices, fv.Faces, multiMeshContainer);
        List<Color[]> _multiVertColorList = fv.VertColorList; 
        multiMeshContainer.GetComponent<MultiVertColUpdater>().SetStuff(_multiVertColorList, meshInstance); 
        meshInstance.GetComponent<MeshFilter>().mesh.colors = _multiVertColorList[0];
        BuilderFunctions.AddMat(fv.Opacity, meshInstance, _vertexColors, _opaqueVertexColors);
    }

    private void scatter3(MatFileReader sc)
    {
        GameObject scatterInstance = new GameObject("scatter3Container");
        scatterInstance.transform.parent = transform;
        BuilderFunctions.SpawnScatterSpheres(scatterInstance, sc.Vertices, sc.PointSize, sc.SingleColor, _scatterMat);
    }
    private void drawGraph(MatFileReader gr) => graphContainer.GetComponent<GraphController>().BuildGraph(gr.GraphPointList, gr.SingleColor);
    private void CamDistSetter(MatFileReader cd) => CamOrbit.functions.SetCamDistance(cd.CamDistance);
}