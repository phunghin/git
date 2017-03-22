using System;
using Gamelogic.Grids;
using Gamelogic.Grids.Examples;
using UnityEngine;

public class CellController : SpriteCell
{
    [SerializeField]
    CellType _celltype;
    public CellType CellType
    {
        get { return _celltype; }
        set { _celltype = value; }
    }

    [SerializeField]
    int _cellChose;
    public int CellChose
    {
        get { return _cellChose; }
        set { _cellChose = value; }
    }


    [SerializeField]
    int _id;
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    [SerializeField]
    Sprite _sprite;
    public Sprite SpriteCell
    {
        get { return _sprite; }
        set { _sprite = value;

            if(_spriterender != null)
            {
                _spriterender.sprite = _sprite;
            }
            else
            {
                transform.FindChild("Sprite").GetComponent<SpriteRenderer>().sprite = _sprite;
            }
            
        }
    }

    [SerializeField]
    bool _isFinished;
    public bool IsMove
    {
        get { return _isFinished; }
        set { _isFinished = value; }       
    }

    float _Angle;
    public float Angle
    {
        get { return _Angle; }
        set { _Angle = value;
            AddAngle(_Angle);
        }
    }

    bool _isTouchActive;
    public bool IsTouchActive
    {
        set
        {
            _isTouchActive = value;
            Color = _isTouchActive ? Color.gray : Color.white;
        }
    }
    


    public Vector3 positionMove;
    float _timedelay = 0.1f;

    public void SetOderSprite()
    {
        _spriterender.sortingOrder = 99;
    }

    public void DestroyCell(float time, bool destroy = false)
    {   
      if(destroy)
        {
            IsTouchActive = false;
            MoveCell(transform.position,positionMove,1 + time* _timedelay, 0,EaseType.BackIn);
            SetOderSprite();
            Invoke("DestroyGameObject",1+ time * _timedelay);
        } 
      else
        {
            Invoke("DestroyGameObject",time * _timedelay);
        }      
    }
    
    void DestroyGameObject()
    {
        MainGame.Instance.UpdateCellTarget(_id,_celltype);
        Destroy(gameObject);
    }
   
    private LerpFloatHelper _yHelper = new LerpFloatHelper();
    private LerpFloatHelper _xHelper = new LerpFloatHelper();
   
    public void MoveCell(Vector3 start, Vector3 end ,float speed, float time = 0f, EaseType easyType = EaseType.ElasticOut)
    {
        _xHelper.Construct(start.x, end.x, speed, Ease.FromType(easyType));      
        _yHelper.Construct(start.y, end.y, speed, Ease.FromType(easyType));

        Invoke("Play", time);     
    }

    void Play()
    {
        _xHelper.Play();
        _yHelper.Play();
        _isFinished = false;
    }

    void Update()
    {
        if (_isFinished)
        {
            return;
        }

        Vector3 position = transform.position;

        if (!_xHelper.Finished)
        {
            position.x = _xHelper.Update(Time.deltaTime);
        }

        if (!_yHelper.Finished)
        {
            position.y = _yHelper.Update(Time.deltaTime);
        }

        transform.position = position;

        _isFinished = _xHelper.Finished && _yHelper.Finished;
       
    }

    SpriteRenderer _spriterender;
    private void Awake()
    {
        _spriterender = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();             
    }
}

public enum CellType
{
    None,
    Nomal,
    Special,
    Block,
    Animal,
}

