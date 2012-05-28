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
        //to make it compile! todo: remove or fix gaxeaugmented stuff nicely
        public static Canvas _transCanvas;
        public static Canvas _hitCanvas;
        public static StackPanel _puzzleHostingPanel;
        public static Point _smoothGazePoint;
        public static Image _targetImage;
        public static TextBlock tbMouse;


        PuzzleGrid mousePuzzleGrid;
        private Image backgroundImage;
        private Size puzzleSize;
        private PuzzleConfig puzzleNumber;

        #endregion
        
        public Puzzle(PuzzleConfig puzzleNumber)
        {
            InitializeComponent();
            this.puzzleNumber = puzzleNumber;
        }

        private void NewGame()
        {
            backgroundImage = (Image)this.Resources["MasterImage1"]; //Get the game image

            BitmapSource bitmap = (BitmapSource)backgroundImage.Source;
            puzzleSize = new Size(bitmap.PixelWidth * 1.8, bitmap.PixelHeight * 1.8); //Set the size of the image

            //check if a puzzle already exists
            if (mousePuzzleGrid != null)
            {
                PuzzleHostingPanel.Children.Remove(mousePuzzleGrid); //true remove it
            }

            mousePuzzleGrid = new PuzzleGrid(puzzleNumber); //initialize _MousePuzzleGrid

            PuzzleHostingPanel.Children.Add(mousePuzzleGrid); //add grid to the user interface

            PuzzleHostingPanel.IsEnabled = true;    //ENABLE THE PUZZLE, SO THAT TILES CAN BE SELECTED
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
