using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftSimulator
{
    public struct sPassenger
    {
        public int iPresentFloor;
        public int jTargetFloor;
        public bool bDirection; //0-down, 1-up
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

        public List<sPassenger> lPassengersOnTheFloor = new List<sPassenger>();

        public void vAddPassengerToTheFloor(int iPresentFloor, int iTargettFloor)
        {
           bool bDirectonCalc;
           if(iPresentFloor < iTargettFloor)bDirectonCalc = true;
           else bDirectonCalc = false;
           lPassengersOnTheFloor.Add(new sPassenger() { iPresentFloor=iPresentFloor, jTargetFloor=iTargettFloor , bDirection=bDirectonCalc});
        }
    }
    class cLift
    {
        int iPresentNumberOfPeopleInside=0;
        int iMaxNumberOfPeopleInside;
        int iCurrentLevelOfTheLift=0;
        bool bCurrentDirection=true; //0-down, 1-up

        List<sPassenger> lPassengersInTheLift = new List<sPassenger>();
        //int[] iPassengers = new int[iMaxNumberOfPeopleInside];

        void vCalculatingMaxNumberOfPeople(int iMaxWeight, int iWeightOfPerson)
        {
            iMaxNumberOfPeopleInside=iMaxWeight/iWeightOfPerson;
        }

        void vFullElevatorMovement(List<sPassenger> lPassengersOnTheFloor, int iNumberOfFloor, int iNumberOfFloors)
        {
            vRemovePassengersFromTheLift(lPassengersOnTheFloor, iNumberOfFloor);
            vCalculatingTheNextFloor(lPassengersOnTheFloor, iNumberOfFloor, iNumberOfFloors);
            vAddPassengersToTheLift(lPassengersOnTheFloor);
        }

        void vCalculatingTheNextFloor(List<sPassenger> lPassengersOnTheFloor, int iNumberOfFloor, int iNumberOfFloors)
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
       
        void vAddPassengersToTheLift(List<sPassenger> lPassengersOnTheFloor)
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

        void vRemovePassengersFromTheLift(List<sPassenger> lPassengersOnTheFloor, int iNumberOfFloor)
        {
            int iCheckingPerson=0;
                while ((iPresentNumberOfPeopleInside>iCheckingPerson))
                {
                   
                if (lPassengersOnTheFloor[iCheckingPerson].jTargetFloor==iNumberOfFloor)
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
