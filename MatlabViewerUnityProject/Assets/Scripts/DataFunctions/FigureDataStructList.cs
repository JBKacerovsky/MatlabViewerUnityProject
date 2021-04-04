using System.Collections.Generic;
using Accord.IO;

public class FigureDataStructList
{
    // FigureDataStructList reads and stores all data specified by Xfigure .mat files. 
    // Each cell of the Xfigure cell array is read and stored as a FigureDataStruct object in this list
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