using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XoayController : MonoBehaviour
{
    
    public GameObject VongXoay;
    public GameObject VongNgoai;
    public Text ButtonStart;
    Animator _animaVongNgoai;

    RectTransform recttrans;
    [SerializeField]
    bool _isButton = false;
    [SerializeField]
     bool _IsStart = false;
    [SerializeField]
    bool _IsPause = false;
      
    [SerializeField]
    float radius = 360;
    [SerializeField]
    float _min, _max, _speedXoay,_speedCurrent;
    [SerializeField]
    string _startString, _stopString;

    void Start()
    {
        recttrans = VongXoay.GetComponent<RectTransform>();
        _animaVongNgoai = VongNgoai.GetComponent<Animator>();

       
    }

    void Update()
    {
        if (!_IsStart) return;

        Vector3 position = recttrans.localEulerAngles;
                                    
        position.z -= (radius * _speedCurrent * Time.deltaTime);

        if (_IsPause && position.z >= _min && position.z <= _max)
        {
           _IsStart = false;
            _animaVongNgoai.enabled = false;
            _isButton = false;
        }
               
        recttrans.localEulerAngles = position;

    }

    public void StartSpine()
    {        
        if(_isButton)
        {
            // stop
            CancelInvoke("UpdateVongXoay");
            StopS();
        }
        else
        {
            // start
            StartS();
        }
    }

    public void StartS()
    {
        Construct(1,6,10);

        InvokeRepeating("UpdateVongXoay",1.0f,0.5f);
    }

    public void UpdateVongXoay()
    {
        if(_speedCurrent >= _speedXoay)
        {
            CancelInvoke("UpdateVongXoay");
        }
        else
        {
            _speedCurrent += 2;
        }
    }

    public void StopS()
    {
        if(radius <= 50)
        {
            _IsPause = true;           
        }
        else
        {
            radius -= 10 * 2;
            Invoke("StopS", 0.2f);
        }
    }
    
    public void Construct(int item, int totalItem, float speed)
    {
        recttrans.localEulerAngles = Vector3.zero;
        // set item
        float radiusitem = 360 / totalItem;

        _min = radiusitem * item + 1f;
        _max=  radiusitem * item + 4f;

        radius = 360;
        _speedXoay = speed;
        _speedCurrent = 3;

        ButtonStart.text = _stopString;
        _animaVongNgoai.enabled = true;
        _IsStart = true;
        _IsPause = false;
        _isButton = true;
    }
}
