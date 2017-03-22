using Gamelogic.Grids;
using Gamelogic.Grids.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Gamelogic.Extensions;
using UnityEngine.SceneManagement;

public class GridController : GLMonoBehaviour
{
    public SpriteFactory SpriteFactory;

    public Transform parent;
    public Transform parentSquare;

    public GameObject CellDestroyEffect;
    public CellController CellPrefabs;
    RectGrid<CellController> grid;
    IMap3D<RectPoint> map;

    [SerializeField]
    Vector2 CellSize = new Vector2(2.5f, 2.5f);
    [SerializeField]
    int width, height;
    [SerializeField]
    float _speedMove, _speedMoveNew,_cellnomal = 0.25f,_cellSpecial;
    
    [SerializeField]
    Vector3 _MoveNewCell = new Vector3(0, 5, 0);
    [SerializeField]
    float[] timeDelayTest;
      
    public void LoadMap(int level)
    {
        ClearMap();

        if (Load(level))
        {
            BuildMap();
        }
        else
        {
            BuildMapNomal();
        }
    }

    void BuildMap()
    {
        // create grid
        grid = RectGrid<CellController>.BeginShape().Rectangle(width,height).EndShape();

        // create Map
        // rect ( khoang? cach , center cell , .WithWindow(ExampleUtils.ScreenRect) , mid map 
        map = new RectMap(CellSize).AnchorCellMiddleCenter().WithWindow(ExampleUtils.ScreenRect).AlignMiddleCenter(grid).To3DXY();


        // create Cell
        int count = 0;
        foreach (var point in grid)
        {
              InitCell(point,celllist[count].CellType,celllist[count].Id,celllist[count].CellChose);
              count++;
        }

        var _Squarecells = grid.Where(c => grid[c].CellType != CellType.None);

        foreach (var item in _Squarecells.Where(c => (c.X + c.Y) % 2 == 0))
        {
            GetBackgroundSquare(item,parentSquare,map[item],0);
        }

        foreach (var item in _Squarecells.Where(c => (c.X + c.Y) % 2 != 0))
        {
            GetBackgroundSquare(item, parentSquare, map[item], 1);
        }
                            
    }

    void GetBackgroundSquare(RectPoint point,Transform parent, Vector3 position, int id)
    {
        int idchose = 0;
        int angle = 0;
        var items = grid.GetNeighbors(point).Where(c => grid[c].CellType != CellType.None);

        int count = items.Count();

        switch (count)
        {
            case 0:
                idchose = 2;
                break;
            case 1:
                idchose = 3;

                if (grid.Contains(point + RectPoint.South) && grid[point + RectPoint.South].CellType != CellType.None) // ping left
                {
                    angle = 90;
                }
                else
                {
                    if (grid.Contains(point + RectPoint.North) && grid[point + RectPoint.North].CellType != CellType.None) // ping left
                    {
                        angle = 270;
                    }
                    else
                    {
                        if (grid.Contains(point + RectPoint.East) && grid[point + RectPoint.East].CellType != CellType.None) // ping left
                        {
                            angle = 180;
                        }
                    }
                }


                    break;
            case 2:
                if (items.Where(c => c.X == point.X).Count() == 2 || items.Where(c => c.Y == point.Y).Count() == 2)
                {
                    idchose = 5;
                }else
                {
                    idchose = 4;

                    if(grid.Contains(point + RectPoint.West) && grid[point + RectPoint.West].CellType != CellType.None) // ping left
                    {
                        if (grid.Contains(point + RectPoint.North) && grid[point + RectPoint.North].CellType != CellType.None) // ping up
                        {
                            angle = 270;
                        }                              
                    }
                    else
                    {
                        if (grid.Contains(point + RectPoint.North) && grid[point + RectPoint.North].CellType != CellType.None)
                        {
                           angle = 180;
                        }
                        else
                        {
                            angle = 90;
                        }
                    }

                }
                              
                break;
            default:
                idchose = 5;
                break;
        }

         SpriteFactory.AddSquare(parentSquare, map[point], id, idchose, angle);
    }

    


