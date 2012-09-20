using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for Puzzle1.xaml
    /// </summary>
    public partial class Puzzle
    {
        #region PRIVATE PROPERTIES
        //to make it compile! todo: remove or fix gazeaugmented stuff nicely
        //todo static... why?
        public static Canvas TransCanvas;
        public static Canvas HitCanvas;
        public static StackPanel _puzzleHostingPanel;
        public static Point SmoothGazePoint;
        public static Image TargetImage;
        public static TextBlock TbMouse;


        private PuzzleGrid _mousePuzzleGrid;
        private readonly PuzzleConfig _puzzleNumber;

        #endregion
        
        public Puzzle(PuzzleConfig puzzleNumber)
        {
            InitializeComponent();
            _puzzleNumber = puzzleNumber;
        }

        private void NewGame()
        {
            if (_mousePuzzleGrid != null)
                PuzzleHostingPanel.Children.Remove(_mousePuzzleGrid);

            _mousePuzzleGrid = new PuzzleGrid(_puzzleNumber, InteractionMode.PureMouse);
            PuzzleHostingPanel.Children.Add(_mousePuzzleGrid);
            PuzzleHostingPanel.IsEnabled = true;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            NewGame();
        }
        
        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q)
            {
                Window thankYou = new ThankYou();
                thankYou.Show();
                Close();
            }
        }
    }
}
