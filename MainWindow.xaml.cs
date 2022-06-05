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
        List<RadioButton> lrbtnFloorButton= new List<RadioButton>();
        List<Button> lbtnLiftButton = new List<Button>();
        List<Rectangle> lrectFloor = new List<Rectangle>();
        List<Rectangle> lrectLift = new List<Rectangle>();
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
            /*Building.lFloors.Clear();
             Building.vAddFloorsToBulding(CurrentSettings.iNumberOfFloors);
            */
            lrbtnFloorButton.Clear();
            spFloors.Children.Clear();
            spLiftButtons1.Children.Clear();
            spLiftButtons2.Children.Clear();
            lrectFloor.Clear();
            lrectLift.Clear();
            canBuilding.Children.Clear();
            for (int i = 0; i < CurrentSettings.iNumberOfFloors; i++)
            {
                lrbtnFloorButton.Add(new RadioButton());
                lbtnLiftButton.Add(new Button());
                spFloors.Children.Insert(0, lrbtnFloorButton[i]);
                if(i % 2 == 0)
                {
                    spLiftButtons1.Children.Insert(0, lbtnLiftButton[i]);
                }
                else
                {
                    spLiftButtons2.Children.Insert(0, lbtnLiftButton[i]);
                }
                lrectFloor.Insert(0, new Rectangle());
            }
            lrbtnFloorButton[0].IsChecked = true;
            int iIndex = 0;
            foreach (RadioButton rb in lrbtnFloorButton)
            {
                rb.Content = iIndex;
                rb.Style = Application.Current.FindResource("FloorButtonStyle") as Style;
                rb.Click += btnFloorLiftClick;
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
            foreach (Rectangle r in lrectFloor)
            {
                canBuilding.Children.Add(r);
                r.Width = 400;
                r.Height = 5;
                r.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
                Canvas.SetTop(r,35 * iIndex + 20);
                Canvas.SetLeft(r, 5);
                iIndex++;
            }
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
                Canvas.SetTop(r, 35 * CurrentSettings.iNumberOfFloors - 40);
                Canvas.SetLeft(r, 200+ iIndex*25);
                iIndex++;
            }
        }

        private void btnFloorLiftClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
