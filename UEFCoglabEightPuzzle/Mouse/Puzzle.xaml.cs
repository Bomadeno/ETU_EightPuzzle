using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for Puzzle1.xaml
    /// </summary>
    public partial class Puzzle : Window
    {
        #region PRIVATE PROPERTIES
        //to make it compile! todo: remove or fix gazeaugmented stuff nicely
        public static Canvas _transCanvas;
        public static Canvas _hitCanvas;
        public static StackPanel _puzzleHostingPanel;
        public static Point _smoothGazePoint;
        public static Image _targetImage;
        public static TextBlock tbMouse;


        PuzzleGrid mousePuzzleGrid;
        private PuzzleConfig puzzleNumber;

        #endregion
        
        public Puzzle(PuzzleConfig puzzleNumber)
        {
            InitializeComponent();
            this.puzzleNumber = puzzleNumber;
        }

        private void NewGame()
        {
            //check if a puzzle already exists
            if (mousePuzzleGrid != null)
                PuzzleHostingPanel.Children.Remove(mousePuzzleGrid); //true remove it

            mousePuzzleGrid = new PuzzleGrid(puzzleNumber, InteractionMode.PureMouse);
            PuzzleHostingPanel.Children.Add(mousePuzzleGrid);
            PuzzleHostingPanel.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //start a new game when the window is loaded
            NewGame();
        }
        
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q)
            {
                Window thankYou = new ThankYou();
                thankYou.Show();
                this.Close();
            }
        }
    }
}
