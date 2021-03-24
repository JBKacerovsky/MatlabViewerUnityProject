using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI; 

public class GraphController : MonoBehaviour
{
    [SerializeField] private GameObject _graphPrefab; 

    [SerializeField] private Slider slider; 
    [SerializeField] private List<UILineRenderer> graphList;
    private List<double[,]> _graphXvalues;
    private List<double[,]> _graphYvalues;

    public void BuildGraph(double[,] x, double[,] y, Color color)
    {
        graphList = new List<UILineRenderer>();
        _graphXvalues = new List<double[,]>();
        _graphYvalues = new List<double[,]>();

        GameObject _graph = Instantiate(_graphPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        UILineRenderer uILineRenderer = _graph.GetComponent<UILineRenderer>();

        _graph.transform.SetParent(transform, false);

        Vector2[] points = BuilderFunctions.buildPointsArray(x, y, 0);

        uILineRenderer.color = color;
        uILineRenderer.Points = points;

        if (y.GetLength(1) > 1) // if graph has more than 1 set of y values it becomes scrollable
        {
            graphList.Add(uILineRenderer);
            _graphXvalues.Add(x);
            _graphYvalues.Add(y);
        }
    }

    public void UpdateGraph()
    {
        int idx = (int)slider.value;

        for (int i = 0; i < graphList.Count; i++)
        {
            Vector2[] points = BuilderFunctions.buildPointsArray(_graphXvalues[i], _graphYvalues[i], idx);
            graphList[i].Points = points;
        }
    }
}
