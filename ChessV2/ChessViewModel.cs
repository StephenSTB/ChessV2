﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessV2
{
    public class ChessViewModel : ViewModelBase
    {

        ChessBoardModel ChessBoard;

        // Field for the chess piece image source's 
        public ObservableCollection<ImageSource> ChessBoardBackgroundImageSources { get; set; }

        public ObservableCollection<ImageSource> ChessBoardForegroundImageSources { get; set; }

        public ObservableCollection<ImageSource> PromoteImages { get; set; }

        #region Images

        // White Piece Images
        BitmapImage wr = new BitmapImage(new Uri("../../Images/pieces/wr.png", UriKind.Relative));

        BitmapImage wn = new BitmapImage(new Uri("../../Images/pieces/wn.png", UriKind.Relative));

        BitmapImage wb = new BitmapImage(new Uri("../../Images/pieces/wb.png", UriKind.Relative));

        BitmapImage wq = new BitmapImage(new Uri("../../Images/pieces/wq.png", UriKind.Relative));

        BitmapImage wk = new BitmapImage(new Uri("../../Images/pieces/wk.png", UriKind.Relative));

        BitmapImage wp = new BitmapImage(new Uri("../../Images/pieces/wp.png", UriKind.Relative));

        // Black Piece Images

        BitmapImage br = new BitmapImage(new Uri("../../Images/pieces/br.png", UriKind.Relative));

        BitmapImage bn = new BitmapImage(new Uri("../../Images/pieces/bn.png", UriKind.Relative));

        BitmapImage bb = new BitmapImage(new Uri("../../Images/pieces/bb.png", UriKind.Relative));

        BitmapImage bq = new BitmapImage(new Uri("../../Images/pieces/bq.png", UriKind.Relative));

        BitmapImage bk = new BitmapImage(new Uri("../../Images/pieces/bk.png", UriKind.Relative));

        BitmapImage bp = new BitmapImage(new Uri("../../Images/pieces/bp.png", UriKind.Relative));

        // Select Images

        BitmapImage blnk = new BitmapImage(new Uri("../../Images/select/blnk.png", UriKind.Relative));

        BitmapImage shdw = new BitmapImage(new Uri("../../Images/select/shdw.png", UriKind.Relative));

        BitmapImage slct = new BitmapImage(new Uri("../../Images/select/slct.png", UriKind.Relative));

        #endregion


        // Command executed to test if the Board can be changed.
        private readonly DelegateCommand _changeBoardCommand;
        public ICommand ChangeBoardCommand => _changeBoardCommand;

        // Command executed when a square in pawn promotion window is pressed
        private readonly DelegateCommand _promoteCommand;
        public ICommand PromoteCommand => _promoteCommand;


        // Command executed when Reset button is pressed.
        private readonly DelegateCommand _resetBoardCommand;
        public ICommand ResetBoardCommand => _resetBoardCommand;

        // Command executed when Flip Board a button is pressed.
        private readonly DelegateCommand _flipBoardCommand;
        public ICommand FlipBoardCommand => _flipBoardCommand;


        public ImageSource _BoardImage;
        public ImageSource BoardImage
        {
            get => _BoardImage;
            set => SetProperty(ref _BoardImage, value);
        }

        public Visibility _GameOverVisibility;

        public Visibility GameOverVisibility
        {
            get => _GameOverVisibility;
            set => SetProperty(ref _GameOverVisibility, value);
        }

        public string _GameOverText;

        public string GameOverText
        {
            get => _GameOverText;
            set => SetProperty(ref _GameOverText, value);
        }

        bool BoardFliped;

        // Window to promote pawns.
        PawnPromotionWindow ppw;

        public ChessViewModel(ref ChessBoardModel chessBoard)
        {
            _changeBoardCommand = new DelegateCommand(ChangeBoard, CanChangeBoard);
            _promoteCommand = new DelegateCommand(PromotePawn, CanChangeBoard);
            
            // Initialize _resetBoard and _flipBoard Commands. 
            _resetBoardCommand = new DelegateCommand(ResetBoard, CanChangeBoard);
            _flipBoardCommand = new DelegateCommand(FlipBoard, CanChangeBoard);

            BoardImage = new BitmapImage(new Uri("../../Images/board/board.jpg", UriKind.Relative));

            ChessBoardBackgroundImageSources = new ObservableCollection<ImageSource>();
            ChessBoardForegroundImageSources = new ObservableCollection<ImageSource>();

            ChessBoard = chessBoard;

            GameOverVisibility = Visibility.Collapsed;

            GameOverText = "";

            BoardFliped = false;

            InitializeBoardImages();
        }

        private void PromotePawn(object commandParameter)
        {
            int param;
            int.TryParse(commandParameter.ToString(), out param);
            ChessBoard.PromotePawn(param);
            ppw.Close();
            UpdateBoard();
        }

        private bool CanChangeBoard(object arg)
        {
            return true;
        }

        private void ChangeBoard(object commandParameter)
        {
            if (ChessBoard.BoardState.promotePawn)
            {
                return;
            }
            int square;
            int.TryParse(commandParameter.ToString(), out square);

            square = BoardFliped ? 63 - square : square;

            ChessBoard.ChangeBoardState(square);

            UpdateBoard();
            if (ChessBoard.GAMEOVER)
            {
                if (ChessBoard.MATE)
                {
                    Console.WriteLine("Checkmate!");
                    GameOverVisibility = Visibility.Visible;
                    if (ChessBoard.BoardState.WhitesMove)
                    {
                        GameOverText = "Game Over \n Black Won!";
                        return;
                    }
                    GameOverText = "Game Over \n White Won!";
                    return;
                }
                GameOverVisibility = Visibility.Visible;
                return;
            }

            if (ChessBoard.BoardState.promotePawn)
            {
                PawnPromotion();
            }
            //Console.WriteLine(square);

        }

        private void PawnPromotion()
        {
            PromoteImages = new ObservableCollection<ImageSource>();
            if (!ChessBoard.BoardState.WhitesMove)
            {
                PromoteImages.Add(wn);
                PromoteImages.Add(wb);
                PromoteImages.Add(wr);
                PromoteImages.Add(wq);
            }

            PromoteImages.Add(bn);
            PromoteImages.Add(bb);
            PromoteImages.Add(br);
            PromoteImages.Add(bq);

            ppw = new PawnPromotionWindow(this);
            ppw.Show();

        }

        private void ResetBoard(object obj)
        {
            ChessBoard = new ChessBoardModel();

            GameOverVisibility = Visibility.Collapsed;

            ResetChessBoardImages();
        }

        private void UpdateBoard()
        {

            for (int i = 0; i < ChessBoard.backGroundChanges.Count; i++)
            {
                int s = (ChessBoard.backGroundChanges[i].square.row * 8) + ChessBoard.backGroundChanges[i].square.column;
                s = BoardFliped ? 63 - s : s;
                switch (ChessBoard.backGroundChanges[i].piece)
                {
                    // White Pieces
                    case ChessBoardModel.Pieces.wr:
                        ChessBoardBackgroundImageSources[s] = wr;
                        break;
                    case ChessBoardModel.Pieces.wn:
                        ChessBoardBackgroundImageSources[s] = wn;
                        break;
                    case ChessBoardModel.Pieces.wb:
                        ChessBoardBackgroundImageSources[s] = wb;
                        break;
                    case ChessBoardModel.Pieces.wq:
                        ChessBoardBackgroundImageSources[s] = wq;
                        break;
                    case ChessBoardModel.Pieces.wk:
                        ChessBoardBackgroundImageSources[s] = wk;
                        break;
                    case ChessBoardModel.Pieces.wp:
                        ChessBoardBackgroundImageSources[s] = wp;
                        break;

                    // Black Pieces
                    case ChessBoardModel.Pieces.br:
                        ChessBoardBackgroundImageSources[s] = br;
                        break;
                    case ChessBoardModel.Pieces.bn:
                        ChessBoardBackgroundImageSources[s] = bn;
                        break;
                    case ChessBoardModel.Pieces.bb:
                        ChessBoardBackgroundImageSources[s] = bb;
                        break;
                    case ChessBoardModel.Pieces.bq:
                        ChessBoardBackgroundImageSources[s] = bq;
                        break;
                    case ChessBoardModel.Pieces.bk:
                        ChessBoardBackgroundImageSources[s] = bk;
                        break;
                    case ChessBoardModel.Pieces.bp:
                        ChessBoardBackgroundImageSources[s] = bp;
                        break;

                    //Blank
                    case ChessBoardModel.Pieces.blnk:
                        ChessBoardBackgroundImageSources[s] = blnk;
                        break;

                }
            }

            for (int i = 0; i < ChessBoard.previousForeGround.Count; i++)
            {
                int s = (ChessBoard.previousForeGround[i].square.row * 8) + ChessBoard.previousForeGround[i].square.column;
                s = BoardFliped ? 63 - s : s;
                ChessBoardForegroundImageSources[s] = blnk;
            }

            for (int i = 0; i < ChessBoard.currentForeGround.Count; i++)
            {
                int s = (ChessBoard.currentForeGround[i].square.row * 8) + ChessBoard.currentForeGround[i].square.column;
                s = BoardFliped ? 63 - s : s;
                switch (ChessBoard.currentForeGround[i].piece)
                {
                    case ChessBoardModel.Pieces.blnk:
                        ChessBoardForegroundImageSources[s] = shdw;
                        break;
                    default:
                        ChessBoardForegroundImageSources[s] = slct;
                        break;
                }


            }

        }

        private void InitializeBoardImages()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    switch (ChessBoard.BoardState.Board[i, j])
                    {
                        // White
                        case ChessBoardModel.Pieces.wr:
                            ChessBoardBackgroundImageSources.Add(wr);
                            break;
                        case ChessBoardModel.Pieces.wn:
                            ChessBoardBackgroundImageSources.Add(wn);
                            break;
                        case ChessBoardModel.Pieces.wb:
                            ChessBoardBackgroundImageSources.Add(wb);
                            break;
                        case ChessBoardModel.Pieces.wq:
                            ChessBoardBackgroundImageSources.Add(wq);
                            break;
                        case ChessBoardModel.Pieces.wk:
                            ChessBoardBackgroundImageSources.Add(wk);
                            break;
                        case ChessBoardModel.Pieces.wp:
                            ChessBoardBackgroundImageSources.Add(wp);
                            break;

                        // Black
                        case ChessBoardModel.Pieces.br:
                            ChessBoardBackgroundImageSources.Add(br);
                            break;
                        case ChessBoardModel.Pieces.bn:
                            ChessBoardBackgroundImageSources.Add(bn);
                            break;
                        case ChessBoardModel.Pieces.bb:
                            ChessBoardBackgroundImageSources.Add(bb);
                            break;
                        case ChessBoardModel.Pieces.bq:
                            ChessBoardBackgroundImageSources.Add(bq);
                            break;
                        case ChessBoardModel.Pieces.bk:
                            ChessBoardBackgroundImageSources.Add(bk);
                            break;
                        case ChessBoardModel.Pieces.bp:
                            ChessBoardBackgroundImageSources.Add(bp);
                            break;

                        //Blank
                        case ChessBoardModel.Pieces.blnk:
                            ChessBoardBackgroundImageSources.Add(blnk);
                            break;

                    }

                    ChessBoardForegroundImageSources.Add(blnk);
                }
            }
        }

        public void FlipBoard(object obj)
        {
            BoardFliped = !BoardFliped;
            ChessBoard.PieceSelected = false;
            ResetChessBoardImages();
        }

        private void ResetChessBoardImages()
        {
            for (int i = 0; i < 64; i++)
            {
                int s = BoardFliped ? 63 - i : i;
                switch (ChessBoard.BoardState.Board[i / 8, i % 8])
                {
                    // White Pieces
                    case ChessBoardModel.Pieces.wr:
                        ChessBoardBackgroundImageSources[s] = wr;
                        break;
                    case ChessBoardModel.Pieces.wn:
                        ChessBoardBackgroundImageSources[s] = wn;
                        break;
                    case ChessBoardModel.Pieces.wb:
                        ChessBoardBackgroundImageSources[s] = wb;
                        break;
                    case ChessBoardModel.Pieces.wq:
                        ChessBoardBackgroundImageSources[s] = wq;
                        break;
                    case ChessBoardModel.Pieces.wk:
                        ChessBoardBackgroundImageSources[s] = wk;
                        break;
                    case ChessBoardModel.Pieces.wp:
                        ChessBoardBackgroundImageSources[s] = wp;
                        break;

                    // Black Pieces
                    case ChessBoardModel.Pieces.br:
                        ChessBoardBackgroundImageSources[s] = br;
                        break;
                    case ChessBoardModel.Pieces.bn:
                        ChessBoardBackgroundImageSources[s] = bn;
                        break;
                    case ChessBoardModel.Pieces.bb:
                        ChessBoardBackgroundImageSources[s] = bb;
                        break;
                    case ChessBoardModel.Pieces.bq:
                        ChessBoardBackgroundImageSources[s] = bq;
                        break;
                    case ChessBoardModel.Pieces.bk:
                        ChessBoardBackgroundImageSources[s] = bk;
                        break;
                    case ChessBoardModel.Pieces.bp:
                        ChessBoardBackgroundImageSources[s] = bp;
                        break;

                    //Blank
                    case ChessBoardModel.Pieces.blnk:
                        ChessBoardBackgroundImageSources[s] = blnk;
                        break;

                }

                ChessBoardForegroundImageSources[s] = blnk;
            }
        }
    }
}
