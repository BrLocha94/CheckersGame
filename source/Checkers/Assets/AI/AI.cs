using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AI : MonoBehaviour
{
    BoardTile[,] board;
    List<BoardPiece> pieces;
    List<BoardInfoHolder> listAvaliableMoves = new List<BoardInfoHolder>(); //movimentos possiveis de uma peca
    Dictionary<BoardPiece, List<BoardInfoHolder>> dictionaryAvaliableMoves = new Dictionary<BoardPiece, List<BoardInfoHolder>>(); //consulta de movimentos

    void AIBoardPiece(BoardPiece target)
    {

    }

    void listPossibleMoves()
    {
        board = null;
        pieces.Clear();
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

        }
    }

    void AIPlay()
    {
        //when it's possible to eliminate, the ai should do a eliminate play
        //when it's not, the ai should do a random piece play
    }
}
