using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BoardPiece : Piece
{
    public BoardTile currentTile { get; set; }

    public void InitializeBoardPiece(bool isTop)
    {
        SetInitialConfig(isTop);
    }

    public bool CheckPieceType(PieceTypes pieceType)
    {
        return IsPieceEqual(pieceType);
    }

    public void CheckPromotion(int boardSize)
    {
        if (IsKing()) return;

        if(IsTopMoviment() && currentTile.row == 0 || !IsTopMoviment() && currentTile.row == boardSize - 1)
            PromotePiece();
    }

    #region Mouse Click Treatement

    bool clicked = false;

    void OnMouseDown()
    {
        if(IsPieceEqual(Board.instance.currentPieceType))
            clicked = true;
    }

    void OnMouseUp()
    {
        if(clicked == true)
        {
            OnClick();
            clicked = false;
        }
    }

    void OnMouseExit()
    {
        clicked = false;
    }

    void OnClick()
    {
        if (Board.instance == null) return;

        if (Board.instance.CanCheckMoves() == false) return;

        Board.instance.BoardPieceClicked(this);
    }

    #endregion
}