    void BuildMapNomal()
    {
        // create grid
        grid = RectGrid<CellController>.BeginShape().Rectangle(width, height).EndShape();

        // create Map
        // rect ( khoang? cach , center cell , .WithWindow(ExampleUtils.ScreenRect) , mid map 
        map = new RectMap(CellSize).AnchorCellMiddleCenter().WithWindow(ExampleUtils.ScreenRect).AlignMiddleCenter(grid).To3DXY();


        // create Cell
        foreach (var point in grid)
        {         
            InitCell(point,CellType.Nomal);
        }


    }
 
    [SerializeField]
    bool _istouch = true;
    RectPoint _pointCurrent;
    RectPoint _pointLate;
    RectPoint _pointDefault;
    HashSet<RectPoint> _HashSetCurrent = new HashSet<RectPoint>();
    void Update()
    {
        #region Mouse Move
        if (Input.GetMouseButton(0))
        {
            if (grid == null || !_istouch) return;

            RectPoint rectPoint = map[ScreenToWorldPoint(Input.mousePosition)];

            _pointCurrent = rectPoint;

            if (_pointCurrent != _pointLate)
            {
                if(_HashSetCurrent.Count() != 0)
                {
                    foreach (var item in _HashSetCurrent)
                    {
                        grid[item].IsTouchActive = false;
                    }

                    _HashSetCurrent.Clear();
                }

                if (grid.Contains(rectPoint))
                {
                    _HashSetCurrent = Algorithms.GetConnectedSet(grid,rectPoint, CheckCellMatchNomal);

                    foreach (var item in _HashSetCurrent)
                    {
                        grid[item].IsTouchActive = true;
                    }
                }
            }
                        
            _pointLate = rectPoint;
        }
        #endregion

        else
        {
            #region Mouse Up

            if (Input.GetMouseButtonUp(0))
            {
                if (grid == null || !_istouch) return;

                if (_HashSetCurrent.Count() != 0)
                {
                    foreach (var item in _HashSetCurrent)
                    {
                        grid[item].IsTouchActive = false;
                    }

                    _HashSetCurrent.Clear();

                    _pointLate = _pointDefault;
                }

                RectPoint rectPoint = map[ScreenToWorldPoint(Input.mousePosition)];
             
                if (grid.Contains(rectPoint))
                {
                    // disable touch
                    _istouch = false;

                    // check input type
                    var cellType = SwitchCheckConnectType(rectPoint, grid[rectPoint].CellType);

                    // chose handle cell
                StartCoroutine(SwitchClickCell(cellType, rectPoint, (result, time) =>
                    {
                        // wait handle done
                        if (result)
                        {

                            Debug.Log("return");
                            // move cell and makenew             
                                                                                
                            StartCoroutine(MoveAndNewCell(time));

                            var cellAnimal = grid.Where(c => CheckMatchCellType(c, CellType.Animal));

                            if (cellAnimal.Count() != 0)
                            {
                                StartCoroutine(WaitForIsCheck((c) =>
                                {
                                    if (c)
                                    {
                                        _istouch = false;

                                        foreach (var item in cellAnimal)
                                        {
                                            if (!grid.Contains(item + RectPoint.South))
                                            {
                                                // false
                                                DestroyCellNew(item, 0.1f);
                                            }
                                        }

                                        StartCoroutine(MoveAndNewCell(0.1f));
                                    }
                                }));
                            }


                            // wait handle cell
                            StartCoroutine(WaitForIsCheck((touchcallback) =>
                            {
                                if (touchcallback)
                                {
                                    LimitChose--;
                                    MainGame.Instance.UpdateLImit(LimitChose);
                                    MainGame.Instance.UpdateScore(scoreCurrent);

                                    // count target == 0 
                                    if (CountTarget.All(c => c == 0))
                                    {
                                        _istouch = false;
                                        StartCoroutine(ActionFinish());
                                    }
                                    else if (LimitChose == 0)
                                    {
                                        _istouch = false;
                                        MainGame.Instance.GameOver();

                                    }
                                }
                            }));

                        }
                        else
                        {
                            // return false
                            _istouch = true;
                        }
                    }));

                }

                
            }

            #endregion
        }

    }

