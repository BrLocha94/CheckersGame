using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BoardTile : Tile
{
    public BoardPiece currentPiece { get; set; }

    public void SetBoardTile(int row, int column)
    {
        SetInitialConfig(row, column);
    }

    public bool IsMovimentAllowed()
    {
        return (baseTile == TileBase.Black) ? true : false;
    }

    #region Mouse Click Treatement

    bool clicked = false;

    void OnMouseDown()
    {
        clicked = true;
    }

    void OnMouseUp()
    {
        if (clicked == true)
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
        if (Board.instance != null)
        {
            Board.instance.BoardTileClickd(this);
        }
    }

    #endregion
}
