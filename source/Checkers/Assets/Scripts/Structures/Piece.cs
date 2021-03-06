﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : SpriteBase
{
    [Header("Crown Sprite to activate/deactivate")]
    [SerializeField]
    private GameObject crownSprite;

    [Header("General colors to use on this board")]
    [SerializeField]
    private Color colorBaseWhite;
    [SerializeField]
    private Color colorBaseBlack;

    public PieceTypes pieceType { get; private set; }
    public PieceInitialDirection pieceInitialDirection { get; private set; }

    protected void SetInitialConfig(bool isOnBoardTop)
    {
        if (isOnBoardTop == true)
        {
            pieceType = PieceTypes.Black;
            pieceInitialDirection = PieceInitialDirection.Down;
        }
        else
        {
            pieceType = PieceTypes.White;
            pieceInitialDirection = PieceInitialDirection.Up;
        }

        spriteRenderer.color = GetBaseColor();
    }

    private Color GetBaseColor()
    {
        return (pieceType == PieceTypes.White) ? colorBaseWhite : colorBaseBlack;
    }

    protected bool PromotePiece()
    {
        if(pieceType == PieceTypes.White)
        {
            pieceType = PieceTypes.WhiteKing;
            crownSprite.SetActive(true);
            return true;
        }

        if(pieceType == PieceTypes.Black)
        {
            pieceType = PieceTypes.BlackKing;
            crownSprite.SetActive(true);
            return true;
        }

        return false;
    }

    protected bool IsPieceEqual(PieceTypes pieceType)
    {
        if ((pieceType == PieceTypes.White || pieceType == PieceTypes.WhiteKing) &&
           (this.pieceType == PieceTypes.White || this.pieceType == PieceTypes.WhiteKing)) return true;

        if ((pieceType == PieceTypes.Black || pieceType == PieceTypes.BlackKing) &&
           (this.pieceType == PieceTypes.Black || this.pieceType == PieceTypes.BlackKing)) return true;

        return false;
    }

    public bool IsKing()
    {
        return (pieceType == PieceTypes.WhiteKing || pieceType == PieceTypes.BlackKing);
    }

    public bool IsDownMoviment()
    {
        return pieceInitialDirection == PieceInitialDirection.Down;
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
    Up,
    Down
}