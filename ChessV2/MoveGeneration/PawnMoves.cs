using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessV2
{
    public class PawnMoves
    {
        public List<ChessBoardModel.Square> getPotentialMoves(ref ChessBoardModel.ChessBoardState chessBoardState)
        {
            List<ChessBoardModel.Square> potentialMoves = new List<ChessBoardModel.Square>();

            ChessBoardModel.Square s;

            int pawnStart , pawnOffset;

            pawnStart = chessBoardState.WhitesMove ? 6 : 1;
            pawnOffset = chessBoardState.WhitesMove ? -1 : 1;

            if(chessBoardState.SelectedPiece.square.row + pawnOffset > 7 || chessBoardState.SelectedPiece.square.row + pawnOffset < 0)
            {
                return potentialMoves;
            }

            if(chessBoardState.Board[chessBoardState.SelectedPiece.square.row + pawnOffset, chessBoardState.SelectedPiece.square.column] == ChessBoardModel.Pieces.blnk)
            {
                s = new ChessBoardModel.Square(chessBoardState.SelectedPiece.square.row + pawnOffset, chessBoardState.SelectedPiece.square.column);
                potentialMoves.Add(s);

                if(chessBoardState.SelectedPiece.square.row == pawnStart)
                {
                    if(chessBoardState.Board[chessBoardState.SelectedPiece.square.row + (2 * pawnOffset), chessBoardState.SelectedPiece.square.column] == ChessBoardModel.Pieces.blnk)
                    {
                        s = new ChessBoardModel.Square(chessBoardState.SelectedPiece.square.row + (2 * pawnOffset), chessBoardState.SelectedPiece.square.column);
                        potentialMoves.Add(s);
                    }
                }
            }

            if(chessBoardState.SelectedPiece.square.column - 1 >= 0){
                s = new ChessBoardModel.Square(chessBoardState.SelectedPiece.square.row + pawnOffset, chessBoardState.SelectedPiece.square.column - 1);
                if (ChessBoardModel.otherPlayerPiece(s, ref chessBoardState)){
                    potentialMoves.Add(s);
                }
            }

            if (chessBoardState.SelectedPiece.square.column + 1 < 8)
            {
                s = new ChessBoardModel.Square(chessBoardState.SelectedPiece.square.row + pawnOffset, chessBoardState.SelectedPiece.square.column + 1);
                if (ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
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

    }
}
