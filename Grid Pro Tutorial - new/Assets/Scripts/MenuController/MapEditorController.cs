using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorController : MonoBehaviour
{
  
    public void BuildLevel()
    {
        int count = transform.childCount;

        GameObject go = PlayGame.Instance.BuildLevelInMap();
        go.name = string.Format("Level:{0}", count + 1);
        go.transform.SetParent(transform);
        go.transform.position = Vector3.zero;
    }

    public void RepPlayBuildLevel()
    {
        int count = transform.childCount;

        Vector3[] position = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            position[i] = transform.GetChild(0).transform.position;
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        for (int i = 0; i < count; i++)
        {
            GameObject go = PlayGame.Instance.BuildLevelInMap();
            go.name = string.Format("Level:{0}",i + 1);
            go.transform.SetParent(transform);
            go.transform.position = position[i];
        }

        Debug.Log("fix xong");
    }


}
