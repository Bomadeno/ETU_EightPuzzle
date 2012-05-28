using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for MouseButton2.xaml
    /// </summary>
    public partial class MouseButton2 : Button
    {
        #region PRIVATE FIELDS AND PUBLIC PROPERTIES


        #region COLORS and BRUSHES
        private readonly Color _mouseEnterColor;
        private readonly Color _mouseClickedColor;
        private readonly SolidColorBrush _mouseEnterBrush;
        private readonly SolidColorBrush _mouseClickedBrush;
        private SolidColorBrush _whiteButtonBackground;

        #endregion


        #endregion
        
        public MouseButton2()
        {
            InitializeComponent();

            _mouseClickedColor = (Color)ColorConverter.ConvertFromString("#fe2712");
            _mouseEnterColor = (Color)ColorConverter.ConvertFromString("#66b032");

            _mouseEnterBrush = new SolidColorBrush(_mouseEnterColor);
            _mouseClickedBrush = new SolidColorBrush(_mouseClickedColor);

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
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            this.Background = Brushes.White;   //set background to white when mouse leaves
            base.OnMouseLeave(e);
        }
    }
}
