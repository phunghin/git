using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCellCtor : MonoBehaviour
{   
    public void Contruct(float delay,Color color)
    {
        var test = GetComponent<ParticleSystem>().main;

        test.startDelay = delay * 0.1f + 0.5f;
        test.startColor = color;
    
        Destroy(gameObject, delay * 0.1f + 1);
    }

}
