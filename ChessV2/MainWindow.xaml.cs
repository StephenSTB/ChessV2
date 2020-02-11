using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Declare ChessBoard page.
        ChessBoard ChessBoard;

        // Declare ChessViewModel.
        ChessViewModel ViewModel;

        // Declare ChessBoardModel.
        ChessBoardModel ChessBoardModel;

        public MainWindow()
        {
            InitializeComponent();

            ChessBoardModel = new ChessBoardModel();                // Initialize ChessBoardModel.

            ViewModel =  new ChessViewModel(ref ChessBoardModel);   // Initialize ViewModel.

            ChessBoard = new ChessBoard(ref ViewModel);             // Initialize ChessBoard.

            ChessBoardFrame.Content = ChessBoard;                   // Set ChessBoardFrame content to the ChessBoard.

            DataContext = ViewModel;                                // Set the DataContext to the ViewModel.

            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //e.Cancel = true;
            //Do whatever you want here..
            Console.WriteLine("Close");
            ViewModel.CloseEngines();
        }
    }

}
