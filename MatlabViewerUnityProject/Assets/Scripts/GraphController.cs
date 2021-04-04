using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI; 

public class GraphController : MonoBehaviour
{
    [SerializeField] private GameObject _graphPrefab = default; 
    [SerializeField] private Slider slider = default;

    private List<UILineRenderer> _graphList = new List<UILineRenderer>();
    private List<List<Vector2[]>> _graphPointsList = new List<List<Vector2[]>>(); 

    public void BuildGraph(List<Vector2[]> points, Color color)
    {
        _graphList = new List<UILineRenderer>();

        GameObject _graph = Instantiate(_graphPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        UILineRenderer uILineRenderer = _graph.GetComponent<UILineRenderer>();

        _graph.transform.SetParent(transform, false);

        uILineRenderer.color = color;
        uILineRenderer.Points = points[0];

        if (points.Count > 1)
        {
            _graphList.Add(uILineRenderer);
            _graphPointsList.Add(points); 
        }        
    }

    public void UpdateGraph()
    {
        int idx = (int)slider.value;
        
        for (int i = 0; i < _graphList.Count; i++)
        {
            Vector2[] points = _graphPointsList[i][idx];
            _graphList[i].Points = points;
        }
    }
}
