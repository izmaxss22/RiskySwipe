using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public static class ValueParsers
{
    // Перемешать список
    public static void ShuffleList<T>(List<T> list)
    {
        Random rng = new Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // Перевод списка в строку со значениями разделенными "?" 
    public static string ListToString<ListType>(List<ListType> list)
    {
        string stringFromList = string.Join("?", list);
        return stringFromList;
    }

    // Перевод строки со значениями разделенными "?" в список строк значений
    public static List<string> StringToListStrings(string i)
    {
        return i.Split('?').ToList();
    }

    // Перевод строки в список float
    public static List<float> StringToListFloats(string i)
    {
        List<string> stringList = i.Split('?').ToList();
        List<float> floatList = new List<float>();
        foreach (var item in stringList)
        {
            floatList.Add(float.Parse(item, CultureInfo.InvariantCulture));
        }
        return floatList;
    }
    // Перевод строки в список float
    public static List<int> StringToListInt(string i)
    {
        List<string> stringList = i.Split('?').ToList();
        List<int> floatList = new List<int>();
        foreach (var item in stringList)
        {
            floatList.Add(int.Parse(item));
        }
        return floatList;
    }

    // Перевод строки с набором вектором 3 в масив вектором 3
    public static List<Vector3> StringWithVectors3ToListVectors3(string stringWithVectors)
    {
        var movePointsStringList = StringToListStrings(stringWithVectors);
        var list = new List<Vector3>();
        foreach (var item in movePointsStringList)
        {
            list.Add(StringWithVector3ToVector3(item));
        }

        return list;
    }

    // Перевод строки с одним вектором 3 в вектор 3
    public static Vector3 StringWithVector3ToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');
        var _l = float.Parse(sArray[0], CultureInfo.InvariantCulture);
        var _b = float.Parse(sArray[1], CultureInfo.InvariantCulture);
        var _v = float.Parse(sArray[2], CultureInfo.InvariantCulture);
        // store as a Vector3
        Vector3 result = new Vector3(_l, _b, _v);

        return result;
    }

    public static bool ConvertIntToBool(int state)
    {
        if (state == 1) return true;
        else return false;
    }

    public static int ConvertBoolToInt(bool state)
    {
        if (state == true) return 1;
        else return 0;
    }

    public static int[] ConvertStringToIntArray(string stringForConverte)
    {
        string[] stringConvertedToCharArray = stringForConverte.Split(",".ToCharArray());
        int[] array = new int[stringConvertedToCharArray.Length];
        for (int i = 0; i < stringConvertedToCharArray.Length; i++)
            array[i] = int.Parse(stringConvertedToCharArray[i]);

        return array;
    }

    public static string ConvertIntArrayToString(int[] array)
    {
        string convertedString = "";
        // Составление из значений массива единую строку разделяя значения запятой
        foreach (int item in array) convertedString += item + ",";
        // Удаление последней запятой
        convertedString = convertedString.Remove(convertedString.Length - 1);

        return convertedString;
    }

}
