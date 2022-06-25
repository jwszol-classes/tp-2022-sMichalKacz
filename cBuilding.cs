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
using System.Windows.Threading;

namespace LiftSimulator
{
    enum ePassengerState
    {
        Waiting = 0,
        InTheLift = 1,
        Leaving = 2
    }
    class cPassenger
    {
        public int iPresentFloor;
        public int iTargetFloor;
        public int iPositionInLift = 0;
        public bool bDirection; //0-down, 1-up
        public bool bAnimationInProgress = false;
        public ePassengerState state = ePassengerState.Waiting; 
        public Image imgPassenger = new Image();
        public TextBlock tbTargetFloor = new TextBlock();
        Canvas canOwnerCanvas;
        public cPassenger(int iTopPosition, int iLeftPosition, int iTargetFloor, Canvas can)
        {
            tbTargetFloor.Visibility = Visibility.Hidden;
            imgPassenger.Source = new BitmapImage(new Uri("pack://application:,,,/Graphics/Passenger.png", UriKind.Absolute));
            imgPassenger.Height = 30;
            imgPassenger.MouseEnter += imgPassengerMouseOver;
            imgPassenger.MouseLeave += imgPassengerMouseLeave;
            imgPassenger.Tag =iTargetFloor;
            Canvas.SetTop(imgPassenger, iTopPosition);
            Canvas.SetLeft(imgPassenger, iLeftPosition);
            canOwnerCanvas = can;
            canOwnerCanvas.Children.Add(imgPassenger);
            canOwnerCanvas.Children.Add(tbTargetFloor);
        }
        void vRemovePassenger()
        {
            canOwnerCanvas.Children.Remove(imgPassenger);
            canOwnerCanvas.Children.Remove(tbTargetFloor);
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
            if(state == ePassengerState.Leaving)
            {
                vRemovePassenger();
            }
            bAnimationInProgress = false;
            if(state == ePassengerState.Waiting)
            {
                return;
            }
            else if(state == ePassengerState.InTheLift)
            Canvas.SetLeft(imgPassenger, 500+ 20*iPositionInLift);
            Canvas.SetTop(imgPassenger, 100);
            imgPassenger.Height = 60;
            iPresentFloor = iTargetFloor;
            return;
        }
        public void vMovePassengerAnimation(int iTargetPosition)
        {

            if (iTargetPosition - Canvas.GetLeft(imgPassenger) == 0)
                return;
            bAnimationInProgress = true;
            Storyboard sbPassengerMovement = new Storyboard();
            double a = Canvas.GetLeft(imgPassenger);
            sbPassengerMovement.Duration = new Duration(TimeSpan.FromSeconds(Math.Abs(iTargetPosition - Canvas.GetLeft(imgPassenger))/ 25));

            DoubleAnimation daPassengerPosition = new DoubleAnimation();
            daPassengerPosition.Duration = sbPassengerMovement.Duration;
            sbPassengerMovement.Children.Add(daPassengerPosition);

            daPassengerPosition.From = Canvas.GetLeft(imgPassenger);
            daPassengerPosition.To = iTargetPosition;

            Storyboard.SetTarget(daPassengerPosition, imgPassenger);
            Storyboard.SetTargetProperty(daPassengerPosition, new PropertyPath("(Canvas.Left)"));
            if (state==ePassengerState.Waiting)
                sbPassengerMovement.FillBehavior = FillBehavior.HoldEnd;
            else
                sbPassengerMovement.FillBehavior = FillBehavior.Stop;
            sbPassengerMovement.Completed += vAnimationCompleted;
            sbPassengerMovement.Begin();
        }
    }

