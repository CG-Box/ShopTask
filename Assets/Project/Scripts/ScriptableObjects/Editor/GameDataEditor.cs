using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameData_SO), editorForChildClasses: true)]
public class GameDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //GUI.enabled = Application.isPlaying;

        GameData_SO targetObject = target as GameData_SO;
        if (GUILayout.Button("Reset (Game restart needed)"))
            targetObject.ResetToDefault();

        if (GUILayout.Button("Add 3000 Tickets"))
            targetObject.AddTickets(3000);

        if (GUILayout.Button("Complete Level 5"))
            targetObject.TrySetHighestCompletedLevel(5);

        if (GUILayout.Button("Complete Level 10"))
            targetObject.TrySetHighestCompletedLevel(10);
    }
}