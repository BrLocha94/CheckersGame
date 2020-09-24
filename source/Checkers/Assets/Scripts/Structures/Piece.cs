using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Piece : SpriteBase
{
    [Header("General colors to use on this board")]
    [SerializeField]
    private Color colorBaseWhite;
    [SerializeField]
    private Color colorBaseBlack;

    public PieceTypes pieceType { get; private set; }

    protected void SetInitialConfig(bool isPlayer)
    {
        if (isPlayer == true)
            pieceType = PieceTypes.White;
        else
            pieceType = PieceTypes.Black;

        spriteRenderer.color = GetBaseColor();
    }

    private Color GetBaseColor()
    {
        return (pieceType == PieceTypes.White) ? colorBaseWhite : colorBaseBlack;
    }
}

public enum PieceTypes
{
    Null,
    White,
    WhiteKing,
    Black,
    BlackKing
}