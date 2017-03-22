using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField]
    Text _textLevel;
    [SerializeField]
    GameObject StarShow;
    [SerializeField]
    GameObject TargetCell;
    [SerializeField]
    GameObject ItemShow;

    int _level;
    void ResetData()
    {
        for (int i = 0; i < StarShow.transform.childCount; i++)
        {         
                StarShow.transform.GetChild(i).Hide();           
        }

        for (int i = 0; i < TargetCell.transform.childCount; i++)
        {
            TargetCell.transform.GetChild(i).Hide();
        }

    }
    #region Contruct PlayGame View
    public void contruct(int level, int star ,int score , Sprite[] spritetarget , int[] counttarget)
    {
        ResetData();
        _level = level;
        _textLevel.text = string.Format("Level: {0}", level);

        for (int i = 0; i < star; i++)
        {
            StarShow.transform.GetChild(i).Show();
        }

        for (int i = 0; i < counttarget.Length; i++)
        {
            GameObject current = TargetCell.transform.GetChild(i).gameObject;
            current.Show();
            current.GetComponent<Image>().sprite = spritetarget[i];
            current.transform. GetChild(0).GetComponent<Text>().text = counttarget[i].ToString();
        }
    }

    public void OnClickButton()
    {
        PlayGame.Instance.RunLevelInSeLect(_level);
    }

    public void OnClickExit()
    {
        gameObject.Hide();
        PlayGame.Instance.ClosePanal(name);
    }

    #endregion

    #region Contruct MainGame View
    [SerializeField]
    Text _score;
    public void contructMainGame(int level, int star, int score, Sprite[] spritetarget)
    {
        ResetData();
        _level = level;
        _textLevel.text = string.Format("Level: {0}", level);

        for (int i = 0; i < star; i++)
        {
            StarShow.transform.GetChild(i).Show();
        }

        for (int i = 0; i < spritetarget.Length; i++)
        {
            GameObject current = TargetCell.transform.GetChild(i).gameObject;
            current.Show();
            current.GetComponent<Image>().sprite = spritetarget[i];
            current.transform.GetChild(0).GetComponent<Text>().text = "0";
        }

        _score.text = "Score :" + score.ToString();
    }

    public void ClickExitMainGame()
    {
        //
       StartCoroutine(MainGame.Instance.LoadSceen(1));
    }

    public void ClickShareGame()
    {
        //
        StartCoroutine(MainGame.Instance.LoadSceen(1));
    }
    #endregion
}
