using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField]
    private float timeToStartCalculation = 1f;

    BoardTile[,] board;
    List<BoardPiece> pieces = new List<BoardPiece>();
    //List<BoardInfoHolder> listAvaliableMoves = new List<BoardInfoHolder>(); //possible piece moves
    List<AIMoviment> listAvaliableMoves = new List<AIMoviment>();
    //Dictionary<BoardPiece, List<BoardInfoHolder>> dictionaryAvaliableMoves = new Dictionary<BoardPiece, List<BoardInfoHolder>>(); //listing all piece moves in a dictionary
    BoardPiece pieceDestroyed = new BoardPiece();

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
        Debug.Log("ENTERED AIBOARDTILE");
        if (OnBoardLimits(row, column) == false) return false;

        if (board[row, column].currentPiece == null)
        {
            Debug.Log("CURRENT PIECE == NULL");


            AIMoviment newMove = new AIMoviment();
            newMove.piece = actualPiece;
            newMove.target = board[row, column];
            newMove.eliminatedBy = lastPiece;

            Debug.Log("-----------AVALIABLE MOVES: " + listAvaliableMoves.Count);


            listAvaliableMoves.Add(newMove);

            if (isKing == true)
                AIBoardTile(actualPiece, row + rowFactor, column + columnFactor, rowFactor, columnFactor, targetType, isKing, lastPiece);

            return true;
        }

        //Check effects when piece != null
        if (board[row, column].currentPiece != null && lastPiece == null)
        {
            Debug.Log("CURRENT PIECE != NULL");
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
        //dictionaryAvaliableMoves.Clear();

        board = Board.instance.GetBoard();
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
            listAvaliableMoves = new List<AIMoviment>();
            //getting piece tile coordinates
            int actualRow = pieces[i].currentTile.row;
            int actualCol = pieces[i].currentTile.column;

            if (pieces[i].IsKing()) //if the actual piece is a king
            {
                //checking diagonals
                bool downRight = AIKingTile(pieces[i], actualRow + 1, actualCol + 1, 1, 1, PieceTypes.White);
                bool downLeft = AIKingTile(pieces[i], actualRow + 1, actualCol - 1, 1, -1, PieceTypes.White);
                bool upRight = AIKingTile(pieces[i], actualRow - 1, actualCol + 1, -1, 1, PieceTypes.White);
                bool upLeft = AIKingTile(pieces[i], actualRow - 1, actualCol - 1, -1, -1, PieceTypes.White);
            }
            else
            {
                if (pieces[i].IsDownMoviment())
                {
                    bool upRight = AIBoardTile(pieces[i], actualRow - 1, actualCol + 1, -1, 1, PieceTypes.White);
                    bool upLeft = AIBoardTile(pieces[i], actualRow - 1, actualCol - 1, -1, -1, PieceTypes.White);
                }
                else
                {
                    bool downRight = AIBoardTile(pieces[i], actualRow + 1, actualCol + 1, 1, 1, PieceTypes.White);
                    bool downLeft = AIBoardTile(pieces[i], actualRow + 1, actualCol - 1, 1, -1, PieceTypes.White);
                }
            }

            //dictionaryAvaliableMoves.Add(pieces[i], listAvaliableMoves);
        }
    }

    AIMoviment AIPlay()
    {
        //when it's possible to eliminate, the ai should do a eliminate play
        //when it's not, the ai should do a random piece play
        ListPossibleMoves();
        int rand = Random.Range(0, listAvaliableMoves.Count);
        return listAvaliableMoves[rand];
    }

    IEnumerator IaMovimentRoutine(float timer)
    {
        yield return new WaitForSeconds(timer);

        AIMoviment moviment = AIPlay();

        Board.instance.AIMove(moviment);
    }
}