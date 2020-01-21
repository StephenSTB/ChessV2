using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessV2
{
    public class KnightMoves
    {

        public KnightMoves()
        {

        }

        // Returns the potential Knight moves from th SelectedPiece in the given ChessBoardState.
        public List<ChessBoardModel.Square> getPotentialMoves(ref ChessBoardModel.ChessBoardState chessBoardState)
        {
            List<ChessBoardModel.Square> potentialMoves = new List<ChessBoardModel.Square>();

            ChessBoardModel.Square square = chessBoardState.SelectedPiece.square;


            if((square.row - 2 >= 0) && (square.column - 1 >= 0))
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row - 2, square.column - 1);
                if(chessBoardState.Board[s.row,s.column] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if ((square.row - 2 >= 0) && (square.column + 1 < 8))
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row - 2, square.column + 1);
                if (chessBoardState.Board[s.row, s.column] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if ((square.row - 1 >= 0) && (square.column + 2 < 8))
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row - 1, square.column + 2);
                if (chessBoardState.Board[s.row, s.column] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if ((square.row + 1 < 8 ) && (square.column + 2 < 8))
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row + 1, square.column + 2);
                if (chessBoardState.Board[s.row, s.column] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if ((square.row + 2 < 8) && (square.column + 1 < 8))
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row + 2, square.column + 1);
                if (chessBoardState.Board[s.row, s.column] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if ((square.row + 2 < 8) && (square.column - 1 >= 0))
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row + 2, square.column - 1);
                if (chessBoardState.Board[s.row, s.column] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if ((square.row + 1 < 8 ) && (square.column - 2 >= 0))
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row + 1, square.column - 2);
                if (chessBoardState.Board[s.row, s.column] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if ((square.row - 1 >= 0) && (square.column - 2 >= 0))
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row - 1, square.column - 2);
                if (chessBoardState.Board[s.row, s.column] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }



            return potentialMoves;
        }
    }

    
}
