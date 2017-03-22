using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonController : MonoBehaviour , ITouchEventListener
{
    [SerializeField]
    GameObject _start;
    [SerializeField]
    Sprite _Active;   
    [SerializeField]
    GameObject _levelText;
    [SerializeField]
    bool _isActive = false;
    int _level,_score,_star;
    private float _left, _top, _right, _bottom;
    [SerializeField]
    SpriteRenderer _spriterender;

    private static readonly Vector3 NormalScale = Vector3.one;
    private static readonly Vector3 SelectedScale = new Vector3(1.1f, 1.1f, 1.0f);
    Vector3 _positionCurrent;
    private void Start()
    {
        // Get sprite renderer
        _spriterender = GetComponent<SpriteRenderer>();
        if (_spriterender == null) return;

        // Get bounds
        Bounds bounds = _spriterender.sprite.bounds;

        // Set touch bounding box
        _left = bounds.min.x;
        _bottom = bounds.min.y;
        _right = bounds.max.x;
        _top = bounds.max.y;

      
        TouchManager.Instance.AddEventListener(this);
    }

    private void OnDestroy()
    {
        TouchManager.SafeRemoveEventListener(this);
    }

    public void Contruct(int level, int star, int score, bool current = false)
    {
        _level = level;
        _star = star;
        _score = score;

        for (int i = 0; i < star; i++)
        {
            _start.transform.GetChild(i).Show();
        }

        if(!current)
        {
            if(_spriterender != null)
            {
                _spriterender.sprite = _Active;
            }
            else
            {
                _spriterender = GetComponent<SpriteRenderer>();
                _spriterender.sprite = _Active;
            }

        }
                  
        _isActive = true;
    }

    public bool OnTouchPressed(Vector3 position)
    {
        if (!_isActive) return false;

        float x = position.x - transform.position.x;
        float y = position.y - transform.position.y;

       // Check if touch inside
        if (x >= _left && x <= _right && y >= _bottom && y <= _top)
        {
           
            _positionCurrent = position;

            transform.localScale = SelectedScale;
            return true;
        }
     
        return false;
    }

    public bool OnTouchMoved(Vector3 position)
    {
        return true;
    }

    public void OnTouchReleased(Vector3 position)
    {
        if(_positionCurrent == position)
        {
            PlayGame.Instance.ClickLevelSelect(_level);
        }

        transform.localScale = NormalScale;
    }
}
