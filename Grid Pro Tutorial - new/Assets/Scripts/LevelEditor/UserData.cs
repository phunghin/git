using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
[Serializable]
public class UserData
{
    public static readonly string UserFormat = "userdata.json";

    #region Singleton

    private static UserData _instance;

    public static UserData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UserData();

                if (!Helper.LoadJson<UserData>(UserFormat, ref _instance,true))
                {
                    Debug.Log("user resert");
                    _instance.Resert();
                }
            }

            return _instance;
        }
    }

    #endregion

    public int Heath = 5;
    public int Coin = 50;
    public List<LevelUserData> LevelUserData = new List<LevelUserData>();
    public List<ItemData> ItemUserData = new List<ItemData>();


    public void Resert()
    {
        Heath = 5;
        Coin = 50;
        LevelUserData = new List<LevelUserData>();
        ItemUserData = new List<ItemData>();
    }

    
    public void InsertLevel(int level , LevelUserData data)
    {
        int count = LevelUserData.Count();
      
        if (level > count)
        {
            LevelUserData.Add(data);
        }
        else
        {
            if (LevelUserData[level-1].Score < data.Score)
            {
                LevelUserData[level-1] = data;
            }   
        }
    }

    public bool Save()
    {
        Helper.SaveJson<UserData>(this,UserFormat);
        return true;
    }

}


[Serializable]
public class LevelUserData
{
    public int Star;
    public int Score;

    public LevelUserData(int star , int score)
    {
        this.Star = star;
        this.Score = score;
    }
}

[Serializable]
public class ItemData
{
    public CellData Cell;
    public int Amount;

    public ItemData(CellData cell, int amount)
    {
        this.Cell = cell;
        this.Amount = amount;
    }
}

