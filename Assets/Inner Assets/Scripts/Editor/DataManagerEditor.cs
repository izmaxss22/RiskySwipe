using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataManager))]
public class DataManagerEditor : Editor
{
    private DataManager targetScript;
    private void OnEnable()
    {
        targetScript = (DataManager)target;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("CALCULATE STARS POINTS FOR GROUPED LEVELS")) targetScript.CalculatePointCountsForReachStars();
        if (GUILayout.Button("CALCULATE REWARDS COUNTS ON CHESTS")) targetScript.CalculateRewardsOnChests();
    }
}
