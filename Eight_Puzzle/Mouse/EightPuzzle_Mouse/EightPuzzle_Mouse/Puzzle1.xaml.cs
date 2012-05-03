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

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for Puzzle1.xaml
    /// </summary>
    public partial class Puzzle1 : Window
    {
        #region PRIVATE PROPERTIES
        MousePuzzleGrid _MousePuzzleGrid;                         //instance of MousePuzzleGrid
        private int _numRows;                           //Number of rows in the grid
        private Image masterImage;                      //image resources
        private Size _puzzleSize;                       //Size of the puzzle

        private StreamReader _sr;               //read stream from text file
        private ArrayList _gameConfigList;      //list of game configurations

        #endregion
        

        public Puzzle1()
        {
            InitializeComponent();
            _numRows = 3; //default _numRows value

        }

        private void NewGame()
        {    //*** START A NEW GAME ***
            masterImage = (Image)this.Resources["MasterImage1"]; //Get the game image

            BitmapSource bitmap = (BitmapSource)masterImage.Source;
            _puzzleSize = new Size(bitmap.PixelWidth * 1.8, bitmap.PixelHeight * 1.8); //Set the size of the image

            //check if a puzzle already exisits
            if (_MousePuzzleGrid != null)
            {
                PuzzleHostingPanel.Children.Remove(_MousePuzzleGrid); //true remove it
            }

            _MousePuzzleGrid = new MousePuzzleGrid(); //initialise _MousePuzzleGrid

            _MousePuzzleGrid.NumRows = _numRows; //number of rows in the grid

            _MousePuzzleGrid.PuzzleImage = masterImage; //background image

            _MousePuzzleGrid.PuzzleSize = _puzzleSize; //size of the puzzle

            //get the game configuration
            GetGameConfig();

            PuzzleHostingPanel.Children.Add(_MousePuzzleGrid); //add grid to the user interface

            PuzzleHostingPanel.IsEnabled = true;    //ENABLE THE PUZZLE, SO THAT TILES CAN BE SELECTED

        }

        /// <summary>
        /// Open a new game once the window is opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            //start a new game when the puzzle is loaded
            NewGame();

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

            if (_gameConfigList != null)
            {
                if (_gameConfigList[0].ToString() == "A")
                {
                    _MousePuzzleGrid.ConfigType = "A";           //Use game congiguration A
                    _MousePuzzleGrid.StartConfig("ConfigA");       //Use start configuration A
                }
                else if (_gameConfigList[0].ToString() == "B")
                {
                    _MousePuzzleGrid.ConfigType = "B";         //Use game configuration B
                    _MousePuzzleGrid.StartConfig("ConfigB");       //Use start configuration B
                }
                else if (_gameConfigList[0].ToString() == "C")
                {
                    _MousePuzzleGrid.ConfigType = "C";         //Use trial configuration
                    _MousePuzzleGrid.StartConfig("C");       //Use the trial configuration to start
                }
            }
            

           
          

           

        }
        
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();   //Close the window
            }
            else if (e.Key == Key.PageDown)
            {
                Window prePuzzle2 = new PrePuzzle2();
                prePuzzle2.Show();
                this.Close();
            }
            else if (e.Key == Key.PageUp)
            {
                Window prePuzzle1 = new PrePuzzle1();
                prePuzzle1.Show();
                this.Close();
            }


        }

        
    }
}
