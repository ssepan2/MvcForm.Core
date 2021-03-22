ModelAndSettings v0.2

Purpose:
To serve as a reference architecture and template for console and winforms apps that share a common model and settings file. 
Also demonstrates my interpretation of the Model-View-Controller pattern. 


Usage notes:

~N/A


Enhancements:

0.4:
~TODO:modify forms interface to include more standard menus/buttons/actions
~TODO:review design around args param, and whether it should go into view (now that model is not init until view loaded)

0.3:
~Handlers were not being correctly wired up to sub-components of Settings or Model
~Using Ssepan.Application 2.8
~Added ModelCompoment sub-component to Model, to demonstrate notification from component NOT backed by Settings or SettignsComponent (which DO perform notification internally).

0.2:
~Handler in Program.cs was not wired up in forms or console; added 'PropertyChanged += PropertyChangedEventHandlerDelegate;' to Main.
~No handler wired up to Settings in forms or console. Renamed 'PropertyChangedEventHandlerDelegate' to 'ModelPropertyChangedEventHandlerDelegate'. Wired up latter in 'InitViewModel' after model handler in view. Currently handling 'Dirty' property.
~Added 'SettingsPropertyChangedEventHandlerDelegate' to new static property 'DefaultHandler' in SettingsController. 
~Modified OnPropertyChanged in SettingsBase to fire OnChanged for 'Dirty' when property name is not 'Dirty'.
~Fixed bug in GetPathForSave in Ssepan.Io where SaveAs did not display dialog for '(new)'.
~Modified Settings in Ssepan.Application to implement part of its interfaces as a new interface ISettingsComponent, and implemented an example as a property SomeComponent which copies the PropertyChanged handlers from settings and implements its own Equals, Dirty, CopyTo, Sync, etc, so Settings does not need to know the details.
~Updated to Framework 4.8
~Updated Ssepan.* to 2.7

0.1: (RELEASED)
~Refactored to use new MVVM / MVC hybrid.
~Updated Ssepan.* to 2.6


Fixes:
~N/A

Known Issues:
~Filename passed in command line argument is converted/passed in DOS 8.3 equivalent format. Cannot compare file extension directly. Status: research. 
~Running this app under Vista or Windows 7 requires that the library that writes to the event log (Ssepan.Utility.dll) have its name added to the list of allowed 'sources'. Rather than do it manually, the one way to be sure to get it right is to simply run the application the first time As Administrator, and the settings will be added. After that you may run it normally. To register additional DLLs for the event log, you can use this trick any time you get an error indicating that you cannot write to it. Or you can manually register DLLs by adding a key called '<filename>.dll' under HKLM\System\CurrentControlSet\services\eventlog\Application\, and adding the string value 'EventMessageFile' with the value like <C>:\<Windows>\Microsoft.NET\Framework\v2.0.50727\EventLogMessages.dll (where the drive letter and Windows folder match your system). Status: work-around. 

Possible Enhancements:
~n/a


Steve Sepan
ssepanus@yahoo.com
6/4/2014