using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(CameraControl))]
[CanEditMultipleObjects]

public class CameraControlEditor : Editor
{
    public SerializedProperty
        solution_Prop,
        speed_Prop,
        scaling_Prop,
        factor_Prop,
        angle_Prop,
        visible_Prop,
        intrusive_Prop,
        gradsOn_Prop,
        greensOff_Prop,
        mappingAngle_Prop,
        orientation_Prop;

	void OnEnable()
	{
        solution_Prop = serializedObject.FindProperty("solution");
        speed_Prop = serializedObject.FindProperty("rotationSpeed");
        scaling_Prop = serializedObject.FindProperty("scalingRotationSpeed");
        factor_Prop = serializedObject.FindProperty("scalingRotationFactor");
        angle_Prop = serializedObject.FindProperty("rotationStartingAngle");
        visible_Prop = serializedObject.FindProperty("visiblePanels");
        intrusive_Prop = serializedObject.FindProperty("intrusivePanels");
        gradsOn_Prop = serializedObject.FindProperty("gradientsOn");
        greensOff_Prop = serializedObject.FindProperty("greensOff");
        mappingAngle_Prop = serializedObject.FindProperty("rotationMappingAngle");
        orientation_Prop = serializedObject.FindProperty("orientation");
    }

	public override void OnInspectorGUI()
	{
        serializedObject.Update();

        EditorGUILayout.PropertyField(solution_Prop);

        CameraControl.Solution so = (CameraControl.Solution)solution_Prop.enumValueIndex;

        switch(so)
        {
            case CameraControl.Solution.Panels:
                EditorGUILayout.PropertyField(speed_Prop, new GUIContent("Rotation Speed"));
                EditorGUILayout.PropertyField(scaling_Prop, new GUIContent("Scaling Rotation"));
                EditorGUILayout.PropertyField(factor_Prop, new GUIContent("Scaling Factor"));
                EditorGUILayout.IntSlider(angle_Prop, 0, 180, new GUIContent("Rotation Angle"));
                EditorGUILayout.PropertyField(visible_Prop, new GUIContent("Visible Panels"));
                EditorGUILayout.PropertyField(intrusive_Prop, new GUIContent("Intrusive Panels"));
                EditorGUILayout.PropertyField(gradsOn_Prop, new GUIContent("Gradients On"));
                EditorGUILayout.PropertyField(greensOff_Prop, new GUIContent("Greens Off"));
                break;

            case CameraControl.Solution.RotationMapping:
                EditorGUILayout.IntSlider(mappingAngle_Prop, 0, 120, new GUIContent("Rotation Mapping Angle"));
                EditorGUILayout.IntSlider(orientation_Prop, -180, 180, new GUIContent("Camera Orientation"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
	}
}