using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ChessV2.ChessBoardModel;

namespace ChessV2
{
    /// <summary>
    /// ChessEngine class to automate the play of the white or black pieces.
    /// </summary>
    class ChessEngine
    {
        // Declare ChessBoardModel object to be used by the engine.
        ChessBoardModel ChessBoard;

        // Declare ChessViewModel object to be used by the engine.
        ChessViewModel ViewModel;

        // Declare Player Variable to indicate the player the engine is controlling.
        private bool Player;

        // Declare Thread Engine which runs the player logic.
        Thread Engine;

        // Declare BoardDepth to hold the depth of the Engine (minimax) algorithm.
        int BoardDepth;

        public ChessEngine(ref ChessBoardModel chessBoard, ref ChessViewModel viewModel, bool player)
        {
            // Initialize ChessBoard.
            ChessBoard = chessBoard;

            // Initialize ViewModel.
            ViewModel = viewModel;

            // Initialize the Player true = white, false = black.
            Player = player;

            // Initialize BoardDepth to 4.
            BoardDepth = 4;

            // ThreadStart variable initializes with the start method which the Thread will run.
            ThreadStart engineStart = new ThreadStart(start); 

            // Initialize the Engine Thread with engineStart.
            Engine = new Thread(engineStart);

            // Start Engine Thread;
            Engine.Start(); 
        }

        // Method that the Engine thread uses.
        private void start()
        {
            // While Loop to constantly run the engine algoritm until the end of the game or the engine is stopped.
            while (true)
            {
                // Wait a second until the current player is tested.
                Thread.Sleep(1000);

                // Condtion to test if it is the Engines move or if GAMEOVER.
                if (Player == ChessBoard.BoardState.WhitesMove && !ChessBoard.GAMEOVER)
                {
                    
                    Square[] bestMove = new Square[2]; bestMove[0] = new Square(0, 0); bestMove[1] = new Square(0, 0); // Initialize bestMove to hold the best move the Engine can find.

                    // Declare and initialize currentBoard to be a clone of the current BoardState.
                    ChessBoardState currentBoard = ChessBoard.cloneChessBoardState(ref ChessBoard.BoardState);

                    // Declare BestMoveValue to hold the best move and min/max double value for minimax algorithm.
                    //double[] BestMoveValue;

                    // Set BestMoveValue to the result of the miniMax method.
                    //BestMoveValue = miniMax(ref currentBoard, double.MinValue, double.MaxValue, BoardDepth);

                    // Set the best move to the best move value.
                    /*bestMove[0] = new Square((int)BestMoveValue[1], (int)BestMoveValue[2]);*/

                    // Condition to test if miniMax could not find a move.

                  

                    double[] value = new double[5];

                    value = miniMax(ref currentBoard, double.MinValue, double.MaxValue, BoardDepth);

                    bestMove[0].row = (int)value[1]; bestMove[0].column = (int)value[2]; bestMove[1].row = (int)value[3]; bestMove[1].column = (int)value[4];

                    if (bestMove[0].row == 0 && bestMove[0].column == 0 && bestMove[1].row == 0 && bestMove[1].column == 0)
                    {
                        bestMove = getBestMove(ref currentBoard); // Set best move the best move that can be found in one iteration.
                    }

                    //Console.WriteLine("Start Move " + " " + bestMove[0].row + " " + bestMove[0].column + " " + bestMove[1].row + " " + bestMove[1].column + " square 1 " +  (bestMove[0].row * 8 + bestMove[0].column) + " square 2 " + (bestMove[1].row * 8 + bestMove[1].column));

                    ViewModel.ChangeBoard((bestMove[0].row * 8 + bestMove[0].column));

                    //Thread.Sleep(5000);

                    ViewModel.ChangeBoard((bestMove[1].row * 8 + bestMove[1].column));

                    //Console.WriteLine("Whos Move? " + ChessBoard.BoardState.WhitesMove);

                    
                }

                Thread.Sleep(1000);
               
            }
        }


