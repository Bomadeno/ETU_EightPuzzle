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
    /// Interaction logic for PrePuzzle2.xaml
    /// </summary>
    public partial class PrePuzzle2 : Window
    {
        public PrePuzzle2()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            else if (e.Key == Key.PageUp)
            {
                Window puzzle1 = new Puzzle1();
                puzzle1.Show();
                this.Close();
            }
            else
            {
                Window puzzle2 = new Puzzle2();
                puzzle2.Show();
                this.Close();
            }

        }
    }
}
