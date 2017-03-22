using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    [SerializeField]
    float _min, _max;
    public void Contruct(float min, float max)
    {
        _min = min;
        _max = max;

    }

    public void UpCamera(float deltaY)
    {

        Vector3 position = transform.position;
        
        if (position.y > _min && deltaY > 0)
        {
            position.y -= deltaY;            
        }
        else
        {
            if(position.y < _max && deltaY < 0)
            {
                position.y -= deltaY;
            }
        }

        transform.position = position;
    }
}
