using System.Windows;
using System.Windows.Input;

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for PrePuzzle1.xaml
    /// </summary>
    public partial class PrePuzzle : Window
    {
        private PuzzleConfig puzzleNumber;

        public PrePuzzle() : this(0) {}

        public PrePuzzle(PuzzleConfig puzzleNumber)
        {
            InitializeComponent();
            this.puzzleNumber = puzzleNumber;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Window puzzle1 = new Puzzle(puzzleNumber);
            puzzle1.Show();
            this.Close();
        }
    }
}
