using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    [SerializeField]
    private Text TextTitle;

    [SerializeField]
    private Text TextMessage;

    [SerializeField]
    private Text TextButton;

    
    public void Contrust(string title, string buttonOK, string message)
    {
        TextTitle.text = title;
        TextButton.text = buttonOK;
        TextMessage.text = message;
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    public void Oke()
    {
        Destroy(gameObject);
    }
}
