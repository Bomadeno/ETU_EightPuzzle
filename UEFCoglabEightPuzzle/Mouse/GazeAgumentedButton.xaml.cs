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
    /// Interaction logic for GazeAgumentedButton.xaml
    /// </summary>
    public partial class GazeAgumentedButton : Button
    {
        #region PRIVATE FIELDS AND PUBLIC PROPERTIES
        private static Int32 _DWELL_TIME;                           //Total dwell time for the class in milliseconds value is constant
        private static Int32 _dwellTimeCurrentButton;                          //Dwell time of the button currently being selected

        private static ArrayList _buttonList = new ArrayList();                //List of all buttons that have been created
        private static ArrayList _hitResultsList = new ArrayList();           //Stores the results of the hittest        
        private static ArrayList _selectedButtonList = new ArrayList();         //List of the currently and previously selected buttons 

        private static DispatcherTimer _timer = new DispatcherTimer();     //Dwelltime timer

        private static MouseEventArgs _mouseEnterArgs;// = new MouseEventArgs(Mouse.PrimaryDevice, 0);                                     //Mouse enter event argument
        private static MouseEventArgs _mouseLeaveArgs;// = new MouseEventArgs(Mouse.PrimaryDevice, 0);                                      //mouse leave event argument

        /// <summary>
        /// Dwell time duration in milliseconds
        /// </summary>
        public static Int32 DwellTimeDuration
        {
            set
            {
                _DWELL_TIME = value;                             //Assign value for the class dwell time 
                _dwellTimeCurrentButton = _DWELL_TIME;                        //Dwell time for the currently selected button
                           
            }
        }

        #region COLORS and BRUSHES
        private SolidColorBrush _mouseEnterBrush;     //mouse enter brush
        private static SolidColorBrush _mouseClickedBrush;  //mouse clicked brush
        private SolidColorBrush _whiteButtonBackground;

        private Color _greenBrush = new Color();              //green brush
        private Color _redBrush = new Color();              //red brush
        #endregion

        #endregion

        public GazeAgumentedButton()
        {
            InitializeComponent();

            //Add created button to the button list
            _buttonList.Add(this);

            //initalize timer
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 100);      //timer interval is 100 milliseconds
            _timer.Tick += new EventHandler(_timer_Tick);

            #region MOUSE EVENTS
            _mouseEnterArgs = new MouseEventArgs(Mouse.PrimaryDevice, 0);
            _mouseEnterArgs.RoutedEvent = Mouse.MouseEnterEvent;

            _mouseLeaveArgs = new MouseEventArgs(Mouse.PrimaryDevice, 0);
            _mouseLeaveArgs.RoutedEvent = Mouse.MouseLeaveEvent;
            #endregion

            #region BRUSHES
            _redBrush = (Color)ColorConverter.ConvertFromString("#fe2712");
            _greenBrush = (Color)ColorConverter.ConvertFromString("#66b032");

            _mouseEnterBrush = new SolidColorBrush(_greenBrush);
            _mouseClickedBrush = new SolidColorBrush(_redBrush);
            #endregion
           
          //Set the button stlye
          //  this.Style = this.FindResource("TraficLightButton") as Style;
            this.Background = WhiteBackground();
        }

        private SolidColorBrush WhiteBackground()
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


        protected override void OnMouseEnter(MouseEventArgs e)
        {
            this.Background = _mouseEnterBrush; //set background to green when mouse enters
            //Start dwell timer
            _timer.Start();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            this.Background = Brushes.White;   //set background to white when mouse leaves

            //Stop dwell timer
            _timer.Stop();

            //remove button from selected index
            _selectedButtonList.Clear();

            base.OnMouseLeave(e);
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            
            //perform another hitest to make sure oint is still inside
            //if inside then decrement the counter
            foreach (GazeAgumentedButton gab in _selectedButtonList)
            {
                Rect bounds = gab.TransformToAncestor(Puzzle._hitCanvas).TransformBounds(new Rect(0, 0, gab.ActualWidth, gab.ActualHeight));
                if (bounds.Contains(Puzzle._smoothGazePoint))
                {
                    if (_dwellTimeCurrentButton > 0 )
                    {
                        //decrement the timer 
                        _dwellTimeCurrentButton--;  //decrement the timer 
                    }
                }
                else
                {

                    //raise the mouse enter event
                    gab.RaiseEvent(_mouseLeaveArgs);
                    break;
                }
            }
            //else raise the mouseleave event

        }

        public static void OnHitTest(Point p)
        {
            // Clear the contents of the list used for hit test results.
            _hitResultsList.Clear();
            
            // Set up a callback to receive the hit test results enumeration.
            VisualTreeHelper.HitTest(Puzzle._hitCanvas,
                                     null,
                                     new HitTestResultCallback(GazeButtonHitTestResult),
                                     new PointHitTestParameters(p));

            // Perform actions on the hit test results list and the selected button list is empty
            if ((_hitResultsList.Count > 0) && (_selectedButtonList.Count == 0))
            {
                ProcessHitTestResultsList(p);
            }

            //check to see if gaze has left the currently selected button
            //if (selectedButtonList.Count > 0)
            //{

            //}
        }

        /// <summary>
        /// Hit Test Result Behavior -Handle the hit test results enumeration in the callback.
        /// only handles buttons
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal static HitTestResultBehavior GazeButtonHitTestResult(HitTestResult result)
        {// Add the hit test result to the list that will be processed after the enumeration.

            if ((Equals(Puzzle._hitCanvas, result.VisualHit)) || (Equals(Puzzle._transCanvas, result.VisualHit)) ||
                (Equals(Puzzle.tbMouse, result.VisualHit)) || (Equals(Puzzle._puzzleHostingPanel, result.VisualHit)) || (Equals(Puzzle._targetImage, result.VisualHit)))
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

            foreach (GazeAgumentedButton gab in _buttonList)
            {

                Rect bounds = gab.TransformToAncestor(Puzzle._hitCanvas).TransformBounds(new Rect(0, 0, gab.ActualWidth, gab.ActualHeight));
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
            foreach (GazeAgumentedButton gab in _selectedButtonList)
            {
                GazeAugmentedPuzzleGrid.IsProcessingButton = true;
                gab.Background = _mouseClickedBrush;
                gab.RaiseEvent(new RoutedEventArgs(ClickEvent));
            }
        }

    }

}
