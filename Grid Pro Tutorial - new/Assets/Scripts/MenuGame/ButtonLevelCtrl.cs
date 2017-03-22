using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevelCtrl : MonoBehaviour
{
    public GameObject textlevel;
    public GameObject textStar;
    public GameObject textScore;
    int levelCurrent;

    private void Start()
    {
       
    }


    public void Contruct(int level , int star = 0, int score = 0)
    {
        levelCurrent = level;
        textlevel.GetComponent<Text>().text =  "Level : " + level;
        textStar.GetComponent<Text>().text =    "Star : " + star;
        textScore.GetComponent<Text>().text = "Score : " + score;
    }

    public void MoveLevel()
    {
        Manager.Instance.LevelChose = levelCurrent;

        MenuManager.Instance.RunLevel();
    }
}