        // Method to find the best move using the miniMax algorithm.
        private double[] miniMax(ref ChessBoardState currentBoard, double alpha , double beta, int depth)
        {
            bool checkmate = false;

            // Base case condition to test if the depth is 0 or the game is over.
            if(depth == 0 || (checkmate = ChessBoard.gameOver(ref currentBoard)) == true)
            {
                // Condition to test if the game is over.
                if (checkmate)
                {
                    // Condition to test if the game is over and it's Black's move 
                    if (!currentBoard.WhitesMove)
                    {
                        double[] blackMax = new double[5]; blackMax[0] = double.MaxValue;
                        return blackMax; //return max black double
                    }
                    double[] whiteMax = new double[5]; whiteMax[0] = double.MinValue;
                    return whiteMax; //return min white double
                }
                double[] boardValue = new double[5]; boardValue[0] = getBoardValue(currentBoard);
                return boardValue; // Return the boardValue of completed algorithm.
            }

            // Condition to Choose best move for white.
            if (currentBoard.WhitesMove)
            {
                // Initiaize value array to hold the best board value and potentially the best move.
                double[] value = new double[5]; value[0] = double.MinValue; 
                // Create ChessPiece array to iterate through Whites pieces to find the best move.
                List<ChessPiece> pieces = new List<ChessPiece>();
                // Add each of whites pieces to the pieces array.
                foreach(ChessPiece piece in currentBoard.WhitePieces)
                {
                    pieces.Add(new ChessPiece(piece.piece, piece.square.row, piece.square.column));
                }

                // Initialize variable to the given alpha.
                double a = alpha;

                // Iterate through pieces to find best move.
                foreach(ChessPiece piece in pieces)
                {
                    // set Selected piece to piece.
                    currentBoard.SelectedPiece = piece;
                    // Initialize moves for the selected piece.
                    List<Square> moves = ChessBoard.getValidMoves(ref currentBoard);

                    // Iterate through piece moves.
                    foreach(Square move in moves)
                    {
                        // Clone currentBoard to Board to move pieces.
                        ChessBoardState Board = ChessBoard.cloneChessBoardState(ref currentBoard);

                        // Move piece to move.
                        ChessBoard.movePiece(ref Board, move); 

                        // Change turn.
                        Board.WhitesMove = !Board.WhitesMove;

                        // Recursive call to find the value at the lower board depth.
                        double v = miniMax(ref Board, a, beta, depth - 1)[0];

                        // Reselect piece. 
                       // Board.SelectedPiece = piece;

                        // Condition to test if the value of makeing this move is the best move so far.
                        if(v > value[0])
                        {
                            // Set first position of value to v.
                            value[0] = v;
                            // Condition to test if is is the base depth to set the best move.
                            if(depth == BoardDepth)
                            {
                                // Set the best move to this move.
                                value[1] = piece.square.row; value[2] = piece.square.column; value[3] = move.row; value[4] = move.column;
                            }
                        }
                        // Set a to the max of a and the best value.
                        a = Math.Max(a, value[0]);

                        // Condition to test if a is greater then beta meaning worse.
                        if(a > beta)
                        {
                            break; // break as a is greater then beta.
                        }
                    }
                    // Condition to test if a is greater then beta
                    if (a > beta)
                    {
                        break; // break as a is greater then beta.
                    }
                }
                return value;
            }
            else
            {
                // Initiaize value array to hold the best board value and potentially the best move.
                double[] value = new double[5]; value[0] = double.MaxValue;
                // Create ChessPiece array to iterate through Whites pieces to find the best move.
                List<ChessPiece> pieces = new List<ChessPiece>();
                // Add each of whites pieces to the pieces array.
                foreach (ChessPiece piece in currentBoard.BlackPieces)
                {
                    pieces.Add(new ChessPiece(piece.piece, piece.square.row, piece.square.column));
                }

                // Initialize variable to the given beta.
                double b = beta;

                // Iterate through pieces to find best move.
                foreach (ChessPiece piece in pieces)
                {
                    // set Selected piece to piece.
                    currentBoard.SelectedPiece = piece;
                    // Initialize moves for the selected piece.
                    List<Square> moves = ChessBoard.getValidMoves(ref currentBoard);

                    // Iterate through piece moves.
                    foreach (Square move in moves)
                    {
                        // Clone currentBoard to Board to move pieces.
                        ChessBoardState Board = ChessBoard.cloneChessBoardState(ref currentBoard);

                        // Move piece to move.
                        ChessBoard.movePiece(ref Board, move);

                        // Change turn.
                        Board.WhitesMove = !Board.WhitesMove;

                        // Recursive call to find the value at the lower board depth.
                        double v = miniMax(ref Board, alpha, b, depth - 1)[0];

                        // Reselect piece. 
                        Board.SelectedPiece = piece;

                        // Condition to test if the value of makeing this move is the best move so far.
                        if (v < value[0])
                        {
                            // Set first position of value to v.
                            value[0] = v;
                            // Condition to test if is is the base depth to set the best move.
                            if (depth == BoardDepth)
                            {
                                // Set the best move to this move.
                                value[1] = piece.square.row; value[2] = piece.square.column; value[3] = move.row; value[4] = move.column;
                            }
                        }

                        // Set a to the max of a and the best value.
                        b = Math.Min(b, value[0]);

                        // Condition to test if a is greater then beta meaning worse.
                        if (alpha > b)
                        {
                            break; // break as alapha is greater then b.
                        }
                    }
                    // Condition to test if alpha is greater then b
                    if (alpha > b)
                    {
                        break; // break as alpha is greater then b.
                    }
                }
                return value;
            }

            throw new NotImplementedException();
        }