    class cBuilding
    {
        int iNumberOfLifts=1;
        public int iNumberOfFloors;
        public int stuckInAnimation = 0; 
        public List<cFloor> lFloors = new List<cFloor>();
        public List<cLift> lLifts = new List<cLift>();
        public Canvas canBuilding = new Canvas();
        public void vAddFloorsToBulding(int inewNumberOfFloors)
        {
            Grid.SetRow(canBuilding, 1);
            Grid.SetColumn(canBuilding, 2);
            for(int i = 0; i < inewNumberOfFloors; i++)
            {
                lFloors.Add(new cFloor() { iNumberOfFloor=i});
            }
            iNumberOfFloors=inewNumberOfFloors;
        }
        void vLiftTimerTick(object sender, EventArgs e)
        {
            if (lLifts[0].bAnimationInProgress || lLifts[0].lPassengersLeavingTheLift.Count > 0)
                return;
            foreach (cPassenger p in lLifts[0].lPassengersInTheLift)
            {
                if (p.bAnimationInProgress)
                    return;
            }
            if (lLifts[0].bIsOpened)
                vOpenCloseLiftDoorAnimation();
            else
                vMoveLiftUpDownAnimationCompleted(null, null);
        }
        public void vAddLiftsToBulding(cSettings s)
        {
            DispatcherTimer dtLiftTimer = new DispatcherTimer();
            dtLiftTimer.Interval = new TimeSpan(0, 0, 1);
            dtLiftTimer.Tick += vLiftTimerTick;
            dtLiftTimer.Start();
            lLifts.Add(new cLift(s));
            foreach (cLift l in lLifts)
            {
                canBuilding.Children.Add(l.rectLiftDoorLeft);
                canBuilding.Children.Add(l.rectLiftDoorRight);
                canBuilding.Children.Add(l.rectLiftInside);
                canBuilding.Children.Add(l.tbMassOfPeopleInside);
            }
        }
        public void vAddPassengers(int iPresentFloor, int iTargettFloor, int iNumberOfFloors)
        {
            lFloors[iPresentFloor].vAddPassengerToTheFloor(iPresentFloor, iTargettFloor, iNumberOfFloors, canBuilding);
        }
        public void vMoveLiftUpDownAnimationCompleted(object sender, EventArgs e)
        {
            lLifts[0].bAnimationInProgress = false;
            int iNumberOfFloor = lLifts[0].iCurrentLevelOfTheLift;
            //lLifts[0].vRemovePassengersFromTheLift(lFloors[iNumberOfFloor].lPassengersOnTheFloor, iNumberOfFloor);
            bool bIsAnyoneGoOutFromTheLift;
            bIsAnyoneGoOutFromTheLift=lLifts[0].vCheckingThatIsAnyoneGoOutFromTheLift(lLifts[0].lPassengersInTheLift, iNumberOfFloor);//?
            

            if((lFloors[iNumberOfFloor].lPassengersOnTheFloor.Count>0&& lLifts[0].lPassengersInTheLift.Count < lLifts[0].iMaxNumberOfPeopleInside)||(bIsAnyoneGoOutFromTheLift==true))
            {
                vOpenCloseLiftDoorAnimation();
            }
            else
            {
                vCalculatingTheNextFloor(iNumberOfFloor);
                vMoveLiftUpDownAnimation();
            }
            
            return;
        }
        void vOpenCloseLiftDoorAnimationCompleted(object sender, EventArgs e)
        {
            lLifts[0].bAnimationInProgress = false;
            int iNumberOfFloor = lLifts[0].iCurrentLevelOfTheLift;

            if (lLifts[0].bIsOpened == true)
            {
            lLifts[0].vRemovePassengersFromTheLift(iNumberOfFloor);
                vCalculatingTheNextFloor(iNumberOfFloor);
            lLifts[0].vAddPassengersToTheLift(lFloors[iNumberOfFloor].lPassengersOnTheFloor);
                return;
            }
            else
            {
                //vCalculatingTheNextFloor(iNumberOfFloor);
                
                vMoveLiftUpDownAnimation();
            }
            
                                   
            return;
        }

