using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections;
using System.Windows.Threading;

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for GazeAugmentedButton.xaml
    /// </summary>
    public partial class GazeAugmentedButton : Button
    {
        #region PRIVATE FIELDS AND PUBLIC PROPERTIES
        private static Int32 _dwellTime;                               //Total dwell time for the class in milliseconds value is constant
        private static Int32 _dwellTimeCurrentButton;                   //Dwell time of the button currently being selected

        private static ArrayList _buttonList = new ArrayList();         //List of all buttons that have been created
        private static ArrayList _hitResultsList = new ArrayList();     //Stores the results of the hittest        
        private static ArrayList _selectedButtonList = new ArrayList(); //List of the currently and previously selected buttons 

        private static DispatcherTimer _dwellTimeTimer = new DispatcherTimer();

        private static MouseEventArgs _mouseEnterArgs;                  //Mouse enter event argument
        private static MouseEventArgs _mouseLeaveArgs;                  //mouse leave event argument

        /// <summary>
        /// Dwell time duration in milliseconds
        /// </summary>
        public static Int32 DwellTimeDuration
        {
            set
            {
                _dwellTime = value;
                _dwellTimeCurrentButton = _dwellTime;
            }
        }

        #region COLORS and BRUSHES
        private SolidColorBrush _mouseEnterBrush;
        private static SolidColorBrush _mouseClickedBrush;
        private SolidColorBrush _whiteButtonBackground;

        private Color _greenBrush;
        private Color _redBrush;
        #endregion

        #endregion

        public GazeAugmentedButton()
        {
            InitializeComponent();

            _buttonList.Add(this);

            _dwellTimeTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _dwellTimeTimer.Tick += DwellTimeTimerTick;

            #region MOUSE EVENTS
            _mouseEnterArgs = new MouseEventArgs(Mouse.PrimaryDevice, 0) {RoutedEvent = Mouse.MouseEnterEvent};

            _mouseLeaveArgs = new MouseEventArgs(Mouse.PrimaryDevice, 0) {RoutedEvent = Mouse.MouseLeaveEvent};

            #endregion

            #region BRUSHES
            _redBrush = (Color)ColorConverter.ConvertFromString("#fe2712");
            _greenBrush = (Color)ColorConverter.ConvertFromString("#66b032");

            _mouseEnterBrush = new SolidColorBrush(_greenBrush);
            _mouseClickedBrush = new SolidColorBrush(_redBrush);
            #endregion
           
            Background = WhiteBackground();
        }

        private SolidColorBrush WhiteBackground()
        {
            _whiteButtonBackground = new SolidColorBrush {Color = Colors.White};

            BorderBrush = new SolidColorBrush(Colors.Black);
            Margin = new Thickness(10);
            FontSize = 60;
            FontWeight = FontWeights.SemiBold;

            return _whiteButtonBackground;
        }


        protected override void OnMouseEnter(MouseEventArgs e)
        {
            Background = _mouseEnterBrush;
            //Start dwell timer
            _dwellTimeTimer.Start();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            this.Background = Brushes.White;   //set background to white when mouse leaves

            _dwellTimeTimer.Stop();

            //remove button from selected index
            _selectedButtonList.Clear();

            base.OnMouseLeave(e);
        }

        void DwellTimeTimerTick(object sender, EventArgs e)
        {
            //perform another hitest to make sure point is still inside
            //if inside then decrement the counter
            foreach (GazeAugmentedButton gab in _selectedButtonList)
            {
                Rect bounds = gab.TransformToAncestor(Puzzle.HitCanvas).TransformBounds(new Rect(0, 0, gab.ActualWidth, gab.ActualHeight));
                if (bounds.Contains(Puzzle.SmoothGazePoint))
                {
                    if (_dwellTimeCurrentButton > 0 )
                    {
                        _dwellTimeCurrentButton--;
                    }
                }
                else
                {
                    gab.RaiseEvent(_mouseLeaveArgs);
                    break;
                }
            }
        }

        public static void OnHitTest(Point p)
        {
            _hitResultsList.Clear();
            
            // Set up a callback to receive the hit test results enumeration.
            VisualTreeHelper.HitTest(Puzzle.HitCanvas,
                                     null,
                                     GazeButtonHitTestResult,
                                     new PointHitTestParameters(p));

            // Perform actions on the hit test results list and the selected button list is empty
            if ((_hitResultsList.Count > 0) && (_selectedButtonList.Count == 0))
            {
                ProcessHitTestResultsList(p);
            }
        }

        /// <summary>
        /// Hit Test Result Behavior -Handle the hit test results enumeration in the callback.
        /// only handles buttons
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal static HitTestResultBehavior GazeButtonHitTestResult(HitTestResult result)
        {// Add the hit test result to the list that will be processed after the enumeration.

            if ((Equals(Puzzle.HitCanvas, result.VisualHit)) || (Equals(Puzzle.TransCanvas, result.VisualHit)) ||
                (Equals(Puzzle.TbMouse, result.VisualHit)) || (Equals(Puzzle._puzzleHostingPanel, result.VisualHit)) || (Equals(Puzzle.TargetImage, result.VisualHit)))
            {
            }
            else
            {
                _hitResultsList.Add(result.VisualHit);

            }
            // Set the behavior to stop the enumeration of visuals.
            return HitTestResultBehavior.Continue;

        }

        /// <summary>
        /// Process the results of the hit test after the enumeration in the callback.
        /// </summary>
        internal static void ProcessHitTestResultsList(Point p)
        {

            foreach (GazeAugmentedButton gab in _buttonList)
            {

                Rect bounds = gab.TransformToAncestor(Puzzle.HitCanvas).TransformBounds(new Rect(0, 0, gab.ActualWidth, gab.ActualHeight));
                if (bounds.Contains(p))
                {
                    //add the button to the selected button list
                    _selectedButtonList.Add(gab);

                    //raise the mouse enter event
                    gab.RaiseEvent(_mouseEnterArgs);

                    break;
                }
            }
        }

        public static void ButtonSelected()
        {
            //Click the selected button
            foreach (GazeAugmentedButton gab in _selectedButtonList)
            {
                gab.Background = _mouseClickedBrush;
                gab.RaiseEvent(new RoutedEventArgs(ClickEvent));
            }
        }
    }
}
