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
        public int iNumberOfFloors;
        public List<cFloor> lFloors = new List<cFloor>();
        public List<cLift> lLifts = new List<cLift>();

        public void vAddFloorsToBulding(int inewNumberOfFloors)
        {
            
            for(int i = 0; i < inewNumberOfFloors; i++)
            {
                lFloors.Add(new cFloor() { iNumberOfFloor=i});
            }
            iNumberOfFloors=inewNumberOfFloors;
        }

        public void vAddLiftsToBulding()
        {
            lLifts.Add(new cLift(lFloors.Count));
        }

        public void vMoveLiftUpDownAnimationCompleted(object sender, EventArgs e)
        {
            if (lLifts[0].bAnimationInProgress && sender == null)
                return;
            lLifts[0].bAnimationInProgress = false;
            int iNumberOfFloor = lLifts[0].iCurrentLevelOfTheLift;
            //lLifts[0].vRemovePassengersFromTheLift(lFloors[iNumberOfFloor].lPassengersOnTheFloor, iNumberOfFloor);
            bool bIsAnyoneGoOutFromTheLift;
            bIsAnyoneGoOutFromTheLift=lLifts[0].vCheckingThatIsAnyoneGoOutFromTheLift(lLifts[0].lPassengersInTheLift, iNumberOfFloor);//?
            

            if((lFloors[iNumberOfFloor].lPassengersOnTheFloor.Count>0)||(bIsAnyoneGoOutFromTheLift==true))
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
            lLifts[0].vRemovePassengersFromTheLift(lLifts[0].lPassengersInTheLift, iNumberOfFloor);
                vCalculatingTheNextFloor(iNumberOfFloor);
            lLifts[0].vAddPassengersToTheLift(lFloors[iNumberOfFloor].lPassengersOnTheFloor);
            vOpenCloseLiftDoorAnimation();
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
        int iMaxNumberOfPeopleInside=100;
        public int iCurrentLevelOfTheLift=0;
        public int iCurrentDirection = 0; //0-down, 1-up
        public bool bIsOpened = false;
        public bool bAnimationInProgress = false;
        public Rectangle rectLiftDoorRight = new Rectangle();
        public Rectangle rectLiftDoorLeft = new Rectangle();

        public List<cPassenger> lPassengersInTheLift = new List<cPassenger>();
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
        cLift(cSettings s)
        {
            vCalculatingMaxNumberOfPeople(s.iLiftWeightLimit, s.iHumanWeight);
        }
        void vCalculatingMaxNumberOfPeople(int iMaxWeight, int iWeightOfPerson)
        {
            iMaxNumberOfPeopleInside=iMaxWeight/iWeightOfPerson;
        }

       /* void vFullElevatorMovement(List<cPassenger> lPassengersOnTheFloor, int iNumberOfFloor, int iNumberOfFloors)
        {
            vRemovePassengersFromTheLift(lPassengersOnTheFloor, iNumberOfFloor);
            vCalculatingTheNextFloor(lPassengersOnTheFloor, iNumberOfFloor, iNumberOfFloors);
            vAddPassengersToTheLift(lPassengersOnTheFloor);
        }*/
       
        public void vAddPassengersToTheLift(List<cPassenger> lPassengersOnTheFloor)
        {
           
                int iCheckingPerson=0;
                while ((iMaxNumberOfPeopleInside - iPresentNumberOfPeopleInside>0) && (lPassengersOnTheFloor.Count>iCheckingPerson))
                {
                   
                if (lPassengersOnTheFloor[iCheckingPerson].bDirection==(iCurrentDirection>0))
               /* if (((lPassengersOnTheFloor[iCheckingPerson].iTargetFloor-iCurrentLevelOfTheLift>0)&&(bCurrentDirection==true))||
                    ((lPassengersOnTheFloor[iCheckingPerson].iTargetFloor-iCurrentLevelOfTheLift<0)&&(bCurrentDirection==false)))*/

                    {
                        lPassengersInTheLift.Add(lPassengersOnTheFloor[iCheckingPerson]);
                        lPassengersOnTheFloor.RemoveAt(iCheckingPerson);
                        iPresentNumberOfPeopleInside=iPresentNumberOfPeopleInside+1;
                    }
                else
                {
                    iCheckingPerson=iCheckingPerson+1;
                }
                }
                
        }

        public void vRemovePassengersFromTheLift(List<cPassenger> lPassengersInTheLift, int iNumberOfFloor)
        {
            int iCheckingPerson=0;
                while ((iPresentNumberOfPeopleInside>iCheckingPerson))
                {
                   
                if (lPassengersInTheLift[iCheckingPerson].iTargetFloor==iNumberOfFloor)
                    {
                        
                        //lPassengersOnTheFloor.Add((sPassenger)lPassengersInTheLift[iCheckingPerson]);
                        lPassengersInTheLift.RemoveAt(iCheckingPerson);
                        iPresentNumberOfPeopleInside=iPresentNumberOfPeopleInside-1;
                    }
                else
                {
                    iCheckingPerson=iCheckingPerson+1;
                }
                }
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
