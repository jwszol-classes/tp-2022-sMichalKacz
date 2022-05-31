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
using System.Windows.Shapes;

namespace LiftSimulator
{
    public class cSettings
    {
        public int iNumberOfFloors = 4;
        public int iNumberOfLifts = 1;
        public int iHumanWeight = 70;
        public int iLiftWeightLimit = 700;
    }
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            Owner = App.Current.MainWindow;
            InitializeComponent();
        }
        public void ChangeSettings(ref cSettings S)
        {
            tbSetNumberOfFloors.Text = S.iNumberOfFloors.ToString();
            tbSetNumberOfLifts.Text = S.iNumberOfLifts.ToString();
            tbSetHumanWeight.Text = S.iHumanWeight.ToString();
            tbSetLiftWeightLimit.Text = S.iLiftWeightLimit.ToString();
            bool bClose = (bool)this.ShowDialog();
            if (bClose)
            {
                S.iNumberOfFloors = Convert.ToInt32(tbSetNumberOfFloors.Text);
                S.iNumberOfLifts = Convert.ToInt32(tbSetNumberOfLifts.Text);
                S.iHumanWeight = Convert.ToInt32(tbSetHumanWeight.Text);
                S.iLiftWeightLimit = Convert.ToInt32(tbSetLiftWeightLimit.Text);
                return;
            }
            else
            {
                return;
            }
        }

        private void btnSettingsOkCancelButton_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button)
                this.DialogResult = Convert.ToBoolean((sender as Button).Tag);
        }
        private void btnChangeNumberOfFloors_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
