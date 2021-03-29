using System.Collections.Generic; 
using UnityEngine;

public static class ColorParser
{
    public static Color GetColor(double[,] col)
    {
        Color _color = new Color(0, 0, 0);
        _color.r = (float)col[0, 0];
        _color.g = (float)col[0, 1];
        _color.b = (float)col[0, 2];

        return _color;
    }
        
    public static Color[] BuildColorArray(double[,] vertCol, Gradient colMap)
    {
        Color[] colors = new Color[vertCol.GetLength(0)];

        for (int i = 0; i < vertCol.GetLength(0); i++)
        {
            colors[i] = colMap.Evaluate((float)vertCol[i, 0]);
        }
        return (colors);
    }

    public static List<Color[]> GetVertexColorList(double[,] colors, double[,] ColMapMatrix)
    {
        Gradient colMap = MatrixToColormap(ColMapMatrix);

        List < Color[]> ColorList = new List<Color[]>();

        for (int i = 0; i < colors.GetLength(1); i++)
        {
            ColorList.Add(BuildColorArray(DataParser.getColumnDouble(colors, i), colMap));
        }

        return ColorList;
    }

    public static Gradient MatrixToColormap(double[,] matrix)
    {
        UnityEngine.Gradient map = new UnityEngine.Gradient();

        int rowCount = matrix.GetLength(0);
        GradientColorKey[] colorKey = new GradientColorKey[rowCount];

        Color temp = new Color();

        for (int i = 0; i < rowCount; i++)
        {
            temp.r = (float)matrix[i, 0];
            temp.g = (float)matrix[i, 1];
            temp.b = (float)matrix[i, 2];

            colorKey[i].color = temp;
            colorKey[i].time = (float)i / rowCount;
        }

        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        map.SetKeys(colorKey, alphaKey);

        return (map);
    }

}
