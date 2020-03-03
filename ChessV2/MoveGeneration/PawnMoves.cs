using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChessV2.ChessBoardModel;

namespace ChessV2
{
    public class PawnMoves
    {
        public List<Square> getPotentialMoves(ref ChessBoardState chessBoardState)
        {
            List<Square> potentialMoves = new List<Square>();

            Square s;

            int pawnStart , pawnOffset;

            pawnStart = chessBoardState.WhitesMove ? 6 : 1;
            pawnOffset = chessBoardState.WhitesMove ? -1 : 1;

            if(chessBoardState.SelectedPiece.square.row + pawnOffset > 7 || chessBoardState.SelectedPiece.square.row + pawnOffset < 0)
            {
                return potentialMoves;
            }

            if(chessBoardState.Board[chessBoardState.SelectedPiece.square.row + pawnOffset, chessBoardState.SelectedPiece.square.column] == Pieces.blnk)
            {
                s = new Square(chessBoardState.SelectedPiece.square.row + pawnOffset, chessBoardState.SelectedPiece.square.column);
                potentialMoves.Add(s);

                if(chessBoardState.SelectedPiece.square.row == pawnStart)
                {
                    if(chessBoardState.Board[chessBoardState.SelectedPiece.square.row + (2 * pawnOffset), chessBoardState.SelectedPiece.square.column] == Pieces.blnk)
                    {
                        s = new Square(chessBoardState.SelectedPiece.square.row + (2 * pawnOffset), chessBoardState.SelectedPiece.square.column);
                        potentialMoves.Add(s);
                    }
                }
            }

            if(chessBoardState.SelectedPiece.square.column - 1 >= 0){
                s = new Square(chessBoardState.SelectedPiece.square.row + pawnOffset, chessBoardState.SelectedPiece.square.column - 1);
                if (otherPlayerPiece(s, ref chessBoardState)){
                    potentialMoves.Add(s);
                }
            }

            if (chessBoardState.SelectedPiece.square.column + 1 < 8)
            {
                s = new Square(chessBoardState.SelectedPiece.square.row + pawnOffset, chessBoardState.SelectedPiece.square.column + 1);
                if (otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                }
            }

            // TODO: EnPassant

            for (int i = 0; i < chessBoardState.enPassantPieces.Count; i++)
            {
                if (chessBoardState.enPassantPieces[i].Equals(chessBoardState.SelectedPiece))
                {
                    potentialMoves.Add(chessBoardState.enPassantMove);
                }
            }

            return potentialMoves;
        }

        //Method to test if the selected pawn is attaking the other players king.
        internal bool attakingKing(ref ChessBoardState chessBoardState)
        {
            // Declare otherKingSquare to be the square the other players king is on.
            Square otherKingSquare = chessBoardState.WhitesMove ? chessBoardState.BlackKingSquare : chessBoardState.WhiteKingSquare;

            int pawnOffset = chessBoardState.WhitesMove ? -1 : 1;

            if((chessBoardState.SelectedPiece.square.row + pawnOffset == otherKingSquare.row) && ((chessBoardState.SelectedPiece.square.column + 1 == otherKingSquare.column) || (chessBoardState.SelectedPiece.square.column - 1  == otherKingSquare.column)))
            {
                return true;
            }
            
            return false;
        }
    }
}
