using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField]
    private float timeToStartCalculation = 1f;

    BoardTile[,] board;
    List<BoardPiece> pieces = new List<BoardPiece>();
    List<AIMoviment> listAvaliableMoves = new List<AIMoviment>();

    #region AIRegion

    public GameStates currentGameState { get; private set; }

    void OnGameStateChange(GameStates newGameState)
    {
        currentGameState = newGameState;

        if (newGameState == GameStates.AIMoviment)
            StartCoroutine(IaMovimentRoutine(timeToStartCalculation));
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

    bool OnBoardLimits(int row, int column)
    {
        if (row < 0 || row >= board.GetLength(0)) return false;

        if (column < 0 || column >= board.GetLength(1)) return false;

        return true;
    }

    bool AIKingTile(BoardPiece actualPiece, int row, int column, int rowFactor, int columnFactor, PieceTypes targetType)
    {
        return AIBoardTile(actualPiece, row, column, rowFactor, columnFactor, targetType, true);
    }

    bool AIBoardTile(BoardPiece actualPiece, int row, int column, int rowFactor, int columnFactor, PieceTypes targetType, bool isKing = false, BoardPiece lastPiece = null)
    {
        if (OnBoardLimits(row, column) == false) return false;

        if (board[row, column].currentPiece == null)
        {
            AIMoviment newMove = new AIMoviment();
            newMove.piece = actualPiece;
            newMove.target = board[row, column];
            newMove.eliminatedBy = lastPiece;

            listAvaliableMoves.Add(newMove);

            if (isKing == true)
                AIBoardTile(actualPiece, row + rowFactor, column + columnFactor, rowFactor, columnFactor, targetType, isKing, lastPiece);

            return true;
        }

        //Check effects when piece != null
        if (board[row, column].currentPiece != null && lastPiece == null)
        {
            if (board[row, column].currentPiece.CheckPieceType(targetType))
                return false;

            return AIBoardTile(actualPiece, row + rowFactor, column + columnFactor, rowFactor, columnFactor, targetType, isKing, board[row, column].currentPiece);
        }

        return false;
    }

    void ListPossibleMoves()
    {
        board = null;
        pieces.Clear();

        board = Board.instance.GetBoard();
        listAvaliableMoves = new List<AIMoviment>();

        //checking dark pieces
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j].currentPiece == null)
                    continue;

                if (board[i, j].currentPiece.CheckPieceType(PieceTypes.Black))
                {
                    pieces.Add(board[i, j].currentPiece);
                }
            }
        }
        //list possible piece moves
        for (int i = 0; i < pieces.Count; i++)
        {
            //getting piece tile coordinates
            int actualRow = pieces[i].currentTile.row;
            int actualCol = pieces[i].currentTile.column;

            if (pieces[i].IsKing()) //if the actual piece is a king
            {
                //checking diagonals
                bool downRight = AIKingTile(pieces[i], actualRow + 1, actualCol + 1, 1, 1, PieceTypes.Black);
                bool downLeft = AIKingTile(pieces[i], actualRow + 1, actualCol - 1, 1, -1, PieceTypes.Black);
                bool upRight = AIKingTile(pieces[i], actualRow - 1, actualCol + 1, -1, 1, PieceTypes.Black);
                bool upLeft = AIKingTile(pieces[i], actualRow - 1, actualCol - 1, -1, -1, PieceTypes.Black);
            }
            else
            {
                if (pieces[i].IsDownMoviment())
                {
                    bool upRight = AIBoardTile(pieces[i], actualRow - 1, actualCol + 1, -1, 1, PieceTypes.Black);
                    bool upLeft = AIBoardTile(pieces[i], actualRow - 1, actualCol - 1, -1, -1, PieceTypes.Black);
                }
                else
                {
                    bool downRight = AIBoardTile(pieces[i], actualRow + 1, actualCol + 1, 1, 1, PieceTypes.Black);
                    bool downLeft = AIBoardTile(pieces[i], actualRow + 1, actualCol - 1, 1, -1, PieceTypes.Black);
                }
            }
        }
    }

    AIMoviment AIPlay()
    {
        //do a random ai play
        ListPossibleMoves();

        List<AIMoviment> priorityMoves = new List<AIMoviment>();

        for(int i = 0; i < listAvaliableMoves.Count; i++)
        {
            if(listAvaliableMoves[i].eliminatedBy != null)
            {
                priorityMoves.Add(listAvaliableMoves[i]);
            }
        }

        if(priorityMoves.Count > 0)
        {
            int rand = Random.Range(0, priorityMoves.Count);
            return priorityMoves[rand];
        }
        else
        {
            int rand = Random.Range(0, listAvaliableMoves.Count);
            return listAvaliableMoves[rand];
        }
    }

    IEnumerator IaMovimentRoutine(float timer)
    {
        yield return new WaitForSeconds(timer);

        AIMoviment moviment = AIPlay();

        Board.instance.AIMove(moviment);
    }
}