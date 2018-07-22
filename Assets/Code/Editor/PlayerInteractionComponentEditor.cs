using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerInteractionComponentEditor : Editor
{
    private static GUIStyle sLabelStyle = null;

    [DrawGizmo(GizmoType.Active | GizmoType.NonSelected | GizmoType.Selected)]
    static void OnDrawGizmo(PlayerInteractionComponent playerInteractionComponent, GizmoType gizmoType)
    {
        if (sLabelStyle == null)
        {
            sLabelStyle = new GUIStyle(GUI.skin.label);
            sLabelStyle.normal.textColor = Color.magenta;
        }

        if (playerInteractionComponent.CurrentNearInteractableObjects == null || playerInteractionComponent.CurrentNearInteractableObjects.Length <= 0)
        {
            return;
        }

        Handles.color = Color.magenta;

        float dotThreshold = playerInteractionComponent.PickupSearchSettings.SearchDotProductThreshold;

        foreach(InteractableObject interactable in playerInteractionComponent.CurrentNearInteractableObjects)
        {
            Vector3 directionFromPlayer = (interactable.transform.position - playerInteractionComponent.transform.position);
            float distanceFromPlayer = directionFromPlayer.magnitude;
            float dot = Vector3.Dot(playerInteractionComponent.PlayerCamera.transform.forward, directionFromPlayer.normalized);

            Vector3 objectSize = interactable.GetRenderer().bounds.extents;
            Vector3 labelPos = interactable.transform.position + (Vector3.up * objectSize.y);
            Handles.Label(labelPos, string.Format("{0} ({1} : {2})", interactable.name, dot.ToString(), dotThreshold.ToString()), sLabelStyle);
            labelPos.y -= 0.5f;
            Handles.Label(labelPos, string.Format("{0}m : {1}m", System.Math.Round(distanceFromPlayer, 3), playerInteractionComponent.PickupSearchSettings.MaxDistance), sLabelStyle);
        }

        Handles.color = Color.white;

        Vector3 centreScreen = Camera.main.ScreenToWorldPoint(new Vector3(0.5f, 0.5f, 0f)) + Camera.main.transform.forward;

        float screenSpaceSearchSize = playerInteractionComponent.PickupSearchSettings.ScreenSpaceSearchRadius;

        screenSpaceSearchSize = HandleUtility.GetHandleSize(centreScreen) * screenSpaceSearchSize;

        Handles.Disc(Quaternion.identity, centreScreen, Vector3.forward, screenSpaceSearchSize, false, 1f);
    }
}
