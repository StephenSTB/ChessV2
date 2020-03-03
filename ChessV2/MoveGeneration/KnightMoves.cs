using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChessV2.ChessBoardModel;

namespace ChessV2
{
    public class KnightMoves
    {

        public KnightMoves()
        {

        }

        // Returns the potential Knight moves from th SelectedPiece in the given ChessBoardState.
        public List<Square> getPotentialMoves(ref ChessBoardState chessBoardState)
        {
            List<Square> potentialMoves = new List<Square>();

            Square square = chessBoardState.SelectedPiece.square;


            if((square.row - 2 >= 0) && (square.column - 1 >= 0))
            {
                Square s = new Square(square.row - 2, square.column - 1);
                if(chessBoardState.Board[s.row,s.column] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if ((square.row - 2 >= 0) && (square.column + 1 < 8))
            {
                Square s = new Square(square.row - 2, square.column + 1);
                if (chessBoardState.Board[s.row, s.column] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if ((square.row - 1 >= 0) && (square.column + 2 < 8))
            {
                Square s = new Square(square.row - 1, square.column + 2);
                if (chessBoardState.Board[s.row, s.column] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if ((square.row + 1 < 8 ) && (square.column + 2 < 8))
            {
                Square s = new Square(square.row + 1, square.column + 2);
                if (chessBoardState.Board[s.row, s.column] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if ((square.row + 2 < 8) && (square.column + 1 < 8))
            {
                Square s = new Square(square.row + 2, square.column + 1);
                if (chessBoardState.Board[s.row, s.column] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if ((square.row + 2 < 8) && (square.column - 1 >= 0))
            {
                Square s = new Square(square.row + 2, square.column - 1);
                if (chessBoardState.Board[s.row, s.column] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if ((square.row + 1 < 8 ) && (square.column - 2 >= 0))
            {
                Square s = new Square(square.row + 1, square.column - 2);
                if (chessBoardState.Board[s.row, s.column] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if ((square.row - 1 >= 0) && (square.column - 2 >= 0))
            {
                Square s = new Square(square.row - 1, square.column - 2);
                if (chessBoardState.Board[s.row, s.column] == Pieces.blnk || otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }



            return potentialMoves;
        }

        internal bool attakingKing(ref ChessBoardState chessBoardState)
        {
            // Declare otherKingSquare to be the square the other players king is on.
            Square otherKingSquare = chessBoardState.WhitesMove ? chessBoardState.BlackKingSquare : chessBoardState.WhiteKingSquare;

            // Declare rowDif and colDif to find the difference between the knight position and other players king position. 
            int rowDif = Math.Abs(chessBoardState.SelectedPiece.square.row - otherKingSquare.row);
            int colDif = Math.Abs(chessBoardState.SelectedPiece.square.column - otherKingSquare.column);

            // Condition to test if the king is attacked by the knight.
            if((rowDif == 1 && colDif == 2) || (rowDif == 2 && colDif == 1))
            {
                return true;
            }

            return false;
        }
    }

    
}
