using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

  //  [Serializable]
public class LevelData
{
    public static readonly string LevelFormat = "Level{0}.json";
    
    // time or move
    public int Mode;

    // Width + Height
    public int Width;
    public int Height;

    // map
    public ListCellData CellData;

    public int Limit;

    
    public bool Save(int level)
    {       
        Helper.SaveJson<LevelData>(this, string.Format(LevelFormat,level));
        return true;
    }

    public static LevelData Load(int level)
    {
        LevelData data = null;
        string filename = LevelFormat.Replace(".json","");

        Helper.LoadJson<LevelData>(string.Format(filename,level), ref data);
        return data;
    }
}

[Serializable]
public class CellData
{
    public CellType CellType; 
    public int Id; 
    public int CellChose;
    

    public CellData(CellType type, int id, int chose = 0)
    {
        this.CellType = type;
        this.Id = id;
        this.CellChose = chose;
    }
}

[Serializable]
public class ListCellData
{
    public List<CellData> ListCell;

    public List<CellData> CellTarget;
    public List<int> NumberTarget;

    public List<int> ListStar;
}