        // Method to get the best move given a ChessBoardState.
        private Square[] getBestMove(ref ChessBoardState boardState)
        {
            // Initialize currentBoard varaible to a clone of the given boardState.
            ChessBoardState currentBoard = ChessBoard.cloneChessBoardState(ref boardState);

            // Initialize originalBoardValue to the currentBoard value via getBoardValue method.
            double originalBoardValue = getBoardValue(currentBoard);

            // Initialize the bestBoardValue to the currentBoard value via getBoardValue method.
            double bestBoardValue = originalBoardValue;

            Console.WriteLine(bestBoardValue);

            // Initialize the list containing the best move
            Square[] bestMove = new Square[2]; bestMove[0] = new Square(0, 0); bestMove[1] = new Square(0, 0);

            // Initialize pieces to the WhitePieces or BlackPieces depending on who's move it is.
            List<ChessPiece> playerPieces = currentBoard.WhitesMove ? currentBoard.WhitePieces : currentBoard.BlackPieces;
            
            // Create pieces list to hold pieces to move
            List<ChessPiece> pieces = new List<ChessPiece>();

            // Initialize pieces.
            foreach (ChessPiece cp in playerPieces)
            {
                pieces.Add(new ChessPiece(cp.piece, cp.square.row, cp.square.column));
            }

            // Loop to iterate through the pieces and find the best move
            for(int i = 0; i < pieces.Count; i++)
            {
                // Set the cloned board SelectedPiece to the ith piece.
                currentBoard.SelectedPiece = pieces[i];

                // Initialize currentPieceMoves to the valid moves of the ith piece.
                List<Square> currentPieceMoves = ChessBoard.getValidMoves(ref currentBoard); 

                // Loop through the moves of the ith piece to check if there is the better move represented by the bestBoardValue.
                foreach (Square cpm in currentPieceMoves)
                {
                    // Move the SelectedPiece to a valid square.
                    ChessBoard.movePiece(ref currentBoard, cpm);

                    // Get the value of the board after the ith piece has been moved.
                    double value = getBoardValue(currentBoard);

                    // Condition to test if the bestBoardValue and bestMove should be updated.
                    if((value < bestBoardValue && !currentBoard.WhitesMove) || (value > bestBoardValue && currentBoard.WhitesMove))
                    {
                        bestBoardValue = value;
                        bestMove[0].row = pieces[i].square.row; bestMove[0].column = pieces[i].square.column;
                        bestMove[1].row = cpm.row; bestMove[1].column = cpm.column;
                    }

                    // reclone the ChessBoardState for another iteration.
                    currentBoard = ChessBoard.cloneChessBoardState(ref boardState);
                    // Reselect the piece being evaluated.
                    currentBoard.SelectedPiece = pieces[i];
                }
                
            }

            // Condition to test if the bestBoardValue is the same as originalBoardValue 
            if(bestBoardValue == originalBoardValue)
            {
                bestMove = getRandomMove(pieces, ref currentBoard); // Get a random move as there is no best move.
            }

           

            return bestMove;
        }

