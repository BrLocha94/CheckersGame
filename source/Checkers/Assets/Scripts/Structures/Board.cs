﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoSingleton<Board>
{
    [Header("Board basic dimensions")]
    [SerializeField]
    [Range(4, 20)]
    private int rows = 8;
    [SerializeField]
    [Range(4, 20)]
    private int columns = 8;

    [Space]

    [Header("Tile prefab used instantiate board")]
    [SerializeField]
    private BoardTile tilePrefab = null;
    [SerializeField]
    private SpriteRenderer framePrefab = null;
    [SerializeField]
    private GameObject tilesParent = null;

    [Space]

    [Header("Pieces prefab to instantiate")]
    [SerializeField]
    private BoardPiece piecePrefab = null;
    [SerializeField]
    private GameObject piecesParent = null;

    private List<BoardPiece> listBoardPieces = new List<BoardPiece>();

    private BoardTile[,] board;

    BoardPiece currentPiece = null;
    List<BoardInfoHolder> listAvaliableMoves = new List<BoardInfoHolder>();

    #region Delegates Region

    public PieceTypes currentPieceType { get; private set; }
    public GameStates currentGameState { get; private set; }

    void OnGameStateChange(GameStates newGameState)
    {
        currentGameState = newGameState;

        if (newGameState == GameStates.Initializing)
            InitializeBoard();
        else if (newGameState == GameStates.SpawningPieces)
            SpawPieces();
    }

    private void OnEnable()
    {
        GameController.onGameStateChange += OnGameStateChange;
    }

    private void OnDisable()
    {
        GameController.onGameStateChange -= OnGameStateChange;
    }

    #endregion

    #region Board Creation

    private void InitializeBoard()
    {
        if (tilePrefab != null)
        {
            Sprite targetSprite = tilePrefab.GetComponent<SpriteRenderer>().sprite;

            if (targetSprite != null)
            {
                Vector2 offset = targetSprite.bounds.size;
                Vector2 baseSpaw = SetBaseSpaw(transform.position, offset);

                CreateBoard(offset, baseSpaw);
            }
            else
                Debug.Log("Class Board, method Start: Cant acess tile prefab sprite");
        }
        else
            Debug.Log("Class Board, method Start: Tile prefab is null");
    }

    private void CreateBoard(Vector2 offset, Vector2 baseSpaw)
    {
        board = new BoardTile[rows, columns];

        //Create frame
        if (framePrefab != null)
        {   
            SpriteRenderer frame = Instantiate(framePrefab, 
                                            new Vector3(transform.position.x,
                                                        transform.position.y,
                                                        0f),
                                            framePrefab.transform.rotation);

            frame.transform.SetParent(tilesParent.transform);

            frame.transform.localScale = new Vector2(columns + 0.3f, rows + 0.3f);
        }

        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                BoardTile tile = Instantiate(tilePrefab,
                                        new Vector3(baseSpaw.x + (offset.x * j),
                                                    baseSpaw.y + (offset.y * i),
                                                    0f),
                                        tilePrefab.transform.rotation);

                tile.transform.SetParent(tilesParent.transform);

                board[i, j] = tile;
                board[i, j].InitializeBoardTile(i, j);
            }
        }
    }

    private Vector2 SetBaseSpaw(Vector2 baseSpaw, Vector2 offset)
    {
        if (columns > 1)
        {
            float baseX = baseSpaw.x;
            baseX = baseX + (-1 * (offset.x * columns) / 2) + (offset.x / 2);
            baseSpaw.x = baseX;
        }
        if (rows > 1)
        {
            float baseY = baseSpaw.y;
            baseY = baseY + (-1 * (offset.y * rows) / 2) + (offset.y / 2);
            baseSpaw.y = baseY;
        }

        return baseSpaw;
    }

    private void SpawPieces()
    {
        ClearPiecesList();
        StartCoroutine(SpawPiecesRoutine());
    }

    private void ClearPiecesList()
    {
        if (listBoardPieces.Count == 0) return;

        for(int i = listBoardPieces.Count - 1; i >= 0; i++)
        {
            BoardPiece piece = listBoardPieces[i];
            listBoardPieces.RemoveAt(i);
            Destroy(piece.gameObject);
        }
    }

    IEnumerator SpawPiecesRoutine()
    {
        if (board != null)
        {
            //Initialize pieces on TOP
            //Com Pieces
            for (int i = 0; i < (rows/2 - 1); i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (!board[i, j].IsMovimentAllowed()) continue;

                    BoardPiece piece = Instantiate(piecePrefab, piecesParent.transform);

                    piece.transform.position = board[i, j].transform.position;
                    piece.InitializeBoardPiece(false);

                    piece.currentTile = board[i, j];
                    board[i, j].currentPiece = piece;

                    listBoardPieces.Add(piece);
                }
            }

            yield return new WaitForSeconds(0.5f);

            //Initialize pieces on DOWN
            //Player pieces
            for (int i = rows - 1; i > rows/2; i--)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (!board[i, j].IsMovimentAllowed()) continue;

                    BoardPiece piece = Instantiate(piecePrefab, piecesParent.transform);

                    piece.transform.position = board[i, j].transform.position;
                    piece.InitializeBoardPiece(true);

                    piece.currentTile = board[i, j];
                    board[i, j].currentPiece = piece;

                    listBoardPieces.Add(piece);
                }
            }

            currentPieceType = PieceTypes.White;
            VisualController.instance.UpdateCurrentPlayer(currentPieceType);
        }

        yield return null;
    }

    #endregion

    void Update()
    {
        if(currentPiece != null && listAvaliableMoves.Count > 0)
        {
            if (Input.GetMouseButton(1))
            {
                ClearLastTileEffects();
            }
        }    
    }

    public void BoardPieceClicked(BoardPiece target)
    {
        if (currentPiece != null) return;

        int actualRow = target.currentTile.row;
        int actualCol = target.currentTile.column;

        if (target.IsKing())
        {
            bool downRight = CheckKingTile(actualRow + 1, actualCol + 1,  1,  1, target.pieceType);
            bool downLeft  = CheckKingTile(actualRow + 1, actualCol - 1,  1, -1, target.pieceType);
            bool upRight   = CheckKingTile(actualRow - 1, actualCol + 1, -1,  1, target.pieceType);
            bool upLeft    = CheckKingTile(actualRow - 1, actualCol - 1, -1, -1, target.pieceType);

            if (downRight || downLeft || upRight || upLeft)
                currentPiece = target;
        }
        else
        {
            if (target.IsDownMoviment())
            {
                bool upRight = CheckBoardTile(actualRow - 1, actualCol + 1, -1, 1, target.pieceType);
                bool upLeft = CheckBoardTile(actualRow - 1, actualCol - 1, -1, -1, target.pieceType);
                if (upRight || upLeft)
                    currentPiece = target;
            }
            else
            {
                bool downRight = CheckBoardTile(actualRow + 1, actualCol + 1, 1, 1, target.pieceType);
                bool downLeft = CheckBoardTile(actualRow + 1, actualCol - 1, 1, -1, target.pieceType);
                if (downRight || downLeft)
                    currentPiece = target;
            }
        }
    }

    private bool CheckKingTile(int row, int column, int rowFactor, int columnFactor, PieceTypes targetType)
    {
        return CheckBoardTile(row, column, rowFactor, columnFactor, targetType, true);
    }

    private bool CheckBoardTile(int row, int column, int rowFactor, int columnFactor, PieceTypes targetType, bool isKing = false, BoardPiece lastPiece = null)
    {
        if (OnBoardLimits(row, column) == false) return false;

        if(board[row, column].currentPiece == null)
        {
            if(lastPiece != null)
                board[row, column].ApplyColorEffect(false);
            else
                board[row, column].ApplyColorEffect(true);

            BoardInfoHolder newInfo = new BoardInfoHolder();
            newInfo.piece = lastPiece;
            newInfo.tile = board[row, column];

            listAvaliableMoves.Add(newInfo);

            if(isKing == true)
                CheckBoardTile(row + rowFactor, column + columnFactor, rowFactor, columnFactor, targetType, isKing, lastPiece);

            return true;
        }

        //Check effects when piece != null
        if(board[row, column].currentPiece != null && lastPiece == null)
        {
            if (board[row, column].currentPiece.CheckPieceType(targetType))
                return false;

            return CheckBoardTile(row + rowFactor, column + columnFactor, rowFactor, columnFactor, targetType, isKing, board[row, column].currentPiece);
        }

        return false;
    }

    public void BoardTileClickd(BoardTile target)
    {
        if (currentPiece == null) return;

        if (listAvaliableMoves.Count <= 0) return;

        if (TileWasNotChecked(target) == true) return;

        BoardPiece removedPiece = GetRemovedPiece(target);

        MovePiece(currentPiece, target);

        ClearLastTileEffects();

        if (removedPiece != null)
        {
            listBoardPieces.Remove(removedPiece);
            Destroy(removedPiece.gameObject);

            //Check if game is over
            if (CheckGameOver() == true)
            {
                if (listBoardPieces[0].CheckPieceType(PieceTypes.White))
                    GameController.instance.ChangeGameState(GameStates.GameClear);
                else
                    GameController.instance.ChangeGameState(GameStates.GameOver);
            }
            else
                
            //else next turn
            NextTurn();
        }
        else
            NextTurn();
    }

    public void AIMove(AIMoviment moviment)
    {
        if(moviment == null)
        {
            Debug.Log("moviment from AI is null...GOING TO NEXT TURN");
            NextTurn();
            return;
        }

        if (moviment.piece == null) Debug.Log("AI moviment piece is null");

        if (moviment.target == null) Debug.Log("AI moviment tile is null");

        //if (moviment.eliminatedBy == null) Debug.Log("AI moviment emilimated piece is null");

        MovePiece(moviment.piece, moviment.target);

        if(moviment.eliminatedBy != null)
        {
            listBoardPieces.Remove(moviment.eliminatedBy);
            Destroy(moviment.eliminatedBy.gameObject);

            if (CheckGameOver() == true)
            {
                if (listBoardPieces[0].CheckPieceType(PieceTypes.White))
                    GameController.instance.ChangeGameState(GameStates.GameClear);
                else
                    GameController.instance.ChangeGameState(GameStates.GameOver);
            }
            else
                NextTurn();
        }
        else
            NextTurn();
    }

    private bool CheckGameOver()
    {
        bool white = false;
        bool black = false;

        for(int i = 0; i < listBoardPieces.Count; i++)
        {
            if (listBoardPieces[i].CheckPieceType(PieceTypes.White))
                white = true;
            else if (listBoardPieces[i].CheckPieceType(PieceTypes.Black))
                black = true;

            if (white && black) return false;
        }

        return true;
    }

    private void MovePiece(BoardPiece currentPiece, BoardTile targetTile)
    {
        BoardTile currentTile = currentPiece.currentTile;

        currentTile.currentPiece = null;
        targetTile.currentPiece = currentPiece;

        currentPiece.currentTile = targetTile;

        currentPiece.MoveTo(targetTile.transform.position);
        //currentPiece.transform.position = targetTile.transform.position;

        currentPiece.CheckPromotion(rows);
    }

    private void ClearLastTileEffects()
    {
        for (int i = listAvaliableMoves.Count - 1; i >= 0; i--)
        {
            listAvaliableMoves[i].tile.RemoveColorEffect();
            listAvaliableMoves.RemoveAt(i);
        }

        currentPiece = null;
    }

    private bool OnBoardLimits(int row, int column)
    {
        if (row < 0 || row >= rows) return false;

        if (column < 0 || column >= columns) return false;

        return true;
    }

    private bool TileWasNotChecked(BoardTile target)
    {
        if (target == null) return false;

        if (listAvaliableMoves.Count <= 0) return true;

        for(int i = 0; i < listAvaliableMoves.Count; i++)
        {
            if (listAvaliableMoves[i].tile == target) return false;
        }

        return true;
    }

    private BoardPiece GetRemovedPiece(BoardTile target)
    {
        if (target == null) return null;

        if (listAvaliableMoves.Count <= 0) return null;

        for (int i = 0; i < listAvaliableMoves.Count; i++)
        {
            if (listAvaliableMoves[i].tile == target)
                return listAvaliableMoves[i].piece;
        }

        return null;
    }

    private int GetFactor(int current, int target)
    {
        if (current > target) return -1;

        if (current < target) return 1;

        return 0;
    }

    public BoardTile[,] GetBoard()
    {
        return board;
    }
    
    private void NextTurn()
    {
        if (currentPieceType == PieceTypes.White)
            currentPieceType = PieceTypes.Black;
        else if (currentPieceType == PieceTypes.Black)
            currentPieceType = PieceTypes.White;

        VisualController.instance.UpdateCurrentPlayer(currentPieceType);


        if (GameController.instance.againstAI == true && currentPieceType == PieceTypes.Black)
            GameController.instance.ChangeGameState(GameStates.AIMoviment);
        else
            GameController.instance.ChangeGameState(GameStates.Running);
    }

    public bool CanCheckMoves()
    {
        return currentGameState == GameStates.Running ? true : false;
    }
}
