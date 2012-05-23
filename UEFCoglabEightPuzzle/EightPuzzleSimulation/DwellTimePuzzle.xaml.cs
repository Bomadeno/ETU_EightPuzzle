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
using System.Windows.Threading;
using TetComp;

namespace EightPuzzleSimulation
{
    /// <summary>
    /// Interaction logic for DwellTimePuzzle.xaml
    /// </summary>
    public partial class DwellTimePuzzle : Window
    {
        #region PRIVATE PROPERTIES

        public static Canvas _transCanvas;              //static transparent canvas 
        public static Canvas _hitCanvas;
        public static StackPanel _puzzleHostingPanel;   //puzze hosting panel
        
        public static Point _smoothGazePoint;
        public static Point _gazePoint;                 //Gaze Point
        public static bool _entered = false;            //determine if a button has previously been entered
        DispatcherTimer gazePointTimer;                 //retrieve the gaze point every couple of seconds
     
        DwellTimePuzzleGrid _dwellTimePuzzleGrid;                         //instance of MousePuzzleGrid
        private int _numRows;                           //Number of rows in the grid
        private Image masterImage;                      //image resources
        private Size _puzzleSize;                       //Size of the puzzle

        //******************to be removed
        public static TextBlock tbMouse;
        public static TextBlock tbDwellTime;

        //EYE TRACKING COMPONENTS
        ITetTrackStatus tetTrackStatus;
        private AxTetComp.AxTetTrackStatus axTetTrackStatus; // track status window
        TetClient tetClient;                //Tobii Eye Tracking client

        #endregion

        public DwellTimePuzzle()
        {
            InitializeComponent();
            DwellTimeButton.DWELL_TIME = 1000;
            _numRows = 3; //default _numRows value

            //TETCOMP 
            tetClient = new TetClientClass();
            _ITetClientEvents_Event tetClientEvents = (_ITetClientEvents_Event)tetClient;
            tetClientEvents.OnGazeData += new _ITetClientEvents_OnGazeDataEventHandler(TetClientEvent_OnGazeData);
           

            #region INITIALIZE STATIC CANVASES AND HOSTINGPANEL
            //Main window canvas
            _hitCanvas = new Canvas();
            _hitCanvas.Width = 1280;    //1280 //in tobii
            _hitCanvas.Height = 1024;   //1024 //in tobii
            //Canvas.SetLeft(_hitCanvas, 0);
            //Canvas.SetTop(_hitCanvas, 0);
            this.AddChild(_hitCanvas);

            _puzzleHostingPanel = new StackPanel();
            _puzzleHostingPanel.Margin = new Thickness(100);
            Canvas.SetLeft(_puzzleHostingPanel, 200);
            Canvas.SetTop(_puzzleHostingPanel, 50);
            _hitCanvas.Children.Add(_puzzleHostingPanel);


            //gaze point timer
            gazePointTimer = new DispatcherTimer();
            gazePointTimer.IsEnabled = true;
            gazePointTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            gazePointTimer.Tick += new EventHandler(gazePointTimer_Tick);

            //Initialize the gazepoint array and set the initial capacity to 5
            _gazePoint = new Point();
            _smoothGazePoint = new Point();

            //Mouse pointer text block
            tbMouse = new TextBlock();
            tbMouse.Text = "0";
            Canvas.SetTop(tbMouse, 0);
            Canvas.SetLeft(tbMouse, 0);
            _hitCanvas.Children.Add(tbMouse);


            //Dwell time text block
            tbDwellTime = new TextBlock();
            tbDwellTime.Text = "0";
            Canvas.SetTop(tbDwellTime, 0);
            Canvas.SetLeft(tbDwellTime, 200);
            _hitCanvas.Children.Add(tbDwellTime);

            //***********TRANSPARENT LAYER*************\\
            _transCanvas = new Canvas();
            _transCanvas.Background = Brushes.Transparent;
            _transCanvas.Width = _hitCanvas.Width;
            _transCanvas.Height = _hitCanvas.Height;
            Canvas.SetLeft(_transCanvas, 0);
            Canvas.SetTop(_transCanvas, 0);
            _hitCanvas.Children.Add(_transCanvas);

            #endregion
        }

