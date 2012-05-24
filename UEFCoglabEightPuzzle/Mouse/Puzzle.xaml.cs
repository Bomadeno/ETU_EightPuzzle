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
    public partial class Puzzle : Window
    {
        #region PRIVATE PROPERTIES

        MousePuzzleGrid mousePuzzleGrid;    //instance of MousePuzzleGrid
        private int numRows;                //Number of rows in the grid
        private Image masterImage;          //image resources
        private Size puzzleSize;            //Size of the puzzle
        private int puzzleNumber;

        #endregion
        
        public Puzzle(int puzzleNumber)
        {
            InitializeComponent();
            this.puzzleNumber = puzzleNumber;
            numRows = 3; //default _numRows value
        }

        private void NewGame()
        {    //*** START A NEW GAME ***
            masterImage = (Image)this.Resources["MasterImage1"]; //Get the game image

            BitmapSource bitmap = (BitmapSource)masterImage.Source;
            puzzleSize = new Size(bitmap.PixelWidth * 1.8, bitmap.PixelHeight * 1.8); //Set the size of the image

            //check if a puzzle already exists
            if (mousePuzzleGrid != null)
            {
                PuzzleHostingPanel.Children.Remove(mousePuzzleGrid); //true remove it
            }

            mousePuzzleGrid = new MousePuzzleGrid(numRows); //initialize _MousePuzzleGrid
            mousePuzzleGrid.PuzzleImage = masterImage; //background image
            mousePuzzleGrid.PuzzleSize = puzzleSize; //size of the puzzle

            //get the game configuration
            GetGameConfig();

            PuzzleHostingPanel.Children.Add(mousePuzzleGrid); //add grid to the user interface

            PuzzleHostingPanel.IsEnabled = true;    //ENABLE THE PUZZLE, SO THAT TILES CAN BE SELECTED
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //start a new game when the window is loaded
            NewGame();
        }
        
        private void GetGameConfig()
        {
            if (puzzleNumber == 0)
            {
                mousePuzzleGrid.ConfigType = "T";           //Use game configuration A
                mousePuzzleGrid.StartConfig("Trial");       //Use start configuration A
            }
            else if (puzzleNumber == 1)
            {
                mousePuzzleGrid.ConfigType = "A";           //Use game configuration A
                mousePuzzleGrid.StartConfig("ConfigA");       //Use start configuration A
            }
            else if(puzzleNumber == 2)
            {
                mousePuzzleGrid.ConfigType = "B";         //Use game configuration B
                mousePuzzleGrid.StartConfig("ConfigB");       //Use start configuration B
            }
            else
            {
                mousePuzzleGrid.ConfigType = "C";
                mousePuzzleGrid.StartConfig("C");
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
                if (puzzleNumber < 3)
                {
                    Window nextPrePuzzle = new PrePuzzle(puzzleNumber + 1);
                    nextPrePuzzle.Show();
                    this.Close();
                }
                else
                {
                    Window thankYou = new ThankYou();
                    thankYou.Show();
                    this.Close();
                }
            }
            else if (e.Key == Key.PageUp)
            {
                Window thisPuzzlesPrePuzzle = new PrePuzzle(puzzleNumber);
                thisPuzzlesPrePuzzle.Show();
                this.Close();
            }
        }
    }
}
