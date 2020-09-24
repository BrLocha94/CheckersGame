# CheckersGame
Simple checkers game made with Unity Engine (2018.4.18)

Game Rules:

The game is played in a 8x8 board alternating light and dark squares.

The playing pieces only move on dark squares.

Every player starts out with 12 pieces with a color (black or white) laid out on the 12 dark squares nearest.

The light pieces player starts the game.

Pieces may only move one diagonal space forward.

To eliminate an opponent's piece, a player piece can perform a jump.

A piece can jump over an opponent's piece if they are adjacent, even forward or backward.

The jump is mandatory when it's possible.

If there is more than one ways to jump in a turn is mandatory to perform the a play that jumps the maximum as possible pieces.

A piece can't jump over an opponent's piece if there is another opponent's piece right after the one that would be eliminated.

When a player's piece lands in one of the squares at the far end of the board, its move ends there and it becomes a king.

The king is a wider movements piece. It can move forward or backward in any diagonal and how many squares as it wants. 

The king can't jump a same color piece.

The king can land on any available square after a eliminated opponent's piece

The king can jump a opponent's piece and a piece can jump a opponent's king.

The piece that, during a multiple jump play, passes through a far end square but don't stop there don't become a king.

In a multiple jump play the piece can pass through an empty square more than once, but cannot eliminate the same opponent's piece twice.

In a multiple jump play it's not allowed to eliminate the same piece twice and the eliminated pieces can't be removed from the board before the play ends.

Wins the player who eliminate all the opponent's pieces.

Draw situations:
After 20 successive king plays, no elimination or piece moves, it's declare draw

2 kings v 2 kings; 2 kings v 1 king; 2 kings v 1 king and 1 piece; king v king; king v king and piece: it's declare draw after 5 plays.