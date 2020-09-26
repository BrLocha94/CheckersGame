using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BoardPiece : Piece
{
    public void SetBoardPiece(bool isPlayer)
    {
        SetInitialConfig(isPlayer);
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
