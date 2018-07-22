﻿/******************************************************************************
 * Copyright (c) 2014 Game Loop
 * All Rights reserved.
 *****************************************************************************/

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EasyVoiceSettings))]
public class EasyVoiceSettingsInspector : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Label("This is the settings asset of your Easy Voice plugin");
        if (EasyVoiceEditorWindow.window != null)
        {
            if (GUILayout.Button("Switch to Easy Voice window"))
            {
                EditorWindow.FocusWindowIfItsOpen<EasyVoiceEditorWindow>();
            }
        }
        else
        {
            if (GUILayout.Button("Open Easy Voice window"))
            {
                EasyVoiceEditorWindow.Open();
            }
        }
    }
}