using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiftSimulator
{
    public partial class MainWindow : Window
    {
        cSettings CurrentSettings = new cSettings();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow tempSettingsWindow = new SettingsWindow();
            rectDarkWindow.Visibility = Visibility.Visible;
            tempSettingsWindow.ChangeSettings(ref CurrentSettings);
            rectDarkWindow.Visibility = Visibility.Hidden;
        }
    }
}
