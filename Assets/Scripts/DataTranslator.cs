using UnityEngine;
using System;

public class DataTranslator : MonoBehaviour {

    public static string KILLS_SYMBOL = "[KILLS]";
    public static string DEATH_SYMBOL = "[DEATHS]";

    public static string ValueToData(int _newKills,int _newDeaths)
    {
        return KILLS_SYMBOL + _newKills + '/' + DEATH_SYMBOL + _newDeaths;
    }
     
    
    public static int DataToKills(string data)
    {
        return int.Parse(DataToValue(data, KILLS_SYMBOL));//int.parse converts string to int,not using .ToInt as it will convert null value to zero 
    }


    public static int DataToDeaths(string data)
    {
        return int.Parse(DataToValue(data, DEATH_SYMBOL));
    }

    private static string DataToValue(string data,string symbol)
    {
        string[] pieces = data.Split('/');
        foreach(string piece in pieces)
        {
            if (piece.StartsWith(symbol))
               return piece.Substring(symbol.Length);
        }
        Debug.Log(symbol + " not found in " + data);
        return "";
    }
}
