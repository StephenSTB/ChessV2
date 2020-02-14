using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
            BoardDepth = 5;

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
                    // Declare and initialize boardState string to be the string represnting the current BoardState.
                    string boardState = getBoardString(ref ChessBoard.BoardState);

                    // Declare and initialize dbMove to get a possible best move found by the ChessDatabase.
                    int[] dbBestMove = dbRetrieveMove(boardState);

                    // Condition to test if a best move was found.
                    if (!(dbBestMove[0] == 0 && dbBestMove[1] == 0))
                    {
                        Console.WriteLine($"Player: {ChessBoard.BoardState.WhitesMove} dbBestMove: {dbBestMove[0]} {dbBestMove[1]}");

                        // Change the BoardState based on the dbBestMove
                        ChessBoard.ChangeBoardState(dbBestMove[0]);

                        ViewModel.UpdateBoard();

                        ChessBoard.ChangeBoardState(dbBestMove[1]);

                        ViewModel.UpdateBoard();
                    }
                    else
                    {
                        // Initialize bestMove to hold the best move the Engine can find.
                        Square[] bestMove = new Square[2]; bestMove[0] = new Square(0, 0); bestMove[1] = new Square(0, 0);

                        // Declare and initialize currentBoard to be a clone of the current BoardState.
                        ChessBoardState currentBoard = ChessBoard.cloneChessBoardState(ref ChessBoard.BoardState);

                        // Variable to hold the best board value and the best move.
                        double[] BestBoardValue = new double[5];

                        // Initialize the BestBoarValue using the miniMax algorithm to find the best move with the given BoardDepth.
                        BestBoardValue = miniMax(ref currentBoard, double.MinValue, double.MaxValue, BoardDepth);

                        // Set the bestMove via BestBoardValue;.
                        bestMove[0].row = (int)BestBoardValue[1]; bestMove[0].column = (int)BestBoardValue[2]; bestMove[1].row = (int)BestBoardValue[3]; bestMove[1].column = (int)BestBoardValue[4];

                        // Condition to test if miniMax found a best move use the getBestMove method if not.
                        if (bestMove[0].row == 0 && bestMove[0].column == 0 && bestMove[1].row == 0 && bestMove[1].column == 0)
                        {
                            bestMove = getBestMove(ref currentBoard); // Set best move the best move that can be found in one iteration.
                        }

                        // Declare and initalize Move string to be inserted into the ChessDatabase.
                        string Move = (bestMove[0].row * 8 + bestMove[0].column) + " " + (bestMove[1].row * 8 + bestMove[1].column);

                        Console.WriteLine($"Player: {ChessBoard.BoardState.WhitesMove} miniMaxBestMove: {Move}");

                        // Update the ChessDatabase with the best move found.
                        dbUpdateAddRecord(boardState, BoardDepth, Move);

                        // Change the BoardState base on the bestMove
                        ChessBoard.ChangeBoardState((bestMove[0].row * 8 + bestMove[0].column));

                        ViewModel.UpdateBoard();

                        ChessBoard.ChangeBoardState((bestMove[1].row * 8 + bestMove[1].column));

                        ViewModel.UpdateBoard();
                    }

                    // Condition to test if GAMEOVER
                    if (ChessBoard.GAMEOVER)
                    {
                        if (ChessBoard.MATE) // Condition to test if it is checkmate.
                        {
                            Console.WriteLine("Checkmate!");
                            ViewModel.GameOverVisibility = Visibility.Visible; // Access the ViewModel to change the visability of the end game popup.
                            // Condition to test who won;
                            if (ChessBoard.BoardState.WhitesMove)
                            {
                                ViewModel.GameOverText = "Game Over \n Black Won!"; // Change game over text to Black won.
                                return;
                            }
                            ViewModel.GameOverText = "Game Over \n White Won!";// Change game over text to White won.
                            return;
                        }
                        ViewModel.GameOverVisibility = Visibility.Visible;// Access the ViewModel to change the visability of the end game popup.
                        ViewModel.GameOverText = "Game Over \n Draw"; // Change game over text to Draw.
                        return;
                    }

                }

                Thread.Sleep(1000);
               
            }
        }

        private int[] dbRetrieveMove(string boardState)
        {
            // String to select the entry in the data base with the boardstate if it exists.
            string selectSQL = "SELECT TOP 1 * FROM ChessTable WHERE BoardState='" + boardState + "' AND Depth ='" + BoardDepth + "'";

            // Get the DataTable resulting from the select query.
            DataTable tbl = ChessDB.GetDataTable(selectSQL);

            // Initialize bestMove.
            int[] bestMove = new int[2]; bestMove[0] = 0; bestMove[1] = 0;

            // Condtion to test if the query had a result.
            if (tbl.Rows.Count != 0)
            {
                // Get the Moves string.
                string dbMoves = tbl.Rows[0]["Moves"].ToString();
                string[] MoveSplit = dbMoves.Split("Black".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);

                // Condition to test which players turn it is.
                if (ChessBoard.BoardState.WhitesMove)
                {
                    // Declare and initialize moveS to be a possible white move.
                    string moveS = MoveSplit[0].Trim("White ".ToCharArray());
                    // Conditon to test if moveS has a move.
                    if (moveS.Length > 0)
                    {
                        // Set bestMove to the move found in the database.
                        bestMove = moveS.Split(' ').Select(m => int.Parse(m)).ToArray(); 
                        return bestMove;
                    }
                   
                }
                else
                {
                    // Declare and initialize moveS to be a possible black move.
                    string moveS = MoveSplit[1].Trim();
                    if (moveS.Length > 0)
                    {
                        // Set bestMove to the move found in the database.
                        bestMove = moveS.Split(' ').Select(m => int.Parse(m)).ToArray();
                        return bestMove;
                    }
                }
            }
            return bestMove;
        }

        // Method to return the string that represents the chess board state.
        private string getBoardString(ref ChessBoardState boardState)
        {
            string state = "";
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    state += boardState.Board[i, j].ToString();
                }
            }
            return state;
        }

        // Method to update the Chess Database.
        private string dbUpdateAddRecord(string boardState, int Depth, string Moves)
        {
            // String to select the entry in the data base with the boardstate if it exists.
            string selectSQL = "SELECT TOP 1 * FROM ChessTable WHERE BoardState='" + boardState + "' AND Depth ='" + Depth + "'";

            // Get the DataTable resulting from the select query.
            DataTable tbl = ChessDB.GetDataTable(selectSQL);

            // Declare insertSQL string to hold the insert query.
            string insertSQL = "";

            // Condition to test if the data table has entries. The boardState has been entered before.
            if (tbl.Rows.Count == 0)
            {
                
                // Condition to test whos move is being entered.
                if (ChessBoard.BoardState.WhitesMove)
                {
                    insertSQL = "INSERT INTO ChessTable(BoardState, Depth, Moves) VALUES('" + boardState + "', '" +  Depth + ("', 'White " + Moves + " Black") + "')"; // Insert White move.
                }
                else
                {
                    insertSQL = "INSERT INTO ChessTable(BoardState, Depth, Moves) VALUES('" + boardState + "', '" + Depth +  ("', 'White " + "Black " + Moves) + "')"; // Insert Black move.
                }

                // Execute the INSERT.
                ChessDB.ExecuteSQL(insertSQL);

                tbl = ChessDB.GetDataTable(selectSQL);

                return tbl.Rows[0]["Moves"].ToString();
            }
            else
            {
                // Get the Moves string to be updated.
                string dbMoves = tbl.Rows[0]["Moves"].ToString();
                // Declare MoveSplit string array to hold the split of the two players moves.
                string[] MoveSplit = dbMoves.Split("Black".ToCharArray());
                // Declare string to hold the SQL UPDATE query
                string updateSQL = "";

                // Condtion to test whos move will be updated.
                if (ChessBoard.BoardState.WhitesMove)
                {
                    updateSQL = "UPDATE ChessTable SET Moves = '" + ("White " + Moves + " Black" + MoveSplit[1]) + "' WHERE BoardState = '" + boardState + "'"; // UPDATE White move.
                    Console.WriteLine($"White Move UPDATE: {("White " + Moves + " Black" + MoveSplit[1])}" );
                }
                else
                {
                    updateSQL = "UPDATE ChessTable SET Moves = '" + (MoveSplit[1] + "Black " + Moves) + "' WHERE BoardState = '" + boardState + "'"; // UPDATE Black move.
                    Console.WriteLine($"Black Move UPDATE: {(MoveSplit[1] + "Black " + Moves)}");
                }

            }

            return tbl.Rows[0]["Moves"].ToString();
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
                    List<Square> moves = ChessBoard.getPotentialMoves(ref currentBoard);

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

                        // Condition to test if the value of makeing this move is the best move so far.
                        if(v > value[0] && validMove(ref Board, ref currentBoard))
                        {
                            // Set first position of value to v.
                            value[0] = v;
                            // Condition to test if is is the base depth to set the best move.
                            if(depth == BoardDepth)
                            {
                                Console.WriteLine($"Player: White miniMaxBestMove: {v}");
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

                        // Condition to test if the value of makeing this move is the best move so far.
                        if (v < value[0] && validMove(ref Board, ref currentBoard))
                        {
                            // Set first position of value to v.
                            value[0] = v;
                            // Condition to test if is is the base depth to set the best move.
                            if (depth == BoardDepth)
                            {
                                Console.WriteLine($"Player: Black miniMaxBestMove: {v}");
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

        private bool validMove(ref ChessBoardState movedBoard, ref ChessBoardState currentBoard)
        {
            // Condition to test if the player is in check making the move invalid
            if (!ChessBoard.Check(ref movedBoard))
            {
                // Condition to test if it was a king move potentially doing an illegal castle.
                if(movedBoard.SelectedPiece.piece == Pieces.wk || movedBoard.SelectedPiece.piece == Pieces.bk)
                {
                    // Conditio to test if a King side castle occured
                    if(movedBoard.SelectedPiece.square.row == currentBoard.SelectedPiece.square.row &&
                        (movedBoard.SelectedPiece.square.column == (currentBoard.SelectedPiece.square.column + 2)  || movedBoard.SelectedPiece.square.column == (currentBoard.SelectedPiece.square.column - 2)))
                    {
                        // Save selectedPiece to revert the board after operations.
                        ChessPiece selectedPiece = movedBoard.SelectedPiece;

                        // Initialize variable to hold other players pieces to test if a castle through Check occured.
                        List<ChessPiece> otherPlayersPieces = currentBoard.WhitesMove ? movedBoard.BlackPieces : movedBoard.WhitePieces;

                        // Declare the square that the king moved though to castle.
                        Square castleSquare;

                        // condition to test which side was castled to.
                        if (movedBoard.SelectedPiece.square.column == (currentBoard.SelectedPiece.square.column + 2))
                        {
                            castleSquare = new Square(movedBoard.SelectedPiece.square.row, movedBoard.SelectedPiece.square.column - 1);// King side
                        }
                        else
                        {
                            castleSquare = new Square(movedBoard.SelectedPiece.square.row, movedBoard.SelectedPiece.square.column + 1);// Queen sied
                        }
                         
                        // Loop to test if any of the otherPlayersPieces attacked the castleSquare. 
                        foreach(ChessPiece op in otherPlayersPieces)
                        {
                            // Set Selected piece to the other players piece.
                            movedBoard.SelectedPiece = op;
                            // Get the potential moves of the op piece
                            List<Square> playerPotentialMoves = ChessBoard.getPotentialMoves(ref movedBoard);

                            // Loop to test if any of the potential moves are the castleSquare.
                            foreach(Square s in playerPotentialMoves)
                            {
                                if (s.Equals(castleSquare))
                                {
                                    movedBoard.SelectedPiece = selectedPiece; // Reset SelectedPiece
                                    return false;
                                }
                            }
                        }
                        movedBoard.SelectedPiece = selectedPiece; // Reset SelectedPiece
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            return false;
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
