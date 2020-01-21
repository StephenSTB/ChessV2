using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessV2
{
    public class RookMoves
    {
        public List<ChessBoardModel.Square> getPotentialMoves(ref ChessBoardModel.ChessBoardState chessBoardState)
        {
            List<ChessBoardModel.Square> potentialMoves = new List<ChessBoardModel.Square>();

            ChessBoardModel.Square square = chessBoardState.SelectedPiece.square;

            for(int i = 1; square.row - i >= 0; i++)
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row - i, square.column);
                if (chessBoardState.Board[square.row - i, square.column] == ChessBoardModel.Pieces.blnk)
                {
                    potentialMoves.Add(s);
                    continue;
                }
                else if(ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                    break;
                }
                break;
            }

            for (int i = 1; square.column + i < 8; i++)
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row, square.column + i);
                if (chessBoardState.Board[square.row, square.column + i] == ChessBoardModel.Pieces.blnk)
                {
                    potentialMoves.Add(s);
                    continue;
                }
                else if (ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                    break;
                }
                break;
            }

            for (int i = 1; square.row + i < 8; i++)
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row + i, square.column);
                if (chessBoardState.Board[square.row + i, square.column] == ChessBoardModel.Pieces.blnk)
                {
                    potentialMoves.Add(s);
                    continue;
                }
                else if (ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                    break;
                }
                break;
            }

            for (int i = 1; square.column - i >= 0; i++)
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row, square.column - i);
                if (chessBoardState.Board[square.row, square.column - i] == ChessBoardModel.Pieces.blnk)
                {
                    potentialMoves.Add(s);
                    continue;
                }
                else if (ChessBoardModel.otherPlayerPiece(s, ref chessBoardState))
                {
                    potentialMoves.Add(s);
                    break;
                }
                break;
            }

            return potentialMoves;
        }
    }
}