    [SerializeField]
    float testtime;
    
    #region init - move - cell

    IEnumerator MoveAndNewCell(float time)
    {
        yield return new WaitForSeconds(time);

        var _countCell = CountCellMoveNew();

        for (int i = 0; i < grid.Height; i++)
        {           
            var cellmove = _countCell.WhereCell(p => p != 0).Where(c => c.Y == i && grid[c] != null);
           
            if (cellmove.Count() != 0)
            {
                yield return new WaitForSeconds(0.1f);

                foreach (var item in cellmove)
                {
                    MoveCellTest(item, grid[item], _countCell[item]);
                }
            }                    
        }
     
        var _cellNull = grid.Where(c => grid[c] == null);
            
        for (int i = 0; i < grid.Height; i++)
        {          
            var cellCol = _cellNull.Where(c => c.Y == i);
            
            if(cellCol.Count() != 0)
            {
                yield return new WaitForSeconds(0.1f);
                foreach (var point in cellCol)
                {
                    MoveMakeNewCell(point, InitCell(point, CellType.Nomal));
                }
            }            
       }
        

        StartCoroutine(WaitForIsMove((c) =>
        {
            if (c) _istouch = true;
        }));

    }

    CellController InitCell(RectPoint point, CellType type, int id = 0, int idchose = 0)
    {
        CellController cell = Instantiate(CellPrefabs);
        Vector3 worldPoint = map[point];
        cell.transform.parent = parent;
        cell.transform.localScale = Vector3.one;
        cell.transform.localPosition = worldPoint;
        cell.name = point.ToString();
        grid[point] = cell;

        cell.CellType = type;

        if (type == CellType.Nomal)
        {
            int idcell = Random.Range(0, 4);
            cell.Id = idcell;
            cell.SpriteCell = SpriteFactory.AddSpriteCell(type,idcell);
        }
        else
        {
            cell.Id = id;
            cell.CellChose = idchose;
            cell.SpriteCell = SpriteFactory.AddSpriteCell(type, id,idchose);
        }
             
        return cell;
    }

    IEnumerator WaitForIsMove(Action<bool> callback)
    {
        while (grid.Values.Any(c => c != null && !c.IsMove))
        {
            yield return new WaitForFixedUpdate();
        }

        callback(true);
    }

    IEnumerator WaitForIsCheck(Action<bool> callback)
    {
        while (!_istouch)
        {
            yield return new WaitForFixedUpdate();
        }

        callback(true);
    }


    IGrid<int, RectPoint> CountCellMoveNew()
    {
        var _countCellMove = grid.CloneStructure<int>();

        foreach (var point in grid)
        {
            int count = 0;

            if(grid[point] != null && grid[point].CellType != CellType.None)
            {
                count = grid.Where(c => c.X == point.X && c.Y < point.Y && grid[c] == null ).Count();
            }

            _countCellMove[point] = count;
        }

        return _countCellMove;
    }


    void MoveCellTest(RectPoint start, CellController cell, int cellmove)
    {     
        RectPoint destination = start;
        int count = 1;
        for (int i = 0; i < cellmove; i++)
        {
            destination += RectPoint.South;
            count++;
            if (grid.Contains(destination))
            {
                while(CheckMatchCellType(destination,CellType.None))
                {
                    destination += RectPoint.South;
                    count++;
                }
            }           
        }
    
        cell.MoveCell(map[start], map[destination],_speedMoveNew, 0,EaseType.Linear);
               
        cell.name = destination.ToString();
        grid[start] = null;
        grid[destination] = cell;
      
    }

