using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFactory : ScriptableObject
{
    public Sprite[] SpriteBg;	   
    public Sprite[] NoneBg;  
    public Sprite[] SpecialBg;
    public Sprite[] BlockBg;
    public Sprite[] AnimalBg;

    public Sprite AddSpriteCell1(CellType type, int id)
    {
        switch (type)
        {
            case CellType.None: return null;
            case CellType.Nomal: return SpriteBg[id];
            case CellType.Special: return SpecialBg[id];
            case CellType.Block: return BlockBg[id];
            case CellType.Animal: return AnimalBg[0];
            default: return null;
        }
    }

    public Sprite[] GetSpriteCell(CellData[] celldata)
    {
        Sprite[] sprite = new Sprite[celldata.Length];

        for (int i = 0; i < sprite.Length; i++)
        {
            sprite[i] = AddSpriteCell(celldata[i].CellType,celldata[i].Id);
        }

        return sprite;
    }

    public Sprite AddSpriteCell(CellType type, int id, int chose = 0)
    {
        switch (type)
        {
            case CellType.None: return null;
            case CellType.Nomal: return SpriteBg[id];
            case CellType.Special:

                if(id == 0)
                {
                    if(chose == 0)
                    {
                        return SpecialBg[0];
                    }
                    else
                    {
                        return SpecialBg[1];
                    }
                }
                else
                {
                    return SpecialBg[id + 1];
                }

            case CellType.Block: return BlockBg[id];
            case CellType.Animal: return AnimalBg[0];
            default: return null;
        }
    }

    public GameObject SquareFrefabs;
    public Sprite[] bgSqiare;
    public GameObject AddSquare(Transform parent,Vector3 position, int id , int background, int rotato = 0)
    {
        GameObject go = Instantiate(SquareFrefabs);
        go.transform.SetParent(parent);
        go.transform.position = position;

        go.transform.GetComponent<SpriteRenderer>().sprite = bgSqiare[id];
        go.transform.FindChild("Sprite").GetComponent<SpriteRenderer>().sprite = bgSqiare[background];
        go.transform.FindChild("Sprite").transform.TranslateZ(rotato);

        return go;
    }

   
    public Color[] ColorBreak;

    public Color AddColor(int id)
    {
        return ColorBreak[id];
    }

    
}


