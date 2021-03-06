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
        cBuilding Building;
        List<Button> lbtnLiftButton = new List<Button>();
        
        
        int iChosenFloor = 0;
        public MainWindow()
        {
            InitializeComponent();
            vCreateNewBuilding();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow tempSettingsWindow = new SettingsWindow();
            rectDarkWindow.Visibility = Visibility.Visible;
            if(tempSettingsWindow.bChangeSettings(ref CurrentSettings))
            {
                vCreateNewBuilding();
            }
            rectDarkWindow.Visibility = Visibility.Hidden;
        }
        private void vCreateNewBuilding()
        {
            iChosenFloor = 0;
            if(Building!=null)
            {
                gMainGrid.Children.Remove(Building.canBuilding);
            }
            Building = new cBuilding();
            Building.vAddFloorsToBulding(CurrentSettings.iNumberOfFloors);
            spFloors.Children.Clear();
            spLiftButtons1.Children.Clear();
            spLiftButtons2.Children.Clear();
            lbtnLiftButton.Clear();
            gMainGrid.Children.Add(Building.canBuilding);
            for (int i = 0; i < CurrentSettings.iNumberOfFloors; i++)
            {
                lbtnLiftButton.Add(new Button());
                
                if(i % 2 == 0)
                {
                    spLiftButtons1.Children.Insert(0, lbtnLiftButton[i]);
                }
                else
                {
                    spLiftButtons2.Children.Insert(0, lbtnLiftButton[i]);
                }
            }
            Building.lFloors[0].rbtnFloorButton.IsChecked = true;
            int iIndex = 0;
            foreach (cFloor f in Building.lFloors)
            {
                Building.canBuilding.Children.Add(f.rectFloor);
                f.rectFloor.Width = 400;
                f.rectFloor.Height = 5;
                f.rectFloor.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
                Canvas.SetTop(f.rectFloor, 40 * iIndex + 20);
                Canvas.SetLeft(f.rectFloor, 5);
                f.rbtnFloorButton.Content = iIndex;
                f.rbtnFloorButton.Style = Application.Current.FindResource("FloorButtonStyle") as Style;
                f.rbtnFloorButton.Click += rbtnChooseFloorClick;
                spFloors.Children.Insert(0, f.rbtnFloorButton);
                iIndex++;
            }
            iIndex = 0;
            foreach (Button b in lbtnLiftButton)
            {
                b.Content = iIndex;
                b.Style = Application.Current.FindResource("LiftButtonStyle") as Style;
                b.Click += btnFloorLiftClick;
                iIndex++;
            }
            iIndex = 0;
            Building.vAddLiftsToBulding(CurrentSettings);
            rbtnChooseFloorClick(null, null);
        }
        private void rbtnChooseFloorClick(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton)
            {
                iChosenFloor = Convert.ToInt32((sender as RadioButton).Content);
            }
            foreach (Button b in lbtnLiftButton)
            {
                b.Style = Application.Current.FindResource("LiftButtonStyle") as Style;
            }
            lbtnLiftButton[iChosenFloor].Style= Application.Current.FindResource("DisabledLiftButtonStyle") as Style;
        }
        private void btnFloorLiftClick(object sender, RoutedEventArgs e)
        {
            if(sender is Button)
            {
                if (Convert.ToInt32((sender as Button).Content) == iChosenFloor)
                    return;
                Building.vAddPassengers(iChosenFloor, Convert.ToInt32((sender as Button).Content), CurrentSettings.iNumberOfFloors);
                
            }
        }
    }
}
