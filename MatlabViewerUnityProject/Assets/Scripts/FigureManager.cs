using UnityEngine;
using UnityEngine.UI.Extensions;
using System.Collections.Generic;

public class FigureManager : MonoBehaviour
{
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
    private Dictionary<string, System.Action<FigureDataStruct>> _matTypes;

    void Start()
    {
        _matTypes = new Dictionary<string, System.Action<FigureDataStruct>>
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
        string _path = Application.streamingAssetsPath + System.IO.Path.DirectorySeparatorChar + fileSelectionDropDown.Text;
        List<FigureDataStruct> h2 = new FigureDataStructList(_path).DataStructList;
        for (int i = 0; i < h2.Count; i ++)
        {
            FigureDataStruct temp = h2[i]; 
            _matTypes[temp.Type](temp); 
        }
    }
    private void FVmeshSingleColor(FigureDataStruct fv)
    {
        GameObject meshInstance = BuilderFunctions.InstantiateMesh(fv.Vertices, fv.Faces, transform);
        BuilderFunctions.AddMat(fv.Opacity, meshInstance, _singleColor, _opaqueSingleColor);
        meshInstance.GetComponent<MeshRenderer>().material.SetColor("_color", fv.SingleColor);
    }
    private void FVmeshVertexColor(FigureDataStruct fv)
    {
        GameObject meshInstance = BuilderFunctions.InstantiateMesh(fv.Vertices, fv.Faces, transform);
        BuilderFunctions.AddMat(fv.Opacity, meshInstance, _vertexColors, _opaqueVertexColors);
        meshInstance.GetComponent<MeshFilter>().mesh.colors = fv.VertColorList[0]; 
    }
    private void FVmeshMultiVertColor(FigureDataStruct fv)
    {
        GameObject meshInstance = BuilderFunctions.InstantiateMesh(fv.Vertices, fv.Faces, multiMeshContainer);
        List<Color[]> _multiVertColorList = fv.VertColorList; 
        multiMeshContainer.GetComponent<MultiVertColUpdater>().SetStuff(_multiVertColorList, meshInstance); 
        meshInstance.GetComponent<MeshFilter>().mesh.colors = _multiVertColorList[0];
        BuilderFunctions.AddMat(fv.Opacity, meshInstance, _vertexColors, _opaqueVertexColors);
    }
    private void scatter3(FigureDataStruct sc)
    {
        GameObject scatterInstance = new GameObject("scatter3Container");
        scatterInstance.transform.parent = transform;
        BuilderFunctions.SpawnScatterSpheres(scatterInstance, sc.Vertices, sc.PointSize, sc.SingleColor, _scatterMat);
    }
    private void drawGraph(FigureDataStruct gr) => graphContainer.GetComponent<GraphController>().BuildGraph(gr.GraphPointList, gr.SingleColor);
    private void CamDistSetter(FigureDataStruct cd) => CamOrbit.functions.SetCamDistance(cd.CamDistance);
}