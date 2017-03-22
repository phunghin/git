using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : SimpleSingleton<PlayGame> , ITouchEventListener
{
    UserData _userdata;
    public SpriteFactory SpriteFactory;
    public CameraController CameraController;
    void Start()
    {
        TouchManager.Instance.AddEventListener(this);
        LoadData();
        CameraController.Contruct(0, 10);
    }

    private void OnDestroy()
    {
        TouchManager.SafeRemoveEventListener(this);
    }

    #region Button - Click

    public void ClickBuyLife()
    {

    }

    public GameObject Shops;
    public void ClickBuyCoin()
    {
        TouchManager.Instance.Enabled = false;
        Shops.Show();
    }

    public void ClickFbLogin()
    {

    }


    public GameObject Settings;
    public void ClickSettings()
    {
        TouchManager.Instance.Enabled = false;
        Settings.Show();
    }

    public GameObject Message;

    public void CreateMessage()
    {

        Canvas canvas = FindObjectOfType<Canvas>();

        GameObject popup = Message.CreateUI(canvas.transform);
        MessageController script = popup.GetComponent<MessageController>();
        script.Contrust("Popup","Ok","Message!");
    }

    #endregion

    public GameObject Levelpanal;
    public GameObject LevelParent;
    public GameObject LevelPrefabs;

    public void LoadData()
    {
        _userdata = UserData.Instance;
        // co user load level
        int count = _userdata.LevelUserData.Count;

        for (int i = 0; i < count; i++)
        {
            LevelParent.transform.GetChild(i).GetComponent<LevelButtonController>().Contruct(i + 1, _userdata.LevelUserData[i].Star, _userdata.LevelUserData[i].Score);
        }
            LevelParent.transform.GetChild(count).GetComponent<LevelButtonController>().Contruct(count + 1,0,0,true);
    }

    public GameObject BuildLevelInMap()
    {
        return Instantiate(LevelPrefabs);
    }
   
    public void ClickLevelSelect(int level)
    {
        LevelData levelcurrent = LevelData.Load(level);

        Levelpanal.Show();
        if (_userdata.LevelUserData.Count >= level)
        {
            LevelUserData _usercurrent = _userdata.LevelUserData[level - 1];

            Levelpanal.GetComponent<LevelSelectController>().contruct(level, _usercurrent.Star, _usercurrent.Score, SpriteFactory.GetSpriteCell(levelcurrent.CellData.CellTarget.ToArray()), levelcurrent.CellData.NumberTarget.ToArray());
        }
        else
        {
            Levelpanal.GetComponent<LevelSelectController>().contruct(level,0,0,SpriteFactory.GetSpriteCell(levelcurrent.CellData.CellTarget.ToArray()),levelcurrent.CellData.NumberTarget.ToArray());
        }

        TouchManager.Instance.Enabled = false;
    }

    public void ClosePanal(string panal)
    {
        Debug.Log("Close " + panal);
        TouchManager.Instance.Enabled = true;
    }

    public void RunLevelInSeLect(int level)
    {
        Manager.Instance.LevelChose = level;
        SceneManager.LoadScene("MainGame");
    }


    Vector3 _TouchCurrent;
    
    public bool OnTouchPressed(Vector3 position)
    {
        _TouchCurrent = position;
        return true;
    }

    public bool OnTouchMoved(Vector3 position)
    {
        float y = position.y - _TouchCurrent.y;
        Debug.Log(position.y - _TouchCurrent.y);
        _TouchCurrent = position;
        CameraController.UpCamera(y * 0.5f);
        return true;
    }

    public void OnTouchReleased(Vector3 position)
    {
        
    }
}
