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
        public bool bChangeSettings(ref cSettings S)
        {
            tbSetNumberOfFloors.Text = S.iNumberOfFloors.ToString();
            tbSetNumberOfLifts.Text = S.iNumberOfLifts.ToString();
            tbSetHumanWeight.Text = S.iHumanWeight.ToString();
            tbSetLiftWeightLimit.Text = S.iLiftWeightLimit.ToString();
            bool bClose = (bool)this.ShowDialog();
            if (bClose)
            {
                int iNewNumberOfFloors=0;
                int iNewNumberOfLifts=0;
                int iNewHumanWeight=0;
                int iNewLiftWeightLimit=0;

                try {
                iNewNumberOfFloors = Convert.ToInt32(tbSetNumberOfFloors.Text);       
                iNewNumberOfLifts = Convert.ToInt32(tbSetNumberOfLifts.Text);
                iNewHumanWeight = Convert.ToInt32(tbSetHumanWeight.Text);
                iNewLiftWeightLimit = Convert.ToInt32(tbSetLiftWeightLimit.Text);
                    }
                catch(FormatException) {

                    }

                if (iNewNumberOfFloors>1)
                {
                S.iNumberOfFloors = iNewNumberOfFloors;      
                }
                if (iNewNumberOfLifts>0)
                {
                S.iNumberOfLifts = iNewNumberOfLifts;      
                }
                if (iNewHumanWeight>0)
                {
                S.iHumanWeight = iNewHumanWeight;      
                }
                if (iNewLiftWeightLimit>S.iHumanWeight)
                {
                S.iLiftWeightLimit = iNewLiftWeightLimit;      
                }
                return true;
                
            }
            else
            {
                return false;
            }
        }

        private void btnSettingsOkCancelButton_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button)
                this.DialogResult = Convert.ToBoolean((sender as Button).Tag);
        }
        private void btnChangeNumberOfFloors_Click(object sender, RoutedEventArgs e)
        {
            tbSetNumberOfFloors.Text = (Convert.ToInt32(tbSetNumberOfFloors.Text) + Convert.ToInt32((sender as Button).Tag)).ToString();
        }
        private void btnChangeNumberOfLifts_Click(object sender, RoutedEventArgs e)
        {
            tbSetNumberOfLifts.Text = (Convert.ToInt32(tbSetNumberOfLifts.Text) + Convert.ToInt32((sender as Button).Tag)).ToString();
        }
        private void btnChangeHumanWeight_Click(object sender, RoutedEventArgs e)
        {
            tbSetHumanWeight.Text = (Convert.ToInt32(tbSetHumanWeight.Text) + 5 * Convert.ToInt32((sender as Button).Tag)).ToString();
        }

        private void btnChangeLiftWeightLimit_Click(object sender, RoutedEventArgs e)
        {
            tbSetLiftWeightLimit.Text = (Convert.ToInt32(tbSetLiftWeightLimit.Text) + 50 * Convert.ToInt32((sender as Button).Tag)).ToString();
        }
    }
}
