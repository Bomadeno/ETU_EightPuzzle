using System.Windows;
using System.Windows.Input;

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for ThankYou.xaml
    /// </summary>
    public partial class ThankYou : Window
    {
        public ThankYou()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Window startupWindow = new StartupWindow();
            startupWindow.Show();
            Close();
        }
    }
}
