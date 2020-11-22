using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField]
    private float timeToStartCalculation = 1f;

    BoardTile[,] board;
    List<BoardPiece> pieces = new List<BoardPiece>();
    List<BoardInfoHolder> listAvaliableMoves = new List<BoardInfoHolder>(); //possible piece moves
    Dictionary<BoardPiece, List<BoardInfoHolder>> dictionaryAvaliableMoves = new Dictionary<BoardPiece, List<BoardInfoHolder>>(); //listing all piece moves in a dictionary
    BoardPiece pieceDestroyed = new BoardPiece();
    Dictionary<BoardPiece, BoardPiece> eliminated = new Dictionary<BoardPiece, BoardPiece>(); //get pieces that will be eliminated by a piece moviment

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

    bool AIKingTile(int row, int column, int rowFactor, int columnFactor, PieceTypes targetType)
    {
        return AIBoardTile(row, column, rowFactor, columnFactor, targetType, true);
    }

    bool AIBoardTile(int row, int column, int rowFactor, int columnFactor, PieceTypes targetType, bool isKing = false, BoardPiece lastPiece = null)
    {
        Debug.Log("ENTERED AIBOARDTILE");
        if (OnBoardLimits(row, column) == false) return false;

        if (board[row, column].currentPiece == null)
        {
            BoardInfoHolder newInfo = new BoardInfoHolder();
            newInfo.piece = lastPiece;
            newInfo.tile = board[row, column];

            Debug.Log("New Info piece: " + newInfo.piece);
            Debug.Log("New Info tile: " + newInfo.tile);

            listAvaliableMoves.Add(newInfo);
            pieceDestroyed = lastPiece;

            if (isKing == true)
                AIBoardTile(row + rowFactor, column + columnFactor, rowFactor, columnFactor, targetType, isKing, lastPiece);

            return true;
        }

        //Check effects when piece != null
        if (board[row, column].currentPiece != null && lastPiece == null)
        {
            if (board[row, column].currentPiece.CheckPieceType(targetType))
                return false;

            return AIBoardTile(row + rowFactor, column + columnFactor, rowFactor, columnFactor, targetType, isKing, board[row, column].currentPiece);
        }

        return false;
    }

    void ListPossibleMoves()
    {
        board = null;
        pieces.Clear();
        dictionaryAvaliableMoves.Clear();
        eliminated.Clear();

        board = Board.instance.GetBoard();
        //checking dark pieces
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if(board[i,j].currentPiece == null)
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
            listAvaliableMoves.Clear();
            //getting piece tile coordinates
            int actualRow = pieces[i].currentTile.row;
            int actualCol = pieces[i].currentTile.column;

            if (pieces[i].IsKing()) //if the actual piece is a king
            {
                //checking diagonals
                bool downRight = AIKingTile(actualRow + 1, actualCol + 1, 1, 1, PieceTypes.White);
                bool downLeft = AIKingTile(actualRow + 1, actualCol - 1, 1, -1, PieceTypes.White);
                bool upRight = AIKingTile(actualRow - 1, actualCol + 1, -1, 1, PieceTypes.White);
                bool upLeft = AIKingTile(actualRow - 1, actualCol - 1, -1, -1, PieceTypes.White);
            }
            else
            {
                if (pieces[i].IsDownMoviment())
                {
                    bool upRight = AIBoardTile(actualRow - 1, actualCol + 1, -1, 1, PieceTypes.White);
                    bool upLeft = AIBoardTile(actualRow - 1, actualCol - 1, -1, -1, PieceTypes.White);
                }
                else
                {
                    bool downRight = AIBoardTile(actualRow + 1, actualCol + 1, 1, 1, PieceTypes.White);
                    bool downLeft = AIBoardTile(actualRow + 1, actualCol - 1, 1, -1, PieceTypes.White);
                }
            }

            dictionaryAvaliableMoves.Add(pieces[i], listAvaliableMoves);
            eliminated.Add(pieces[i], pieceDestroyed);
        }
    }

    AIMoviment AIPlay()
    {
        //when it's possible to eliminate, the ai should do a eliminate play
        //when it's not, the ai should do a random piece play
        ListPossibleMoves();
        AIMoviment aiMoviment = new AIMoviment();
        
        // gerando uma lista de keys (pecas)
        List<BoardPiece> playablePieces = new List<BoardPiece>(dictionaryAvaliableMoves.Keys);

        Debug.Log("Playable pieces count " + playablePieces.Count);

        // sorteando um index para pegar uma peça da lista 
        int pieceIndex = Random.Range(0, dictionaryAvaliableMoves.Count);

        Debug.Log("Piece index " + pieceIndex);

        aiMoviment.piece = playablePieces[pieceIndex];

        Debug.Log("AI Moviment piece " + aiMoviment.piece);

        List<BoardInfoHolder> infoMove = new List<BoardInfoHolder>();
        infoMove = dictionaryAvaliableMoves[aiMoviment.piece];

        Debug.Log("Availiable moves " + infoMove.Count);

        // sorteando o index do movimento que a peça ira fazer
        int moveIndex = Random.Range(0, infoMove.Count);

        Debug.Log("Move index " + moveIndex);

        //define o alvo depois de escolher o tile possivel de jogada
        aiMoviment.target = infoMove[moveIndex].tile;

        Debug.Log("AI Moviment target " + aiMoviment.target);

        //define a peça eliminada pela jogada
        aiMoviment.eliminatedBy = eliminated[aiMoviment.piece];

        Debug.Log("AI Moviment eliminated by " + aiMoviment.eliminatedBy);

        return aiMoviment;
    }

    IEnumerator IaMovimentRoutine(float timer)
    {
        yield return new WaitForSeconds(timer);

        AIMoviment moviment = AIPlay();

        Board.instance.AIMove(moviment);
    }
}
