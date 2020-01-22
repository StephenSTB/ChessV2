using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessV2
{
    public class KingMoves
    {
        public List<ChessBoardModel.Square> getPotentialMoves(ref ChessBoardModel.ChessBoardState  chessBoardState)
        {
            List<ChessBoardModel.Square> potentialMoves = new List<ChessBoardModel.Square>();

            ChessBoardModel.Square square = chessBoardState.SelectedPiece.square;

            // Top 3 Squares
            if (square.row - 1 >= 0)
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row - 1, square.column);
                if (chessBoardState.Board[square.row - 1, square.column] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {                 
                    potentialMoves.Add(s);
                }
                if(square.column - 1 >= 0)
                {
                    s = new ChessBoardModel.Square(square.row - 1, square.column - 1);
                    if (chessBoardState.Board[square.row - 1, square.column - 1] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                    {
                        potentialMoves.Add(s);
                    }
                }
                if (square.column + 1 < 8)
                {
                    s = new ChessBoardModel.Square(square.row - 1, square.column + 1);
                    if (chessBoardState.Board[square.row - 1, square.column + 1] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                    {
                        potentialMoves.Add(s);
                    }
                        
                }
            }
            // Bottom 3 Squares
            if (square.row + 1 < 8)
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row + 1, square.column);
                if (chessBoardState.Board[square.row + 1, square.column] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
                if (square.column - 1 >= 0)
                {
                    s = new ChessBoardModel.Square(square.row + 1, square.column - 1);
                    if (chessBoardState.Board[square.row + 1, square.column - 1] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                    {
                        potentialMoves.Add(s);
                    }
                }
                if (square.column + 1 < 8)
                {
                    s = new ChessBoardModel.Square(square.row + 1, square.column + 1);
                    if (chessBoardState.Board[square.row + 1, square.column + 1] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                    {
                        potentialMoves.Add(s);
                    }

                }
            }


            if (square.column - 1 >= 0)
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row, square.column -1);
                if (chessBoardState.Board[square.row , square.column - 1] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            if (square.column + 1 < 8)
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row, square.column + 1);
                if (chessBoardState.Board[square.row , square.column + 1] == ChessBoardModel.Pieces.blnk || ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
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
                    ChessBoardModel.Square KingSquare = chessBoardState.WhitesMove ? chessBoardState.WhiteKingSquare : chessBoardState.BlackKingSquare;
                    if (chessBoardState.Board[KingSquare.row, KingSquare.column + 1] == ChessBoardModel.Pieces.blnk && chessBoardState.Board[KingSquare.row, KingSquare.column + 2] == ChessBoardModel.Pieces.blnk)
                    {
                        ChessBoardModel.Square s = new ChessBoardModel.Square(KingSquare.row, KingSquare.column + 2);
                        potentialMoves.Add(s);
                    }
                }

                // Queen Side Castle
                if (!RooksMoved[1])
                {
                    ChessBoardModel.Square KingSquare = chessBoardState.WhitesMove ? chessBoardState.WhiteKingSquare : chessBoardState.BlackKingSquare;
                    if(chessBoardState.Board[KingSquare.row, KingSquare.column - 1] == ChessBoardModel.Pieces.blnk && chessBoardState.Board[KingSquare.row, KingSquare.column - 2] == ChessBoardModel.Pieces.blnk && chessBoardState.Board[KingSquare.row, KingSquare.column - 3] == ChessBoardModel.Pieces.blnk)
                    {
                        ChessBoardModel.Square s = new ChessBoardModel.Square(KingSquare.row, KingSquare.column - 2);
                        potentialMoves.Add(s);
                    }

                }
            }

            return potentialMoves;
        } 
    }
}
