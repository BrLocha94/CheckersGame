using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoSingleton<Board>
{
    [Header("Board basic dimensions")]
    [SerializeField]
    private int rows = 8;
    [SerializeField]
    private int columns = 8;

    [Header("Tile prefab used instantiate board")]
    [SerializeField]
    private BoardTile tilePrefab;
    [SerializeField]
    private GameObject tilesParent;

    [Header("Pieces prefab to instantiate")]
    [SerializeField]
    private BoardPiece piecePrefab;
    [SerializeField]
    private GameObject piecesParent;

    private List<BoardPiece> listBoardPiecesPlayer = new List<BoardPiece>();
    private List<BoardPiece> listBoardPiecesComp = new List<BoardPiece>();

    private BoardTile[,] board;

    BoardPiece currentPiece = null;
    List<BoardTile> listCurrentTiles = new List<BoardTile>();

    #region Board Creation

    void Start()
    {
        if (tilePrefab != null)
        {
            Sprite targetSprite = tilePrefab.GetComponent<SpriteRenderer>().sprite;

            if (targetSprite != null)
            {
                Vector2 offset = targetSprite.bounds.size;
                Vector2 baseSpaw = SetBaseSpaw(transform.position, offset);

                CreateBoard(offset, baseSpaw);
                SpawPieces();
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
        if (board != null)
        {
            //Initialize pieces on TOP
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (!board[i, j].IsMovimentAllowed()) continue;

                    BoardPiece piece = Instantiate(piecePrefab, piecesParent.transform);

                    piece.transform.position = board[i, j].transform.position;
                    piece.InitializeBoardPiece(false, false);

                    piece.currentTile = board[i, j];
                    board[i, j].currentPiece = piece;

                    listBoardPiecesComp.Add(piece);
                }
            }

            //Initialize pieces on DOWN
            for (int i = rows - 1; i > rows - 4; i--)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (!board[i, j].IsMovimentAllowed()) continue;

                    BoardPiece piece = Instantiate(piecePrefab, piecesParent.transform);

                    piece.transform.position = board[i, j].transform.position;
                    piece.InitializeBoardPiece(true, true);

                    piece.currentTile = board[i, j];
                    board[i, j].currentPiece = piece;

                    listBoardPiecesPlayer.Add(piece);
                }
            }
        }
    }

    #endregion

    void Update()
    {
        if(currentPiece != null && listCurrentTiles.Count > 0)
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

        //Check logic and if can move currentPiece == target
        if (target.IsKing())
        {
            //color effects
        }
        else
        {

            if (target.IsTopMoviment())
            {
                if (CheckBoardTile(actualRow - 1, actualCol + 1) || CheckBoardTile(actualRow - 1, actualCol + 1))
                    currentPiece = target;

                //color effects
                /*
                if(OnBoardLimits(actualRow - 1, actualCol + 1) && board[actualRow - 1, actualCol + 1].currentPiece == null) //check top right diagonal
                {
                    board[actualRow - 1, actualCol + 1].ApplyColorEffect(true);
                    listCurrentTiles.Add(board[actualRow - 1, actualCol + 1]);
                    currentPiece = target;
                }
                */
                
                if(OnBoardLimits(actualRow - 1, actualCol - 1) && board[actualRow - 1, actualCol - 1].currentPiece == null) //check top left diagonal
                {
                    board[actualRow - 1, actualCol - 1].ApplyColorEffect(true);
                    listCurrentTiles.Add(board[actualRow - 1, actualCol - 1]);
                    currentPiece = target;
                }

            }
            else
            {
                //color effects
                if(OnBoardLimits(actualRow + 1, actualCol - 1) && board[actualRow + 1, actualCol - 1].currentPiece == null) //check bottom right diagonal
                {
                    board[actualRow + 1, actualCol - 1].ApplyColorEffect(true);
                    listCurrentTiles.Add(board[actualRow + 1, actualCol - 1]);
                    currentPiece = target;
                }

                if(OnBoardLimits(actualRow + 1, actualCol + 1) && board[actualRow + 1, actualCol + 1].currentPiece == null) //check bottom left diagonal
                {
                    board[actualRow + 1, actualCol + 1].ApplyColorEffect(true);
                    listCurrentTiles.Add(board[actualRow + 1, actualCol + 1]);
                    currentPiece = target;
                }

            }
        }
    }

    private bool CheckBoardTile(int row, int column, PieceTypes targetType = PieceTypes.Null)
    {
        if (OnBoardLimits(row, column) == false) return false;

        if(board[row, column].currentPiece == null)
        {
            board[row, column].ApplyColorEffect(true);
            listCurrentTiles.Add(board[row, column ]);
            return true;
        }

        return false;
    }

    public void BoardTileClickd(BoardTile target)
    {
        if (currentPiece == null) return;

        if (listCurrentTiles.Count <= 0) return;

        //Check target on listCurrentTiles and aplly color strategy

        for(int i = 0; i < listCurrentTiles.Count; i++)
        {
            if(target.Equals(listCurrentTiles[i]))
            {
                listCurrentTiles[i].RemoveColorEffect();
            }
        }
    }

    private void ClearLastTileEffects()
    {
        for (int i = listCurrentTiles.Count - 1; i >= 0; i--)
        {
            listCurrentTiles[i].RemoveColorEffect();
            listCurrentTiles.RemoveAt(i);
        }

        currentPiece = null;
    }

    private bool OnBoardLimits(int row, int column)
    {
        if (row < 0 || row >= rows) return false;

        if (column < 0 || column >= columns) return false;

        return true;
    }
}
