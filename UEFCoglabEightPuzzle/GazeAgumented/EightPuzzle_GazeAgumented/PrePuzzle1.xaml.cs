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
using System.IO;
using System.Collections;

namespace EightPuzzle_GazeAgumented
{
    /// <summary>
    /// Interaction logic for PrePuzzle1.xaml
    /// </summary>
    public partial class PrePuzzle1 : Window
    {
        private StreamReader _sr;               //read stream from text file
        private ArrayList _gameConfigList;      //list of game configurations
        private string _currentPuzzle;

        public PrePuzzle1()
        {
            InitializeComponent();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
            else if (e.Key == Key.PageUp)
            {
                Window trial = new Trial();
                trial.Show();
            }
            else
            {
               

                if (_gameConfigList != null)
                {
                    if (_currentPuzzle == "A")
                    {
                        Window puzzle1 = new Puzzle1();
                        puzzle1.Show();
                        this.Close();
                    }
                    else if (_currentPuzzle == "B")
                    {
                        Window puzzle2 = new Puzzle2();
                        puzzle2.Show();
                        this.Close();
                    }
                    else if (_currentPuzzle == "C")
                    {
                        Window puzzle3 = new Puzzle3();
                        puzzle3.Show();
                        this.Close();
                    }
                }
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //get the config for the next puzzle
            GetGameConfig();
        }
        private void GetGameConfig()
        {
            _sr = new StreamReader("c:\\EightPuzzleConfig.txt");
            _gameConfigList = new ArrayList();
            string _line = "";
            while (_line != null)
            {
                _line = _sr.ReadLine();
                if (_line != null)
                {
                    _gameConfigList.Add(_line);
                }
            }
            _sr.Close();
            _currentPuzzle = _gameConfigList[0].ToString();

          

        }
    }
}
