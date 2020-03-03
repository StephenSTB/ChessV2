using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChessV2.ChessBoardModel;

namespace ChessV2
{
    public class QueenMoves
    {
        // Returns the potential queen moves from th SelectedPiece in the given ChessBoardState.
        public List<Square> getPotentialMoves(ref ChessBoardState chessBoardState)
        {
            List<Square> potentialMoves = new List<Square>();

            Square square = chessBoardState.SelectedPiece.square;

            // Diagonal Moves
            // First Diagonal
            for (int i = 1; (square.row - i >= 0) && (square.column - i >= 0); i++)
            {
                Square s = new Square(square.row - i, square.column - i);
                if (chessBoardState.Board[square.row - i, square.column - i] == Pieces.blnk)
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

            // Second Diagonal
            for (int i = 1; (square.row - i >= 0) && (square.column + i < 8); i++)
            {
                Square s = new Square(square.row - i, square.column + i);
                if (chessBoardState.Board[square.row - i, square.column + i] == Pieces.blnk)
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

            // Third Diagonal
            for (int i = 1; (square.row + i < 8) && (square.column + i < 8); i++)
            {
                Square s = new Square(square.row + i, square.column + i);
                if (chessBoardState.Board[square.row + i, square.column + i] == Pieces.blnk)
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

            // Forth Diagonal
            for (int i = 1; (square.row + i < 8) && (square.column - i >= 0); i++)
            {
                Square s = new Square(square.row + i, square.column - i);
                if (chessBoardState.Board[square.row + i, square.column - i] == Pieces.blnk)
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

            // Files
            for (int i = 1; square.row - i >= 0; i++)
            {
                Square s = new Square(square.row - i, square.column);
                if (chessBoardState.Board[square.row - i, square.column] == Pieces.blnk)
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
            /*
            for (int i = 0; i < potentialMoves.Count; i++)
            {
                Console.Write($"({potentialMoves[i].row}, {potentialMoves[i].column})");
            }*/

            return potentialMoves;
        }

        public bool attackingKing(ref ChessBoardState chessBoardState)
        {
            // Declare otherKingSquare to be the square the other players king is on.
            Square otherKingSquare = chessBoardState.WhitesMove ? chessBoardState.BlackKingSquare : chessBoardState.WhiteKingSquare;

            // Declare and initialize the queen square to be the square of the selected piece.
            Square queenSquare = chessBoardState.SelectedPiece.square;

            // Diagonal Conditions
            // Condition to test if the other players king is on a diagonal of the queen.
            if (Math.Abs(queenSquare.row - otherKingSquare.row) == Math.Abs(queenSquare.column - otherKingSquare.column))
            {
                // Condition to test if the king is on the First or Second diagonal.
                if ((otherKingSquare.row < queenSquare.row))
                {
                    // Condition to test if the king is on the First diagonal.
                    if ((otherKingSquare.column < queenSquare.column))
                    {
                        // First Diagonal Loop
                        for (int i = 1; (queenSquare.row - i >= 0) && (queenSquare.column - i >= 0); i++)
                        {
                            // Condition to test if the square holds a blnk piece.
                            if (chessBoardState.Board[queenSquare.row - i, queenSquare.column - i] == Pieces.blnk)
                            {
                                continue;
                            }
                            // Condition to test if the square holds the other king piece.
                            else if (queenSquare.row - i == otherKingSquare.row && queenSquare.column - i == otherKingSquare.column)
                            {
                                return true; // The king is attacked by the queen return true.
                            }
                            return false;
                        }
                    }
                    else  // The king is on the Second diagonal.
                    {
                        // Second Diagonal Loop
                        for (int i = 1; (queenSquare.row - i >= 0) && (queenSquare.column + i < 8); i++)
                        {
                            // Condition to test if the square holds a blnk piece.
                            if (chessBoardState.Board[queenSquare.row - i, queenSquare.column + i] == Pieces.blnk)
                            {
                                continue;
                            }
                            // Condition to test if the square holds the other king piece.
                            else if (queenSquare.row - i == otherKingSquare.row && queenSquare.column + i == otherKingSquare.column)
                            {
                                return true; // The king is attacked by the queen return true.
                            }
                            return false;
                        }
                    }
                }

                // Condition to test if the king is on the Thrid or Fourth diagonal.
                if ((otherKingSquare.row > queenSquare.row))
                {
                    // Condition to test if the king is on the Third diagonal.
                    if ((otherKingSquare.column > queenSquare.column))
                    {
                        // Third Diagonal Loop
                        for (int i = 1; (queenSquare.row + i < 8) && (queenSquare.column + i < 8); i++)
                        {
                            // Condition to test if the square holds a blnk piece
                            if (chessBoardState.Board[queenSquare.row + i, queenSquare.column + i] == Pieces.blnk)
                            {
                                continue;
                            }
                            // Condition to test if the square holds the other king piece.
                            else if (queenSquare.row + i == otherKingSquare.row && queenSquare.column + i == otherKingSquare.column)
                            {
                                return true; // The king is attacked by the queen return true.
                            }
                            return false;
                        }
                    }
                    else // The king is on the Fourth diagonal.
                    {
                        // Fourth Diagonal Loop
                        for (int i = 1; (queenSquare.row + i < 8) && (queenSquare.column - i < 8); i++)
                        {
                            // Condition to test if the square holds a blnk piece
                            if (chessBoardState.Board[queenSquare.row + i, queenSquare.column - i] == Pieces.blnk)
                            {
                                continue;
                            }
                            // Condition to test if the square holds the other king piece.
                            else if (queenSquare.row + i == otherKingSquare.row && queenSquare.column - i == otherKingSquare.column)
                            {
                                return true; // The king is attacked by the queen return true.
                            }
                            return false;
                        }
                    }
                }
            }

            // File Conditions
            // Condition to test if the selected queen is on the same row as the other king.
            if (queenSquare.row == otherKingSquare.row)
            {
                // Contition to test if the king is on a lower column.
                if (queenSquare.column > otherKingSquare.column)
                {
                    // First File Loop.
                    for (int i = 1; queenSquare.column - i >= 0; i++)
                    {
                        // Contition to test if the square is blank.
                        if (chessBoardState.Board[queenSquare.row, queenSquare.column - i] == Pieces.blnk)
                        {
                            continue;
                        }
                        // Condition to test if the square holds the other players king.
                        else if (queenSquare.column - i == otherKingSquare.column)
                        {
                            return true;// The king is attacked by the queen return true.
                        }
                        return false;
                    }
                }
                else // The king is on a higher column
                {
                    // Second File Loop.
                    for (int i = 1; queenSquare.column + i < 8; i++)
                    {
                        // Contition to test if the square is blank.
                        if (chessBoardState.Board[queenSquare.row, queenSquare.column + i] == Pieces.blnk)
                        {
                            continue;
                        }
                        // Condition to test if the square holds the other players king.
                        else if (queenSquare.column + i == otherKingSquare.column)
                        {
                            return true;// The king is attacked by the queen return true.
                        }
                        return false;
                    }
                }
            }
            // Condition to test if the selected queen is on the same column as the other king.
            else if (queenSquare.column == otherKingSquare.column)
            {
                // Contition to test if the king is on a lower row.
                if (queenSquare.row > otherKingSquare.row)
                {
                    // Third File Loop.
                    for (int i = 1; queenSquare.row - i >= 0; i++)
                    {
                        // Contition to test if the square is blank.
                        if (chessBoardState.Board[queenSquare.row - i, queenSquare.column] == Pieces.blnk)
                        {
                            continue;
                        }
                        // Condition to test if the square holds the other players king.
                        else if (queenSquare.row - i == otherKingSquare.row)
                        {
                            return true;// The king is attacked by the queen return true.
                        }
                        return false;
                    }
                }
                else // the king is on a higher row.
                {
                    // Foruth File Loop.
                    for (int i = 1; queenSquare.row + i < 8; i++)
                    {
                        // Contition to test if the square is blank.
                        if (chessBoardState.Board[queenSquare.row + i, queenSquare.column] == Pieces.blnk)
                        {
                            continue;
                        }
                        // Condition to test if the square holds the other players king.
                        else if (queenSquare.row + i == otherKingSquare.row)
                        {
                            return true;// The king is attacked by the queen return true.
                        }
                        return false;
                    }
                }
            }

            return false;
        }
    }
}
