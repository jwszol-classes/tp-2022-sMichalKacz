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
        cBuilding Building = new cBuilding();
        List<Button> lbtnLiftButton = new List<Button>();
        List<Rectangle> lrectLift = new List<Rectangle>();
        
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
            Building.lFloors.Clear();
            Building.vAddFloorsToBulding(CurrentSettings.iNumberOfFloors);
            spFloors.Children.Clear();
            spLiftButtons1.Children.Clear();
            spLiftButtons2.Children.Clear();
            lrectLift.Clear();
            canBuilding.Children.Clear();
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
                canBuilding.Children.Add(f.rectFloor);
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
            /*cBuilding.vAddLiftsToBulding()
            */
            for (int i = 0; i < CurrentSettings.iNumberOfLifts; i++)
            {
                lrectLift.Add(new Rectangle());
            }
            iIndex = 0;
            foreach (Rectangle r in lrectLift)
            {
                canBuilding.Children.Add(r);
                r.Width = 20;
                r.Height = 25;
                r.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF555555"));
                Canvas.SetTop(r, 40 * CurrentSettings.iNumberOfFloors - 40);
                Canvas.SetLeft(r, 200+ iIndex*25);
                iIndex++;
            }
        }
        private void rbtnChooseFloorClick(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton)
            {
                iChosenFloor = Convert.ToInt32((sender as RadioButton).Content);
            }
        }
        private void btnFloorLiftClick(object sender, RoutedEventArgs e)
        {
            if(sender is Button)
            {
                Building.lFloors[iChosenFloor].vAddPassengerToTheFloor(iChosenFloor, Convert.ToInt32((sender as Button).Content), CurrentSettings.iNumberOfFloors);
                canBuilding.Children.Add(Building.lFloors[iChosenFloor].lPassengersOnTheFloor[Building.lFloors[iChosenFloor].lPassengersOnTheFloor.Count-1].imgPassenger);
                canBuilding.Children.Add(Building.lFloors[iChosenFloor].lPassengersOnTheFloor[Building.lFloors[iChosenFloor].lPassengersOnTheFloor.Count - 1].tbTargetFloor);
            }
        }
        
    }
}