    void MoveMakeNewCell(RectPoint finish , CellController cell)
    {
        Vector3 _destination = map[finish];
        Vector3 _start = _destination + _MoveNewCell;
        cell.transform.position = _start;

        cell.MoveCell(_start, _destination,_speedMoveNew, 0, EaseType.Linear);

        cell.name = finish.ToString();
        grid[finish] = cell;
    }

    #endregion

    #region Cell - match - check connect  

    HashSet<RectPoint> SwitchCheckConnectType(RectPoint rectPoint, CellType type)
    {
        HashSet<RectPoint> _hashSet = new HashSet<RectPoint>();
        switch (type)
        {
            case CellType.Nomal: _hashSet = Algorithms.GetConnectedSet(grid, rectPoint, CheckCellMatchNomal); break;
            case CellType.Special: _hashSet = Algorithms.GetConnectedSet(grid, rectPoint, CheckCellMatchSpecial); break;
            case CellType.None:
                break;
        }

        return _hashSet;
    }

    bool CheckCellMatchNomal(RectPoint r1, RectPoint r2)
    {
        if (grid[r1] == null || grid[r1].CellType != CellType.Nomal) return false;
        if (grid[r2] == null || grid[r2].CellType != CellType.Nomal) return false;
       
        return grid[r1].Id == grid[r2].Id;
    }

    bool CheckCellMatchSpecial(RectPoint r1, RectPoint r2)
    {
        if (grid[r1] == null) return false;
        if (grid[r2] == null) return false;

        return grid[r1].CellType == grid[r2].CellType;
    }

    bool CheckMatchType(RectPoint r1)
    {
        if (grid[r1] == null) return false;

        return grid[r1].CellType == CellType.Nomal || grid[r1].CellType == CellType.Block;             
    }

    bool CheckMatchCellType(RectPoint r1,CellType type)
    {
        if (grid[r1] == null) return false;

        return grid[r1].CellType == type;
    }
  
    IEnumerator ActionFinish()
    {
        yield return new WaitForSeconds(1);

        var celltarget = grid.Where(c => CheckMatchCellType(c,CellType.Nomal)).OrderBy(c => Guid.NewGuid()).Take(LimitChose);

        int total = celltarget.Count();
        int count = 0;
        float time;

        if(total > 9)
        {
            time = 0.01f;
        }
        else
        {
            time = 0.1f;
        }
        LimitChose--;

        foreach (var item in celltarget)
        {
            yield return new WaitForSeconds(time * count++);

            int id = grid[item].Id;
            DestroyCellNew(item, 0);

            InitCell(item, CellType.Special, Random.Range(0,2), Random.Range(0, 2));

            
            MainGame.Instance.UpdateLImit(LimitChose--);
        }

        yield return new WaitForSeconds(1);
        var cellaction = grid.Where(c => CheckMatchCellType(c, CellType.Special));

        foreach (var item in cellaction)
        {           
            if(grid[item] != null)
            {
                StartCoroutine(ActionSpecialCurrent(item));
                yield return new WaitForSeconds(0.25f);
            }            
        }


        StartCoroutine(MoveAndNewCell(1f));

        StartCoroutine(WaitForIsCheck( (touchcallback) => 
        {
            if (touchcallback)
            {
                _istouch = false;
                // save game
                MainGame.Instance.UpdateCanvas(0,CountTarget,scoreCurrent);
                // next level
                MainGame.Instance.NextLevel(GetStar(),scoreCurrent);
            }
        }));      
    }


    #endregion

    #region Click - Cell - Type