        public void vMoveLiftUpDownAnimation()
        {
            if (lLifts[0].iCurrentDirection == 0)
                return;
            stuckInAnimation = 1;
            lLifts[0].iCurrentLevelOfTheLift = lLifts[0].iCurrentLevelOfTheLift + lLifts[0].iCurrentDirection;
            lLifts[0].bAnimationInProgress = true;
            Storyboard sbLiftMovement = new Storyboard();
            sbLiftMovement.Duration = new Duration(TimeSpan.FromSeconds(1+2*Math.Abs(lLifts[0].iCurrentDirection)));

            DoubleAnimation daLeftDoorPosition = new DoubleAnimation();
            DoubleAnimation daRightDoorPosition = new DoubleAnimation();
            daLeftDoorPosition.Duration = sbLiftMovement.Duration;
            daRightDoorPosition.Duration = sbLiftMovement.Duration;
            sbLiftMovement.Children.Add(daLeftDoorPosition);
            sbLiftMovement.Children.Add(daRightDoorPosition);
            daLeftDoorPosition.From = Canvas.GetTop(lLifts[0].rectLiftDoorLeft);
            daLeftDoorPosition.To = Canvas.GetTop(lLifts[0].rectLiftDoorLeft) - lLifts[0].iCurrentDirection * 40;
            daRightDoorPosition.From = Canvas.GetTop(lLifts[0].rectLiftDoorRight);
            daRightDoorPosition.To = Canvas.GetTop(lLifts[0].rectLiftDoorRight) - lLifts[0].iCurrentDirection * 40;
            Storyboard.SetTarget(daLeftDoorPosition, lLifts[0].rectLiftDoorLeft);
            Storyboard.SetTargetProperty(daLeftDoorPosition, new PropertyPath("(Canvas.Top)"));
            Storyboard.SetTarget(daRightDoorPosition, lLifts[0].rectLiftDoorRight);
            Storyboard.SetTargetProperty(daRightDoorPosition, new PropertyPath("(Canvas.Top)"));
            sbLiftMovement.Completed += vMoveLiftUpDownAnimationCompleted;
            sbLiftMovement.Begin();
        }

        public void vOpenCloseLiftDoorAnimation()
        {
            stuckInAnimation = 3;
            lLifts[0].bAnimationInProgress = true;
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
            sbDoorOpening.Completed += vOpenCloseLiftDoorAnimationCompleted;
            sbDoorOpening.Begin();
        }

        public  void vCalculatingTheNextFloor( int iNumberOfFloor, int iNumberOfLift=0)
        {
            /*if (iNumberOfFloor == 0)
            {
                lLifts[iNumberOfLift].bCurrentDirection=true;
            }
            else if (iNumberOfFloor == iNumberOfFloors - 1)
            {
                lLifts[iNumberOfLift].bCurrentDirection =false;
            }
            /*else*/ if ((lLifts[iNumberOfLift].lPassengersInTheLift.Count == 0)&&(lFloors[iNumberOfFloor].lPassengersOnTheFloor.Count>0))
            {
                if (lFloors[iNumberOfFloor].lPassengersOnTheFloor[0].bDirection)
                    lLifts[iNumberOfLift].iCurrentDirection = 1;
                else
                    lLifts[iNumberOfLift].iCurrentDirection = -1;
            }
            else if(lLifts[iNumberOfLift].lPassengersInTheLift.Count > 0)
            {
                return;
            }
            else
            {
                
                int iNextDirectionUp = 0;
                int iIndex = 0;
                foreach (cFloor f in lFloors)
                {
                    if(f.lPassengersOnTheFloor.Count > 0 && iIndex<lLifts[iNumberOfLift].iCurrentLevelOfTheLift)
                    {
                        iNextDirectionUp = -1;
                        if (lLifts[iNumberOfLift].iCurrentDirection==-1)
                        {
                            break;
                        }
                    }
                    else if(f.lPassengersOnTheFloor.Count > 0)
                    {
                        iNextDirectionUp = 1;
                        break;
                    }
                    
                    iIndex++;
                }
                lLifts[iNumberOfLift].iCurrentDirection = iNextDirectionUp;
            }
        }
    }
    class cFloor
    {
        public int iNumberOfFloor;
        public RadioButton rbtnFloorButton = new RadioButton();
        public Rectangle rectFloor = new Rectangle();
        public List<cPassenger> lPassengersOnTheFloor = new List<cPassenger>();

