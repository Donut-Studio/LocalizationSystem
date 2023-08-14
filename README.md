# Localization System Readme
Welcome!
This is my solution for adding multiple languages to your game.
This localization tool is based on a tutorial from Game Dev Guide. Watch his video for
more details: [Youtube](https://www.youtube.com/watch?v=c-dzg4M20wY))
Feel free to use it anywere in your projects.
Cheers, Donut Studio!


***
# Features
- comma-seperated-value file for the translation (you are able to use another program to edit it)
- config file for adjusting everything directly in the editor
- supports different fonts for different languages
- custom editor window to add/remove keys, languages and sort the file
- easy setup and usage with many available methods


***
# Installation
1. download the code or .unitypackage
2. import everything into your project
3. add languages and text to the .csv
4. add the languages and fonts to the config file
5. implement the system into your game


***
# Usage
After importing you're ready to start implementing.
Firstly, go to the `Resources` folder and open the csv file with your prefered app (e.g. Excel). 
The values are seperated with a semicolon (the lines and field seperator can be changed in the csvloader class).

The first line (header) contains the language attributes.
The following lines contain the keys and values for each language.
The keys must be different from each other. Seperate the keys with `/` to make it easier selecting them in the editor.
Extend the file with your languages, keys and values (preferably with another application but make sure to save it as UTF-8):

| key | en | fr | ge |
| --- | --- | --- | --- |
| menu/play | Play | Jouer | Spielen |
| menu/quit | Quit | Sortie | Verlassen |
| game/dialog1 | Welcome adventurer! | Bienvenue aventurier! | Willkommen Abenteurer! |

The file itself should look like this:
> key;en;fr;ge \
> menu/play;Play;Jouer;Spielen \
> menu/quit;Quit;Sortie;Verlassen \
> game/dialog1;Welcome adventurer!;Bienvenue aventurier!;Willkommen Abenteurer!

---
Now go to the `Resources` folder and add your languages to the array.
You can also select a specific font for the language.
The index represents the attributes from the csv file:

> 0 => en (English) \
> 1 => fr (French) \
> 2 => ge (German)

---
To initialise the localization system 
use the following code:
```csharp
LocalizationSystem.Initialize();
```

This should be done once at the start of the game.
However, if you call other methods from the class and the system is not initialized, 
it will try and initialize it first. So selecting a language at the start of the game should do the trick:

```csharp
LocalizationSystem.SetLanguage(0, true); // in this example you will select English (index => 0)
```
If you like to update any values after a change, listen to the following event: `LocalizationSystem.onRefresh`
You can also invoke this event with a method: `LocalizationSystem.RefreshTextElements();`

---
To get a localized value use this code:
```csharp
bool success = LocalizationSystem.GetLocalizedValue("key", out string value);
```

---
For text elements (text mesh pro) you've already added a script: `LocalizedText`
Just add them to the corresponding gameobject and you're good to go.
Select the key via the search window and choose wether to update the text on awake and when the language has changed.

There are also multiple methods for directly assigning the font and text to a tmp text object:
```csharp
LocalizationSystem.SetTMP(tmpObject, key); // set the font and text
LocalizationSystem.SetFont(tmpObject); // set the font
LocalizationSystem.SetText(tmpObject, key); // set the text
LocalizationSystem.SetText(tmpObject, "This should be {0} a {2} phrased text! {3}", key1, key2, key3); // set the text with multiple keys
```

---
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
Localization system - Extention for Unity to use multiple languages in your game.
Created by Donut Studio, August 14, 2023.
Released into the public domain.