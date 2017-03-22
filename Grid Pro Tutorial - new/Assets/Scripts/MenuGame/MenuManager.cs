using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : SimpleSingleton<MenuManager>
{
    public GameObject LevelFrefabs;
    public Transform parent;

    public int Level;

    private void Start()
    {
       
        Vector2 position = Vector2.zero;

        int countcell = 0;

        for (int i = 0; i < 15; i++)
        {
            if (countcell == 0)
            {
                position.x = 150;
                position.y -= 150;
            }

            GameObject go =  LevelFrefabs.CreateUI(parent, position);
            go.name = "Level : " + (i + 1);
            go.GetComponent<ButtonLevelCtrl>().Contruct(i + 1);

            position.x += 250;

            if (countcell < 2)
            {
                countcell++;
            }
            else
            {
                countcell = 0;
                position.y -= 50;
            }
        }

        LoadData();
    }


    public void LoadData()
    {
        UserData _userdata = UserData.Instance;      
            // co user load level
        int count = _userdata.LevelUserData.Count;

            for (int i = 0; i < count; i++)
            {
                parent.GetChild(i + 1).GetComponent<ButtonLevelCtrl>().Contruct(i + 1, _userdata.LevelUserData[i].Star, _userdata.LevelUserData[i].Score);
            }
        

      //  _userdata.Save();
    }

    public void RunLevel()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForFixedUpdate();
        SceneManager.LoadScene("MainGame");            
    }
}
