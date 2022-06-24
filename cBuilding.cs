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
using System.Windows.Media.Animation;

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
        void vAnimationCompleted(object sender, EventArgs e)
        {
            return;
        }
        public void vMovePassengerAnimation(int iDistance)
        {
            Storyboard sbPassengerMovement = new Storyboard();
            sbPassengerMovement.Duration = new Duration(TimeSpan.FromSeconds(iDistance/5));

            DoubleAnimation daLeftDoorPosition = new DoubleAnimation();
            daLeftDoorPosition.Duration = sbPassengerMovement.Duration;
            sbPassengerMovement.Children.Add(daLeftDoorPosition);

            daLeftDoorPosition.From = Canvas.GetTop(imgPassenger);
            daLeftDoorPosition.To = Canvas.GetTop(imgPassenger) + iDistance;

            Storyboard.SetTarget(daLeftDoorPosition, imgPassenger);
            Storyboard.SetTargetProperty(daLeftDoorPosition, new PropertyPath("(Canvas.Left)"));
            sbPassengerMovement.Completed += vAnimationCompleted;
            sbPassengerMovement.Begin();
        }
    }

    class cBuilding
    {
        int iNumberOfLifts=1;
        public List<cFloor> lFloors = new List<cFloor>();
        public List<cLift> lLifts = new List<cLift>();
        public void vAddFloorsToBulding(int iNumberOfFloors)
        {
            
            for(int i = 0; i < iNumberOfFloors; i++)
            {
                lFloors.Add(new cFloor() { iNumberOfFloor=i});
            }
        }
        public void vAddLiftsToBulding()
        {
            lLifts.Add(new cLift(lFloors.Count));
        }
        void vAnimationCompleted(object sender, EventArgs e)
        {
            return;
        }
        public void vMoveLiftUpDownAnimation()
        {
            Storyboard sbLiftMovement = new Storyboard();
            sbLiftMovement.Duration = new Duration(TimeSpan.FromSeconds(3));

            DoubleAnimation daLeftDoorPosition = new DoubleAnimation();
            DoubleAnimation daRightDoorPosition = new DoubleAnimation();
            daLeftDoorPosition.Duration = sbLiftMovement.Duration;
            daRightDoorPosition.Duration = sbLiftMovement.Duration;
            sbLiftMovement.Children.Add(daLeftDoorPosition);
            sbLiftMovement.Children.Add(daRightDoorPosition);
            if (lLifts[0].bCurrentDirection)
            {
                daLeftDoorPosition.From = Canvas.GetTop(lLifts[0].rectLiftDoorLeft);
                daLeftDoorPosition.To = Canvas.GetTop(lLifts[0].rectLiftDoorLeft) - 40;
                daRightDoorPosition.From = Canvas.GetTop(lLifts[0].rectLiftDoorRight);
                daRightDoorPosition.To = Canvas.GetTop(lLifts[0].rectLiftDoorRight) - 40;
            }
            else
            {
                daLeftDoorPosition.From = Canvas.GetTop(lLifts[0].rectLiftDoorLeft);
                daLeftDoorPosition.To = Canvas.GetTop(lLifts[0].rectLiftDoorLeft) + 40;
                daRightDoorPosition.From = Canvas.GetTop(lLifts[0].rectLiftDoorRight);
                daRightDoorPosition.To = Canvas.GetTop(lLifts[0].rectLiftDoorRight) + 40;
            }
            Storyboard.SetTarget(daLeftDoorPosition, lLifts[0].rectLiftDoorLeft);
            Storyboard.SetTargetProperty(daLeftDoorPosition, new PropertyPath("(Canvas.Top)"));
            Storyboard.SetTarget(daRightDoorPosition, lLifts[0].rectLiftDoorRight);
            Storyboard.SetTargetProperty(daRightDoorPosition, new PropertyPath("(Canvas.Top)"));
            sbLiftMovement.Completed += vAnimationCompleted;
            sbLiftMovement.Begin();
        }
        public void vOpenCloseLiftDoorAnimation()
        {
            Storyboard sbDoorOpening = new Storyboard();
            sbDoorOpening.Duration = new Duration(TimeSpan.FromSeconds(3));

            DoubleAnimation daLeftDoorWidth = new DoubleAnimation();
            DoubleAnimation daRightDoorWidth = new DoubleAnimation();
            DoubleAnimation daRightDoorPosition = new DoubleAnimation();
            daLeftDoorWidth.Duration = sbDoorOpening.Duration;
            daRightDoorWidth.Duration = sbDoorOpening.Duration;
            daRightDoorPosition.Duration = sbDoorOpening.Duration;
            sbDoorOpening.Children.Add(daLeftDoorWidth);
            sbDoorOpening.Children.Add(daRightDoorWidth);
            sbDoorOpening.Children.Add(daRightDoorPosition);
            if (lLifts[0].bIsOpened)
            {
                daLeftDoorWidth.From = 1;
                daLeftDoorWidth.To = 10;
                daRightDoorWidth.From = 1;
                daRightDoorWidth.To = 10;
                daRightDoorPosition.From = Canvas.GetLeft(lLifts[0].rectLiftDoorRight);
                daRightDoorPosition.To = Canvas.GetLeft(lLifts[0].rectLiftDoorRight) - 9;
            }
            else
            {
                daLeftDoorWidth.From = 10;
                daLeftDoorWidth.To = 1;
                daRightDoorWidth.From = 10;
                daRightDoorWidth.To = 1;
                daRightDoorPosition.From = Canvas.GetLeft(lLifts[0].rectLiftDoorRight);
                daRightDoorPosition.To = Canvas.GetLeft(lLifts[0].rectLiftDoorRight) + 9;
            }
            lLifts[0].bIsOpened = !lLifts[0].bIsOpened;
            Storyboard.SetTarget(daLeftDoorWidth, lLifts[0].rectLiftDoorLeft);
            Storyboard.SetTargetProperty(daLeftDoorWidth, new PropertyPath("Width"));
            Storyboard.SetTarget(daRightDoorWidth, lLifts[0].rectLiftDoorRight);
            Storyboard.SetTargetProperty(daRightDoorWidth, new PropertyPath("Width"));
            Storyboard.SetTarget(daRightDoorPosition, lLifts[0].rectLiftDoorRight);
            Storyboard.SetTargetProperty(daRightDoorPosition, new PropertyPath("(Canvas.Left)"));
            sbDoorOpening.Completed += vAnimationCompleted;
            sbDoorOpening.Begin();
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
           lPassengersOnTheFloor.Add(new cPassenger((iNumberOfFloors - iNumberOfFloor - 1) * 40 - 10, 150 - 10 * lPassengersOnTheFloor.Count, iTargettFloor) { iPresentFloor=iPresentFloor, iTargetFloor=iTargettFloor , bDirection=bDirectonCalc});
        }
    }
    class cLift
    {
        int iPresentNumberOfPeopleInside=0;
        int iMaxNumberOfPeopleInside;
        int iCurrentLevelOfTheLift=0;
        public bool bCurrentDirection = true; //0-down, 1-up
        public bool bIsOpened = false;
        public Rectangle rectLiftDoorRight = new Rectangle();
        public Rectangle rectLiftDoorLeft = new Rectangle();

        List<cPassenger> lPassengersInTheLift = new List<cPassenger>();
        //int[] iPassengers = new int[iMaxNumberOfPeopleInside];
        public cLift(int iNumberOfFloors)
        {
            rectLiftDoorRight.Width = 10;
            rectLiftDoorRight.Height = 30;
            rectLiftDoorRight.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF555555"));
            Canvas.SetTop(rectLiftDoorRight, 40 * iNumberOfFloors - 45);
            Canvas.SetLeft(rectLiftDoorRight, 270);

            rectLiftDoorLeft.Width = 10;
            rectLiftDoorLeft.Height = 30;
            rectLiftDoorLeft.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF555555"));
            Canvas.SetTop(rectLiftDoorLeft, 40 * iNumberOfFloors - 45);
            Canvas.SetLeft(rectLiftDoorLeft, 260);
        }
        
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
