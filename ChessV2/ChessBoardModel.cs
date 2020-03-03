using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessV2
{

    public class ChessBoardModel
    {

        // Enumerated type to represent each of the pieces and a blank square.
        public enum Pieces
        {
            bb, bk, bn, bp, bq, br,

            wb, wk, wn, wp, wq, wr,

            blnk
        }

        public struct Square
        {
            public int row; // row
            public int column; // column

            public Square(int i, int j)
            {
                row = i;
                column = j;
            }
        }

        // Struct to hold the state of a chess piece
        public struct ChessPiece
        {
            public Pieces piece;
            public Square square;

            public ChessPiece(Pieces p, int i, int j)
            {
                piece = p;
                square = new Square(i, j);
            }

            public ChessPiece(Pieces p, Square s)
            {
                piece = p;
                square = s;
            }
        }


        // Stuct to hold the state of the chess board
        public struct ChessBoardState
        {
            public bool WhitesMove;

            public ChessPiece SelectedPiece;

            public List<ChessPiece> WhitePieces;
            public List<ChessPiece> BlackPieces;

            public Pieces[,] Board;

            public Square WhiteKingSquare;
            public Square BlackKingSquare;

            public bool[] WhiteRooksMoved;
            public bool[] BlackRooksMoved;

            public bool WhiteKingMoved;
            public bool BlackKingMoved;

            public Square enPassantMove;

            public List<ChessPiece> enPassantPieces;

            public List<Square> ValidMoves;

            public bool promotePawn;

            public bool WhiteAlwaysQueen;
            public bool BlackAlwaysQueen;

            public List<string> BoardPositions;

        }
        // The Chess board state representing this ChessBoardModel
        public ChessBoardState BoardState;

        

        // Variable to determine if a piece to move was previously selected.
        public bool PieceSelected;

        public BishopMoves BishopMoves;
        public KnightMoves KnightMoves;
        public RookMoves RookMoves;
        public PawnMoves PawnMoves;
        public KingMoves KingMoves;
        public QueenMoves QueenMoves;


        public List<ChessPiece> currentForeGround;
        public List<ChessPiece> previousForeGround;

        public List<ChessPiece> backGroundChanges;

        public bool GAMEOVER;
        public bool MATE;

        // Constructor
        public ChessBoardModel()
        {
            InitializeModel();
            InitializeBoardState();
        }

        // Method to initialize the model variables.
        private void InitializeModel()
        {
            PieceSelected = false;
            currentForeGround = new List<ChessPiece>();
            backGroundChanges = new List<ChessPiece>();

            // Initialize the pieces movement objects.
            BishopMoves = new BishopMoves();
            KnightMoves = new KnightMoves();
            RookMoves = new RookMoves();
            PawnMoves = new PawnMoves();
            KingMoves = new KingMoves();
            QueenMoves = new QueenMoves();

        }

        public void InitializeBoardState()
        {
            // Create new ChessBoardState to hold the state of the chess board.
            BoardState = new ChessBoardState();

            BoardState.WhitesMove = true;

            BoardState.WhitePieces = new List<ChessPiece>();
            BoardState.BlackPieces = new List<ChessPiece>();


            BoardState.Board = new Pieces[8, 8];

            // White Back Rank.
            BoardState.Board[7, 0] = Pieces.wr;
            BoardState.Board[7, 1] = Pieces.wn;
            BoardState.Board[7, 2] = Pieces.wb;
            BoardState.Board[7, 3] = Pieces.wq;
            BoardState.Board[7, 4] = Pieces.wk;
            BoardState.Board[7, 5] = Pieces.wb;
            BoardState.Board[7, 6] = Pieces.wn;
            BoardState.Board[7, 7] = Pieces.wr;

            // White Pawns.
            BoardState.Board[6, 0] = Pieces.wp;
            BoardState.Board[6, 1] = Pieces.wp;
            BoardState.Board[6, 2] = Pieces.wp;
            BoardState.Board[6, 3] = Pieces.wp;
            BoardState.Board[6, 4] = Pieces.wp;
            BoardState.Board[6, 5] = Pieces.wp;
            BoardState.Board[6, 6] = Pieces.wp;
            BoardState.Board[6, 7] = Pieces.wp;

            // Black Back Rank.
            BoardState.Board[0, 0] = Pieces.br;
            BoardState.Board[0, 1] = Pieces.bn;
            BoardState.Board[0, 2] = Pieces.bb;
            BoardState.Board[0, 3] = Pieces.bq;
            BoardState.Board[0, 4] = Pieces.bk;
            BoardState.Board[0, 5] = Pieces.bb;
            BoardState.Board[0, 6] = Pieces.bn;
            BoardState.Board[0, 7] = Pieces.br;

            // Black Pawns.
            BoardState.Board[1, 0] = Pieces.bp;
            BoardState.Board[1, 1] = Pieces.bp;
            BoardState.Board[1, 2] = Pieces.bp;
            BoardState.Board[1, 3] = Pieces.bp;
            BoardState.Board[1, 4] = Pieces.bp;
            BoardState.Board[1, 5] = Pieces.bp;
            BoardState.Board[1, 6] = Pieces.bp;
            BoardState.Board[1, 7] = Pieces.bp;

            for (int i = 2; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    BoardState.Board[i, j] = Pieces.blnk;
                }
            }

            // Add White pieces to list and dictionary.
            for (int i = 6; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ChessPiece piece = new ChessPiece(BoardState.Board[i, j], i, j);
                    BoardState.WhitePieces.Add(piece);
                }
            }

            // Add Black pieces to list and dictionary.
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ChessPiece piece = new ChessPiece(BoardState.Board[i, j], i, j);
                    BoardState.BlackPieces.Add(piece);
                }
            }

            // Initialize the Kings positions (for testing check).
            BoardState.WhiteKingSquare = new Square(7, 4);
            BoardState.BlackKingSquare = new Square(0, 4);

            // Initialize that the rooks have not moved (important for castling).
            BoardState.WhiteRooksMoved = new bool[2]; BoardState.WhiteRooksMoved[0] = false; BoardState.WhiteRooksMoved[1] = false;
            BoardState.BlackRooksMoved = new bool[2]; BoardState.BlackRooksMoved[0] = false; BoardState.BlackRooksMoved[1] = false;

            // Initialize that the Kings have not moved (important for castling).
            BoardState.WhiteKingMoved = false; BoardState.BlackKingMoved = false;

            BoardState.ValidMoves = new List<Square>();

            BoardState.enPassantPieces = new List<ChessPiece>();

            BoardState.promotePawn = false;

            BoardState.BoardPositions = new List<string>();

        }

        // Changes the state of the chessboard (players moves).
        public void ChangeBoardState(int sqr)
        {
            Square square = getSquare(sqr);

            // Condtion to test if a piece as already been selected and we are looking for a possible move.
            if (!PieceSelected)
            {
                // condtion to test if the square selected has a piece of the current player.
                if (currentPlayerPiece(square, ref BoardState))
                {  
                    BoardState.SelectedPiece = new ChessPiece(BoardState.Board[square.row, square.column], square);
                    BoardState.ValidMoves = getValidMoves(ref BoardState);
                    PieceSelected = !PieceSelected;
                    // Console.WriteLine($" {BoardState.ValidMoves.Count} valid moves."); 
                }
                updateForeground();
                return;
            }

            if (BoardState.ValidMoves.Contains(square))
            {
                //Console.WriteLine("Found Valid Square");
                movePiece(ref BoardState, square);

                if (gameOver(ref BoardState))
                {
                    GAMEOVER = true;
                }
            }
            PieceSelected = !PieceSelected;
            // Clear ValidMoves;
            BoardState.ValidMoves.Clear();
            updateForeground();
        }

        // Adds the current board position to the list of board positions
        private void addBoardPosition(ref ChessBoardState currentBoard)
        {
            // Declare boarePosition string to hold the string represtation of the current Board.
            string boardPosition = "";

            // Loops to add each squares piece to the list.
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    // Add square's piece to the boardPostition string.
                    boardPosition += currentBoard.Board[i, j].ToString();
                }
            }

            // Add boardPosition to the list of boardPositions;
            currentBoard.BoardPositions.Add(boardPosition);
        }


        public bool gameOver(ref ChessBoardState chessBoardState)
        {
            // Condition to test that there has been a move.
            if (chessBoardState.BoardPositions.Count > 0)
            {
                // Declare currentPosition to be the last position in the BoardPositions list.
                string currentPosition = chessBoardState.BoardPositions[chessBoardState.BoardPositions.Count - 1];

                // Declare Duplicate Positions list to find the list of positions that are the same as the currentPosition.
                List<string> DuplicatePositions = chessBoardState.BoardPositions.FindAll(p => p.Equals(currentPosition));

                // Condition to test that the position has been reached 3 times indicating a draw.
                if (DuplicatePositions.Count == 3)
                {
                    return true; // the Game is a draw by repitition.
                }
            }

            ChessPiece selectedPiece = chessBoardState.SelectedPiece;

            List<ChessPiece> playerPieces = chessBoardState.WhitesMove ? chessBoardState.WhitePieces : chessBoardState.BlackPieces;

            ChessPiece[] bgChanges = new ChessPiece[backGroundChanges.Count];
            backGroundChanges.CopyTo(bgChanges);

            for (int i = 0; i < playerPieces.Count; i++)
            {
                chessBoardState.SelectedPiece = playerPieces[i];
                if (getValidMoves(ref chessBoardState).Count != 0)
                {
                    backGroundChanges.AddRange(bgChanges);
                    return false;
                }
            }

            chessBoardState.WhitesMove = !chessBoardState.WhitesMove;
            if (Check(ref chessBoardState))
            {
                MATE = true;
            }
            chessBoardState.WhitesMove = !chessBoardState.WhitesMove;
            backGroundChanges.AddRange(bgChanges);

            chessBoardState.SelectedPiece = selectedPiece;

            return true;
        }

        // Method to update the ForeGround lists for the CryptoChessBoardViewModel 
        private void updateForeground()
        {
            previousForeGround = currentForeGround;

            currentForeGround = new List<ChessPiece>();

            for (int i = 0; i < BoardState.ValidMoves.Count; i++)
            {
                currentForeGround.Add(new ChessPiece(Pieces.blnk, BoardState.ValidMoves[i]));
            }
            if (PieceSelected)
            {
                currentForeGround.Add(new ChessPiece(BoardState.SelectedPiece.piece, BoardState.SelectedPiece.square));
            }


        }

        #region movePiece Methods

        public void movePiece(ref ChessBoardState chessBoardState, Square square)
        {
            // Cleare backGroundChanges List.
            backGroundChanges.Clear();

            // Call to KingMove Method to handle King move conditions.
            KingMove(ref chessBoardState, square);

            // Call to RookMove Method to handle Rook move conditions.
            RookMove(ref chessBoardState);

            // Call to enPassant Method to handle enPassant move conditions.
            EnPassant(ref chessBoardState, square);

            // Call to PawnPromotion Method to handle PawnPromotion move conditions.
            PawnPromotion(ref chessBoardState, square);

            // Remove the other players piece.
            if (otherPlayerPiece(square, ref chessBoardState))
            {

                // Initialize otherPlayerList to the other players ChessPiece List.
                List<ChessPiece> otherPlayerList = chessBoardState.WhitesMove ? chessBoardState.BlackPieces : chessBoardState.WhitePieces;

                ChessPiece capturedPiece = otherPlayerList.Find(x => x.square.Equals(square));
                otherPlayerList.Remove(capturedPiece);
            }

            List<ChessPiece> currentPlayerList = chessBoardState.WhitesMove ? chessBoardState.WhitePieces : chessBoardState.BlackPieces;
            
            // Remove the Selected Piece from the current players ChessPiece List.
            currentPlayerList.Remove(chessBoardState.SelectedPiece);

            // Set the Board SelectedPiece position to blank.
            chessBoardState.Board[chessBoardState.SelectedPiece.square.row, chessBoardState.SelectedPiece.square.column] = Pieces.blnk;

            // Set the Boards square position to the SelectedPiece.
            chessBoardState.Board[square.row, square.column] = chessBoardState.SelectedPiece.piece;

            // Declare blnk ChessPiece to remove the selected pieces previous position from the board.
            ChessPiece blnk = new ChessPiece(Pieces.blnk, chessBoardState.SelectedPiece.square);

            // Create a newPiece representing the new ChessPiece.
            ChessPiece newPiece = new ChessPiece(chessBoardState.SelectedPiece.piece, square);

            //Console.WriteLine(currentPlayerList.Count);


            currentPlayerList.Add(newPiece);

            chessBoardState.SelectedPiece = newPiece;

            backGroundChanges.Add(blnk);
            backGroundChanges.Add(newPiece);

            // Clear ValidMoves List.
            chessBoardState.ValidMoves.Clear();

            // Add the current board position to the list of previous board positions. (help detect stalemate)
            addBoardPosition(ref chessBoardState);

            // Change the players move.
            chessBoardState.WhitesMove = !chessBoardState.WhitesMove;
        }

        private void PawnPromotion(ref ChessBoardState chessBoardState, Square square)
        {
            // Condition to test if the SelectedPiece is a pawn.
            if (chessBoardState.SelectedPiece.piece == Pieces.wp || chessBoardState.SelectedPiece.piece == Pieces.bp)
            {
                // Initialize backRank to be the row of the other players backRank.
                int backRank = chessBoardState.WhitesMove ? 0 : 7;
                // Condtion to test if the row the pawn is being moved to is the backRank.
                if (square.row == backRank)
                {
                    // Condition to test if it is WhitesMove and to queen.
                    if (chessBoardState.WhitesMove && chessBoardState.WhiteAlwaysQueen)
                    {
                        chessBoardState.SelectedPiece.piece = Pieces.wq;
                        return;
                    }
                    // Condition to test if it is BlacksMove and to queen.
                    if (!chessBoardState.WhitesMove && chessBoardState.BlackAlwaysQueen)
                    {
                        chessBoardState.SelectedPiece.piece = Pieces.bq;
                        return;
                    }
                    chessBoardState.promotePawn = true;
                    //Console.WriteLine("promote!");
                }
            }
        }

        public void PromotePawn(int param)
        {
            //Console.WriteLine($"{BoardState.SelectedPiece.piece.ToString()}, ({BoardState.SelectedPiece.square.row}, {BoardState.SelectedPiece.square.column} ");
            if (!BoardState.WhitesMove)
            {
                BoardState.WhitePieces.Remove(BoardState.SelectedPiece);
                switch (param)
                {
                    case 0:
                        BoardState.SelectedPiece.piece = Pieces.wn;
                        break;
                    case 1:
                        BoardState.SelectedPiece.piece = Pieces.wb;
                        break;
                    case 2:
                        BoardState.SelectedPiece.piece = Pieces.wr;
                        break;
                    case 3:
                        BoardState.SelectedPiece.piece = Pieces.wq;
                        break;
                }
                BoardState.WhitePieces.Add(BoardState.SelectedPiece);

            }
            else
            {
                BoardState.BlackPieces.Remove(BoardState.SelectedPiece);
                switch (param)
                {
                    case 0:
                        BoardState.SelectedPiece.piece = Pieces.bn;
                        break;
                    case 1:
                        BoardState.SelectedPiece.piece = Pieces.bb;
                        break;
                    case 2:
                        BoardState.SelectedPiece.piece = Pieces.br;
                        break;
                    case 3:
                        BoardState.SelectedPiece.piece = Pieces.bq;
                        break;
                }
                BoardState.BlackPieces.Add(BoardState.SelectedPiece);
            }
            BoardState.Board[BoardState.SelectedPiece.square.row, BoardState.SelectedPiece.square.column] = BoardState.SelectedPiece.piece;
            BoardState.promotePawn = false;
            backGroundChanges = new List<ChessPiece>();
            backGroundChanges.Add(BoardState.SelectedPiece);
            return;
        }

        // Method to handle En Passant move conditions.
        private void EnPassant(ref ChessBoardState chessBoardState, Square square)
        {

            // Declare new enPassantePieces ChessPiece List.
            chessBoardState.enPassantPieces = new List<ChessPiece>();

            // Condition to test if the Selected Piece is a pawn.
            if (chessBoardState.SelectedPiece.piece == Pieces.wp || chessBoardState.SelectedPiece.piece == Pieces.bp)
            { 
                // Initialize pawn offset depending whos move it is.
                int pawnOffset = chessBoardState.WhitesMove ? -1 : 1;

                // Conditon to test if the pawn is moving two spaces ahead indicating a potential en passant.
                if (chessBoardState.SelectedPiece.square.row + (2 * pawnOffset) == square.row)
                {
                    // Initialize otherPawn to be the other players pawn piece.
                    Pieces otherPawn = chessBoardState.WhitesMove ? Pieces.bp : Pieces.wp;

                    // condition to test if an adjacent column exists.
                    if (square.column - 1 >= 0)
                    {
                        // Condition to test if the piece next to the current pawn is a pawn of the other players pieces.
                        if (chessBoardState.Board[square.row, square.column - 1] == otherPawn)
                        {
                            // Add the current players pawn to the enPassantPieces.
                            chessBoardState.enPassantPieces.Add(new ChessPiece(chessBoardState.Board[square.row, square.column - 1], square.row, square.column - 1));
                            // Set the enPassantMove to the square behind the adjacent pawn attacked.
                            chessBoardState.enPassantMove = new Square(square.row - pawnOffset, square.column);
                        }
                    }
                    // condition to test if an adjacent column exists.
                    if (square.column + 1 < 8)
                    {
                        // Condition to test if the pices next to the current pawn is a pawn of the other players pieces.
                        if (chessBoardState.Board[square.row, square.column + 1] == otherPawn)
                        {
                            // Add the current players pawn to the enPassantPieces.
                            chessBoardState.enPassantPieces.Add(new ChessPiece(chessBoardState.Board[square.row, square.column + 1], square.row, square.column + 1));
                            // Set the enPassantMove to the square behind the adjacent pawn attacked.
                            chessBoardState.enPassantMove = new Square(square.row - pawnOffset, square.column);
                        }
                    }
                    return;
                }
                
                // Condition to test if the pawn move being made is an enpassant move.
                if (square.Equals(chessBoardState.enPassantMove))
                {
                    // remove the other players pawn.
                    int row = square.row - pawnOffset; int column = square.column;

                    // Set the other players pawn location to blank piece.
                    chessBoardState.Board[row, column] = Pieces.blnk;

                    // Initialize otherPlayerList to the other players ChessPiece List.
                    List<ChessPiece> otherPlayerList = chessBoardState.WhitesMove ? chessBoardState.BlackPieces : chessBoardState.WhitePieces;
                    
                    // Initialize capturedPiece to be the piece captured by enpassant.
                    ChessPiece capturedPiece = otherPlayerList.Find(x => x.square.Equals(new Square(row, column)));
                    // Remove the capturedPiece from the otherPlayerList.
                    otherPlayerList.Remove(capturedPiece);

                    // Set the captured pieces square to blank.
                    ChessPiece blnk = new ChessPiece(Pieces.blnk, new Square(row, column));
                    // Add blank Square to backGroundChanges.
                    backGroundChanges.Add(blnk);
                }
            }
           
        }

        // Method to test if a rook is being moved.
        private void RookMove(ref ChessBoardState chessBoardState)
        {
            // Condition to test if the Selected Piece is a rook.
            if (chessBoardState.SelectedPiece.piece == Pieces.wr || chessBoardState.SelectedPiece.piece == Pieces.br)
            {
                // Condition to test if it is WhitesMove
                if (chessBoardState.WhitesMove)
                {
                    // White King rook
                    if (chessBoardState.WhiteRooksMoved[0] == false && chessBoardState.SelectedPiece.square.Equals(new Square(7, 7)))
                    {
                        chessBoardState.WhiteRooksMoved[0] = true;
                    }
                    // White Queen rook
                    if (chessBoardState.WhiteRooksMoved[1] == false && chessBoardState.SelectedPiece.square.Equals(new Square(7, 0)))
                    {
                        chessBoardState.WhiteRooksMoved[1] = true;
                    }
                    return;
                }
                // Black King rook
                if (chessBoardState.BlackRooksMoved[0] == false && chessBoardState.SelectedPiece.square.Equals(new Square(0, 7)))
                {
                    chessBoardState.BlackRooksMoved[0] = true;
                }
                // Black Queen rook
                if (chessBoardState.BlackRooksMoved[1] == false && chessBoardState.SelectedPiece.square.Equals(new Square(0, 0)))
                {
                    chessBoardState.BlackRooksMoved[1] = true;
                }
            }
        }

        // Method to handle King moves.
        private void KingMove(ref ChessBoardState chessBoardState, Square square)
        {
            // Condition to test if the SelectedPiece is a king.
            if (chessBoardState.SelectedPiece.piece == Pieces.wk || chessBoardState.SelectedPiece.piece == Pieces.bk)
            {
                // Initialize currentPlayerList to the current players ChessPiece List.
                List<ChessPiece> currentPlayerList = chessBoardState.WhitesMove ? chessBoardState.WhitePieces : chessBoardState.BlackPieces;

                // Initialize the KingSquare to the current players King Square.
                Square KingSquare = chessBoardState.WhitesMove ? chessBoardState.WhiteKingSquare : chessBoardState.BlackKingSquare;

                // Initialize the KingMoved bool to the current KindMoved bool.
                bool KingMoved = chessBoardState.WhitesMove ? chessBoardState.WhiteKingMoved : chessBoardState.BlackKingMoved;

                // Initialize the prevKingMoved bool to the current KingMoved bool
                bool prevKingMoved = chessBoardState.WhitesMove ? chessBoardState.WhiteKingMoved : chessBoardState.BlackKingMoved;

                // Initialize the RooksMoved bool array to the current players Rooks Moved array.
                bool[] RooksMoved = chessBoardState.WhitesMove ? chessBoardState.WhiteRooksMoved : chessBoardState.BlackRooksMoved;

                // Initialize the rook to current players rook piece;
                Pieces rook = chessBoardState.WhitesMove ? Pieces.wr : Pieces.br;

                // Condition to test if the King has been moved.
                if (!KingMoved)
                {
                    //King Side Castle
                    if (square.Equals(new Square(KingSquare.row, 6)))
                    {
                        // Set King Side rook to true;
                        RooksMoved[0] = true;

                        // Remove the King side rook from the currentPlayer ChessPiece List.
                        currentPlayerList.Remove(new ChessPiece(rook, new Square(KingSquare.row, 7)));

                        // Set Board King side rook square to blank.
                        chessBoardState.Board[KingSquare.row, 7] = Pieces.blnk;

                        // Set Board King side rook castling square to rook.
                        chessBoardState.Board[KingSquare.row, 5] = rook;

                        // Initialize newRook ChessPiece to reprent the new rook.
                        ChessPiece newRook = new ChessPiece(rook, new Square(KingSquare.row, 5));

                        // Add the newRook to the currentPlayers ChessPieces
                        currentPlayerList.Add(newRook);

                        // Add new Blank and rook ChessPieces to the backgroundChanges.
                        backGroundChanges.Add(new ChessPiece(Pieces.blnk, new Square(KingSquare.row, 7)));
                        backGroundChanges.Add(newRook);
                    }
                    //Queen Side Castle
                    if (square.Equals(new Square(KingSquare.row, 2)))
                    {
                        // Set Queen Side rook to true;
                        RooksMoved[1] = true;

                        // Remove the Queen side rook from the currentPlayer ChessPiece List.
                        currentPlayerList.Remove(new ChessPiece(rook, new Square(KingSquare.row, 0)));

                        // Set Board Queen side rook square to blank.
                        chessBoardState.Board[KingSquare.row, 0] = Pieces.blnk;

                        // Set Board Queen side rook castling square to rook.
                        chessBoardState.Board[KingSquare.row, 3] = rook;

                        // Initialize newRook ChessPiece to reprent the new rook.
                        ChessPiece newRook = new ChessPiece(rook, new Square(KingSquare.row, 3));

                        // Add the newRook to the currentPlayers ChessPieces
                        currentPlayerList.Add(newRook);

                        // Add new Blank and rook ChessPieces to the backgroundChanges.
                        backGroundChanges.Add(new ChessPiece(Pieces.blnk, new Square(KingSquare.row, 0)));
                        backGroundChanges.Add(newRook);
                    }
                    KingMoved = true;
                }

                // Condition to test if it is WhitesMove
                if (chessBoardState.WhitesMove)
                {
                    chessBoardState.WhitePieces = currentPlayerList;

                    // Update White King moved info
                    chessBoardState.WhiteKingMoved = KingMoved;

                    // Update White Rook moved info
                    chessBoardState.WhiteRooksMoved = RooksMoved;
                    
                    // Update ChessBoardState KingSquare info.
                    chessBoardState.WhiteKingSquare = square;
                }
                else
                {
                    chessBoardState.BlackPieces = currentPlayerList;

                    // Update White King moved info
                    chessBoardState.BlackKingMoved = KingMoved;

                    // Update White Rook moved info
                    chessBoardState.BlackRooksMoved = RooksMoved;

                    // Update ChessBoardState KingSquare info.
                    chessBoardState.BlackKingSquare = square;
                }
            }
        }

        #endregion

        #region getValidMoves Methods
        // Method to return the valid moves of the selected piece.
        public List<Square> getValidMoves(ref ChessBoardState chessBoardState)
        {

            List<Square> validMoves = new List<Square>();

            ChessBoardState cBoard = cloneChessBoardState(ref chessBoardState);

            List<Square> potentialMoves = getPotentialMoves(ref cBoard);

            //Console.WriteLine(potentialMoves.Count);

            for (int i = 0; i < potentialMoves.Count; i++)
            {
                //Console.WriteLine($"{potentialMoves[i].row}, {potentialMoves[i].column}");
                movePiece(ref cBoard, potentialMoves[i]);
                if (!Check(ref cBoard))
                {
                    validMoves.Add(potentialMoves[i]);
                }
                cBoard = cloneChessBoardState(ref chessBoardState); // TODO undo move instead.
            }

            backGroundChanges = new List<ChessPiece>();

            if (chessBoardState.SelectedPiece.piece == Pieces.wk || chessBoardState.SelectedPiece.piece == Pieces.bk)
            {
                validCastling(ref chessBoardState, ref validMoves); ;
            }
            //validMoves = potentialMoves;
            return validMoves;
        }


          
        private void validCastling(ref ChessBoardState chessBoardState, ref List<Square> moves)
        {
            //King Side Squares 
            Square s1 = new Square(chessBoardState.SelectedPiece.square.row, chessBoardState.SelectedPiece.square.column + 1);
            Square s2 = new Square(chessBoardState.SelectedPiece.square.row, chessBoardState.SelectedPiece.square.column + 2);

            // Valid King side castle
            if (moves.Contains(s2) && !moves.Contains(s1))
            {
                moves.Remove(s2);
            }

            //Queen Side Squares 
            Square s3 = new Square(chessBoardState.SelectedPiece.square.row, chessBoardState.SelectedPiece.square.column - 1);
            Square s4 = new Square(chessBoardState.SelectedPiece.square.row, chessBoardState.SelectedPiece.square.column - 2);

            if (moves.Contains(s4) && !moves.Contains(s3))
            {
                moves.Remove(s4);
            }

            // Condition to see if king is in Check.
            
            chessBoardState.WhitesMove = !chessBoardState.WhitesMove;
            if(Check(ref chessBoardState))
            {
                //Console.WriteLine($"King in check can't castle");
                if (moves.Contains(s2)) // Remove castling moves if in Check.
                {
                    moves.Remove(s2);
                }
                if (moves.Contains(s4))
                {
                    moves.Remove(s4);
                }
            }
            chessBoardState.WhitesMove = !chessBoardState.WhitesMove;
        }


        //Method to test if the other player is in check.
        public bool Check(ref ChessBoardState chessBoardState)
        {
            List<ChessPiece> playersPieces = chessBoardState.WhitesMove ? chessBoardState.WhitePieces : chessBoardState.BlackPieces;

            //Square otherKingSquare = chessBoardState.WhitesMove ? chessBoardState.BlackKingSquare : chessBoardState.WhiteKingSquare;

            ChessPiece selectedPiece = chessBoardState.SelectedPiece;

            for (int i = 0; i < playersPieces.Count; i++)
            {
                chessBoardState.SelectedPiece = playersPieces[i];
                //Console.Write($" {chessBoardState.SelectedPiece.piece.ToString()}");
                /*List<Square> playerPotentialMoves = getPotentialMoves(ref chessBoardState);
                for (int j = 0; j < playerPotentialMoves.Count; j++)
                {
                    if (playerPotentialMoves[j].Equals(otherKingSquare))
                    {
                        chessBoardState.SelectedPiece = selectedPiece;
                        return true;
                    }
                }*/
                if(attackingKing(ref chessBoardState))
                {
                    chessBoardState.SelectedPiece = selectedPiece;
                    return true;
                }
            }
            chessBoardState.SelectedPiece = selectedPiece;
            return false;
        }

        // Method to determine of the SelectedPiece is attacking the other players king 
        public bool attackingKing(ref ChessBoardState chessBoardState)
        {
            // Switch Condition to test which piece is selected.
            switch (chessBoardState.SelectedPiece.piece)
            {
                // return if the piece is attacking the other players king.
                case Pieces.bb: return BishopMoves.attackingKing(ref chessBoardState);
                case Pieces.bk: return KingMoves.attackingKing(ref chessBoardState);
                case Pieces.bn: return KnightMoves.attakingKing(ref chessBoardState);
                case Pieces.bp: return PawnMoves.attakingKing(ref chessBoardState);
                case Pieces.bq: return QueenMoves.attackingKing(ref chessBoardState);
                case Pieces.br: return RookMoves.attackingKing(ref chessBoardState);

                case Pieces.wb: return BishopMoves.attackingKing(ref chessBoardState);
                case Pieces.wk: return KingMoves.attackingKing(ref chessBoardState);
                case Pieces.wn: return KnightMoves.attakingKing(ref chessBoardState);
                case Pieces.wp: return PawnMoves.attakingKing(ref chessBoardState);
                case Pieces.wq: return QueenMoves.attackingKing(ref chessBoardState);
                case Pieces.wr: return RookMoves.attackingKing(ref chessBoardState);
            }

            return false;
        }

        public List<Square> getPotentialMoves(ref ChessBoardState chessBoardState)
        {
            List<Square> potentialMoves = new List<Square>();

            // Get the potential moves for the selected Piece
            Pieces piece = chessBoardState.SelectedPiece.piece;
            switch (piece)
            {
                case Pieces.bb: potentialMoves = BishopMoves.getPotentialMoves(ref chessBoardState); break;
                case Pieces.bk: potentialMoves = KingMoves.getPotentialMoves(ref chessBoardState); break;
                case Pieces.bn: potentialMoves = KnightMoves.getPotentialMoves(ref chessBoardState); break;
                case Pieces.bp: potentialMoves = PawnMoves.getPotentialMoves(ref chessBoardState); break;
                case Pieces.bq: potentialMoves = QueenMoves.getPotentialMoves(ref chessBoardState); break;
                case Pieces.br: potentialMoves = RookMoves.getPotentialMoves(ref chessBoardState); break;

                case Pieces.wb: potentialMoves = BishopMoves.getPotentialMoves(ref chessBoardState); break;
                case Pieces.wk: potentialMoves = KingMoves.getPotentialMoves(ref chessBoardState); break;
                case Pieces.wn: potentialMoves = KnightMoves.getPotentialMoves(ref chessBoardState); break;
                case Pieces.wp: potentialMoves = PawnMoves.getPotentialMoves(ref chessBoardState); break;
                case Pieces.wq: potentialMoves = QueenMoves.getPotentialMoves(ref chessBoardState); break;
                case Pieces.wr: potentialMoves = RookMoves.getPotentialMoves(ref chessBoardState); break;

            }
            return potentialMoves;
        }

        #endregion

        // Method to clone a ChessBoardState
        public ChessBoardState cloneChessBoardState(ref ChessBoardState chessBoardState)
        {
            ChessBoardState cBoard = new ChessBoardState();

            cBoard.WhitesMove = chessBoardState.WhitesMove;

            cBoard.SelectedPiece = chessBoardState.SelectedPiece;

            cBoard.WhitePieces = new List<ChessPiece>();

            cBoard.BlackPieces = new List<ChessPiece>();

            foreach (ChessPiece p in chessBoardState.WhitePieces)
            {
                cBoard.WhitePieces.Add(p);
            }

            foreach (ChessPiece p in chessBoardState.BlackPieces)
            {
                cBoard.BlackPieces.Add(p);
            }

            cBoard.Board = new Pieces[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    cBoard.Board[i, j] = chessBoardState.Board[i, j];
                }
            }

            cBoard.WhiteKingSquare = new Square(chessBoardState.WhiteKingSquare.row, chessBoardState.WhiteKingSquare.column);
            cBoard.BlackKingSquare = new Square(chessBoardState.BlackKingSquare.row, chessBoardState.BlackKingSquare.column);

            cBoard.WhiteRooksMoved = new bool[2]; cBoard.WhiteRooksMoved[0] = chessBoardState.WhiteRooksMoved[0]; cBoard.WhiteRooksMoved[1] = chessBoardState.WhiteRooksMoved[1];
            cBoard.BlackRooksMoved = new bool[2]; cBoard.BlackRooksMoved[0] = chessBoardState.BlackRooksMoved[0]; cBoard.BlackRooksMoved[1] = chessBoardState.BlackRooksMoved[1];

            cBoard.WhiteKingMoved = chessBoardState.WhiteKingMoved;
            cBoard.BlackKingMoved = chessBoardState.BlackKingMoved;

            cBoard.ValidMoves = new List<Square>();

            foreach (Square s in chessBoardState.ValidMoves)
            {
                cBoard.ValidMoves.Add(s);
            }

            cBoard.enPassantPieces = new List<ChessPiece>();
            foreach (ChessPiece p in chessBoardState.enPassantPieces)
            {
                cBoard.enPassantPieces.Add(p);
            }

            cBoard.enPassantMove = new Square(chessBoardState.enPassantMove.row, chessBoardState.enPassantMove.column);

            cBoard.promotePawn = chessBoardState.promotePawn;

            cBoard.WhiteAlwaysQueen = chessBoardState.WhiteAlwaysQueen;

            cBoard.BlackAlwaysQueen = chessBoardState.BlackAlwaysQueen;


            cBoard.BoardPositions = new List<string>();

            foreach (string s in chessBoardState.BoardPositions)
            {
                cBoard.BoardPositions.Add(s);
            }

            return cBoard;
        }

        // Method to determine if the square analyzed has one of the current players pieces on it.
        private bool currentPlayerPiece(Square square, ref ChessBoardState chessBoardState)
        {
            if (chessBoardState.WhitesMove)
            {
                switch (chessBoardState.Board[square.row, square.column])
                {
                    case Pieces.wp: return true;
                    case Pieces.wn: return true;
                    case Pieces.wb: return true;
                    case Pieces.wr: return true;
                    case Pieces.wq: return true;
                    case Pieces.wk: return true;
                }
                return false;
            }
            switch (chessBoardState.Board[square.row, square.column])
            {
                case Pieces.bp: return true;
                case Pieces.bn: return true;
                case Pieces.bb: return true;
                case Pieces.br: return true;
                case Pieces.bq: return true;
                case Pieces.bk: return true;
            }

            return false;
        }

        // Method to determine if the square analyzed has one of the other players pieces on it.
        public static bool otherPlayerPiece(Square square, ref ChessBoardState chessBoardState)
        {
            if (chessBoardState.WhitesMove)
            {
                switch (chessBoardState.Board[square.row, square.column])
                {
                    case Pieces.bp: return true;
                    case Pieces.bn: return true;
                    case Pieces.bb: return true;
                    case Pieces.br: return true;
                    case Pieces.bq: return true;
                    case Pieces.bk: return true;
                }
                return false;
            }
            switch (chessBoardState.Board[square.row, square.column])
            {
                case Pieces.wp: return true;
                case Pieces.wn: return true;
                case Pieces.wb: return true;
                case Pieces.wr: return true;
                case Pieces.wq: return true;
                case Pieces.wk: return true;
            }

            return false;
        }

        // Method to return the square given by the CryptoChessBoardViewModel.
        public Square getSquare(int square)
        {
            Square s = new Square();
            s.row = square / 8;
            s.column = square % 8;
            return s;
        }
    }
}
