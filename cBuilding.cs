using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LiftSimulator
{
    class cPassenger
    {
        public int iPresentFloor;
        public int iTargetFloor;
        public bool bDirection; //0-down, 1-up
        public Image imgPassenger = new Image();
        public TextBlock tbTargetFloor = new TextBlock();
        public cPassenger(int iTopPosition, int iLeftPosition, int iTargetFloor)
        {
            tbTargetFloor.Visibility = Visibility.Hidden;
            imgPassenger.Source = new BitmapImage(new Uri("pack://application:,,,/Graphics/BlankPassenger.png", UriKind.Absolute));
            imgPassenger.Height = 30;
            imgPassenger.MouseEnter += imgPassengerMouseOver;
            imgPassenger.MouseLeave += imgPassengerMouseLeave;
            imgPassenger.Tag =iTargetFloor;
            Canvas.SetTop(imgPassenger, iTopPosition);
            Canvas.SetLeft(imgPassenger, iLeftPosition);
        }
        private void imgPassengerMouseOver(object sender, RoutedEventArgs e)
        {
            tbTargetFloor.Visibility = Visibility.Visible;
            tbTargetFloor.Text = (sender as Image).Tag.ToString();
            Canvas.SetTop(tbTargetFloor, 0);
            Canvas.SetLeft(tbTargetFloor, 0);
        }
        private void imgPassengerMouseLeave(object sender, RoutedEventArgs e)
        {
            tbTargetFloor.Visibility = Visibility.Hidden;
        }
    }

    class cBuilding
    {
        int iNumberOfLifts=1;
        public List<cFloor> lFloors = new List<cFloor>();
        
        public void vAddFloorsToBulding(int iNumberOfFloors)
        {
            
            for(int i = 0; i < iNumberOfFloors; i++)
            {
                lFloors.Add(new cFloor() { iNumberOfFloor=i});
            }
        }
        
    }
    class cFloor
    {
        public int iNumberOfFloor;
        public RadioButton rbtnFloorButton = new RadioButton();
        public Rectangle rectFloor = new Rectangle();
        public List<cPassenger> lPassengersOnTheFloor = new List<cPassenger>();

        public void vAddPassengerToTheFloor(int iPresentFloor, int iTargettFloor, int iNumberOfFloors)
        {
           bool bDirectonCalc;
           if(iPresentFloor < iTargettFloor)bDirectonCalc = true;
           else bDirectonCalc = false;
           lPassengersOnTheFloor.Add(new cPassenger((4 - iNumberOfFloor - 1) * 40 - 10, 10 * lPassengersOnTheFloor.Count, iTargettFloor) { iPresentFloor=iPresentFloor, iTargetFloor=iTargettFloor , bDirection=bDirectonCalc});
        }
    }
    class cLift
    {
        int iPresentNumberOfPeopleInside=0;
        int iMaxNumberOfPeopleInside;
        int iCurrentLevelOfTheLift=0;
        bool bCurrentDirection=true; //0-down, 1-up

        List<cPassenger> lPassengersInTheLift = new List<cPassenger>();
        //int[] iPassengers = new int[iMaxNumberOfPeopleInside];

        void vCalculatingMaxNumberOfPeople(int iMaxWeight, int iWeightOfPerson)
        {
            iMaxNumberOfPeopleInside=iMaxWeight/iWeightOfPerson;
        }

        void vFullElevatorMovement(List<cPassenger> lPassengersOnTheFloor, int iNumberOfFloor, int iNumberOfFloors)
        {
            vRemovePassengersFromTheLift(lPassengersOnTheFloor, iNumberOfFloor);
            vCalculatingTheNextFloor(lPassengersOnTheFloor, iNumberOfFloor, iNumberOfFloors);
            vAddPassengersToTheLift(lPassengersOnTheFloor);
        }

        void vCalculatingTheNextFloor(List<cPassenger> lPassengersOnTheFloor, int iNumberOfFloor, int iNumberOfFloors)
        {
            if (iNumberOfFloor == 0)
            {
                bCurrentDirection=true;
            }
            else if (iNumberOfFloor == iNumberOfFloors - 1)
            {
                bCurrentDirection =false;
            }
            else
            {
                if (lPassengersInTheLift.Count == 0)
                {
                    bCurrentDirection=lPassengersOnTheFloor[0].bDirection;
                }
                else
                {
                    bCurrentDirection=lPassengersInTheLift[0].bDirection;
                }
            }

            if(bCurrentDirection==true)iCurrentLevelOfTheLift=iCurrentLevelOfTheLift+1;
            else iCurrentLevelOfTheLift = iCurrentLevelOfTheLift-1;
        }
       
        void vAddPassengersToTheLift(List<cPassenger> lPassengersOnTheFloor)
        {
           
                int iCheckingPerson=0;
                while ((iMaxNumberOfPeopleInside - iPresentNumberOfPeopleInside>0) && (lPassengersOnTheFloor.Count>iCheckingPerson))
                {
                   
                if (lPassengersOnTheFloor[iCheckingPerson].bDirection==bCurrentDirection)
                    {
                        lPassengersInTheLift.Add(lPassengersOnTheFloor[iCheckingPerson]);
                        lPassengersOnTheFloor.RemoveAt(iCheckingPerson);
                    }
                else
                {
                    iCheckingPerson=iCheckingPerson+1;
                }
                }
                
        }

        void vRemovePassengersFromTheLift(List<cPassenger> lPassengersOnTheFloor, int iNumberOfFloor)
        {
            int iCheckingPerson=0;
                while ((iPresentNumberOfPeopleInside>iCheckingPerson))
                {
                   
                if (lPassengersOnTheFloor[iCheckingPerson].iTargetFloor==iNumberOfFloor)
                    {
                        
                        //lPassengersOnTheFloor.Add((sPassenger)lPassengersInTheLift[iCheckingPerson]);
                        lPassengersInTheLift.RemoveAt(iCheckingPerson);
                    }
                else
                {
                    iCheckingPerson=iCheckingPerson+1;
                }
                }
        }

         /*
        void vAddPassengerToTheLift(int iTargettFloor)
        {
            if (iPresentNumberOfPeopleInside < iMaxNumberOfPeopleInside)
            {
            lPassengersInTheLift.Add(new sPassenger() { jTargetFloor=iTargettFloor });
            iPresentNumberOfPeopleInside=iPresentNumberOfPeopleInside+1;
            }
            
        }

         void vRemovePassengerFromTheLift(int iTargettFloor)
        {
            if (iPresentNumberOfPeopleInside > 0)
            {
            lPassengersInTheLift.RemoveAt(iTargettFloor);
            iPresentNumberOfPeopleInside=iPresentNumberOfPeopleInside+1;
            }
            
        }
        */
    }
}
