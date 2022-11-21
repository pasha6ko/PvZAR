using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Plant)), InitializeOnLoadAttribute]

public class PlantEditor : Editor
{
    Plant plant;
    SerializedObject SerPlant;

    private void OnEnable()
    {
        plant = (Plant)target;
        SerPlant = new SerializedObject(plant);
    }

    public override void OnInspectorGUI()
    {
        
        EditorGUILayout.Space();
        SerPlant.Update();
        plant.GenerateSun = EditorGUILayout.ToggleLeft(new GUIContent("Enable Sun Generation", "Can create money throw time"), plant.GenerateSun);
        EditorGUILayout.Space();
        base.OnInspectorGUI();

    }
}