using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for MouseButton2.xaml
    /// </summary>
    public partial class MouseButton2
    {
        #region PRIVATE FIELDS AND PUBLIC PROPERTIES


        #region COLORS and BRUSHES
        private readonly Color _mouseEnterColor = (Color)ColorConverter.ConvertFromString("#66b032");
        private readonly Color _mouseClickedColor = (Color)ColorConverter.ConvertFromString("#fe2712");
        private readonly SolidColorBrush _mouseEnterBrush;
        private readonly SolidColorBrush _mouseClickedBrush;
        private SolidColorBrush _whiteButtonBackground;

        #endregion


        #endregion
        
        public MouseButton2()
        {
            InitializeComponent();


            _mouseEnterBrush = new SolidColorBrush(_mouseEnterColor);
            _mouseClickedBrush = new SolidColorBrush(_mouseClickedColor);

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
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            Background = Brushes.White;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Background = _mouseClickedBrush;
            base.OnMouseDown(e);
        }
    }
}
