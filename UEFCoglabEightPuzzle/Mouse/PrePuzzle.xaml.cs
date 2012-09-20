using System.Windows;
using System.Windows.Input;

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for PrePuzzle1.xaml
    /// </summary>
    public partial class PrePuzzle
    {
        private readonly PuzzleConfig _puzzleNumber;

        public PrePuzzle() : this(0) {}

        public PrePuzzle(PuzzleConfig puzzleNumber)
        {
            InitializeComponent();
            _puzzleNumber = puzzleNumber;
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            Window puzzle1 = new Puzzle(_puzzleNumber);
            puzzle1.Show();
            Close();
        }
    }
}
