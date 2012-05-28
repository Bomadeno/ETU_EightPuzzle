using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;

namespace EightPuzzle_DwellTime
{
    public partial class DwellTimePuzzleGrid : Grid
    {
        #region PRIVATE FIELDS
        private PuzzleLogic _puzzleLogic; //instace of PuzzleLogic
        private Size _masterPuzzleSize = Size.Empty; //Size of the puzzle
        private UIElement _puzzleImage; // the puzzle image to be chopped up
        private int _numRows = -1; //number of rows (and columns) in the puzzle
        private string _ConfigType; //Specify the configuration type

        private int _moves = 0;      //Number of moves that have been made
        private string _selectedButtonNumber;        //Number of the button that has been clicked
        private string _moveDirection;               //Direction that the selected tile has moved
        private DateTime _gridOpenTime;              //Time the grid is opened
        private DateTime _gridCloseTime;             //Time the grid is closed
        private DateTime _currentTime;              //Time when the button is moved
        private DateTime _firstMoveTime;            //Time that the fist moves was made

        private int _backTrack = 0;             //number of times the users backtracks
        private string _previousMove;              //previously move
        private DateTime _previousTime;          //time that the previous move was made
        private string _data;                       //data to be written to text file

        private static string _buttonMoveStatus;
        private static bool _isProcessingButton = false;    //Determine whether or not a button is currently being processed

        public static string ButtonMoveStatus
        {
            get { return _buttonMoveStatus; }
        }
        private StreamWriter _streamWriter;
        private string _currentFile;            //currentfile

        
        #endregion

        #region PUBLIC PROPERTIES

        public int NumRows
        {
            get { return _numRows; }
            set
            {
                // Only support setting this once per DwellTimePuzzleGrid instance.
                if (_numRows != -1)
                {
                    throw new InvalidOperationException("NumRows already initialized for this DwellTimePuzzleGrid instance.");
                }
                else
                {
                    _numRows = value;
                    CheckToSetup();
                }
            }
        }

        public UIElement PuzzleImage
        {
            get { return _puzzleImage; }
            set
            {
                if (_puzzleImage != null)
                {
                    throw new InvalidOperationException("ElementForPuzzle already initialized for this DwellTimePuzzleGrid instance.");
                }
                else
                {
                    _puzzleImage = value;
                    CheckToSetup();
                }
            }
        }

        public Size PuzzleSize
        {
            get { return _masterPuzzleSize; }
            set
            {
                if (!_masterPuzzleSize.IsEmpty)
                {
                    throw new InvalidOperationException("SizeForPuzzle already initialized for this DwellTimePuzzleGrid instance.");
                }
                else
                {
                    _masterPuzzleSize = value;
                    CheckToSetup();
                }
            }
        }

        public string ConfigType
        {
            get { return _ConfigType; }
            set
            {
                if (_ConfigType != null)
                {
                    throw new InvalidOperationException("ElementForPuzzle already initialized for this DwellTimePuzzleGrid instance.");
                }
                else
                {
                    _ConfigType = value;
                    CheckToSetup();
                }
            }
        }

        private void CheckToSetup()
        {
            if (_numRows != -1 && (_ConfigType != null) && (_puzzleImage != null) && !_masterPuzzleSize.IsEmpty)
            {
                SetupTheDwellTimePuzzleGridStructure();
            }
        }

        public static bool IsProcessingButton
        {
            get { return _isProcessingButton; }
            set { _isProcessingButton = value; }
        }
        #endregion

        public DwellTimePuzzleGrid()
        {
            //*** DEFAULT CONSTRUCTOR ***
            InitializeComponent();

            // Centralize handling of all clicks in the puzzle grid.
            //this.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnPuzzleButtonClick));
            this.AddHandler(DwellTimeButton.ClickEvent, new RoutedEventHandler(OnPuzzleButtonClick));
            this.AddHandler(DwellTimeButtonA.ClickEvent, new RoutedEventHandler(OnPuzzleButtonClickA));
            this.AddHandler(DwellTimeButtonB.ClickEvent, new RoutedEventHandler(OnPuzzleButtonClickB));
            this.AddHandler(DwellTimeButtonC.ClickEvent, new RoutedEventHandler(OnPuzzleButtonClickC));
            
           
        }