        private void NewGame()
        {    //*** START A NEW GAME ***
            masterImage = (Image)this.Resources["MasterImage1"]; //Get the game image

            BitmapSource bitmap = (BitmapSource)masterImage.Source;
            _puzzleSize = new Size(bitmap.PixelWidth * 1.8, bitmap.PixelHeight * 1.8); //Set the size of the image

            //check if a puzzle already exisits
            if (_dwellTimePuzzleGrid != null)
            {
                _puzzleHostingPanel.Children.Remove(_dwellTimePuzzleGrid); //true remove it
            }

            _dwellTimePuzzleGrid = new DwellTimePuzzleGrid(); //initialise _MousePuzzleGrid

            _dwellTimePuzzleGrid.NumRows = _numRows; //number of rows in the grid

            _dwellTimePuzzleGrid.PuzzleImage = masterImage; //background image

            _dwellTimePuzzleGrid.PuzzleSize = _puzzleSize; //size of the puzzle

            _dwellTimePuzzleGrid.ConfigType = "A";           //Use game congiguration A
            //_dwellTimePuzzleGrid.ConfigType = "B";         //Use game configuration B
            //_dwellTimePuzzleGrid.ConfigType = "T";         //Use trial configuration

            _puzzleHostingPanel.Children.Add(_dwellTimePuzzleGrid); //add grid to the user interface


            _dwellTimePuzzleGrid.StartConfig("ConfigA");       //Use start configuration A
            //_dwellTimePuzzleGrid.StartConfig("ConfigB");       //Use start configuration B
            //_dwellTimePuzzleGrid.StartConfig("Trial");       //Use the trial configuration to start


        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //check to see whether or not to load the trackstaus canvas
            LoadTrackStatusObject();

            //start a new game when the puzzle is loaded
            NewGame();
            //this.Cursor = Cursors.Hand;
            //hide the cursor
            this.Cursor = Cursors.None;

        }

        private void LoadTrackStatusObject()
        {

             this.axTetTrackStatus = new AxTetComp.AxTetTrackStatus();

             this.axTetTrackStatus.Enabled = true;
             this.axTetTrackStatus.Name = "axTetTrackStatus";
             this.axTetTrackStatus.OcxState = ((System.Windows.Forms.AxHost.State)(Resources.FindName("axTetTrackStatus.OcxState")));

             //used to host System.Windows.Forms control in WPF
             System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
             host.Child = axTetTrackStatus; //add the trackstatus window to the host
             host.Width = 120;       //Size of the host controls the size of the control
             host.Height = 70;
             Canvas.SetBottom(host, 5);      //position the host on Canvas_TrackStatus 
             Canvas.SetRight(host, 10);
             //this.Canvas_TrackStatus.Children.Add(host); // add the host to the track status canvas
             _hitCanvas.Children.Add(host);
             
             // Retreive underlying references to ActiveX controls
             tetTrackStatus = (ITetTrackStatus)axTetTrackStatus.GetOcx();

             //*** try to contect to the server

             try
             {
                 // Connect to the TET server if necessary
                 if (!tetTrackStatus.IsConnected) tetTrackStatus.Connect("193.167.42.30", (int)TetConstants.TetConstants_DefaultServerPort);

                 // Start the track status meter
                 if (!tetTrackStatus.IsTracking) tetTrackStatus.Start();
                
                 //Connect to the TET Client if necessary
                 if (!tetClient.IsConnected)
                 {
                     tetClient.Connect("193.167.42.30", (int)TetConstants.TetConstants_DefaultServerPort, TetSynchronizationMode.TetSynchronizationMode_Server);
                 }
                 tetClient.StartTracking();//Start tracking
             }
             catch (Exception ex)
             {

                 MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
             }

            
             

        }

        void gazePointTimer_Tick(object sender, EventArgs e)
        {

            _smoothGazePoint = _gazePoint; //Get the smooth gaze point    
            tbMouse.Text = _smoothGazePoint.X.ToString() + " : " + _smoothGazePoint.Y.ToString();
            if(DwellTimePuzzleGrid.IsProcessingButton == false)
                DwellTimeButton.OnHitTest(_smoothGazePoint);
            
        }

        //private void Window_MouseMove(object sender, MouseEventArgs e)
        //{
        //    _gazePoint = e.GetPosition(this);
        //   // DwellTimeButton.OnHitTest(_smoothGazePoint);


        //}

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();   //Close the window
            }
           


        }

        private void TetClientEvent_OnGazeData(ref TetGazeData gazeData)
        {

            float x, y;
            double xCoord, yCoord;
            int SCREEN_WIDTH = 1280;
            int SCREEN_HEIGHT = 1024;


            //Use data only if both the left and right eyes are found by the eye tracker
            if (gazeData.validity_lefteye == 0 && gazeData.validity_righteye == 0)
            {

                // let the x and y be the average of the left and right eye
                x = (gazeData.x_gazepos_lefteye + gazeData.x_gazepos_righteye) / 2;
                y = (gazeData.y_gazepos_lefteye + gazeData.y_gazepos_righteye) / 2;

                xCoord = x * SCREEN_WIDTH;
                yCoord = y * SCREEN_HEIGHT;

                _gazePoint = new Point(xCoord, yCoord);


            }//if interaction

        }//method
    }
}
