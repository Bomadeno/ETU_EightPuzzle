using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for PrePuzzle1.xaml
    /// </summary>
    public partial class PrePuzzle : Window
    {
        private int puzzleNumber;

        public PrePuzzle() : this(0) {}

        public PrePuzzle(int puzzleNumber)
        {
            InitializeComponent();
            this.puzzleNumber = puzzleNumber;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            else if (e.Key == Key.PageUp)
            {
                if (puzzleNumber > 0)
                {
                    Window previousPuzzle = new PrePuzzle(puzzleNumber - 1);
                    previousPuzzle.Show();
                    this.Close();
                }
            }
            else
            {
                Window puzzle1 = new Puzzle(puzzleNumber);
                puzzle1.Show();
                this.Close();
            }
        }
    }
}
