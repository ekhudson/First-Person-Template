using System.Reflection;

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Transform))]
[CanEditMultipleObjects]
public class TransformEditor : Editor
{
    private static bool sShowSnapDirection = false;

    public override void OnInspectorGUI()
    {
        SerializedProperty positionProperty = serializedObject.FindProperty("m_LocalPosition");
        SerializedProperty rotationProperty = serializedObject.FindProperty("m_LocalRotation");
        SerializedProperty scaleProperty = serializedObject.FindProperty("m_LocalScale");

        EditorGUILayout.BeginVertical();

        //Position
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("P", GUILayout.Width(20f)))
        {
            Undo.RecordObjects(targets, "Positions Reset");
            positionProperty.vector3Value = Vector3.zero;
        }
        EditorGUILayout.PropertyField(positionProperty);
        EditorGUILayout.EndHorizontal();

        //Rotation
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("R", GUILayout.Width(20f)))
        {
            Undo.RecordObjects(targets, "Rotations Reset");
            foreach (Object t in (Object[])targets)
            {
                ((Transform)t).localEulerAngles = Vector3.zero;
            }
            rotationProperty.serializedObject.SetIsDifferentCacheDirty();
        }

        DrawRotationPropertyField(rotationProperty);
        EditorGUILayout.EndHorizontal();

        //Scale
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("S", GUILayout.Width(20f)))
        {
            Undo.RecordObjects(targets, "Scales Reset");
            scaleProperty.vector3Value = Vector3.one;
        }

        EditorGUILayout.PropertyField(scaleProperty);

        Rect scaleRect = GUILayoutUtility.GetLastRect();

        scaleRect.width = EditorGUIUtility.labelWidth;

        float scaleMod = 1f;
        scaleMod = EditorGUI.FloatField(scaleRect, "Local Scale", scaleMod, GUI.skin.label);
        scaleProperty.vector3Value *= scaleMod;

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Snap Options", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        sShowSnapDirection = EditorGUILayout.Toggle("Show Snap Direction", sShowSnapDirection);

        if (GUILayout.Button("Edit Snap Prefs"))
        {
            Assembly assembly = Assembly.GetAssembly(typeof(EditorWindow));
            System.Type t = assembly.GetType("UnityEditor.PreferencesWindow");
            MethodInfo methodInfo = t.GetMethod("ShowPreferencesWindow", BindingFlags.NonPublic | BindingFlags.Static);
            methodInfo.Invoke(null, null);
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Snap To Nearest Surface"))
        {
            //GameObjectUtilityHelpers.SnapObjectToNearestSurface();
        }

        EditorGUILayout.EndVertical();

        if (GUI.changed)
        {
            SceneView.RepaintAll();
        }

        serializedObject.ApplyModifiedProperties();
    }

    //Borrowed from interwebs http://wiki.unity3d.com/index.php?title=TransformInspector
    private void DrawRotationPropertyField(SerializedProperty rotationProperty)
    {
        Transform transform = (Transform)targets[0];
        Quaternion localRotation = transform.localRotation;
        foreach (Object t in (Object[])targets)
        {
            if (!SameRotation(localRotation, ((Transform)t).localRotation))
            {
                EditorGUI.showMixedValue = true;
                break;
            }
        }

        EditorGUI.BeginChangeCheck();

        Vector3 eulerAngles = EditorGUILayout.Vector3Field("Local Rotation", localRotation.eulerAngles);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(this.targets, "Rotation Changed");
            foreach (UnityEngine.Object obj in this.targets)
            {
                Transform t = (Transform)obj;
                t.localEulerAngles = eulerAngles;
            }
            rotationProperty.serializedObject.SetIsDifferentCacheDirty();
        }

        EditorGUI.showMixedValue = false;
    }

    private bool SameRotation(Quaternion rot1, Quaternion rot2)
    {
        if (rot1.x != rot2.x) return false;
        if (rot1.y != rot2.y) return false;
        if (rot1.z != rot2.z) return false;
        if (rot1.w != rot2.w) return false;
        return true;
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.Pickable)]
    static void DrawGizmos(Transform transform, GizmoType gizmoType)
    {
    //    if (sShowSnapDirection)
    //    {
    //        Vector3 snapDirection = LynxEditorPrefs.SnapDirection;

    //        Vector3 lineDirection = snapDirection.normalized;

    //        if (LynxEditorPrefs.SnapSpace == Space.Self)
    //        {
    //            lineDirection = transform.TransformVector(snapDirection).normalized;
    //        }

    //        Color oldColor = Gizmos.color;

    //        RaycastHit hit;
    //        if (GameObjectUtilityHelpers.TryGetSnapPosition(transform, out hit))
    //        {
    //            Gizmos.color = Color.green;
    //            Gizmos.DrawSphere(hit.point, LynxEditorPrefs.SnapPreviewRadius);
    //            Gizmos.DrawLine(hit.point, hit.point + hit.normal);
    //            lineDirection *= Vector3.Distance(transform.position, hit.point);
    //        }
    //        else
    //        {
    //            Gizmos.color = Color.red;
    //            lineDirection *= LynxEditorPrefs.SnapPreviewLength;
    //        }

    //        Gizmos.DrawLine(transform.position, transform.position + lineDirection);

    //        Gizmos.color = oldColor;
    //    }
    }
}
