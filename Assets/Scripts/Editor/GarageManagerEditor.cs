using System.Collections.Generic;
using Garage;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GarageManager))]
public class GarageManagerEditor : Editor
{
    private SerializedProperty garages;

    private void OnEnable()
    {
        garages = serializedObject.FindProperty("garages");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(garages, true);

        GUILayout.Space(10);

        if (GUILayout.Button("Refresh Garages"))
        {
            RefreshGarages();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void RefreshGarages()
    {
        string[] guids = AssetDatabase.FindAssets("t:GarageData");

        List<GarageData> foundGarages = new List<GarageData>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            GarageData garage =
                AssetDatabase.LoadAssetAtPath<GarageData>(path);

            if (garage != null)
            {
                foundGarages.Add(garage);
            }
        }

        garages.arraySize = foundGarages.Count;

        for (int i = 0; i < foundGarages.Count; i++)
        {
            garages.GetArrayElementAtIndex(i).objectReferenceValue =
                foundGarages[i];
        }

        serializedObject.ApplyModifiedProperties();

        Debug.Log($"Garage list refreshed. Found: {foundGarages.Count}");
    }
}