        // Method to return a random move for the player if a best move can not be found
        private Square[] getRandomMove(List<ChessPiece> pieces, ref ChessBoardState currentBoard)
        {
            // Initialize Random number generator to pick a random piece and a random move.
            Random rng = new Random();

            // Declare Square array for a the best move.
            Square[] bestMove = new Square[2];

            // Loop through the players pieces until a random move is found. 
            while(pieces.Count > 0)
            {
                // Initialize index to be a random piece index in pieces. 
                int index = rng.Next(pieces.Count);

                // Declare selectedPiece to be the piece at index in pieces.
                ChessPiece selectedPiece = pieces[index];

                // Set currentBoards Selected piece to selected piece to find valid moves for it.
                currentBoard.SelectedPiece = selectedPiece;

                // Get valid moves for the selectedPiece
                currentBoard.ValidMoves = ChessBoard.getValidMoves(ref currentBoard);

                // Condition to test that ValidMoves has atleast one move.
                if (currentBoard.ValidMoves.Count == 0)
                {
                    pieces.RemoveAt(index); // Remove the piece from Pieces if there were no vaild moves.
                    continue;
                }

                // Set index to a random index in the range of ValidMoves.
                index = rng.Next(currentBoard.ValidMoves.Count);

                // Set move to the random Move for the SelectedPiece.
                Square move = currentBoard.ValidMoves[index];

                // Set the first index of bestMove to the SelectedPiece randomly found.
                bestMove[0] = selectedPiece.square;
                // Set the second index of bestMove to the move randomly found.
                bestMove[1] = move;

                // Return the random move.
                return bestMove;

            }
            return bestMove;
            
        }

        private double getBoardValue(ChessBoardState chessBoardState)
        {
            double value = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int[] n = new int[2]; n[0] = i; n[1] = j;
                    value += getPieceValue(chessBoardState.Board[i, j], n);
                }
            }

