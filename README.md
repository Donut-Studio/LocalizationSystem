# Localization System Readme
Welcome!
This is my solution for adding multiple languages to your game.
This localization tool is based on a tutorial from Game Dev Guide. Watch his video for
more details: [Youtube](https://www.youtube.com/watch?v=c-dzg4M20wY))
Feel free to use it anywere in your projects.
Cheers, Donut Studio!


***
# Installation
1. download the code or .unitypackage
2. import everything into your project
3. add languages and text to the .csv
4. implement the system into your game


***
# Usage
After importing you're ready to start implementing.
Firstly, go to the Resources folder and open the csv file with your prefered app (e.g. Excel). 
The values are seperated with a semicolon (the field seperator can be changed in the csvloader class).

The first line (header) contains the language attributes.
The following lines contain the key and values for each language.
The keys must be different from each other. Seperate the keys with `/` to make it easier selecting them.
Extend the file with your languages, keys and values (preferably with another application):

| key | en | fr | ge |
| --- | --- | --- | --- |
| menu/play | Play | Jouer | Spielen |
| menu/quit | Quit | Sortie | Verlassen |
| game/dialog1 | Welcome adventurer! | Bienvenue aventurier! | Willkommen Abenteurer! |

The file itself should look like this:
> key;en;fr;ge
> menu/play;Play;Jouer;Spielen
> menu/quit;Quit;Sortie;Verlassen
> game/dialog1;Welcome adventurer!;Bienvenue aventurier!;Willkommen Abenteurer!

---
Now open the `Language.cs` and add your languages to the enum. 
The index represents the attributes from the csv file:
```csharp
public enum Language : int
{
    English = 0,
    French = 1,
    German = 2
}
```

---
To initialise the localization system or swap the language use the following code:
```csharp
LocalizationSystem.SetLanguage(Language.English, true);
```

Make sure to add `using DonutStudio.Utilities.LocalizationSystem;`!
If you like to update any values after a change, listen to the following event: `LocalizationSystem.onRefresh`
You can also invoke this event with a method: `LocalizationSystem.RefreshTextElements();`


To get a localized value use this code:
```csharp
bool success = LocalizationSystem.GetLocalizedValue("key", out string value);
```

---
For text elements you've already got two scripts: `LocalizedTMPro`, `LocalizedText`
Just add them to the corresponding gameobject and you're good to go.
Select the key via the search window and choose wether to update the text on awake and when the language has changed.


By the way, you can use the search window property anywere in your scripts:
```csharp
[SerializeField()]
private LocalizedValue localizedValue;
```
To get the key simply type: `localizedValue.key;`,
or call the function `localizedValue.GetValue(out string value);` which returns true if a value for this key exists.

---
To keep working on your game without having to switch to another application,
I've added a editor window to add and remove keys.
To access it go to the toolbar, press on `Donut Studio` and `Open Localizer`.
However you can only add the keys to the file and not the values.
Those should be added later with your preferred application.


***
# Credits
Localization system - Extention for Unity to enable localization in your game.
Created by Donut Studio, June 26, 2022.
Released into the public domain.