# Lift Simulator

## Description
- Project was made in C# and Xaml.
- Project solves the problem of control system of the lift in apartment building.
- As in result of the project authors improved team working and programming skills. 

### How to use it?
1. First of all you can change default settings pressing button with a gear in upper right corner.
2. After that you should choose floor pressing small buttons with numbers of floors (they are on the left side near horizontal lines of floors)
3. To choose passenger's target floor you should press bigger button with number on the left side. 
> On every floor number with current level will not be available!
4. After those steps lift will start working. You could add new passengers with diifferent current and target floors in the whole time of program working.
> If you change settings during program working current passengers and lifts positions will be completly cleared 

###Libraries used in C# parts of project:
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
using System.Windows.Data;
using System.Windows.Documents;



### The content of key files
#### cBuilding.cs
1. class **cPassenger**- includes functions of animation the people on the floors such as:
* walking into the lift and going out of it
* hovering the cursor over passenger
2. class **cBuilding**- it is the largest class in whole program responsible for creating system of floors and lifts. In this class there are located functions responsible for:
> number of lifts is in default setting equal to 1 but this could be quite easly changed to version with higher number of lifts 
* calculating the direction and next floor which the elevator should go to.
Purpose of alghorithm is to carry passengers with as few passes as possible. Lift moves only one floor every time and checks:
1. *does anyone go out of the elevator?*
2. *does anyone want to enter the lift (it is true only if there is enough place in the lift and the directions of lift and passengers are the same)?*
>here algorithm also checks the case if there is anybody in the lift and if lift is also in the highest or lowest floor with passenger, than lift will stop and take the passenger to wanted direction (up/down).
3. *if at least one anwser for previous questions is positive, lift will stop on this floor. Else lift will push on.*
* animations of opening and closing doors in the lift 
* animations of lift vertical moving
3. class **cFloor** is only responsible for adding new passengers to the proper floors
4. class **cLift** is responsible for storage data like:
* maximum lifting capacity
* present number of people inside the lift
* present level of the lift
* and functions of adding and removing passengers to the lift
#### **MainWindow.xaml** and **MainWindow.xaml.cs**
This two files are responsible for layout of main window. They define place of buttons and other structures. **MainWindow.xaml.cs** is also including creator of building with floors and lifts.
#### **SettingsWindow.xaml** and **SettingsWindow.xaml.cs**
This files are responsible for service all data which user could change:
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
