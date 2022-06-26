# LiftSimulator

## Description
	Project was made for pass a subject of Programming Techniques.
	Project solves the problem of control system in apartment building.
	Results of the project were improving team working and programming skills. 
	
	###The content of key files
		####cBuilding.cs
			-class cPassenger- includes functions of animation the people on the floors like:
				*walking to the lift and going out of the lift
				*hovering the cursor over passenger
			-class cBuilding- it is the biggest class in whole programm responsible for creating system of floors and lifts.
			> number of lifts is in default setting equal to 1 but this could be quite easy changed to version with bigger number of lifts
			In this class are located functions responsible for:
				*animations of opening and closing doors in the lift 
				*animations of lift vertical driving
				*calculating the direction and next floor which the elevator should go to.
					Purpose of alghorithm is to carry passengers in as few passes as possible. Lift while moving in every floor and checks if:
					1. anyone go out of the elevator?
					2. is anyone would like to go to the lift (if only there is enough place in lift and the directions of lift and passenger are same)?
			-class cFloor is only respond for adding new passengers to the proper floor
			-class cLift is responsible for storage data like:
				maximum lifting capacity
				present number of people inside the lift
				present level of the lift
			and functions of adding and removing passengers to the lift
		####MainWindow.xaml
	
## Requirements
	*Windows 10 or newer
	*development environment serving C#

## Compilation

	*The code can be compiled with the provided makefile using the standard `make` command.

## Credits
	*authors of project are:
	-[Kacper Kałużny](https://github.com/kacperkaluzny)
	-[Michał Kaczmarek](https://github.com/sMichalKacz)

## License

## Known bugs
	*złe dane
