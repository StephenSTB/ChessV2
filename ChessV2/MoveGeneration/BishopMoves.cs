using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChessV2.ChessBoardModel;

namespace ChessV2
{
    
    public class BishopMoves
    {
        Dictionary<int[], List<List<int[]>>> AllPotentialMoves;

        public BishopMoves()
        {
            AllPotentialMoves = new Dictionary<int[], List<List<int[]>>>();

            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    int[] square = new int[2]; square[0] = i; square[1] = j;
                    List<List<int[]>> squareDoubleList = new List<List<int[]>>();

                    squareDoubleList.Add(new List<int[]>());
                    for(int k = 1; ((i-k >= 0) && (j-k >= 0)); k++)
                    {
                        int[] move = new int[2]; move[0] = i - k; move[1] = j - k;
                        squareDoubleList[0].Add(move);
                    }

                    squareDoubleList.Add(new List<int[]>());
                    for (int k = 1; ((i - k >= 0) && (j + k < 8)); k++)
                    {
                        int[] move = new int[2]; move[0] = i - k; move[1] = j + k;
                        squareDoubleList[1].Add(move);
                    }

                    squareDoubleList.Add(new List<int[]>());
                    for (int k = 1; ((i + k < 8) && (j + k < 8)); k++)
                    {
                        int[] move = new int[2]; move[0] = i + k; move[1] = j + k;
                        squareDoubleList[2].Add(move);
                    }

                    squareDoubleList.Add(new List<int[]>());
                    for (int k = 1; ((i + k < 8) && (j - k >= 0)); k++)
                    {
                        int[] move = new int[2]; move[0] = i + k; move[1] = j - k;
                        squareDoubleList[3].Add(move);
                    }

                    AllPotentialMoves[square] = squareDoubleList;
                }
            }
        }

        // Returns the potential Bishop moves from th SelectedPiece in the given ChessBoardState.
        public List<Square> getPotentialMoves(ref ChessBoardState chessBoardState)
        {
            List<Square> potentialMoves = new List<Square>();

            Square square = chessBoardState.SelectedPiece.square;


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
            /*
            for(int i = 0; i < potentialMoves.Count; i++)
            {
                Console.Write($"({potentialMoves[i].row}, {potentialMoves[i].column})");
            }*/

            return potentialMoves;
        }

        public bool attackingKing(ref ChessBoardState chessBoardState)
        {
            // Declare otherKingSquare to be the square the other players king is on.
            Square otherKingSquare = chessBoardState.WhitesMove ? chessBoardState.BlackKingSquare : chessBoardState.WhiteKingSquare;

            // Declare and initialize the bishop square to be the square of the selected piece.
            Square bishopSquare = chessBoardState.SelectedPiece.square;

            // Condition to test if the other players king is on a diagonal of the bishop.
            if (Math.Abs(bishopSquare.row - otherKingSquare.row) == Math.Abs(bishopSquare.column - otherKingSquare.column))
            {
                // Condition to test if the king is on the First or Second diagonal.
                if ((otherKingSquare.row < bishopSquare.row))
                {
                    // Condition to test if the king is on the First diagonal.
                    if ((otherKingSquare.column < bishopSquare.column))
                    {
                        // First Diagonal Loop
                        for (int i = 1; (bishopSquare.row - i >= 0) && (bishopSquare.column - i >= 0); i++)
                        {
                            // Condition to test if the square holds a blnk piece.
                            if (chessBoardState.Board[bishopSquare.row - i, bishopSquare.column - i] == Pieces.blnk)
                            {
                                continue;
                            }
                            // Condition to test if the square holds the other king piece.
                            else if (bishopSquare.row - i == otherKingSquare.row && bishopSquare.column - i == otherKingSquare.column)
                            {
                                return true; // The king is attacked by the bishop return true.
                            }
                            return false;
                        }
                    }
                    else  // The king is on the Second diagonal.
                    {
                        // Second Diagonal Loop
                        for (int i = 1; (bishopSquare.row - i >= 0) && (bishopSquare.column + i < 8); i++)
                        {
                            // Condition to test if the square holds a blnk piece.
                            if (chessBoardState.Board[bishopSquare.row - i, bishopSquare.column + i] == Pieces.blnk)
                            {
                                continue;
                            }
                            // Condition to test if the square holds the other king piece.
                            else if (bishopSquare.row - i == otherKingSquare.row && bishopSquare.column + i == otherKingSquare.column)
                            {
                                return true; // The king is attacked by the bishop return true.
                            }
                            return false;
                        }
                    }
                }
               
                // Condition to test if the king is on the Thrid or Fourth diagonal.
                if ((otherKingSquare.row > bishopSquare.row) )
                {
                    // Condition to test if the king is on the Third diagonal.
                    if ((otherKingSquare.column > bishopSquare.column))
                    {
                        // Third Diagonal Loop
                        for (int i = 1; (bishopSquare.row + i < 8) && (bishopSquare.column + i < 8); i++)
                        {
                            // Condition to test if the square holds a blnk piece
                            if (chessBoardState.Board[bishopSquare.row + i, bishopSquare.column + i] == Pieces.blnk)
                            {
                                continue;
                            }
                            // Condition to test if the square holds the other king piece.
                            else if (bishopSquare.row + i == otherKingSquare.row && bishopSquare.column + i == otherKingSquare.column)
                            {
                                return true; // The king is attacked by the bishop return true.
                            }
                            return false;
                        }
                    }
                    else // The king is on the Fourth diagonal.
                    {
                        // Fourth Diagonal Loop
                        for (int i = 1; (bishopSquare.row + i < 8) && (bishopSquare.column - i < 8); i++)
                        {
                            // Condition to test if the square holds a blnk piece
                            if (chessBoardState.Board[bishopSquare.row + i, bishopSquare.column - i] == Pieces.blnk)
                            {
                                continue;
                            }
                            // Condition to test if the square holds the other king piece.
                            else if (bishopSquare.row + i == otherKingSquare.row && bishopSquare.column - i == otherKingSquare.column)
                            {
                                return true; // The king is attacked by the bishop return true.
                            }
                            return false;
                        }
                    }
                }
            }
            return false;
        }

    }
}
