using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CustomSensorVolume))]
public class CustomSensorVolumeEditor : Editor
{
    private static CustomSensorVolume sTarget = null;

    public CustomSensorVolume Target
    {
        get
        {
            if (sTarget == null)
            {
                sTarget = target as CustomSensorVolume; 
            }

            return sTarget;
        }
        set
        {
            sTarget = value;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        foreach(KeyValuePair<int, List<GameObject>> pair in Target.CurrentObjectDict)
        {
            GUILayout.BeginVertical(GUI.skin.box);            
            GUILayout.Label(LayerMask.LayerToName(pair.Key), EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            foreach (GameObject obj in pair.Value)
            {
                if (obj == null)
                {
                    GUI.color = Color.red;
                    GUILayout.Label("BAD OBJECT", EditorStyles.whiteLabel);
                    GUI.color = Color.white;
                }
                else
                {
                    GUILayout.Label(obj.name);
                }               
            }

            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
            Repaint();
        }
    }

}
