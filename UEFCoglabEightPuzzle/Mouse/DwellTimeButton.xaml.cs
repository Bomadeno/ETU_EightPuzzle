using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections;
using System.Windows.Threading;

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for DwellTimeButton.xaml
    /// </summary>
    public partial class DwellTimeButton : Button
    {
        #region PRIVATE FIELDS AND PUBLIC PROPERTIES
        private static ArrayList _buttonList = new ArrayList();             //List of all buttons that have been created
        private static ArrayList _hitResultsList = new ArrayList();         //Stores the results of the hit-test        
        private static ArrayList _selectedButtonList = new ArrayList();     //List of the currently and previously selected buttons 
        private static ArrayList _enteredButtonList = new ArrayList();

        private DispatcherTimer _timer;
       
        private static int _DWELL_TIME;
        private int _dwelltime;
        private bool _isClicked;

       
        public static int DWELL_TIME
        {
            set 
            {
                _DWELL_TIME = value;
            }
        }

        private static bool _entered = false;                               //determine if gaze is inside button

        private static MouseEventArgs _mouseEnterArgs;
        private static MouseEventArgs _mouseLeaveArgs;

        private AnimationClock _enterClock;
        private AnimationClock _leaveClock;
        private ColorAnimation _enterColorAnimation;                        // = new ColorAnimation();
        private ColorAnimation _leavecolorAnimation;                        // = new ColorAnimation();
     
        #region COLORS and BRUSHES
        private SolidColorBrush _animatedBrush;     //Brush to be animated
        private SolidColorBrush _whiteButtonBackground;

        private Color _greenBrush = new Color();              //green brush
        private Color _redBrush = new Color();              //red brush

        private Color _blueBrush0 = new Color();
        private Color _blueBrush1 = new Color();

        private LinearGradientBrush _backgroundBrush;
        private GradientStop _gradientStop0;
        private GradientStop _gradientStop1;
        #endregion

       
        #endregion

        public DwellTimeButton()
        {
            InitializeComponent();

            _isClicked = false;

            _buttonList.Add(this);

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            _timer.Tick += new EventHandler(_timer_Tick);
         //   _timer.Start();

            #region MOUSE EVENTS
            _mouseEnterArgs = new MouseEventArgs(Mouse.PrimaryDevice, 0);
            _mouseEnterArgs.RoutedEvent = Mouse.MouseEnterEvent;

            _mouseLeaveArgs = new MouseEventArgs(Mouse.PrimaryDevice, 0);
            _mouseLeaveArgs.RoutedEvent = Mouse.MouseLeaveEvent;
            #endregion

            #region ANIMATION COLORS
            //_greenBrush = (Color)ColorConverter.ConvertFromString("#66b032");     
            _redBrush = (Color)ColorConverter.ConvertFromString("#fe2712");
            _greenBrush = (Color)ColorConverter.ConvertFromString("#ffffff");
        #endregion

            #region COLOR ANIMATION
            _animatedBrush = new SolidColorBrush();
            _animatedBrush.Color = _greenBrush; //inital color

            _enterColorAnimation = new ColorAnimation();
            _enterColorAnimation.To = _redBrush;
            _enterColorAnimation.Duration = TimeSpan.FromMilliseconds(_DWELL_TIME);
            _enterClock = _enterColorAnimation.CreateClock();
            _enterClock.CurrentTimeInvalidated += new EventHandler(_enterClock_CurrentTimeInvalidated);

            _leavecolorAnimation = new ColorAnimation();
            _leavecolorAnimation.Duration = TimeSpan.FromMilliseconds(_DWELL_TIME/2);
            _leaveClock = _leavecolorAnimation.CreateClock();
            
            #endregion

            _dwelltime = _DWELL_TIME; //set the value for the dwell time

           // this.Background =  buttonBackground();
            this.Background = whiteBackground();
        }
       
        private LinearGradientBrush buttonBackground()
        {
            _backgroundBrush = new LinearGradientBrush();
            _blueBrush0 = (Color)ColorConverter.ConvertFromString("#306EFF");
            _blueBrush1 = (Color)ColorConverter.ConvertFromString("#AFDCEC");

            _gradientStop0 = new GradientStop(_blueBrush0, 0);
            _gradientStop1 = new GradientStop(_blueBrush1, 1);

            _backgroundBrush.GradientStops.Add(_gradientStop0);
            _backgroundBrush.GradientStops.Add(_gradientStop1);
            
            this.BorderBrush = new SolidColorBrush(Colors.Black);
            this.BorderThickness = new Thickness(10);
            this.Margin = new Thickness(10);
            this.FontSize = 60;
            this.FontWeight = FontWeights.SemiBold;
            return _backgroundBrush;
        }
        
        private SolidColorBrush whiteBackground()
        {
            _whiteButtonBackground = new SolidColorBrush();
            _whiteButtonBackground.Color = Colors.White;

            this.BorderBrush = new SolidColorBrush(Colors.Black);
            this.Margin = new Thickness(10);
            this.FontSize = 60;
            //this.Padding = new Thickness(4);
            this.FontWeight = FontWeights.SemiBold;
           

            return _whiteButtonBackground;
        }
      
        void _timer_Tick(object sender, EventArgs e)
            {
                
                    foreach (DwellTimeButton db in _selectedButtonList)
                    {
                        Rect bounds = db.TransformToAncestor(Puzzle.HitCanvas).TransformBounds(new Rect(0, 0, db.ActualWidth, db.ActualHeight));
                        if (bounds.Contains(Puzzle.SmoothGazePoint) && (_entered == false))
                        {
                            _entered = true;
                            _enteredButtonList.Add(db);
                           
                        }
                        else if (!(bounds.Contains(Puzzle.SmoothGazePoint) && (_entered == true)))
                        {
                            
                            db.RaiseEvent(_mouseLeaveArgs);
                            _entered = false;
                            _enteredButtonList.Clear();
                           
                        }
                    }
            }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            _timer.Start(); //start the timer

            _isClicked = false;

            if (_entered == false) //check if button was left
                _leaveClock.Controller.Stop(); //stop the leave color animation
            EnterColorAnimation(); //Start/resume the enter color animation
            
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            _timer.Start(); //stop the timer

            _enterClock.Controller.Stop(); //pause the enter color animation
            if(!_isClicked)
                LeaveColorAnimation();         //start/resume the leave color animation

            base.OnMouseLeave(e);
        }

        private void EnterColorAnimation()
        {
            if (_enterClock.IsPaused) //resume the animation
            {
                _enterColorAnimation.From = _animatedBrush.Color;
                _animatedBrush.ApplyAnimationClock(SolidColorBrush.ColorProperty, _enterClock);
                _enterClock.Controller.Resume();
            }
            else //begin a new animation
            {
                _enterColorAnimation.From = _greenBrush;
                _animatedBrush.ApplyAnimationClock(SolidColorBrush.ColorProperty, _enterClock);
                _enterClock.Controller.Begin();

            }

            this.Background = _animatedBrush;
            //ButtonAction();
        }

        private void LeaveColorAnimation()
        {
            if (_leaveClock.IsPaused) //resume the animation
            {

                _leavecolorAnimation.From = _animatedBrush.Color;
                _leavecolorAnimation.To = _greenBrush;
                _animatedBrush.ApplyAnimationClock(SolidColorBrush.ColorProperty, _leaveClock);
                _leaveClock.Controller.Begin();
            }
            else
            {
                _leavecolorAnimation.From = _animatedBrush.Color;
                _leavecolorAnimation.To = _greenBrush;
                _animatedBrush.ApplyAnimationClock(SolidColorBrush.ColorProperty, _leaveClock);
                _leaveClock.Controller.Begin();
            }
                this.Background = _animatedBrush;
               // ButtonAction();
        }

        protected override void OnClick()
        {
            //once the button has been clicked, stop the leave animation 
            //and make all buttons white
            _leaveClock.Controller.Stop();
            foreach (DwellTimeButton db in _buttonList)
            {
                db.Background = Brushes.White;
            }
            base.OnClick();
        }

        void _enterClock_CurrentTimeInvalidated(object sender, EventArgs e)
        {
            if (_enterClock.CurrentProgress.HasValue && _enteredButtonList.Count == 1)
            {
                foreach (DwellTimeButton db in _enteredButtonList)
                {
                    if (_enterClock.CurrentProgress.Value >= 1) //&&move status !=badmove
                    {
                        //DwellTimePuzzleGrid.IsProcessingButton = true;
                        db.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                        //db.Content = "clicked";
                        _isClicked = true;
                        if (PuzzleGrid.ButtonMoveStatus != MoveStatus.BadMove)
                            this.Background = Brushes.White;
                    }
                    else
                        Puzzle.TbMouse.Text = _enterClock.CurrentProgress.Value.ToString();
                }


            }
        }

        #region HITTEST
        public static void OnHitTest(Point p)
        {
            // Clear the contents of the list used for hit test results.
            _hitResultsList.Clear();

            // Set up a callback to receive the hit test results enumeration.
            VisualTreeHelper.HitTest(Puzzle.HitCanvas,
                                     null,
                                     new HitTestResultCallback(GazeButtonHitTestResult),
                                     new PointHitTestParameters(p));

            // Perform actions on the hit test results list and the selected button list is empty
            if ((_hitResultsList.Count > 0) && _entered==false) //(_selectedButtonList.Count <= 0))
            {
                ProcessHitTestResultsList(p);
            }

        }

        internal static HitTestResultBehavior GazeButtonHitTestResult(HitTestResult result)
        {// Add the hit test result to the list that will be processed after the enumeration.

            if ((Equals(Puzzle.HitCanvas, result.VisualHit)) || (Equals(Puzzle.TransCanvas, result.VisualHit)) ||
                (Equals(Puzzle.TbMouse, result.VisualHit)) || (Equals(Puzzle.TbMouse, result.VisualHit)) || (Equals(Puzzle.TargetImage, result.VisualHit)))
            {
            }
            else
            {
                _hitResultsList.Add(result.VisualHit);

            }
            // Set the behavior to stop the enumeration of visuals.
            return HitTestResultBehavior.Continue;

        }

        internal static void ProcessHitTestResultsList(Point p)
        {//SolidColorBrush sb = new SolidColorBrush(_greenBrush);
            foreach (DwellTimeButton db in _buttonList)
            {

                Rect bounds = db.TransformToAncestor(Puzzle.HitCanvas).TransformBounds(new Rect(0, 0, db.ActualWidth, db.ActualHeight));
                if (bounds.Contains(p))
                {

                    //add the button to the selected button list
                    _selectedButtonList.Add(db);
                    //  db.Background = sb;
                    db.RaiseEvent(_mouseEnterArgs);

                    break;

                }
            }


        }

        #endregion

    }



}
