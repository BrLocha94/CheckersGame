using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BoardTile : Tile
{
    public BoardPiece currentPiece { get; set; }

    public void InitializeBoardTile(int row, int column)
    {
        SetInitialConfig(row, column);
    }

    public bool IsMovimentAllowed()
    {
        return (baseTile == TileBase.Black) ? true : false;
    }

    public void ApplyColorEffect(bool isAllowed)
    {
        ColorEffect(isAllowed);
    }

    public void RemoveColorEffect()
    {
        ApplyBaseColor();
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
        if (Board.instance == null) return;

        if (Board.instance.CanCheckMoves() == false) return;

        Board.instance.BoardTileClickd(this);
    }

    #endregion
}
