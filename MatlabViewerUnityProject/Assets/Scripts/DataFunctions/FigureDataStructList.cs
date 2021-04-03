using System.Collections.Generic;
using Accord.IO;

public class FigureDataStructList
{
    public List<FigureDataStruct> DataStructList; 
    public FigureDataStructList(string _path)
    {
        DataStructList = new List<FigureDataStruct>(); 
        MatReader inputMatReader = new MatReader(_path);
        MatNode inputMatNode = inputMatReader.Fields[inputMatReader.FieldNames[0]];

        foreach (var field in inputMatNode.Fields)
        {
            MatNode _struct = inputMatNode.Fields[field.Key];
            DataStructList.Add(new FigureDataStruct(_struct));    // read all variables in this node and add to list
        }
    }
}