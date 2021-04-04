using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI.Extensions; 


public class FileListUpdater : MonoBehaviour
{
    private string[] _files;

    [SerializeField] private AutoCompleteComboBox fileSelectionDropDown = default;

    private void Start()
    {
        UpdateFileList(); 
    }

    public void UpdateFileList()
    {
        _files = Directory.GetFiles(Application.streamingAssetsPath, "*.mat");

        List<string> _fileList = new List<string>();

        for (int i = 0; i < _files.Length; i++)
        {
            string[] temp = _files[i].Split(Path.DirectorySeparatorChar);
            _fileList.Add(temp[temp.Length - 1]);
        }

        fileSelectionDropDown.SetAvailableOptions(_fileList);
        fileSelectionDropDown.ItemsToDisplay = _fileList.Count;
    }
}
