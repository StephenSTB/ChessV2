using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        public List<ChessBoardModel.Square> getPotentialMoves(ref ChessBoardModel.ChessBoardState chessBoardState)
        {
            List<ChessBoardModel.Square> potentialMoves = new List<ChessBoardModel.Square>();

            ChessBoardModel.Square square = chessBoardState.SelectedPiece.square;


            // First Diagonal
            for (int i = 1; (square.row - i >= 0) && (square.column - i >= 0); i++)
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row - i, square.column - i);
                if (chessBoardState.Board[square.row - i, square.column - i] == ChessBoardModel.Pieces.blnk)
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

            // Second Diagonal
            for (int i = 1; (square.row - i >= 0) && (square.column + i < 8); i++)
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row - i, square.column + i);
                if (chessBoardState.Board[square.row - i, square.column + i] == ChessBoardModel.Pieces.blnk)
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

            // Third Diagonal
            for (int i = 1; (square.row + i < 8) && (square.column + i < 8); i++)
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row + i, square.column + i);
                if (chessBoardState.Board[square.row + i, square.column + i] == ChessBoardModel.Pieces.blnk)
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

            // Forth Diagonal
            for (int i = 1; (square.row + i < 8) && (square.column - i >= 0); i++)
            {
                ChessBoardModel.Square s = new ChessBoardModel.Square(square.row + i, square.column - i);
                if (chessBoardState.Board[square.row + i, square.column - i] == ChessBoardModel.Pieces.blnk)
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
            /*
            for(int i = 0; i < potentialMoves.Count; i++)
            {
                Console.Write($"({potentialMoves[i].row}, {potentialMoves[i].column})");
            }*/

            return potentialMoves;
        }

    }
}