            return value;
        }

        // Method to get the value of a piece at a given position on the board.
        private double getPieceValue(Pieces piece, int[] position)
        {
            switch (piece)
            {
                case Pieces.wr:
                    return 50 + whiteRookEval[position[0], position[1]];
                case Pieces.wn:
                    return 30 + knightEval[position[0], position[1]];
                case Pieces.wb:
                    return 30 + whiteBishopEval[position[0], position[1]];
                case Pieces.wk:
                    return 900 + whiteKingEval[position[0], position[1]];
                case Pieces.wq:
                    return 90 + queenEval[position[0], position[1]];
                case Pieces.wp:
                    return 10 + whitePawnEval[position[0], position[1]];

                case Pieces.br:
                    return -50 - blackRookEval[position[0], position[1]];
                case Pieces.bn:
                    return -30 - knightEval[position[0], position[1]];
                case Pieces.bb:
                    return -30 - blackBishopEval[position[0], position[1]];
                case Pieces.bk:
                    return -900 - blackKingEval[position[0], position[1]];
                case Pieces.bq:
                    return -90 - queenEval[position[0], position[1]];
                case Pieces.bp:
                    return -10 - blackPawnEval[position[0], position[1]];

            }
            return 0;
        }

        // Method to stop the Engine.
        public void stop()
        {
            Engine.Abort();
        }

        double[,] whitePawnEval = new double[8, 8]
        {
            { 0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, 0.0 },
            { 5.0,  5.0,  5.0,  5.0,  5.0,  5.0,  5.0, 5.0 },
            { 1.0,  1.0,  2.0,  3.0,  3.0,  2.0,  1.0, 1.0 },
            { 0.5,  0.5,  1.0,  2.5,  2.5,  1.0,  0.5, 0.5 },
            { 0.0,  0.0,  0.0,  2.0,  2.0,  0.0,  0.0, 0.0 },
            { 0.5, -0.5, -1.0,  0.0,  0.0, -1.0, -0.5, 0.5 },
            { 0.5,  1.0,  1.0, -2.0, -2.0,  1.0,  1.0, 0.5 },
            { 0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, 0.0 }
        };

        double[,] blackPawnEval = new double[8, 8]
        {
            { 0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, 0.0 },
            { 0.5,  1.0,  1.0, -2.0, -2.0,  1.0,  1.0, 0.5 },
            { 0.5, -0.5, -1.0,  0.0,  0.0, -1.0, -0.5, 0.5 },
            { 0.0,  0.0,  0.0,  2.0,  2.0,  0.0,  0.0, 0.0 },
            { 0.5,  0.5,  1.0,  2.5,  2.5,  1.0,  0.5, 0.5 },
            { 1.0,  1.0,  2.0,  3.0,  3.0,  2.0,  1.0, 1.0 },
            { 5.0,  5.0,  5.0,  5.0,  5.0,  5.0,  5.0, 5.0 },
            { 0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, 0.0 },
        };


        double[,] knightEval = new double[8, 8]
        {
            { -5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0 },
            { -4.0, -2.0,  0.0,  0.0,  0.0,  0.0, -2.0, -4.0 },
            { -3.0,  0.0,  1.0,  1.5,  1.5,  1.0,  0.0, -3.0 },
            { -3.0,  0.5,  1.5,  2.0,  2.0,  1.5,  0.5, -3.0 },
            { -3.0,  0.0,  1.5,  2.0,  2.0,  1.5,  0.0, -3.0 },
            { -3.0,  0.5,  1.0,  1.5,  1.5,  1.0,  0.5, -3.0 },
            { -4.0, -2.0,  0.0,  0.5,  0.5,  0.0, -2.0, -4.0 },
            { -5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0 }
        };

        double[,] whiteBishopEval = new double[8, 8]
        {
            { -2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0 },
            { -1.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -1.0 },
            { -1.0,  0.0,  0.5,  1.0,  1.0,  0.5,  0.0, -1.0 },
            { -1.0,  0.5,  0.5,  1.0,  1.0,  0.5,  0.5, -1.0 },
            { -1.0,  0.0,  1.0,  1.0,  1.0,  1.0,  0.0, -1.0 },
            { -1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0, -1.0 },
            { -1.0,  0.5,  0.0,  0.0,  0.0,  0.0,  0.5, -1.0 },
            { -2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0 }
        };

        double[,] blackBishopEval = new double[8, 8]
        {
            { -2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0 },
            { -1.0,  0.5,  0.0,  0.0,  0.0,  0.0,  0.5, -1.0 },
            { -1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0, -1.0 },
            { -1.0,  0.0,  1.0,  1.0,  1.0,  1.0,  0.0, -1.0 },
            { -1.0,  0.5,  0.5,  1.0,  1.0,  0.5,  0.5, -1.0 },
            { -1.0,  0.0,  0.5,  1.0,  1.0,  0.5,  0.0, -1.0 },
            { -1.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -1.0 },
            { -2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0 },
        };

        double[,] whiteRookEval = new double[8, 8]
        {
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,  0.0},
            { 0.5, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0,  0.5},
            {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5},
            {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5},
            {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5},
            {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5},
            {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5},
            { 0.0, 0.0, 0.0, 0.5, 0.5, 0.0, 0.0,  0.0}
        };

        double[,] blackRookEval = new double[8, 8]
        {
            { 0.0, 0.0, 0.0, 0.5, 0.5, 0.0, 0.0,  0.0},
            {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5},
            {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5},
            {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5},
            {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5},
            {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5},
            { 0.5, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0,  0.5},
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,  0.0},
        };

        double[,] queenEval = new double[8, 8]
        {
            {-2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0 },
            {-1.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -1.0 },
            {-1.0,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -1.0 },
            {-0.5,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -0.5 },
            { 0.0,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -0.5 },
            {-1.0,  0.5,  0.5,  0.5,  0.5,  0.5,  0.0, -1.0 },
            {-1.0,  0.0,  0.5,  0.0,  0.0,  0.0,  0.0, -1.0 },
            {-2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0 }
        };

        double[,] whiteKingEval = new double[8, 8]
        {
            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            {-2.0, -3.0, -3.0, -4.0, -4.0, -3.0, -3.0, -2.0 },
            {-1.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -1.0 },
            { 2.0,  2.0,  0.0,  0.0,  0.0,  0.0,  2.0,  2.0 },
            { 2.0,  3.0,  1.0,  0.0,  0.0,  1.0,  3.0,  2.0 }
        };

        double[,] blackKingEval = new double[8, 8]
        {
            { 2.0,  3.0,  1.0,  0.0,  0.0,  1.0,  3.0,  2.0 },
            { 2.0,  2.0,  0.0,  0.0,  0.0,  0.0,  2.0,  2.0 },
            {-1.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -1.0 },
            {-2.0, -3.0, -3.0, -4.0, -4.0, -3.0, -3.0, -2.0 },
            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },

        };
    }
}
