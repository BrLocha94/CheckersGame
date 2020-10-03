using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BoardPiece : Piece
{
    public BoardTile currentTile { get; set; }

    public void InitializeBoardPiece(bool isPlayer, bool isTop)
    {
        SetInitialConfig(isPlayer, isTop);
    }

    public void Promote()
    {
        PromotePiece();
    }

    #region Mouse Click Treatement

    bool clicked = false;

    void OnMouseDown()
    {
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
        if(Board.instance != null)
        {
            Board.instance.BoardPieceClicked(this);
        }
    }

    #endregion
}
