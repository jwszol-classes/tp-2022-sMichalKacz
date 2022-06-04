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
        
        static void vAddFloorsToBulding(int iNumberOfFloors)
        {
            
            for(int i = 0; i < iNumberOfFloors; i++)
            {
               // lFloors.Add(new cFloor() { iNumerOfFloor=i});
            }
        }
        
    }
    class cFloor
    {
        public int iNumerOfFloor;
        //int[] iPassengers = new int[3];

        List<sPassenger> lPassengersOnTheFloor = new List<sPassenger>();

         static void vAddPassengerToTheFloor(int iPresentFloor, int iTargettFloor)
        {
           
         //   lPassengersOnTheFloor.Add(new sPassenger() { iPresentFloor=iPresentFloor, jTargetFloor=iTargettFloor });
            
        }
    }
    class cLift
    {
        int iPresentNumberOfPeopleInside=0;
        int iMaxWeight;
        int iMaxNumberOfPeopleInside;
        bool bCurrentDirection=true; //0-down, 1-up

        static void vCalculatingTheNextFloor()
        {

        }

        static void vAddPassengerToTheLift()
        {

        }

        static void vAddPassengersToTheLift()
        {

        }
    }
}