        public void vAddPassengerToTheFloor(int iPresentFloor, int iTargettFloor, int iNumberOfFloors, Canvas can)
        {
           bool bDirectonCalc;
           if(iPresentFloor < iTargettFloor)bDirectonCalc = true;
           else bDirectonCalc = false;
           lPassengersOnTheFloor.Add(new cPassenger((iNumberOfFloors - iNumberOfFloor - 1) * 40 - 10, 230 - 10 * lPassengersOnTheFloor.Count, iTargettFloor, can) { iPresentFloor=iPresentFloor, iTargetFloor=iTargettFloor , bDirection=bDirectonCalc});
        }
    }
    class cLift
    {
        int iPresentNumberOfPeopleInside=0;
        int iHumanWeight = 0;
        public int iMaxNumberOfPeopleInside=100;
        public int iCurrentLevelOfTheLift=0;
        public int iCurrentDirection = 0; //0-down, 1-up
        public bool bIsOpened = false;
        public bool bAnimationInProgress = false;
        public Rectangle rectLiftDoorRight = new Rectangle();
        public Rectangle rectLiftDoorLeft = new Rectangle();
        public Rectangle rectLiftInside = new Rectangle();
        public TextBlock tbMassOfPeopleInside = new TextBlock();

        public List<cPassenger> lPassengersInTheLift = new List<cPassenger>();
        public List<cPassenger> lPassengersLeavingTheLift = new List<cPassenger>();
        public cLift(cSettings s)
        {
            iHumanWeight = s.iHumanWeight;
            vCalculatingMaxNumberOfPeople(s.iLiftWeightLimit);
            tbMassOfPeopleInside.Text = "0 / " + (iMaxNumberOfPeopleInside * iHumanWeight).ToString() + " kg";
            rectLiftDoorRight.Width = 10;
            rectLiftDoorRight.Height = 30;
            rectLiftDoorRight.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF555555"));
            Canvas.SetTop(rectLiftDoorRight, 40 * s.iNumberOfFloors - 45);
            Canvas.SetLeft(rectLiftDoorRight, 270);

            rectLiftDoorLeft.Width = 10;
            rectLiftDoorLeft.Height = 30;
            rectLiftDoorLeft.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF555555"));
            Canvas.SetTop(rectLiftDoorLeft, 40 * s.iNumberOfFloors - 45);
            Canvas.SetLeft(rectLiftDoorLeft, 260);

            rectLiftInside.Width = 600;
            rectLiftInside.Height = 200;
            rectLiftInside.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF555555"));
            rectLiftInside.StrokeThickness = 5;
            rectLiftInside.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            Canvas.SetTop(rectLiftInside, 40);
            Canvas.SetLeft(rectLiftInside, 430);

            tbMassOfPeopleInside.Width = 200;
            tbMassOfPeopleInside.Height = 30;
            tbMassOfPeopleInside.FontSize = 20;
            Canvas.SetTop(tbMassOfPeopleInside, 10);
            Canvas.SetLeft(tbMassOfPeopleInside, 430);
        }
        void vCalculatingMaxNumberOfPeople(int iMaxWeight)
        {
            iMaxNumberOfPeopleInside=iMaxWeight/iHumanWeight;
        }

       /* void vFullElevatorMovement(List<cPassenger> lPassengersOnTheFloor, int iNumberOfFloor, int iNumberOfFloors)
        {
            vRemovePassengersFromTheLift(lPassengersOnTheFloor, iNumberOfFloor);
            vCalculatingTheNextFloor(lPassengersOnTheFloor, iNumberOfFloor, iNumberOfFloors);
            vAddPassengersToTheLift(lPassengersOnTheFloor);
        }*/
       
