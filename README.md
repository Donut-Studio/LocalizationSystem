# Localization System Readme
Welcome!
This is my solution for adding multiple languages to your game.
This localization tool is based on a tutorial from Game Dev Guide. Watch his video for
more details: [Youtube](https://www.youtube.com/watch?v=c-dzg4M20wY))
Feel free to use it anywere in your projects.

***
# Installation
1. download the code or .unitypackage
2. import everything into your project

***
# Usage
After importing you're ready to start implementing.
Firstly, go to the Resources folder and open the csv file with your prefered app (e.g. Excel). 
The values are seperated with a semicolon.

The first line (header) contains the language attributes.
The following lines contain the key and values for each language.
The keys must be different from each other.
Extend the file with your languages, keys and values:

|key|en|fr|ge|
|menu/quit|Quit|Sortie|Verlassen|
|menu/play|Play|Jouer|Spielen|
|game/dialog1|Welcome adventurer!|Bienvenue aventurier!|Willkommen Abenteurer!|

Seperate the keys with `/` to make it easier selecting them.

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
LocalizationSystem.SwapLanguage(Language.English); // also invokes the onRefresh event
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
Just add them to the corresponding game object and you're good to go.
Select the key via the dropdown and choose wether to update the text on change or not.

By the way, you can use the dropdown property anywere in your scripts:

```csharp
[SerializeField()]
private LocalizedValue key;
```
To get the key simply type: `key.key;`

***
# Credits
Localization system - Extention for Unity to enable localization in your game.
Created by Donut Studio, March 05, 2022.
Released into the public domain.