﻿using System;
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

namespace EightPuzzle_GazeAugmented
{
    /// <summary>
    /// Interaction logic for PreTrail.xaml
    /// </summary>
    public partial class PreTrail : Window
    {
        public PreTrail()
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
                Window thinkAloud = new ThinkAloud();
                thinkAloud.Show();
                this.Close();
            }
            else
            {
                Window trial = new Trial();
                trial.Show();
                this.Close();
            }
            
        }
    }
}
