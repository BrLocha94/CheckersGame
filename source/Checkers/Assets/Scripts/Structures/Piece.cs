﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : SpriteBase
{
    [Header("General colors to use on this board")]
    [SerializeField]
    private Color colorBaseWhite;
    [SerializeField]
    private Color colorBaseBlack;

    public PieceTypes pieceType { get; private set; }
    public PieceInitialDirection pieceInitialDirection { get; private set; }

    protected void SetInitialConfig(bool isPlayer, bool isTop)
    {
        if (isPlayer == true)
            pieceType = PieceTypes.White;
        else
            pieceType = PieceTypes.Black;

        if (isTop == true)
            pieceInitialDirection = PieceInitialDirection.Top;
        else
            pieceInitialDirection = PieceInitialDirection.Bottom;

        spriteRenderer.color = GetBaseColor();
    }

    private Color GetBaseColor()
    {
        return (pieceType == PieceTypes.White) ? colorBaseWhite : colorBaseBlack;
    }

    public bool IsKing()
    {
        return (pieceType == PieceTypes.WhiteKing || pieceType == PieceTypes.BlackKing);
    }

    public bool IsTopMoviment()
    {
        return pieceInitialDirection == PieceInitialDirection.Top;
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

public enum PieceInitialDirection
{
    Null,
    Bottom,
    Top
}