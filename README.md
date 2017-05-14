# Unity Project Settings in a New Window
Menu shortcuts for opening Unity project settings in a window rather than in the Inspector.

Has this ever happened to you?

You're editing a light's bias, and you need to change your quality settings. You go to Edit/Project settings/Quality and tweak something. Now your light was deselected, so you reselect it, and tweak something else about the light. Now your quality settings need more tweaking, so you go back through Edit/Project settings/Quality.

Ad infinitum...

This script solves that by making it super easy to open your project settings in a window so you can have both side-by-side or in tabs.

# To Install: 
Drop `SettingsWindow.cs` into `Assets/Editor/`

# To Use: 
Select one of the new menu items that appear like so in these menus:
"Edit/Project Settings/Input (in a new window)"
"Window/Project Settings/Input"

This script does the same thing as following these steps:

1. Create a new Inspector tab by right-clicking on any window tab
2. Selecting Edit/Project Settings/(some selection)
3. Lock the new inspector tab, so the object it's editing can't change
4. Reselect whatever gameobjects or assets that were previously selected

Tested in Unity 5.6.0f3.
