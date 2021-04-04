using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class MultiVertColUpdater : MonoBehaviour
{
    public Slider slider;
    public GameObject SliderCanvas;

    private List<Color[]> _multiVertColorList;
    private MeshFilter multiVertMeshFilter;

    void Start()
    {
        slider.maxValue = 0;
    }

    public void UpdateVertexColors()
    {
        int tab = (int)slider.value;

        multiVertMeshFilter.mesh.colors = _multiVertColorList[tab];
    }

    public void SetStuff(List<Color[]> colors, GameObject go)
    {
        _multiVertColorList = colors; 
        multiVertMeshFilter = go.GetComponent<MeshFilter>();

        SliderCanvas.SetActive(true);
        slider.maxValue = _multiVertColorList.Count - 1;
        slider.value = 0;
    }
}
