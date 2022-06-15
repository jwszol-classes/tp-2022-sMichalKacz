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
    }

    class cBuilding
    {
        int iNumberOfLifts=1;
        int iNumberOfFloors;
         List<cFloor> lFloors = new List<cFloor>();
        
        void vAddFloorsToBulding(int iNumberOfFloors)
        {
            
            for(int i = 0; i < iNumberOfFloors; i++)
            {
                lFloors.Add(new cFloor() { iNumerOfFloor=i});
            }
        }
        
    }
    class cFloor
    {
        public int iNumerOfFloor;
        //int[] iPassengers = new int[3];

        List<sPassenger> lPassengersOnTheFloor = new List<sPassenger>();

         void vAddPassengerToTheFloor(int iPresentFloor, int iTargettFloor)
        {
           
            lPassengersOnTheFloor.Add(new sPassenger() { iPresentFloor=iPresentFloor, jTargetFloor=iTargettFloor });
            
        }
    }
    class cLift
    {
        int iPresentNumberOfPeopleInside=0;
        int iMaxNumberOfPeopleInside;
        bool bCurrentDirection=true; //0-down, 1-up

        List<sPassenger> lPassengersInTheFlift = new List<sPassenger>();
        //int[] iPassengers = new int[iMaxNumberOfPeopleInside];

        void vCalculatingMaxNumberOfPeople(int iMaxWeight, int iWeightOfPerson)
        {
            iMaxNumberOfPeopleInside=iMaxWeight/iWeightOfPerson;
        }

        void vCalculatingTheNextFloor()
        {

        }

        void vAddPassengerToTheLift(int iTargettFloor)
        {
            if (iPresentNumberOfPeopleInside < iMaxNumberOfPeopleInside)
            {
            lPassengersInTheFlift.Add(new sPassenger() { jTargetFloor=iTargettFloor });
            iPresentNumberOfPeopleInside=iPresentNumberOfPeopleInside+1;
            }
            
        }

         void vRemovePassengerToTheLift(int iTargettFloor)
        {
            if (iPresentNumberOfPeopleInside > 0)
            {
            lPassengersInTheFlift.RemoveAt(iTargettFloor);
            iPresentNumberOfPeopleInside=iPresentNumberOfPeopleInside+1;
            }
            
        }

        void vAddPassengersToTheLift(List<sPassenger> lPassengersOnTheFloor, int iNumerOfFloor)
        {
            if (iPresentNumberOfPeopleInside < iMaxNumberOfPeopleInside)
            {
                int iCheckingPerson=0;
                while ((iMaxNumberOfPeopleInside - iPresentNumberOfPeopleInside>0) && (lPassengersOnTheFloor.Count>iCheckingPerson))
                {
                    if ((iNumerOfFloor > lPassengersOnTheFloor[iCheckingPerson].jTargetFloor) && (bCurrentDirection == false))
                    {
                        //vAddPassengerToTheLift(lPassengersOnTheFloor[i].jTargetFloor);
                    }
                }
            }
        }
    }
}
