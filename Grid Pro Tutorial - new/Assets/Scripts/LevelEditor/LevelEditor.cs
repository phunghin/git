using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelEditor : MonoBehaviour
{

    public GridController _gridCtrl;


    public void BuildMap(int level = 0)
    {
        _gridCtrl.BuildDefault();
    }

    public void LoadMap(int level = 0)
    {
        _gridCtrl.LoadMapOnEditor(level);
    }

    public void SaveMap(int level = 0)
    {
        _gridCtrl.SaveMapOnEditor(level);
    }

    public void ClearMap()
    {
        _gridCtrl.ClearMap();
    }

    
}
