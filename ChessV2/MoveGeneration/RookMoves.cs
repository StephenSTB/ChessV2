using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChessV2.ChessBoardModel;

namespace ChessV2
{
    public class RookMoves
    {
        public List<Square> getPotentialMoves(ref ChessBoardState chessBoardState)
        {
            List<Square> potentialMoves = new List<Square>();

            Square square = chessBoardState.SelectedPiece.square;

            for(int i = 1; square.row - i >= 0; i++)
            {
                Square s = new Square(square.row - i, square.column);
                if (chessBoardState.Board[square.row - i, square.column] == Pieces.blnk)
                {
                    potentialMoves.Add(s);
                    continue;
                }
                else if(otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                    break;
                }
                break;
            }

            for (int i = 1; square.column + i < 8; i++)
            {
                Square s = new Square(square.row, square.column + i);
                if (chessBoardState.Board[square.row, square.column + i] == Pieces.blnk)
                {
                    potentialMoves.Add(s);
                    continue;
                }
                else if (otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                    break;
                }
                break;
            }

            for (int i = 1; square.row + i < 8; i++)
            {
                Square s = new Square(square.row + i, square.column);
                if (chessBoardState.Board[square.row + i, square.column] == Pieces.blnk)
                {
                    potentialMoves.Add(s);
                    continue;
                }
                else if (otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                    break;
                }
                break;
            }

            for (int i = 1; square.column - i >= 0; i++)
            {
                Square s = new Square(square.row, square.column - i);
                if (chessBoardState.Board[square.row, square.column - i] == Pieces.blnk)
                {
                    potentialMoves.Add(s);
                    continue;
                }
                else if (otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                    break;
                }
                break;
            }

            return potentialMoves;
        }

        public bool attackingKing(ref ChessBoardState chessBoardState)
        {
            // Declare otherKingSquare to be the square the other players king is on.
            Square otherKingSquare = chessBoardState.WhitesMove ? chessBoardState.BlackKingSquare : chessBoardState.WhiteKingSquare;

            // Declare and initialize the rook square to be the square of the selected piece.
            Square rookSquare = chessBoardState.SelectedPiece.square;

            // Condition to test if the selected rook is on the same row as the other king.
            if(rookSquare.row == otherKingSquare.row)
            {
                // Contition to test if the king is on a lower column.
                if(rookSquare.column > otherKingSquare.column)
                {
                    // First File Loop.
                    for(int i = 1; rookSquare.column - i >= 0; i++)
                    {
                        // Contition to test if the square is blank.
                        if(chessBoardState.Board[rookSquare.row, rookSquare.column - i] == Pieces.blnk)
                        {
                            continue;
                        }
                        // Condition to test if the square holds the other players king.
                        else if(rookSquare.column - i == otherKingSquare.column)
                        {
                            return true;// The king is attacked by the rook return true.
                        }
                        return false;
                    }
                }
                else // The king is on a higher column
                {
                    // Second File Loop.
                    for (int i = 1; rookSquare.column + i < 8; i++)
                    {
                        // Contition to test if the square is blank.
                        if (chessBoardState.Board[rookSquare.row, rookSquare.column + i] == Pieces.blnk)
                        {
                            continue;
                        }
                        // Condition to test if the square holds the other players king.
                        else if (rookSquare.column + i == otherKingSquare.column)
                        {
                            return true;// The king is attacked by the rook return true.
                        }
                        return false;
                    }
                }
            }
            // Condition to test if the selected rook is on the same column as the other king.
            else if (rookSquare.column == otherKingSquare.column)
            {
                // Contition to test if the king is on a lower row.
                if (rookSquare.row > otherKingSquare.row)
                {
                    // Third File Loop.
                    for (int i = 1; rookSquare.row - i >= 0; i++)
                    {
                        // Contition to test if the square is blank.
                        if (chessBoardState.Board[rookSquare.row - i, rookSquare.column] == Pieces.blnk)
                        {
                            continue;
                        }
                        // Condition to test if the square holds the other players king.
                        else if (rookSquare.row - i == otherKingSquare.row)
                        {
                            return true;// The king is attacked by the rook return true.
                        }
                        return false;
                    }
                }
                else // the king is on a higher row.
                {
                    // Foruth File Loop.
                    for (int i = 1; rookSquare.row + i < 8; i++)
                    {
                        // Contition to test if the square is blank.
                        if (chessBoardState.Board[rookSquare.row + i, rookSquare.column] == Pieces.blnk)
                        {
                            continue;
                        }
                        // Condition to test if the square holds the other players king.
                        else if (rookSquare.row + i == otherKingSquare.row)
                        {
                            return true;// The king is attacked by the rook return true.
                        }
                        return false;
                    }
                }
            }

            return false;
        }
    }
}