        private void SetupTheDwellTimePuzzleGridStructure()
        {
            //*** SET UP THE PUZZLE ***

            //define the row, column the button will appear in and the button style

            //create an instance of puzzleLogic
            _puzzleLogic = new PuzzleLogic(_numRows);

            // Define rows and columns in the Grid
            for (int row = 0; row < _numRows; row++)
            {
                RowDefinition r = new RowDefinition();
                r.Height = GridLength.Auto;
                this.RowDefinitions.Add(r);


                ColumnDefinition c = new ColumnDefinition();
                c.Width = GridLength.Auto;
                this.ColumnDefinitions.Add(c);
            }

            // Now add the buttons
            switch (_ConfigType)
            {
                case "T":
                    {
                        TrialConfig();
                    } break;
                case "A":
                    {
                        AConfig();
                    } break;
                case "B":
                    {
                        BConfig();
                    } break;
                case "C":
                    {
                        CConfig();
                    } break;
            }
        }


        #region TRIAL
        private void TrialConfig()
        {
            #region ROW 1
            DwellTimeButton _00 = new DwellTimeButton();
            _00.SetValue(Grid.RowProperty, 0);
            _00.SetValue(Grid.ColumnProperty, 0);
            _00.Content = "1";
            this.Children.Add(_00);

            DwellTimeButton _01 = new DwellTimeButton();
            _01.SetValue(Grid.RowProperty, 0);
            _01.SetValue(Grid.ColumnProperty, 1);
            _01.Content = "2";
            this.Children.Add(_01);

            DwellTimeButton _02 = new DwellTimeButton();
            _02.SetValue(Grid.RowProperty, 0);
            _02.SetValue(Grid.ColumnProperty, 2);
            _02.Content = "3";
            this.Children.Add(_02);
            #endregion

            #region ROW 2
            DwellTimeButton _10 = new DwellTimeButton();
            _10.SetValue(Grid.RowProperty, 1);
            _10.SetValue(Grid.ColumnProperty, 0);
            _10.Content = "8";
            this.Children.Add(_10);

            //Button _11 = new Button();
            //_11.FontSize = 24;
            //_11.Style = buttonStyle;
            //_11.SetValue(Grid.RowProperty, 1);
            //_11.SetValue(Grid.ColumnProperty, 1);
            //_11.Content = "2";
            //this.Children.Add(_01);

            DwellTimeButton _12 = new DwellTimeButton();
            _12.SetValue(Grid.RowProperty, 1);
            _12.SetValue(Grid.ColumnProperty, 2);
            _12.Content = "4";
            this.Children.Add(_12);
            #endregion

            #region ROW 3
            DwellTimeButton _20 = new DwellTimeButton();
            _20.SetValue(Grid.RowProperty, 2);
            _20.SetValue(Grid.ColumnProperty, 0);
            _20.Content = "7";
            this.Children.Add(_20);

            DwellTimeButton _21 = new DwellTimeButton();
            _21.SetValue(Grid.RowProperty, 2);
            _21.SetValue(Grid.ColumnProperty, 1);
            _21.Content = "6";
            this.Children.Add(_21);

            DwellTimeButton _22 = new DwellTimeButton();
            _22.SetValue(Grid.RowProperty, 2);
            _22.SetValue(Grid.ColumnProperty, 2);
            _22.Content = "5";
            this.Children.Add(_22);
            #endregion

        }

        private void OnPuzzleButtonClick(object sender, RoutedEventArgs e)
        {
            //*** ONCE A TILE HAS BEEN CLICKED MOVE IT IF IT IS A VALID MOVE ***
            if (_isProcessingButton == true)
            {
                //  this.IsEnabled = false;
                DwellTimeButton b = e.Source as DwellTimeButton;  //identify the button that has been clicked
                if (b != null)
                {
                    //Get the row and column of the button that has been cliced
                    int row = (int)b.GetValue(Grid.RowProperty);
                    int col = (int)b.GetValue(Grid.ColumnProperty);

                    //check to see if which direct the button should be moved
                    MoveStatus moveStatus = _puzzleLogic.GetMoveStatus(row, col);
                    _buttonMoveStatus = moveStatus.ToString();  //Move status of the button

                    if (moveStatus != MoveStatus.BadMove)
                    {
                        //as long as the move is valid, animate the movement by calling Animatepiece
                        AnimatePiece(b, row, col, moveStatus);
                    }
                    else
                    {
                        //Bad move, so allow a new button to be selected
                        _isProcessingButton = false;
                        // this.IsEnabled = true;
                    }//if-else
                }//if
            }//_isprocessingButton
        }

