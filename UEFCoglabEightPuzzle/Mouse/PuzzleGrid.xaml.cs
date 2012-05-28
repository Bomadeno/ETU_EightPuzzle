using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.IO;

namespace EightPuzzle_Mouse
{
    public enum PuzzleConfig
    {
        Trial,
        ConfigA,
        ConfigB,
        ConfigC
    }

    public enum InteractionMode
    {
        PureMouse,
        GazeAugmented,
        DwellTime
    }

    public partial class PuzzleGrid
    {

        #region PRIVATE FIELDS

        private readonly PuzzleConfig _puzzleConfig;
        private readonly InteractionMode _interactionMode;
        private PuzzleLogic _puzzleLogic;
        private const int NumRows = 3;

        private int _moves;                     //Number of moves that have been made
        private string _selectedButtonNumber;   //Number of the button that has been clicked
        private string _moveDirection;          //Direction that the selected tile has moved
        private DateTime _gridOpenTime;         //Time the grid is opened
        private DateTime _gridCloseTime;        //Time the grid is closed
        private DateTime _currentTime;          //Time when the button is moved
        private DateTime _firstMoveTime;        //Time that the fist moves was made

        private int _backTrack;                 //number of times the users backtracks
        private string _previousMove;           //previously move
        private DateTime _previousTime;         //time that the previous move was made
        private string _data;                   //data to be written to text file


        public static MoveStatus ButtonMoveStatus { get; private set; }

        private StreamWriter _streamWriter;
        private string _currentFile;
        private bool _isAnimating;
        #endregion


        public PuzzleGrid(PuzzleConfig puzzleConfig, InteractionMode interactionMode)
        {
            InitializeComponent();

            // Centralize handling of all clicks in the puzzle grid.
            AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnPuzzleButtonClick));
            _puzzleConfig = puzzleConfig;
            _interactionMode = interactionMode;

