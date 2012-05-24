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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for MouseButton2.xaml
    /// </summary>
    public partial class MouseButton2 : Button
    {
        #region PRIVATE FIELDS AND PUBLIC PROPERTIES
        

        #region COLORS and BRUSHES
        private SolidColorBrush _mouseEnterBrush;     //mouse enter brush
        protected SolidColorBrush _mouseClickedBrush;  //mouse clicked brush
        private SolidColorBrush _whiteButtonBackground;

        private Color _greenBrush = new Color();              //green brush
        private Color _redBrush = new Color();              //red brush
        #endregion


        #endregion
        
        public MouseButton2()
        {
            InitializeComponent();

            _redBrush = (Color)ColorConverter.ConvertFromString("#fe2712");
            _greenBrush = (Color)ColorConverter.ConvertFromString("#66b032");

            _mouseEnterBrush = new SolidColorBrush(_greenBrush);
            _mouseClickedBrush = new SolidColorBrush(_redBrush);

            this.Background = whiteBackground();
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
