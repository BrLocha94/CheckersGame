using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : SpriteBase
{
    [Header("General colors to use on this board")]
    [SerializeField]
    private Color colorBaseWhite;
    [SerializeField]
    private Color colorBaseBlack;
    [SerializeField]
    private Color colorAllowed;
    [SerializeField]
    private Color colorPreferencial;

    public int row { get; private set; }
    public int column { get; private set; }

    public TileBase baseTile { get; private set; }

    protected void SetInitialConfig(int row, int column)
    {
        this.row = row;
        this.column = column;

        if(row % 2 == 0)
        {
            if (column % 2 == 0)
                baseTile = TileBase.White;
            else
                baseTile = TileBase.Black;
        }
        else
        {
            if (column % 2 == 0)
                baseTile = TileBase.Black;
            else
                baseTile = TileBase.White;
        }

        spriteRenderer.color = GetBaseColor();
    }

    private Color GetBaseColor()
    {
        return (baseTile == TileBase.White) ? colorBaseWhite : colorBaseBlack;
    }
}

public enum TileBase
{
    Null,
    Black,
    White
}