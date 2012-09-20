using System.Windows;
using System.Windows.Input;

namespace EightPuzzle_Mouse
{
    /// <summary>
    /// Interaction logic for ThankYou.xaml
    /// </summary>
    public partial class ThankYou
    {
        public ThankYou()
        {
            InitializeComponent();
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            Window startupWindow = new StartupWindow();
            startupWindow.Show();
            Close();
        }
    }
}
