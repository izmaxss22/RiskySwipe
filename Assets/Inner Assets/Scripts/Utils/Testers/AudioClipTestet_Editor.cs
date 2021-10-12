using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(AudioSourceTester))]
public class AudioClipTestet_Editor : Editor
{

    private AudioSourceTester baseItem;
    public override void OnInspectorGUI()
    {
        baseItem = (AudioSourceTester)target;

        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal("box");
        if (GUILayout.Button("ПРЕД", GUILayout.Height(30)))
        {
            baseItem.SetPreviousItem();
        }
        if (GUILayout.Button("СЛЕД", GUILayout.Height(30)))
        {
            baseItem.SetNextItem();

        }

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("УДАЛИТЬ", GUILayout.Height(30)))
        {
            baseItem.DeleteItem();

        }
    }
}
#endif
