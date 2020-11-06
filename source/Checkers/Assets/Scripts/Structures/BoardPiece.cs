using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BoardPiece : Piece
{
    [SerializeField]
    private float timeAnimationMinimum = 0.15f;
    [SerializeField]
    private float timeAnimationMaximum = 0.5f;

    public BoardTile currentTile { get; set; }

    public void InitializeBoardPiece(bool isOnBoardTop)
    {
        SetInitialConfig(isOnBoardTop);
    }

    public bool CheckPieceType(PieceTypes pieceType)
    {
        return IsPieceEqual(pieceType);
    }

    public void CheckPromotion(int boardSize)
    {
        if (IsKing()) return;

        if(IsDownMoviment() && currentTile.row == 0 || !IsDownMoviment() && currentTile.row == boardSize - 1)
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

    protected override void InitializeOnAwake()
    {
        base.InitializeOnAwake();

        float timeAnimation = Random.Range(timeAnimationMinimum, timeAnimationMaximum);

        iTween.ScaleTo(gameObject, iTween.Hash(
            "time", timeAnimation,
            "scale", Vector3.one,
            "easetype", iTween.EaseType.easeInOutElastic));
    }
}
