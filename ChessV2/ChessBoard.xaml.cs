using System;
using System.Collections.Generic;
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
    /// Interaction logic for ChessBoard.xaml
    /// </summary>
    public partial class ChessBoard : Page
    {
       
        public ChessBoard(ChessViewModel ViewModel)
        {
            InitializeComponent();

            // Set the data context
            DataContext = ViewModel;
        }
        
        /*
        public void Reset()
        {
            ChessBoardModel = new ChessBoardModel();
            ViewModel = new ChessViewModel(ChessBoardModel);
            DataContext = ViewModel;
        }

        public void Flip()
        {
            ViewModel.FlipBoard();
        }*/
    }
}
