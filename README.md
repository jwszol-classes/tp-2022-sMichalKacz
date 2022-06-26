# LiftSimulator

## Description
- Project was made in C# and Xaml.
- Project solves the problem of control system of the lift in apartment building.
- In the results of the project authors improved team working and programming skills. 

### How to use it?
1. First of all you could change default settings pressing bottom with gear in upper right corner.
2. After that you should choose floor pressing small buttons with numbers of floors (in the left site in the nesarest of horizontal lines of floors)
3. To choose passengers target floor you should press bigger button with number on the left site. 
> In every floor number with current level will not be available! <
4. After those steps lift will star working. You could add new passengers with diifferent current and target floors in the whole time of programm working.
5. > If you change settings during work programm current passengers and lifts positions will be completly cleared <

### The content of key files
#### cBuilding.cs
1. class cPassenger- includes functions of animation the people on the floors like:
* walking to the lift and going out of the lift
* hovering the cursor over passenger
2. class cBuilding- it is the largest class in whole programm responsible for creating system of floors and lifts.
> number of lifts is in default setting equal to 1 but this could be quite easy changed to version with bigger number of lifts <
In this class are located functions responsible for:
* animations of opening and closing doors in the lift 
* animations of lift vertical driving
* calculating the direction and next floor which the elevator should go to.
Purpose of alghorithm is to carry passengers in as few passes as possible. Lift everytime moves only one floor and check:
2.1. is anyone go out of the elevator?
2.2. is anyone would like to go to the lift (it is true if only there is enough place in the lift and the directions of lift and passenger are the same)?
>here algorith also check case that if in lift is nobody and in the highest or lowest floor is passenger with diiferent direction, direction of lift would change to direction of the passenger <
3. class cFloor is only responsible for adding new passengers to the proper floor
4. class cLift is responsible for storage data like:
* maximum lifting capacity
* present number of people inside the lift
* present level of the lift
* and functions of adding and removing passengers to the lift
#### **MainWindow.xaml** and **MainWindow.xaml.cs**
This two files are responsible for layout of main window. They define place of buttons and other structures. In **MainWindow.xaml.cs** is also including creator of building with floors and lifts.
#### **SettingsWindow.xaml** and **SettingsWindow.xaml.cs**
This files are responsible for service all data that user could change:
1. Number of floors
2. Human weight
3. Limit of the total weight of passengers in the lift
	
## Requirements
* Windows 10 or newer
## Compilation
* The code can be compiled with the provided makefile using the standard `make` command.
## Authors
- [Kacper Kałużny](https://github.com/kacperkaluzny)
- [Michał Kaczmarek](https://github.com/sMichalKacz)

## Known bugs
* if user in options enter wrong data (letters, numbers that are not natural numbers etc.) program would trash
