using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

public class SettingsWindow
{
    [MenuItem("Edit/Project Settings/Input (in a new window)")]
    [MenuItem("Window/Project Settings/Input")]
    static void InputSettings()
    {
        ShowSettingsInspectorProjectSettings("Edit/Project Settings/Input");
    }
    [MenuItem("Edit/Project Settings/Tags and Layers (in a new window)")]
    [MenuItem("Window/Project Settings/Tags and Layers")]
    static void TagsAndLayersSettings()
    {
        ShowSettingsInspectorProjectSettings("Edit/Project Settings/Tags and Layers");
    }
    [MenuItem("Edit/Project Settings/Audio (in a new window)")]
    [MenuItem("Window/Project Settings/Audio")]
    static void AudioSettings()
    {
        ShowSettingsInspectorProjectSettings("Edit/Project Settings/Audio");
    }
    [MenuItem("Edit/Project Settings/Time (in a new window)")]
    [MenuItem("Window/Project Settings/Time")]
    static void TimeSettings()
    {
        ShowSettingsInspectorProjectSettings("Edit/Project Settings/Time");
    }
    [MenuItem("Edit/Project Settings/Player (in a new window)")]
    [MenuItem("Window/Project Settings/Player")]
    static void PlayerSettings()
    {
        ShowSettingsInspectorProjectSettings("Edit/Project Settings/Player");
    }
    [MenuItem("Edit/Project Settings/Physics (in a new window)")]
    [MenuItem("Window/Project Settings/Physics")]
    static void PhysicsSettings()
    {
        ShowSettingsInspectorProjectSettings("Edit/Project Settings/Physics");
    }
    [MenuItem("Edit/Project Settings/Physics 2D (in a new window)")]
    [MenuItem("Window/Project Settings/Physics 2D")]
    static void Physics2DSettings()
    {
        ShowSettingsInspectorProjectSettings("Edit/Project Settings/Physics 2D");
    }
    [MenuItem("Edit/Project Settings/Quality (in a new window)")]
    [MenuItem("Window/Project Settings/Quality")]
    static void QualitySettings()
    {
        ShowSettingsInspectorProjectSettings("Edit/Project Settings/Quality");
    }
    [MenuItem("Edit/Project Settings/Graphics (in a new window)")]
    [MenuItem("Window/Project Settings/Graphics")]
    static void GraphicsSettings()
    {
        ShowSettingsInspectorProjectSettings("Edit/Project Settings/Graphics");
    }
    [MenuItem("Edit/Project Settings/Network (in a new window)")]
    [MenuItem("Window/Project Settings/Network")]
    static void NetworkSettings()
    {
        ShowSettingsInspectorProjectSettings("Edit/Project Settings/Network");
    }
    [MenuItem("Edit/Project Settings/Editor (in a new window)")]
    [MenuItem("Window/Project Settings/Editor")]
    static void EditorSettings()
    {
        ShowSettingsInspectorProjectSettings("Edit/Project Settings/Editor");
    }
    [MenuItem("Edit/Project Settings/Script Execution Order (in a new window)")]
    [MenuItem("Window/Project Settings/Script Execution Order")]
    static void ScriptExecutionOrderSettings()
    {
        ShowSettingsInspectorProjectSettings("Edit/Project Settings/Script Execution Order");
    }

    public delegate string SelectDelegate(object o);

    public static string SelectProjectSettings(object o)
    {
        string menuItem = o as string;
        EditorApplication.ExecuteMenuItem(menuItem);
        return menuItem.Substring(menuItem.LastIndexOf('/') + 1);
    }
    

    public static void ShowSettingsInspectorProjectSettings(string s)
    {
        ShowInspector(SelectProjectSettings, s);
    }

    // This is unrelated to settings windows but it's a nice bonus feature using the same code.
    // However for reasons unknown, it doesn't work. Worth investigating.
    //[MenuItem("Window/New inspector from selection")]
    //public static void NewInspectorFromSelection()
    //{
    //    ShowInspector(SelectUnityObject, Selection.objects);
    //}
    //public static string SelectUnityObject(object o)
    //{
    //    // Selection didn't change, so there's nothing to do here except return a window title
    //    Selection.objects = o as UnityEngine.Object[];

    //    if (Selection.activeObject != null)
    //    {
    //        return Selection.activeObject.name;
    //    }

    //    return null;
    //}

    public static void ShowInspector(SelectDelegate select, object arg)
    {
        //Type inspectorType = Type.GetType("UnityEditor.InspectorWindow, UnityEditor");
        EditorWindow inspector = ScriptableObject.CreateInstance("UnityEditor.InspectorWindow") as EditorWindow;

        // Before activating the settings window, save our old selection so we can restore it
        UnityEngine.Object[] oldSelection = Selection.objects;

        string title = select(arg);

        bool successfullySetLock = false;

        if (inspector != null)
        {
            // Use reflection (InspectorWindow is inaccessible due to internal protection level) 
            // to lock the inspector we created so it doesn't change its selection when we 
            // restore our selection back to normal
            Type inspectorType = inspector.GetType();

            PropertyInfo isLockedProperty = inspectorType.GetProperty("isLocked");
            if (isLockedProperty != null)
            {
                MethodInfo setter = isLockedProperty.GetSetMethod();
                if (setter != null)
                {
                    try
                    {
                        setter.Invoke(inspector, new object[] { true });
                        successfullySetLock = true;
                    }
                    catch
                    {
                        successfullySetLock = false;
                    }
                }
            }

            // TODO: Try to dock the window? That's more private APIs though.

            // Change the new window's title to match the settings being edited

            // TODO: Unfortunately, this only persists until the next script recompile.
            // There's not really a reliable way to get a list of inspector windows though, so this might prove tricky to fix.
            if (title != null)
            {
                inspector.titleContent = new GUIContent(inspector.titleContent);
                inspector.titleContent.text = title;
                inspector.titleContent.image = null;
            }
        }
        else
        {
            Debug.LogError("Failed to create InspectorWindow. Did the private API change?");
        }

        if (successfullySetLock)
        {
            // Only restore the selection if the lock on the window was set successfully, otherwise the settings window that was requested won't even show up
            Selection.objects = oldSelection;
        }
        else
        {
            Debug.LogError("Looks like something changed in the InspectorWindow's private API. This settings shortcut may not work correctly.");
        }
        
    }
}
