using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(ProceduralBuildings))]

public class ProceduralBuildingEditor : Editor {

	GUIStyle MiddleAlignment;
	GUIStyle GoTo;
	GUIStyle RightAlignment;
    GUIStyle headers;
	float angle;
	Transform PointTransform;
    void GenerateStyle()
    {
		MiddleAlignment = new GUIStyle ();
		GoTo = new GUIStyle ();
		GoTo.alignment = TextAnchor.MiddleCenter;
		MiddleAlignment.alignment = TextAnchor.MiddleCenter;
        headers = new GUIStyle();
        headers.alignment = TextAnchor.MiddleLeft;
        headers.fontStyle = FontStyle.Bold;
    }
    public override void OnInspectorGUI()
    {
        GenerateStyle();
        Texture myTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Crowd Procedural/Editor/logo.png", typeof(Texture));;
		GUILayout.Box(myTexture ,MiddleAlignment );
        GUI.color = Color.Lerp(Color.blue, Color.white, .8f);
        GUILayout.Space(3);
        ProceduralBuildings builder = (ProceduralBuildings)target;
		serializedObject.Update();
		GUILayout.Space(5);
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (30);
		if (GUILayout.Button("◄")){
			builder.BuildingFace = BuildingFace.Left;
		}
		if (GUILayout.Button("►")){
			builder.BuildingFace = BuildingFace.Right;
		}
		GUILayout.Space (30);
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.HelpBox("Building's face is toward " + builder.BuildingFace + " of the building axis", MessageType.None);
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Cornerbuildings"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Buildings"), true);
        serializedObject.ApplyModifiedProperties();
		GUILayout.Space(5);
		EditorGUI.BeginDisabledGroup(!builder.IsBuilding);
			EditorGUI.BeginDisabledGroup(!builder.BuildingsNotNull);
				if (GUILayout.Button("Reset Builder Poisition") && Application.isPlaying)
		        {
		            builder.ResetBuilderPosition = true;
		        }
			EditorGUI.EndDisabledGroup();
        EditorGUI.EndDisabledGroup();
		if (GUILayout.Button(builder.BuildingState) && Application.isPlaying){
			builder.IsBuilding = !builder.IsBuilding;
			if (builder.IsBuilding){
				builder.BuildingState = "Stop Building";
				builder.StartBuilding = true;
			}else {
				builder.BuildingState = "Start Building";
				builder.StopBuilding = true;
				builder.BuildingCurve = false;
			}
		}
		EditorGUI.BeginDisabledGroup (!Application.isPlaying);
			EditorGUI.BeginDisabledGroup(builder.BuildingsNotNull && !builder.IsBuilding);
		        if (GUILayout.Button("Generate new field") && Application.isPlaying)
		        {
		            builder.GenrateNewField = true;
					builder.BuildingCurve = false;
		        }
	        EditorGUI.EndDisabledGroup();
		EditorGUI.EndDisabledGroup();
		EditorGUI.BeginDisabledGroup((builder.BuildingsNotNull || builder.BuildingCurve) && !builder.IsBuilding);
		    EditorGUI.BeginDisabledGroup(!builder.placeable);
		        if (GUILayout.Button("Place Corner Side Building") && Application.isPlaying)
		        {
		            builder.PlaceCornerSideBuilding = true;
		        }
	        EditorGUI.EndDisabledGroup();
		EditorGUI.EndDisabledGroup();
			GUILayout.Space(5);

		EditorGUI.BeginDisabledGroup (!builder.IsBuilding);
			EditorGUI.BeginDisabledGroup(!builder.placeable);
				if (GUILayout.Button(builder.CurveBuildingState) && Application.isPlaying)
				{
					builder.BuildingCurve = !builder.BuildingCurve;
					if (!builder.BuildingCurve) {
				        builder.CurveBuildingState = "Start Curve Building";
						builder.Reset();
			    } else if (builder.BuildingCurve) {
				        builder.CurveBuildingState = "Stop Curve Building";
					}
				}
				EditorGUILayout.BeginHorizontal ();
					angle = builder.CurveBuildingAngle;
					EditorGUI.BeginDisabledGroup(builder.BuildingsNotNull);
					if (GUILayout.Button("Place Curve Building") && Application.isPlaying)
					{
						builder.PlaceCurveBuilding(angle);
					}
					EditorGUI.EndDisabledGroup();
					builder.CurveBuildingAngle = EditorGUILayout.FloatField (builder.CurveBuildingAngle);
				EditorGUILayout.EndHorizontal ();
			EditorGUI.EndDisabledGroup ();
		EditorGUI.EndDisabledGroup ();

		GUILayout.Space(10);
		EditorGUI.BeginDisabledGroup (!builder.StopBuilding);
		EditorGUILayout.BeginHorizontal ();
		PointTransform = builder.PointTransform;
		if (GUILayout.Button("Left side") && Application.isPlaying){
			builder.GoTo (PointTransform , "left");
		}
		GUILayout.Box (" ◄ Go to ► ");
		if (GUILayout.Button("Right side") && Application.isPlaying){
			builder.GoTo (PointTransform , "right");
		}
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space (5);
		builder.PointTransform = (Transform)EditorGUILayout.ObjectField(builder.PointTransform, typeof(Transform), true);
		EditorGUI.EndDisabledGroup ();
        if (!builder.BuilderInBuildingField)
        {
            EditorGUILayout.HelpBox("You're far away from building field please get back to it !", MessageType.Error);
        }
		GUILayout.Space (5);
        
    }

}
