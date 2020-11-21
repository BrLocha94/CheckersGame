using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Play
{
    public BoardPiece piece { get; set; }
    public BoardTile target { get; set; }
    public BoardPiece eliminatedBy { get; set; }
}

public class AI : MonoBehaviour
{
    BoardTile[,] board;
    List<BoardPiece> pieces;
    List<BoardInfoHolder> listAvaliableMoves = new List<BoardInfoHolder>(); //possible piece moves
    Dictionary<BoardPiece, List<BoardInfoHolder>> dictionaryAvaliableMoves = new Dictionary<BoardPiece, List<BoardInfoHolder>>(); //listing all piece moves in a dictionary
    //bool mandatoryMove;
    Dictionary<BoardPiece, BoardPiece> eliminated = new Dictionary<BoardPiece, BoardPiece>(); //get pieces that will be eliminated by a piece moviment

    bool OnBoardLimits(int row, int column)
    {
        if (row < 0 || row >= board.GetLength(0)) return false;

        if (column < 0 || column >= board.GetLength(1)) return false;

        return true;
    }

    bool AIKingTile(int row, int column, int rowFactor, int columnFactor, PieceTypes targetType)
    {
        bool check = AIBoardTile(row, column, rowFactor, columnFactor, targetType, true);

        Debug.Log(check);

        return check;
    }

    bool AIBoardTile(int row, int column, int rowFactor, int columnFactor, PieceTypes targetType, bool isKing = false, BoardPiece lastPiece = null)
    {
        if (OnBoardLimits(row, column) == false) return false;

        if (board[row, column].currentPiece == null)
        {
            if (lastPiece != null)
            {
                board[row, column].ApplyColorEffect(false);
            }
            else
                board[row, column].ApplyColorEffect(true);

            BoardInfoHolder newInfo = new BoardInfoHolder();
            newInfo.piece = lastPiece;
            newInfo.tile = board[row, column];

            listAvaliableMoves.Add(newInfo);

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
        //initiate game board
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
                if (pieces[i].IsTopMoviment())
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
        }
    }

    Play AIPlay()
    {
        //when it's possible to eliminate, the ai should do a eliminate play
        //when it's not, the ai should do a random piece play
        ListPossibleMoves();
        Play p = new Play();
        
        // gerando uma lista de keys (pecas)
        List<BoardPiece> playablePieces = new List<BoardPiece>(dictionaryAvaliableMoves.Keys);
        
        // sorteando um index para pegar uma peça da lista 
        int pieceIndex = Random.Range(0, dictionaryAvaliableMoves.Count - 1);
        
        p.piece = playablePieces[pieceIndex];

        // sorteando o index do movimento que a peça ira fazer
        int moveIndex = Random.Range(0, dictionaryAvaliableMoves[p.piece].Count - 1);
        
        //define o alvo depois de escolher o tile possivel de jogada
        p.target = dictionaryAvaliableMoves[p.piece][moveIndex].tile;

        //define a peça eliminada pela jogada
        p.eliminatedBy = eliminated[p.piece];

        return p;
    }
}