    IEnumerator SwitchClickCell(HashSet<RectPoint> cellconnect, RectPoint pointClick ,Action<bool,float> callback)
    {
        CellType type = grid[pointClick].CellType;
        int _id = grid[pointClick].Id;

        switch (type)
        {
            
            case CellType.Nomal:

 #region Handle - Nomal
                if (cellconnect.Count < 2)
                {
                    callback(false,0);
                    yield break;
                }
                else
                {
                    // check create
                    StartCoroutine(ClickNomalHandel(cellconnect, pointClick, _id,(c1) => 
                    {
                        callback(true,0);
                    }));

                    yield break;
                }
#endregion

            case CellType.Special:

#region Handle - Special
                HashSet<RectPoint> specialHandle = new HashSet<RectPoint>();

                if(cellconnect.Count == 1) // click 1 special
                {
                    specialHandle = GetHandleCellSpecial(pointClick);
                }
                else // click combo special
                {
                    

                    specialHandle = GetHandleComboSpecial(cellconnect, pointClick);
                    yield return new WaitForSeconds(1f);
                }


                if (specialHandle != null)
                {
                    foreach (var item in specialHandle)
                    {                       
                        if (grid[item] != null)
                        {
                            StartCoroutine(ActionSpecialCurrent(item));
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                }

                callback(true, 1);
                yield break;

            #endregion

            default: callback(false, 0); break;
        }

    }

    IEnumerator ClickNomalHandel(HashSet<RectPoint> cellconnect,RectPoint point,int id ,Action<float> callback)
    {        
        int count = cellconnect.Count;
         
        if (count >= 2 && count <= 4)
        {
            foreach (var item in cellconnect)
            {
                var blocks = grid.GetNeighbors(item, c => CheckMatchCellType(c, CellType.Block));

                foreach (var block in blocks)
                {
                    DestroyCellNew(block, _cellnomal);
                }

                DestroyCellNew(item, _cellnomal);
            }

            callback(0.1f);
            yield break;
        }
        else
        {
            
            foreach (var item in cellconnect)
            {
                var blocks = grid.GetNeighbors(item, c => CheckMatchCellType(c, CellType.Block));

                foreach (var block in blocks)
                {
                    DestroyCellNew(block,_cellnomal);
                }

              grid[item].MoveCell(map[item], map[point],0.3f,0,EaseType.Linear);
              DestroyCellNew(item,5,true,false);
              
            }
            
            if (count >= 5 && count <= 6)
            {               
                CellController cell = InitCell(point,CellType.Special,0,Random.Range(0,2));               
                
            }
            else
            {
                if (count >= 7 && count <= 8)
                {
                    InitCell(point, CellType.Special,1,0);
                }
                else
                {
                    InitCell(point,CellType.Special,2,id);                   
                }
            }

            callback(0.1f);
        }      
    }

    HashSet<RectPoint> GetHandleCellSpecial(RectPoint point)
    {
        int _idCell = grid[point].Id;
        HashSet<RectPoint> _cellChose = new HashSet<RectPoint>();

        switch (_idCell)
        {
            //ten lua 
            case 0:

                if (grid[point].CellChose == 0) // row
                {
                    // get all pointX
                    _cellChose.UnionWith(grid.Where(c => c.X == point.X && CheckMatchCellType(c, CellType.Special)));
                }
                else // col
                {
                    // get all pointX
                    _cellChose.UnionWith(grid.Where(c => c.Y == point.Y && CheckMatchCellType(c, CellType.Special)));
                }

                break;
            //bom  
            case 1:
                _cellChose.UnionWith(grid.GetNeighborHood(point, 1).Where(c => CheckMatchCellType(c, CellType.Special)));

                break;
            //rubik
            case 2:
                _cellChose.Add(point);
                break;
        }
     
        return _cellChose;
    }
 
    IEnumerator ActionSpecialCurrent(RectPoint point)
    {
        if (grid[point] == null) yield break;
                        
        int _idCell = grid[point].Id; 
        IEnumerable<RectPoint> _cellChose = null;
        int counttime = 0;

        switch (_idCell)
        {           
            //ten lua 
            case 0:
                
                if (grid[point].CellChose == 0) // row
                {                  
                    // combo clear                  
                    var c1 = grid.Where(c => c.X == point.X && CheckMatchType(c));
                                     
                    foreach (var item in c1)
                    {                                           
                        DestroyCellNew(item,10);
                    }

                    // get all pointX
                    _cellChose = grid.Where(c => c.X == point.X && CheckMatchCellType(c, CellType.Special)); 
                                                     
                }
                else // col
                {
                    var c1 = grid.Where(c => c.Y == point.Y && CheckMatchType(c));

                    foreach (var item in c1)
                    {
                        DestroyCellNew(item,10);
                    }

                    // get all pointX
                    _cellChose = grid.Where(c => c.Y == point.Y && CheckMatchCellType(c,CellType.Special));                
                }

                break;
            //bom  
            case 1:
               

                var bom1 = grid.GetNeighborHood(point, 1).Where(c => CheckMatchType(c));
               
                foreach (var item in bom1)
                {
                    DestroyCellNew(item,10);
                }

                _cellChose = grid.GetNeighborHood(point, 1).Where(c => CheckMatchCellType(c, CellType.Special));

                break;
            //rubik
            case 2:

                var rubik = grid.Where(c => CheckMatchCellType(c, CellType.Nomal)).Where(c => grid[c].Id == grid[point].CellChose);
             
                foreach (var item in rubik)
                {                   
                    DestroyCellNew(item,10);
                }
                break;
        }

        if(_cellChose == null)
        {
            // rubik or error
            DestroyCellNew(point,5,false);
            yield break;
        }
        

        var _cellACtive = _cellChose.Where(c => c != point);

        int count = _cellChose.Count();

        DestroyCellNew(point, 5, false);

        if (count == 0)
        {         
            yield break;
        }
        else
        {

            yield return new WaitForSeconds(0.25f);

            foreach (var item in _cellACtive)
            {                
                if (grid[item] != null)
                {                 
                    StartCoroutine(ActionSpecialCurrent(item));                   
                    yield return new WaitForSeconds(0.25f);
                }                
            }
           
        }
    } 

    HashSet<RectPoint> GetHandleComboSpecial(HashSet<RectPoint> cellconnect, RectPoint point)
    {     
        var _ChoseCombo = cellconnect.OrderByDescending(c => grid[c].Id).Take(2).ToArray();
        var _combo1 = grid[_ChoseCombo[0]].Id;
        var _combo2 = grid[_ChoseCombo[1]].Id;
        point = _ChoseCombo[0];
        int _idChose = grid[_ChoseCombo[1]].CellChose;


        IEnumerable<RectPoint> _rectClear = cellconnect.Where(c => c != _ChoseCombo[0]);
        IEnumerable<RectPoint> _rectchose = null;
        HashSet<RectPoint> _cellCurrent = new HashSet<RectPoint>();
        // move ve click cell

        foreach (var item in _rectClear)
        {
            grid[item].MoveCell(map[item], map[point],0.25f);
        }


        switch (_combo2)
        {
            // Rocket Combo 
            case 0:
                // Rocket x2  1 row + 1 cols
                if (_combo1 == 0)
                {
                    _rectchose = grid.Where(c => c.X == point.X || c.Y == point.Y);
                  
                    foreach (var item in grid.Where(c => c.X == point.X && CheckMatchType(c)).Union(grid.Where(c => c.Y == point.Y && CheckMatchType(c))))
                    {
                        DestroyCellNew(item,10);
                    }                  
                }
                else
                {
                    // Rocket + boom  3row + 3Col
                    if (_combo1 == 1)
                    {
                        _rectchose = grid.Where(c => c.X == point.X || c.Y == point.Y || c.X == point.X - 1 || c.Y == point.Y - 1 || c.X == point.X + 1 || c.Y == point.Y + 1);

                        var v = _rectchose.Where(c => CheckMatchType(c)).Where(c => c.X == point.X || c.X == point.X - 1 || c.X == point.X + 1);

                        foreach (var item in v)
                        {
                            DestroyCellNew(item,10);
                        }

                        var h = _rectchose.Where(c => CheckMatchType(c)).Where(c => c.Y == point.Y || c.Y == point.Y - 1 || c.Y == point.Y + 1);

                        foreach (var item in h)
                        {
                            DestroyCellNew(item,10);
                        }

                    }
                    else
                    {
                        // roket + rubik // change all id = cellchose == roket
                        if (_combo1 == 2)
                        {
                            var _cellnomal = grid.Where(c => grid[c] != null && grid[c].CellType == CellType.Nomal).Where(c => grid[c].Id == _idChose);

                            foreach (var item in _cellnomal)
                            {
                                int id = grid[item].Id;
                                DestroyCellNew(item,0);

                                CellController cell = InitCell(item,CellType.Special,0,Random.Range(0,2));                              
                            }

                            _rectchose = grid.Where(c => CheckMatchCellType(c,CellType.Special) && grid[c].Id == 0);                          
                        }
                    }
                }

                break;
            case 1:
                // boom + boom      area 9 o
                if (_combo1 == 1)
                {
                    _rectchose = grid.GetNeighborHood(_ChoseCombo[0],2);

                    var area = _rectchose.Where(c => CheckMatchType(c));

                    foreach (var item in area)
                    {                      
                        DestroyCellNew(item,10);
                    }                   
                }
                else
                {
                    // boom + rubik // change all id = cellchose == boom
                    if (_combo1 == 2)
                    {
                        var _cellnomal = grid.Where(c => grid[c] != null && grid[c].CellType == CellType.Nomal).Where(c => grid[c].Id == _idChose);

                        foreach (var item in _cellnomal)
                        {                            
                            int id = grid[item].Id;
                            DestroyCellNew(item, 0);

                            InitCell(item,CellType.Special,1,id);
                        }
                        
                        _rectchose = grid.Where(c => CheckMatchCellType(c,CellType.Special) && grid[c].Id == 1);
                    }
                }
                break;
            case 2:
                // rubik == destroy all cellcontroll != null != rot;
                if (_combo2 == 2)
                {
                    var arena = grid.Where(c => grid[c] != null && grid[c].CellType != CellType.None);

                    foreach (var item in arena)
                    {                       
                        DestroyCellNew(item,10);
                    }
                  
                    _rectchose = null;
                }
                break;

        }

        foreach (var item in cellconnect)
        {
            DestroyCellNew(item, 0.5f);
        }
       
        if (_rectchose == null)
        {
            // rubick combo
            return null;
        }
        else
        {
            _cellCurrent.UnionWith(_rectchose.Where(c => CheckMatchCellType(c, CellType.Special)));
            return _cellCurrent;
        }
              
    }
    
    IEnumerator WaitRocketAnimation(int id,CellController cell,Vector3 start, Vector3 end)
    {
        yield return new WaitForSeconds(2);

        
    }

    IEnumerator WaitBoomAnimation(int id, CellController cell)
    {
        yield return new WaitForSeconds(2);

        switch (id)
        {
            case 0:
                break;
            case 1:

                break;
            case 2:
                break;

        }
    }
    #endregion

    #region Destroy - Cell - On 
    // X hang doc , Y hang ngang

    int scoreCurrent;

    void DestroyCellNew(RectPoint point , float time,bool touchactive = true, bool move = true)
    {
        if (grid[point] != null)
        {            
            scoreCurrent += 10;

            grid[point].IsTouchActive = touchactive;
            bool target = FindCellTarget(point);

            if(target && move)
            {
                grid[point].positionMove = PositionTargetMove(point);
                grid[point].DestroyCell(time + Random.Range(2,5),true);
            }
            else
            {
                grid[point].DestroyCell(time,false);

                GameObject go = Instantiate(CellDestroyEffect);
                go.transform.position = map[point];
                go.GetComponent<BreakCellCtor>().Contruct(time, SpriteFactory.AddColor(grid[point].Id));
            }
           
            grid[point] = null;
        }
    }

    bool FindCellTarget(RectPoint point)
    {        
       for (int i = 0; i < CellTarget.Length; i++)
       {
           if(CellTarget[i].Id == grid[point].Id && CellTarget[i].CellType == grid[point].CellType)
            {
                if (CountTarget[i] <= 0)
                {
                    return false;
                }
                    
                CountTarget[i] -= 1;
                return true;
            }
       }
        
        return false;
    }

    Vector3 PositionTargetMove(RectPoint point)
    {
        for (int i = 0; i < CellTarget.Length; i++)
        {
            if(grid[point].Id == CellTarget[i].Id)
            {
                return PositionTarget[i];
            }
        }

        return Vector3.zero;
    }

    public int GetStar()
    {
        if (scoreCurrent < StarScore[0])
        {
            return 1;
        }
        else
        {
            if (scoreCurrent < StarScore[1])
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
    }

    #endregion

    Vector3 ScreenToWorldPoint(Vector3 screenPosition)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(screenPosition);
        position.z = 0;

        return position;
    }


    #region Save - Load - Map
    [SerializeField]
    int LevelChose;
    [SerializeField]
    CellData[] CellTarget;
    [SerializeField]
    int[] CountTarget;
    [SerializeField]
    int LimitChose;
    [SerializeField]
    int[] StarScore = new int[3];
    [SerializeField]
    Vector3[] PositionTarget;
    public Vector3[] SetPosition
    {
        set { PositionTarget = value; }
    }

    void Save(int level)
    { 
        var data = new LevelData();

        // mode
        data.Mode = 0;
        
        data.Width = width;
        data.Height = height;

        // map
        data.CellData = new ListCellData();

        List<CellData> listcellData = new List<CellData>();

        foreach (var item in grid)
        {           
            CellController cell = grid[item];
            listcellData.Add(new CellData(cell.CellType,cell.Id,cell.CellChose));
        }      
        data.CellData.ListCell = listcellData;

        List<CellData> celltarget = new List<CellData>();
        for (int i = 0; i < CellTarget.Length; i++)
        {
            celltarget.Add(CellTarget[i]);
        }

        data.CellData.CellTarget = celltarget;

        List<int> cellnumbertarget = new List<int>();
        for (int i = 0; i < CountTarget.Length; i++)
        {
            cellnumbertarget.Add(CountTarget[i]);
        }

        data.CellData.NumberTarget = cellnumbertarget;
       
        List<int> star = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            star.Add(StarScore[i]);
        }

        data.CellData.ListStar = star;

        data.Limit = LimitChose;

        data.Save(level);
    }

    List<CellData> celllist;

    bool Load(int level)
    {
        LevelData leveldata = LevelData.Load(level);

        if(leveldata != null)
        {
            celllist = new List<CellData>();

            celllist = leveldata.CellData.ListCell;

            width = leveldata.Width;
            height = leveldata.Height;

            CellTarget = leveldata.CellData.CellTarget.ToArray();
            CountTarget = leveldata.CellData.NumberTarget.ToArray();
            StarScore = leveldata.CellData.ListStar.ToArray();
            LimitChose = leveldata.Limit;

            MainGame.Instance.ContrucCanvas(LimitChose, CountTarget, CellTarget);

            return true;
        }

        return false;     
    }

    IEnumerator LoadSceen()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("MainGame");
    }

    public void BuildDefault(int i = 0)
    {      
       BuildMapNomal();
    }

    public void SaveMapOnEditor(int level )
    {
        Save(level);
    }

    public void LoadMapOnEditor(int level)
    {
        Load(level);
        BuildMap();
    }

    public void ClearMap()
    {
        int count = parent.childCount;
        for (int i = 0; i < count; i++)
        {
            DestroyImmediate(parent.GetChild(0).gameObject);
        }

        int count2 = parentSquare.childCount;
        for (int i = 0; i < count2; i++)
        {
            DestroyImmediate(parentSquare.GetChild(0).gameObject);
        }

    }
    #endregion

    void OnGUI()
    {
        
        if (GUILayout.Button("up"))
        {
            Time.timeScale = 0.5f;

            var test = grid.GetNeighbors(new RectPoint(1, 1)).Where(c => grid[c].CellType != CellType.Special);
            foreach (var item in test)
            {
                Debug.Log(item.BasePoint);
            }
        }

        if (GUILayout.Button("Build"))
        {
            Time.timeScale = 1f;
        }
    }

}



