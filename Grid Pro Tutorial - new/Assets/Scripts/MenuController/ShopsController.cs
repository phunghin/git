using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopsController : MonoBehaviour
{
    [SerializeField]
    private Text[] _textCoin;

    [SerializeField]
    private Text[] _textBuy;

    [SerializeField]
    private int[] _AmountCoin;

    [SerializeField]
    private float[] _AmountBuy;

    void Awake()
    {
        int countCoin = _textCoin.Length;

        for (int i = 0; i < countCoin; i++)
        {
            _textCoin[i].text = _AmountCoin[i].ToString();
        }

        int countBuy = _textBuy.Length;

        for (int i = 0; i < countCoin; i++)
        {
            _textBuy[i].text = "$ "+ _AmountBuy[i].ToString();
        }
    }

    void Start()
    {
        
    }

    public void BuyCoin(int id)
    {

    }

    public void Close()
    {
        gameObject.Hide();
        PlayGame.Instance.ClosePanal(name);
    }
	
	
}