        public void vAddPassengersToTheLift(List<cPassenger> lPassengersOnTheFloor)
        {

            int iCheckingPerson = 0;
            while ((iMaxNumberOfPeopleInside - iPresentNumberOfPeopleInside > 0) && (lPassengersOnTheFloor.Count > iCheckingPerson))
            {

                if (lPassengersOnTheFloor[iCheckingPerson].bDirection == (iCurrentDirection > 0))
                /* if (((lPassengersOnTheFloor[iCheckingPerson].iTargetFloor-iCurrentLevelOfTheLift>0)&&(bCurrentDirection==true))||
                     ((lPassengersOnTheFloor[iCheckingPerson].iTargetFloor-iCurrentLevelOfTheLift<0)&&(bCurrentDirection==false)))*/

                {
                    lPassengersOnTheFloor[iCheckingPerson].iPositionInLift = lPassengersInTheLift.Count;
                    lPassengersOnTheFloor[iCheckingPerson].state = ePassengerState.InTheLift;
                    lPassengersInTheLift.Add(lPassengersOnTheFloor[iCheckingPerson]);
                    lPassengersOnTheFloor[iCheckingPerson].vMovePassengerAnimation(260);
                    lPassengersOnTheFloor.RemoveAt(iCheckingPerson);
                    iPresentNumberOfPeopleInside = iPresentNumberOfPeopleInside + 1;
                    
                }
                else
                {
                    iCheckingPerson = iCheckingPerson + 1;
                }
            }
            int iIndex = 0;
            foreach (cPassenger p in lPassengersOnTheFloor)
            {
                if(Canvas.GetLeft(p.imgPassenger)<230 && !p.bAnimationInProgress)
                    p.vMovePassengerAnimation(230 - 10 * iIndex);
                iIndex++;
            }
            tbMassOfPeopleInside.Text = (lPassengersInTheLift.Count * iHumanWeight).ToString() + " / " + (iMaxNumberOfPeopleInside*iHumanWeight).ToString() + " kg";
        }

        public void vRemovePassengersFromTheLift(int iNumberOfFloor)
        {
            int iCheckingPerson=0;
            while ((iPresentNumberOfPeopleInside > iCheckingPerson))
            {

                if (lPassengersInTheLift[iCheckingPerson].iTargetFloor == iNumberOfFloor)
                {

                    //lPassengersOnTheFloor.Add((sPassenger)lPassengersInTheLift[iCheckingPerson]);
                    lPassengersInTheLift[iCheckingPerson].state = ePassengerState.Leaving;
                    lPassengersLeavingTheLift.Add(lPassengersInTheLift[iCheckingPerson]);
                    lPassengersInTheLift.RemoveAt(iCheckingPerson);
                    iPresentNumberOfPeopleInside = iPresentNumberOfPeopleInside - 1;
                }
                else
                {
                    iCheckingPerson = iCheckingPerson + 1;
                }
            }
            int iIndex = 0;
            foreach(cPassenger p in lPassengersInTheLift)
            {
                p.iPositionInLift = iIndex;
                p.vMovePassengerAnimation(500 + 20 * iIndex);
                iIndex++;
            }
            DispatcherTimer dtLeavingPassengersTimer = new DispatcherTimer();
            dtLeavingPassengersTimer.Interval = new TimeSpan(0, 0, 0, 0,  300);
            dtLeavingPassengersTimer.Tick += vLeavingPassengersTick;
            dtLeavingPassengersTimer.Start();
            tbMassOfPeopleInside.Text = (lPassengersInTheLift.Count * iHumanWeight).ToString() + " / " + (iMaxNumberOfPeopleInside * iHumanWeight).ToString() + " kg";
        }
        void vLeavingPassengersTick(object sender, EventArgs e)
        {
            if (lPassengersLeavingTheLift.Count == 0)
            {
                (sender as DispatcherTimer).Stop();
                return;
            }
            Canvas.SetTop(lPassengersLeavingTheLift[0].imgPassenger, Canvas.GetTop(rectLiftDoorRight) - 5);
            Canvas.SetLeft(lPassengersLeavingTheLift[0].imgPassenger, 260);
            lPassengersLeavingTheLift[0].imgPassenger.Height = 30;
            lPassengersLeavingTheLift[0].vMovePassengerAnimation(400);
            lPassengersLeavingTheLift.RemoveAt(0);
        }
        public bool vCheckingThatIsAnyoneGoOutFromTheLift(List<cPassenger> lPassengersInTheLift, int iNumberOfFloor)
        {
            int iCheckingPerson=0;
                while ((iPresentNumberOfPeopleInside>iCheckingPerson))
                {
                   
                if (lPassengersInTheLift[iCheckingPerson].iTargetFloor==iNumberOfFloor)
                    {
                        return true;
                    }
                else
                {
                    iCheckingPerson=iCheckingPerson+1;
                }
                }
                return false;
        }
    }
}
