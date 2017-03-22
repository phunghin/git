using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    [SerializeField]
    GameObject PanelGo;
    RectTransform PanelTranform;

    bool _isActive = false;
    [SerializeField]
    Transform parentShow;

    private static readonly int Show = Animator.StringToHash("PanelShow");
    private static readonly int Hide = Animator.StringToHash("PanelHide");
    Animator _animator;
    [SerializeField]
    GameObject _hideMain;
    [SerializeField]
    private GameObject[] _panelShow;

    void Start()
    {
        _animator = PanelGo.GetComponent<Animator>();      
    }

    
    void HidenAll()
    {
        int count = parentShow.childCount;

        for (int i = 0; i < count; i++)
        {
            parentShow.GetChild(i).gameObject.Hide();
        }
    }

   
    public void ClickPanelButton(int id)
    {
        if (!_isActive)
        {
            HidenAll();
            _panelShow[id].Show();
            _hideMain.Show();
            _animator.Play(Show);
            _isActive = true;
        }
        else
        {
            if (_panelShow[id].activeInHierarchy)
            {
                _animator.Play(Hide);
                _hideMain.Hide();
                _isActive = false;
            }
            else
            {
                HidenAll();
                _panelShow[id].Show();
            }
        }
    }


    [Header("Settings-Panel")]
    [SerializeField]
    Image Sound;
    [SerializeField]
    Image Music;
    [SerializeField]
    Sprite[] NoticeSound;

    [SerializeField]
    Image FbLogin;
    [SerializeField]
    Image FbAvatar;

    [SerializeField]
    Sprite[] FbImage;

    [SerializeField]
    Image NoticeImage;

    [SerializeField]
    Sprite[] NoticeSprite;
    #region Setting Panel
    /// <summary>
    /// Sound Music
    /// </summary>

    bool _music = false;
    bool _sound = false;
    public void ClickSound()
    {              
        if(_sound)
        {
            Sound.sprite = NoticeSound[0];
        }
        else
        {
            Sound.sprite = NoticeSound[1];
        }

        _sound = !_sound;
    }

    public void ClickMusic()
    {
        if (_music)
        {
            Music.sprite = NoticeSound[0];
        }
        else
        {
            Music.sprite = NoticeSound[1];
            
        }

      
        _music = !_music;
      
    }

    /// <summary>
    /// FB
    /// </summary>

    bool _islogin = false; 
    public void ClickFb()
    {
        if(_islogin)
        {
            FbLogin.sprite = FbImage[0];
        }
        else
        {
            FbLogin.sprite = FbImage[1];
        }

       
        _islogin = !_islogin;
        FbAvatar.gameObject.SetActive(_islogin);


    }

    public void ClickInvite()
    {

    }

    public void ClickSend()
    {

    }

    bool _isNotice = false;
    public void ClickNotice()
    {
        if (_isNotice)
        {
            NoticeImage.sprite = NoticeSprite[0];
        }
        else
        {
            NoticeImage.sprite = NoticeSprite[1];
        }


        _isNotice = !_isNotice;
    }
    #endregion
}