            SetupGridStructure();
            ConfigureGame();
        }

        /// <summary>
        /// This creates the 3*3 WPF grid used by the buttons, and creates 8 buttons labeled '1' to '8'
        /// </summary>
        private void SetupGridStructure()
        {
            //create an instance of puzzleLogic
            _puzzleLogic = new PuzzleLogic(NumRows);

            // Define rows and columns in the Grid
            for (var row = 0; row < NumRows; row++)
            {
                var r = new RowDefinition { Height = GridLength.Auto };
                RowDefinitions.Add(r);

                var c = new ColumnDefinition { Width = GridLength.Auto };
                ColumnDefinitions.Add(c);
            }

            //add the buttons in a pile (they are placed according to the underlying PuzzleLogic later)
            for (var i = 0; i < 8; i++)
            {
                var button = GetNewButton();
                button.SetValue(RowProperty, 0);
                button.SetValue(ColumnProperty, 0);
                button.Content = "" + (i + 1);
                Children.Add(button);
            }
        }

        /// <summary>
        /// Creates a new button of the correct interaction type
        /// </summary>
        /// <returns>The kind of button appropriate for this interactionmode</returns>
        private Button GetNewButton()
        {
            switch (_interactionMode)
            {
                case InteractionMode.PureMouse:
                    return new MouseButton2();
                case InteractionMode.GazeAugmented:
                    return new GazeAugmentedButton();
                case InteractionMode.DwellTime:
                    return new DwellTimeButton();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Sets up the logic behind the game and then places the buttons in the grid accordingly
        /// </summary>
        private void ConfigureGame()
        {
            switch (_puzzleConfig)
            {
                case PuzzleConfig.Trial:
                    _puzzleLogic.Config_Trial();
                    break;
                case PuzzleConfig.ConfigA:
                    _puzzleLogic.Config_A();
                    break;
                case PuzzleConfig.ConfigB:
                    _puzzleLogic.Config_B();
                    break;
                case PuzzleConfig.ConfigC:
                    _puzzleLogic.Config_C();
                    break;
            }

            short cellNumber = 1;
            foreach (Button b in Children)
            {
                var location = _puzzleLogic.FindCell(cellNumber++);
                b.SetValue(ColumnProperty, location.Col);
                b.SetValue(RowProperty, location.Row);
            }
        }

        private void OnPuzzleButtonClick(object sender, RoutedEventArgs e)
        {
            //*** ONCE A TILE HAS BEEN CLICKED MOVE IT IF IT IS A VALID MOVE ***

            //The 'way' the button was activated shouldn't matter (mouse/gaze)
            var buttonToMove = e.Source as Button;

            if (buttonToMove == null || _isAnimating) return;

            //Get the row and column of the button that has been clicked
            var row = (int)buttonToMove.GetValue(RowProperty);
            var col = (int)buttonToMove.GetValue(ColumnProperty);

            //check to see if which direct the button should be moved
            var moveStatus = _puzzleLogic.GetMoveStatus(row, col);
            ButtonMoveStatus = moveStatus; //todo fix this horrid static-cross-file-malarkey

            if (moveStatus == MoveStatus.BadMove) return;

            //as long as the move is valid, animate the movement by calling AnimatePiece
            _isAnimating = true;
            AnimatePiece(buttonToMove, row, col, moveStatus);
        }
             
        private void AnimatePiece(Button b, int row, int col, MoveStatus moveStatus)
        {
            double distance; //distance the tile should move
            bool isMoveHorizontal; //determine if move is horizontal or vertical
            if (moveStatus == MoveStatus.Left || moveStatus == MoveStatus.Right)
            {
                isMoveHorizontal = true;
                // If direction is left then the distance = -1, Else direction is right and distance = 1 
                distance = (moveStatus == MoveStatus.Left ? -1 : 1) * b.Width;
            }
            else
            {
                isMoveHorizontal = false;
                // If direction is up then the distance = 1, Else direction is down and distance = -1
                distance = (moveStatus == MoveStatus.Up ? -1 : 1) * b.Height;
            }

            // pull the animation after it's complete, because we move change Grid cells.
            var slideAnim = new DoubleAnimation(distance, TimeSpan.FromSeconds(0.5), FillBehavior.Stop);
            slideAnim.CurrentStateInvalidated += delegate(object sender2, EventArgs e2)
                                                     {
                                                         // Anonymous delegate -- invoke when done.
                                                         var clock = (Clock)sender2;
                                                         if (clock.CurrentState == ClockState.Active) return;

                                                         // remove the render transform and really move the piece in the Grid.
                                                         try
                                                         {
                                                             LogicalMovePiece(b, row, col);

                         
                                                             _moveDirection = moveStatus.ToString();   //get the move direction and the button number
                                                             _selectedButtonNumber = b.Content.ToString();
                                                             //_previousMove = _selectedButtonNumber;      //Get the current and previously selected buttons
                                                             //_previousTime = _currentTime;
                                                             _currentTime = DateTime.Now;            //Set the times when the move was made
                                                             _isAnimating = false;
                           

                                                             PrintToTextFile();  //print data to text file
                                                         }
                                                         catch (Exception ex)
                                                         {
                                                             MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                                         }
                                                     };

            var buttonTransform = new TranslateTransform(0, 0);
            b.RenderTransform = buttonTransform;

               
            //perform the actual slide animation
            var directionProperty = isMoveHorizontal ? TranslateTransform.XProperty : TranslateTransform.YProperty;
            //rootFE.RenderTransform.BeginAnimation(directionProperty, slideAnim);
            b.RenderTransform.BeginAnimation(directionProperty, slideAnim);
        }
       
        private void LogicalMovePiece(Button b, int row, int col)
        { //*** MOVE THE TILE, ASSUMING THE MOVE IS VALID***
            //Identify the cell to move
            
            var newPosition = _puzzleLogic.MovePiece(row, col);
            try
            {
                //change the position of the tile in the grid
                b.SetValue(ColumnProperty, newPosition.Col);
                b.SetValue(RowProperty, newPosition.Row);

                //increment moves by 1
                _moves++;

                if (_puzzleLogic.CheckForPuzzleCompleted())
                {
                    IsEnabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Movement error !!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void MixUpPuzzle()
        { //*** SCRAMBLE THE PUZZLE ***
            _puzzleLogic.MixUpPuzzle(); //Call MixUpPuzzle to shuffle and move the puzzles
            
            short cellNumber = 1;
            foreach (Button b in this.Children)
            {
                PuzzleCell location = _puzzleLogic.FindCell(cellNumber++);
                b.SetValue(ColumnProperty, location.Col);
                b.SetValue(RowProperty, location.Row);         
            }
        }

        #region TEXT FILE DATA
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //get the time the window was opened
            _gridOpenTime = DateTime.Now;

            _currentFile = "Mouse_Puzzle_Config" + Enum.GetName(typeof(PuzzleConfig), _puzzleConfig) + "_" + DateTime.Now.ToFileTime() + ".txt";

            _streamWriter = File.CreateText(_currentFile);
            _streamWriter.WriteLine("Puzzle Opened at:  " + _gridOpenTime.ToString());
            _streamWriter.WriteLine("Time  |  Move Number  |  Selected button  |  Move Direction  |  Back Tracking  |  Move Latency");
            _streamWriter.Close();

        }

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {
            //get the window closing time as the window closes
            _gridCloseTime = DateTime.Now;

            TimeSpan viewDuration = _gridCloseTime - _gridOpenTime;
            TimeSpan playDuration = _currentTime - _firstMoveTime;

            _streamWriter = File.AppendText(_currentFile);
            _streamWriter.WriteLine("");
            _streamWriter.WriteLine("Puzzle Closed at:  " + _gridCloseTime.ToLongTimeString());
            _streamWriter.WriteLine("");
            _streamWriter.WriteLine("");
            _streamWriter.WriteLine("");
            _streamWriter.WriteLine("----------------------- GAME SUMARY -----------------------");
            _streamWriter.WriteLine("");

            _streamWriter.WriteLine("Window Opened Duration - Full time: " + viewDuration.ToString());
            _streamWriter.WriteLine("Game played Duration - Full time: " + playDuration.ToString());
            
            _streamWriter.WriteLine("Number of moves: " + _moves.ToString());
            _streamWriter.WriteLine("Number of back tracks: " + _backTrack.ToString() );
            _streamWriter.Close();

            //open the textFile and append the closing time
        }

        private void PrintToTextFile()
        {
            //Write data to text file in the following format
            //"Time  |  Move Number  |  Selected button  |  Move Direction  |  Back Tracking  |  Move Latency"
            
            _data = "";             //clear previous data if any
            _data += _currentTime.ToLongTimeString() + "  |  ";     //Time
            _data += _moves.ToString() + "   |  ";                   //move number
            _data += _selectedButtonNumber + "  |  ";               //selected button
            _data += _moveDirection + "  |  ";                   //move direction
       
            string bt = BackTracking();//backtracking
            _data += bt + "  |  ";

            string ml = InterMoveLatency(); //move latency
            _data += ml;

            _streamWriter = File.AppendText(_currentFile);
            _streamWriter.WriteLine(_data);
            _streamWriter.Close();


        }

        private string BackTracking()
        {
            //determine if the user backtracked
                
            if (_moves <= 1)            //Fist move so no back tracking
            {
                _previousMove = _selectedButtonNumber;
                return "";
            }
            else
            {
                if (_previousMove == _selectedButtonNumber)
                {
                    _backTrack++;
                    return _backTrack.ToString();
                }
                else
                {
                    _previousMove = _selectedButtonNumber;
                    return "";
                }
            }
        }

        private string InterMoveLatency()
        {
            //Determine the move latency between to moves

            if (_moves <= 1)            //Fist move so no duration
            {
                _previousTime = _currentTime;
                _firstMoveTime = _currentTime;
                return "";
            }
            else
            {
               
                    TimeSpan diff = _currentTime - _previousTime;
                   // return diff.Milliseconds.ToString();
                    string time = diff.Minutes.ToString() + ":" + diff.Seconds.ToString() + ":" + diff.Milliseconds.ToString();
                    return time;
                
            }
        }
        #endregion
    }
}