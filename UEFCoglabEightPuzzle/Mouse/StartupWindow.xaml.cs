using System.Windows;

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            PuzzleConfig puzzleNumber = 0;

            if (radioConfigT.IsChecked.HasValue && (bool)radioConfigT.IsChecked)
            {
                puzzleNumber = PuzzleConfig.Trial;
            } else if (radioConfigA.IsChecked.HasValue && (bool)radioConfigA.IsChecked)
            {
                puzzleNumber = PuzzleConfig.ConfigA;
            } else if (radioConfigB.IsChecked.HasValue && (bool)radioConfigB.IsChecked)
            {
                puzzleNumber = PuzzleConfig.ConfigB;
            } else if (radioConfigC.IsChecked.HasValue && (bool)radioConfigC.IsChecked)
            {
                puzzleNumber = PuzzleConfig.ConfigC;
            }

            if (sender.Equals(mousePuzzleStartButton))
            {
                Window firstPuzzle = new PrePuzzle(puzzleNumber);
                firstPuzzle.Show();
                Close();
            }
        }
    }
}
