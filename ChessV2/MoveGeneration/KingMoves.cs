using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChessV2.ChessBoardModel;

namespace ChessV2
{
    public class KingMoves
    {
        public List<Square> getPotentialMoves(ref ChessBoardState  chessBoardState)
        {
            List<Square> potentialMoves = new List<Square>();

            Square square = chessBoardState.SelectedPiece.square;

            // Top 3 Squares
            if (square.row - 1 >= 0)
            {
                Square s = new Square(square.row - 1, square.column);
                if (chessBoardState.Board[square.row - 1, square.column] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                {                 
                    potentialMoves.Add(s);
                }
                if(square.column - 1 >= 0)
                {
                    s = new Square(square.row - 1, square.column - 1);
                    if (chessBoardState.Board[square.row - 1, square.column - 1] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                    {
                        potentialMoves.Add(s);
                    }
                }
                if (square.column + 1 < 8)
                {
                    s = new Square(square.row - 1, square.column + 1);
                    if (chessBoardState.Board[square.row - 1, square.column + 1] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                    {
                        potentialMoves.Add(s);
                    }
                        
                }
            }
            // Bottom 3 Squares
            if (square.row + 1 < 8)
            {
                Square s = new Square(square.row + 1, square.column);
                if (chessBoardState.Board[square.row + 1, square.column] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
                if (square.column - 1 >= 0)
                {
                    s = new Square(square.row + 1, square.column - 1);
                    if (chessBoardState.Board[square.row + 1, square.column - 1] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                    {
                        potentialMoves.Add(s);
                    }
                }
                if (square.column + 1 < 8)
                {
                    s = new Square(square.row + 1, square.column + 1);
                    if (chessBoardState.Board[square.row + 1, square.column + 1] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                    {
                        potentialMoves.Add(s);
                    }

                }
            }


            if (square.column - 1 >= 0)
            {
                Square s = new Square(square.row, square.column -1);
                if (chessBoardState.Board[square.row , square.column - 1] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if (square.column + 1 < 8)
            {
                Square s = new Square(square.row, square.column + 1);
                if (chessBoardState.Board[square.row , square.column + 1] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            bool KingMoved = chessBoardState.WhitesMove ? chessBoardState.WhiteKingMoved : chessBoardState.BlackKingMoved;
           
            // Conditions for potential castling moves.
            if (!KingMoved)
            {
                bool[] RooksMoved = new bool[2];
                RooksMoved[0] = chessBoardState.WhitesMove ? chessBoardState.WhiteRooksMoved[0] : chessBoardState.BlackRooksMoved[0];
                RooksMoved[1] = chessBoardState.WhitesMove ? chessBoardState.WhiteRooksMoved[1] : chessBoardState.BlackRooksMoved[1];


                // King Side Castle
                if (!RooksMoved[0])
                {
                    Square KingSquare = chessBoardState.WhitesMove ? chessBoardState.WhiteKingSquare : chessBoardState.BlackKingSquare;
                    if (chessBoardState.Board[KingSquare.row, KingSquare.column + 1] == Pieces.blnk && chessBoardState.Board[KingSquare.row, KingSquare.column + 2] == Pieces.blnk)
                    {
                        Square s = new Square(KingSquare.row, KingSquare.column + 2);
                        potentialMoves.Add(s);
                    }
                }

                // Queen Side Castle
                if (!RooksMoved[1])
                {
                    Square KingSquare = chessBoardState.WhitesMove ? chessBoardState.WhiteKingSquare : chessBoardState.BlackKingSquare;
                    if(chessBoardState.Board[KingSquare.row, KingSquare.column - 1] == Pieces.blnk && chessBoardState.Board[KingSquare.row, KingSquare.column - 2] == Pieces.blnk && chessBoardState.Board[KingSquare.row, KingSquare.column - 3] == Pieces.blnk)
                    {
                        Square s = new Square(KingSquare.row, KingSquare.column - 2);
                        potentialMoves.Add(s);
                    }

                }
            }

            return potentialMoves;
        }

        internal bool attackingKing(ref ChessBoardState chessBoardState)
        {
            // Declare otherKingSquare to be the square the other players king is on.
            Square otherKingSquare = chessBoardState.WhitesMove ? chessBoardState.BlackKingSquare : chessBoardState.WhiteKingSquare;

            // Condition to test if the king is one square from the other king.
            if(Math.Abs(chessBoardState.SelectedPiece.square.row - otherKingSquare.row) <= 1 && Math.Abs(chessBoardState.SelectedPiece.square.column - otherKingSquare.column) <= 1)
            {
                return true;
            }
            return false;
        }
    }
}