        private void AnimatePiece(DwellTimeButton b, int row, int col, MoveStatus moveStatus)
        {


            double distance; //distance the tile should move
            bool isMoveHorizontal; //determine íf move is horizontal or vertical

            //get the direction the tile should move
            //Debug.Assert(moveStatus != MoveStatus.BadMove);
            if (moveStatus != MoveStatus.BadMove)
            {
                if (moveStatus == MoveStatus.Left || moveStatus == MoveStatus.Right)
                {
                    isMoveHorizontal = true;
                    // If direction is left then the distance = -1, Else direction is right and distance = 1 
                    //distance = (moveStatus == MoveStatus.Left ? -1 : 1) * rootFE.Width;
                    distance = (moveStatus == MoveStatus.Left ? -1 : 1) * b.Width;
                }
                else
                {
                    isMoveHorizontal = false;
                    // If direction is up then the distance = 1, Else direction is down and distance = -1 
                    //distance = (moveStatus == MoveStatus.Up ? -1 : 1) * rootFE.Height;
                    distance = (moveStatus == MoveStatus.Up ? -1 : 1) * b.Height;
                }

                // pull the animation after it's complete, because we move change Grid cells.
                DoubleAnimation slideAnim = new DoubleAnimation(distance, TimeSpan.FromSeconds(0.5), FillBehavior.Stop);

                //play the click sound
                System.Media.SoundPlayer sp = new System.Media.SoundPlayer("Click.wav");
                sp.Play();

                slideAnim.CurrentStateInvalidated += delegate(object sender2, EventArgs e2)
                {
                    // Anonymous delegate -- invoke when done.
                    Clock clock = (Clock)sender2;
                    if (clock.CurrentState != ClockState.Active)
                    {
                        // remove the render transform and really move the piece in the Grid.
                        try
                        {
                            MovePiece(b, row, col);


                            _moveDirection = moveStatus.ToString();   //get the move direction and the button number
                            _selectedButtonNumber = b.Content.ToString();
                            //_previousMove = _selectedButtonNumber;      //Get the current and previously selected buttons
                            //_previousTime = _currentTime;
                            _currentTime = DateTime.Now;            //Set the times when the move was made


                            PrintToTextFile();  //print data to text file
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                };

                TranslateTransform buttonTransform = new TranslateTransform(0, 0);
                b.RenderTransform = buttonTransform;


                //perform the actual slide animation
                DependencyProperty directionProperty = isMoveHorizontal ? TranslateTransform.XProperty : TranslateTransform.YProperty;
                //rootFE.RenderTransform.BeginAnimation(directionProperty, slideAnim);
                b.RenderTransform.BeginAnimation(directionProperty, slideAnim);
            }
        }

        private void MovePiece(DwellTimeButton b, int row, int col)
        { //*** MOVE THE TILE, ASSUMING THE MOVE IS VALID***
            //Identify the cell to move

            PuzzleCell newPosition = _puzzleLogic.MovePiece(row, col);
            try
            {
                //change the position of the tile in the grid
                b.SetValue(Grid.ColumnProperty, newPosition.Col);
                b.SetValue(Grid.RowProperty, newPosition.Row);

                //increment moves by 1
                _moves++;

                //Once the piece has been moved, then allow the next piece to be moved
                _isProcessingButton = false;
                //this.IsEnabled = true;//enable the grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Movement error !!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        #endregion

        #region CONFIG A
        private void AConfig()
        {
            #region ROW 1
            DwellTimeButtonA _00 = new DwellTimeButtonA();
            //Button _00 = new Button();
            //_00.FontSize = 24;
            //_00.Style = buttonStyle;
            _00.SetValue(Grid.RowProperty, 0);
            _00.SetValue(Grid.ColumnProperty, 0);
            _00.Content = "8";
            this.Children.Add(_00);

            //Button _01 = new Button();
            //_01.FontSize = 24;
            //_01.Style = buttonStyle;
            //_01.SetValue(Grid.RowProperty, 0);
            //_01.SetValue(Grid.ColumnProperty, 1);
            //_01.Content = "2";
            //this.Children.Add(_01);

            DwellTimeButtonA _02 = new DwellTimeButtonA();
            //Button _02 = new Button();
            // _02.FontSize = 24;
            // _02.Style = buttonStyle;
            _02.SetValue(Grid.RowProperty, 0);
            _02.SetValue(Grid.ColumnProperty, 2);
            _02.Content = "2";
            this.Children.Add(_02);
            #endregion

            #region ROW 2
            DwellTimeButtonA _10 = new DwellTimeButtonA();
            //Button _10 = new Button();
            //_10.FontSize = 24;
            //_10.Style = buttonStyle;
            _10.SetValue(Grid.RowProperty, 1);
            _10.SetValue(Grid.ColumnProperty, 0);
            _10.Content = "5";
            this.Children.Add(_10);

            DwellTimeButtonA _11 = new DwellTimeButtonA();
            //Button _11 = new Button();
            //_11.FontSize = 24;
            //_11.Style = buttonStyle;
            _11.SetValue(Grid.RowProperty, 1);
            _11.SetValue(Grid.ColumnProperty, 1);
            _11.Content = "3";
            this.Children.Add(_11);

            DwellTimeButtonA _12 = new DwellTimeButtonA();
            //Button _12 = new Button();
            //_12.FontSize = 24;
            //_12.Style = buttonStyle;
            _12.SetValue(Grid.RowProperty, 1);
            _12.SetValue(Grid.ColumnProperty, 2);
            _12.Content = "1";
            this.Children.Add(_12);
            #endregion

            #region ROW 3
            DwellTimeButtonA _20 = new DwellTimeButtonA();
            //Button _20 = new Button();
            //_20.FontSize = 24;
            // _20.Style = buttonStyle;
            _20.SetValue(Grid.RowProperty, 2);
            _20.SetValue(Grid.ColumnProperty, 0);
            _20.Content = "7";
            this.Children.Add(_20);

            DwellTimeButtonA _21 = new DwellTimeButtonA();
            //Button _21 = new Button();
            // _21.FontSize = 24;
            // _21.Style = buttonStyle;
            _21.SetValue(Grid.RowProperty, 2);
            _21.SetValue(Grid.ColumnProperty, 1);
            _21.Content = "6";
            this.Children.Add(_21);

            DwellTimeButtonA _22 = new DwellTimeButtonA();
            //Button _22 = new Button();
            //_22.FontSize = 24;
            // _22.Style = buttonStyle;
            _22.SetValue(Grid.RowProperty, 2);
            _22.SetValue(Grid.ColumnProperty, 2);
            _22.Content = "4";
            this.Children.Add(_22);
            #endregion

        }

        private void OnPuzzleButtonClickA(object sender, RoutedEventArgs e)
        {
            //*** ONCE A TILE HAS BEEN CLICKED MOVE IT IF IT IS A VALID MOVE ***
            if (_isProcessingButton == true)
            {
                //  this.IsEnabled = false;
                DwellTimeButtonA b = e.Source as DwellTimeButtonA;  //identify the button that has been clicked
                if (b != null)
                {
                    //Get the row and column of the button that has been cliced
                    int row = (int)b.GetValue(Grid.RowProperty);
                    int col = (int)b.GetValue(Grid.ColumnProperty);

                    //check to see if which direct the button should be moved
                    MoveStatus moveStatus = _puzzleLogic.GetMoveStatus(row, col);
                    _buttonMoveStatus = moveStatus.ToString();  //Move status of the button

                    if (moveStatus != MoveStatus.BadMove)
                    {
                        //as long as the move is valid, animate the movement by calling Animatepiece
                        AnimatePieceA(b, row, col, moveStatus);
                    }
                    else
                    {
                        //Bad move, so allow a new button to be selected
                        _isProcessingButton = false;
                        // this.IsEnabled = true;
                    }//if-else
                }//if
            }//_isprocessingButton
        }

        private void AnimatePieceA(DwellTimeButtonA b, int row, int col, MoveStatus moveStatus)
        {


            double distance; //distance the tile should move
            bool isMoveHorizontal; //determine íf move is horizontal or vertical

            //get the direction the tile should move
            //Debug.Assert(moveStatus != MoveStatus.BadMove);
            if (moveStatus != MoveStatus.BadMove)
            {
                if (moveStatus == MoveStatus.Left || moveStatus == MoveStatus.Right)
                {
                    isMoveHorizontal = true;
                    // If direction is left then the distance = -1, Else direction is right and distance = 1 
                    //distance = (moveStatus == MoveStatus.Left ? -1 : 1) * rootFE.Width;
                    distance = (moveStatus == MoveStatus.Left ? -1 : 1) * b.Width;
                }
                else
                {
                    isMoveHorizontal = false;
                    // If direction is up then the distance = 1, Else direction is down and distance = -1 
                    //distance = (moveStatus == MoveStatus.Up ? -1 : 1) * rootFE.Height;
                    distance = (moveStatus == MoveStatus.Up ? -1 : 1) * b.Height;
                }

                // pull the animation after it's complete, because we move change Grid cells.
                DoubleAnimation slideAnim = new DoubleAnimation(distance, TimeSpan.FromSeconds(0.5), FillBehavior.Stop);

                //play the click sound
                System.Media.SoundPlayer sp = new System.Media.SoundPlayer("Click.wav");
                sp.Play();

                slideAnim.CurrentStateInvalidated += delegate(object sender2, EventArgs e2)
                {
                    // Anonymous delegate -- invoke when done.
                    Clock clock = (Clock)sender2;
                    if (clock.CurrentState != ClockState.Active)
                    {
                        // remove the render transform and really move the piece in the Grid.
                        try
                        {
                            MovePieceA(b, row, col);


                            _moveDirection = moveStatus.ToString();   //get the move direction and the button number
                            _selectedButtonNumber = b.Content.ToString();
                            //_previousMove = _selectedButtonNumber;      //Get the current and previously selected buttons
                            //_previousTime = _currentTime;
                            _currentTime = DateTime.Now;            //Set the times when the move was made


                            PrintToTextFile();  //print data to text file
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                };

                TranslateTransform buttonTransform = new TranslateTransform(0, 0);
                b.RenderTransform = buttonTransform;


                //perform the actual slide animation
                DependencyProperty directionProperty = isMoveHorizontal ? TranslateTransform.XProperty : TranslateTransform.YProperty;
                //rootFE.RenderTransform.BeginAnimation(directionProperty, slideAnim);
                b.RenderTransform.BeginAnimation(directionProperty, slideAnim);
            }
        }

        private void MovePieceA(DwellTimeButtonA b, int row, int col)
        { //*** MOVE THE TILE, ASSUMING THE MOVE IS VALID***
            //Identify the cell to move

            PuzzleCell newPosition = _puzzleLogic.MovePiece(row, col);
            try
            {
                //change the position of the tile in the grid
                b.SetValue(Grid.ColumnProperty, newPosition.Col);
                b.SetValue(Grid.RowProperty, newPosition.Row);

                //increment moves by 1
                _moves++;

                //Once the piece has been moved, then allow the next piece to be moved
                _isProcessingButton = false;
                //this.IsEnabled = true;//enable the grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Movement error !!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        #endregion

        #region CONFIG B
        private void BConfig()
        {
            #region ROW 1
            DwellTimeButtonB _00 = new DwellTimeButtonB();
            _00.SetValue(Grid.RowProperty, 0);
            _00.SetValue(Grid.ColumnProperty, 0);
            _00.Content = "6";
            this.Children.Add(_00);

            DwellTimeButtonB _01 = new DwellTimeButtonB();
            _01.SetValue(Grid.RowProperty, 0);
            _01.SetValue(Grid.ColumnProperty, 1);
            _01.Content = "8";
            this.Children.Add(_01);

            DwellTimeButtonB _02 = new DwellTimeButtonB();
            _02.SetValue(Grid.RowProperty, 0);
            _02.SetValue(Grid.ColumnProperty, 2);
            _02.Content = "1";
            this.Children.Add(_02);
            #endregion

            #region ROW 2
            //DwellTimeButtonB _10 = new DwellTimeButtonB();
            //_10.SetValue(Grid.RowProperty, 1);
            //_10.SetValue(Grid.ColumnProperty, 0);
            //_10.Content = "5";
            //this.Children.Add(_10);

            DwellTimeButtonB _11 = new DwellTimeButtonB();
            _11.SetValue(Grid.RowProperty, 1);
            _11.SetValue(Grid.ColumnProperty, 1);
            _11.Content = "7";
            this.Children.Add(_11);

            DwellTimeButtonB _12 = new DwellTimeButtonB();
            _12.SetValue(Grid.RowProperty, 1);
            _12.SetValue(Grid.ColumnProperty, 2);
            _12.Content = "3";
            this.Children.Add(_12);
            #endregion

            #region ROW 3
            DwellTimeButtonB _20 = new DwellTimeButtonB();
            _20.SetValue(Grid.RowProperty, 2);
            _20.SetValue(Grid.ColumnProperty, 0);
            _20.Content = "5";
            this.Children.Add(_20);

            DwellTimeButtonB _21 = new DwellTimeButtonB();
            _21.SetValue(Grid.RowProperty, 2);
            _21.SetValue(Grid.ColumnProperty, 1);
            _21.Content = "4";
            this.Children.Add(_21);

            DwellTimeButtonB _22 = new DwellTimeButtonB();
            _22.SetValue(Grid.RowProperty, 2);
            _22.SetValue(Grid.ColumnProperty, 2);
            _22.Content = "2";
            this.Children.Add(_22);
            #endregion

        }

        private void OnPuzzleButtonClickB(object sender, RoutedEventArgs e)
        {
            //*** ONCE A TILE HAS BEEN CLICKED MOVE IT IF IT IS A VALID MOVE ***
            if (_isProcessingButton == true)
            {
                //  this.IsEnabled = false;
                DwellTimeButtonB b = e.Source as DwellTimeButtonB;  //identify the button that has been clicked
                if (b != null)
                {
                    //Get the row and column of the button that has been cliced
                    int row = (int)b.GetValue(Grid.RowProperty);
                    int col = (int)b.GetValue(Grid.ColumnProperty);

                    //check to see if which direct the button should be moved
                    MoveStatus moveStatus = _puzzleLogic.GetMoveStatus(row, col);
                    _buttonMoveStatus = moveStatus.ToString();  //Move status of the button

                    if (moveStatus != MoveStatus.BadMove)
                    {
                        //as long as the move is valid, animate the movement by calling Animatepiece
                        AnimatePieceB(b, row, col, moveStatus);
                    }
                    else
                    {
                        //Bad move, so allow a new button to be selected
                        _isProcessingButton = false;
                        // this.IsEnabled = true;
                    }//if-else
                }//if
            }//_isprocessingButton
        }

        private void AnimatePieceB(DwellTimeButtonB b, int row, int col, MoveStatus moveStatus)
        {


            double distance; //distance the tile should move
            bool isMoveHorizontal; //determine íf move is horizontal or vertical

            //get the direction the tile should move
            //Debug.Assert(moveStatus != MoveStatus.BadMove);
            if (moveStatus != MoveStatus.BadMove)
            {
                if (moveStatus == MoveStatus.Left || moveStatus == MoveStatus.Right)
                {
                    isMoveHorizontal = true;
                    // If direction is left then the distance = -1, Else direction is right and distance = 1 
                    //distance = (moveStatus == MoveStatus.Left ? -1 : 1) * rootFE.Width;
                    distance = (moveStatus == MoveStatus.Left ? -1 : 1) * b.Width;
                }
                else
                {
                    isMoveHorizontal = false;
                    // If direction is up then the distance = 1, Else direction is down and distance = -1 
                    //distance = (moveStatus == MoveStatus.Up ? -1 : 1) * rootFE.Height;
                    distance = (moveStatus == MoveStatus.Up ? -1 : 1) * b.Height;
                }

                // pull the animation after it's complete, because we move change Grid cells.
                DoubleAnimation slideAnim = new DoubleAnimation(distance, TimeSpan.FromSeconds(0.5), FillBehavior.Stop);

                //play the click sound
                System.Media.SoundPlayer sp = new System.Media.SoundPlayer("Click.wav");
                sp.Play();

                slideAnim.CurrentStateInvalidated += delegate(object sender2, EventArgs e2)
                {
                    // Anonymous delegate -- invoke when done.
                    Clock clock = (Clock)sender2;
                    if (clock.CurrentState != ClockState.Active)
                    {
                        // remove the render transform and really move the piece in the Grid.
                        try
                        {
                            MovePieceB(b, row, col);


                            _moveDirection = moveStatus.ToString();   //get the move direction and the button number
                            _selectedButtonNumber = b.Content.ToString();
                            //_previousMove = _selectedButtonNumber;      //Get the current and previously selected buttons
                            //_previousTime = _currentTime;
                            _currentTime = DateTime.Now;            //Set the times when the move was made


                            PrintToTextFile();  //print data to text file
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                };

                TranslateTransform buttonTransform = new TranslateTransform(0, 0);
                b.RenderTransform = buttonTransform;


                //perform the actual slide animation
                DependencyProperty directionProperty = isMoveHorizontal ? TranslateTransform.XProperty : TranslateTransform.YProperty;
                //rootFE.RenderTransform.BeginAnimation(directionProperty, slideAnim);
                b.RenderTransform.BeginAnimation(directionProperty, slideAnim);
            }
        }

        private void MovePieceB(DwellTimeButtonB b, int row, int col)
        { //*** MOVE THE TILE, ASSUMING THE MOVE IS VALID***
            //Identify the cell to move

            PuzzleCell newPosition = _puzzleLogic.MovePiece(row, col);
            try
            {
                //change the position of the tile in the grid
                b.SetValue(Grid.ColumnProperty, newPosition.Col);
                b.SetValue(Grid.RowProperty, newPosition.Row);

                //increment moves by 1
                _moves++;

                //Once the piece has been moved, then allow the next piece to be moved
                _isProcessingButton = false;
                //this.IsEnabled = true;//enable the grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Movement error !!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        #endregion

        #region CONFIG C
        private void CConfig()
        {
            #region ROW 1
            DwellTimeButtonC _00 = new DwellTimeButtonC();
            _00.SetValue(Grid.RowProperty, 0);
            _00.SetValue(Grid.ColumnProperty, 0);
            _00.Content = "2";
            this.Children.Add(_00);

            DwellTimeButtonC _01 = new DwellTimeButtonC();
            _01.SetValue(Grid.RowProperty, 0);
            _01.SetValue(Grid.ColumnProperty, 1);
            _01.Content = "1";
            this.Children.Add(_01);

            DwellTimeButtonC _02 = new DwellTimeButtonC();
            _02.SetValue(Grid.RowProperty, 0);
            _02.SetValue(Grid.ColumnProperty, 2);
            _02.Content = "6";
            this.Children.Add(_02);
            #endregion

            #region ROW 2
            DwellTimeButtonC _10 = new DwellTimeButtonC();
            _10.SetValue(Grid.RowProperty, 1);
            _10.SetValue(Grid.ColumnProperty, 0);
            _10.Content = "4";
            this.Children.Add(_10);

            //DwellTimeButtonC _11 = new DwellTimeButtonC();
            //_11.SetValue(Grid.RowProperty, 1);
            //_11.SetValue(Grid.ColumnProperty, 1);
            //_11.Content = "7";
            //this.Children.Add(_11);

            DwellTimeButtonC _12 = new DwellTimeButtonC();
            _12.SetValue(Grid.RowProperty, 1);
            _12.SetValue(Grid.ColumnProperty, 2);
            _12.Content = "8";
            this.Children.Add(_12);
            #endregion

            #region ROW 3
            DwellTimeButtonC _20 = new DwellTimeButtonC();
            _20.SetValue(Grid.RowProperty, 2);
            _20.SetValue(Grid.ColumnProperty, 0);
            _20.Content = "7";
            this.Children.Add(_20);

            DwellTimeButtonC _21 = new DwellTimeButtonC();
            _21.SetValue(Grid.RowProperty, 2);
            _21.SetValue(Grid.ColumnProperty, 1);
            _21.Content = "5";
            this.Children.Add(_21);

            DwellTimeButtonC _22 = new DwellTimeButtonC();
            _22.SetValue(Grid.RowProperty, 2);
            _22.SetValue(Grid.ColumnProperty, 2);
            _22.Content = "3";
            this.Children.Add(_22);
            #endregion

        }

        private void OnPuzzleButtonClickC(object sender, RoutedEventArgs e)
        {
            //*** ONCE A TILE HAS BEEN CLICKED MOVE IT IF IT IS A VALID MOVE ***
            if (_isProcessingButton == true)
            {
                //  this.IsEnabled = false;
                DwellTimeButtonC b = e.Source as DwellTimeButtonC;  //identify the button that has been clicked
                if (b != null)
                {
                    //Get the row and column of the button that has been cliced
                    int row = (int)b.GetValue(Grid.RowProperty);
                    int col = (int)b.GetValue(Grid.ColumnProperty);

                    //check to see if which direct the button should be moved
                    MoveStatus moveStatus = _puzzleLogic.GetMoveStatus(row, col);
                    _buttonMoveStatus = moveStatus.ToString();  //Move status of the button

                    if (moveStatus != MoveStatus.BadMove)
                    {
                        //as long as the move is valid, animate the movement by calling Animatepiece
                        AnimatePieceC(b, row, col, moveStatus);
                    }
                    else
                    {
                        //Bad move, so allow a new button to be selected
                        _isProcessingButton = false;
                        // this.IsEnabled = true;
                    }//if-else
                }//if
            }//_isprocessingButton
        }

        private void AnimatePieceC(DwellTimeButtonC b, int row, int col, MoveStatus moveStatus)
        {


            double distance; //distance the tile should move
            bool isMoveHorizontal; //determine íf move is horizontal or vertical

            //get the direction the tile should move
            //Debug.Assert(moveStatus != MoveStatus.BadMove);
            if (moveStatus != MoveStatus.BadMove)
            {
                if (moveStatus == MoveStatus.Left || moveStatus == MoveStatus.Right)
                {
                    isMoveHorizontal = true;
                    // If direction is left then the distance = -1, Else direction is right and distance = 1 
                    //distance = (moveStatus == MoveStatus.Left ? -1 : 1) * rootFE.Width;
                    distance = (moveStatus == MoveStatus.Left ? -1 : 1) * b.Width;
                }
                else
                {
                    isMoveHorizontal = false;
                    // If direction is up then the distance = 1, Else direction is down and distance = -1 
                    //distance = (moveStatus == MoveStatus.Up ? -1 : 1) * rootFE.Height;
                    distance = (moveStatus == MoveStatus.Up ? -1 : 1) * b.Height;
                }

                // pull the animation after it's complete, because we move change Grid cells.
                DoubleAnimation slideAnim = new DoubleAnimation(distance, TimeSpan.FromSeconds(0.5), FillBehavior.Stop);

                //play the click sound
                System.Media.SoundPlayer sp = new System.Media.SoundPlayer("Click.wav");
                sp.Play();

                slideAnim.CurrentStateInvalidated += delegate(object sender2, EventArgs e2)
                {
                    // Anonymous delegate -- invoke when done.
                    Clock clock = (Clock)sender2;
                    if (clock.CurrentState != ClockState.Active)
                    {
                        // remove the render transform and really move the piece in the Grid.
                        try
                        {
                            MovePieceC(b, row, col);


                            _moveDirection = moveStatus.ToString();   //get the move direction and the button number
                            _selectedButtonNumber = b.Content.ToString();
                            //_previousMove = _selectedButtonNumber;      //Get the current and previously selected buttons
                            //_previousTime = _currentTime;
                            _currentTime = DateTime.Now;            //Set the times when the move was made


                            PrintToTextFile();  //print data to text file
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                };

                TranslateTransform buttonTransform = new TranslateTransform(0, 0);
                b.RenderTransform = buttonTransform;


                //perform the actual slide animation
                DependencyProperty directionProperty = isMoveHorizontal ? TranslateTransform.XProperty : TranslateTransform.YProperty;
                //rootFE.RenderTransform.BeginAnimation(directionProperty, slideAnim);
                b.RenderTransform.BeginAnimation(directionProperty, slideAnim);
            }
        }



        private void MovePieceC(DwellTimeButtonC b, int row, int col)
        { //*** MOVE THE TILE, ASSUMING THE MOVE IS VALID***
            //Identify the cell to move

            PuzzleCell newPosition = _puzzleLogic.MovePiece(row, col);
            try
            {
                //change the position of the tile in the grid
                b.SetValue(Grid.ColumnProperty, newPosition.Col);
                b.SetValue(Grid.RowProperty, newPosition.Row);

                //increment moves by 1
                _moves++;

                //Once the piece has been moved, then allow the next piece to be moved
                _isProcessingButton = false;
                //this.IsEnabled = true;//enable the grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Movement error !!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        #endregion

        public void StartConfig(string config)
        {
            switch (config)
            {
                case "Trial": //use the trial configuration
                    {
                        _puzzleLogic.Config_Trial();//Call the trial configuration
                        short cellNumber = 1;
                        foreach (Button b in this.Children)
                        {
                            PuzzleCell location = _puzzleLogic.FindCell(cellNumber++);
                            b.SetValue(Grid.ColumnProperty, location.Col);
                            b.SetValue(Grid.RowProperty, location.Row);

                        }
                    } break;
                case "ConfigA": //use the trial configuration
                    {
                        _puzzleLogic.Config_A();//Call the trial configuration
                    } break;
                case "ConfigB": //use the trial configuration
                    {
                        _puzzleLogic.Config_B();//Call the trial configuration
                    } break;
                case "ConfigC":
                    {
                        _puzzleLogic.Config_C();
                    } break;
            }


        }


        #region TEXT FILE DATA
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //get the time the window was opened
            _gridOpenTime = DateTime.Now;

            _currentFile = "DwellTime_Puzzle_Config" + _ConfigType + "_" + DateTime.Now.ToFileTime() + ".txt";

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