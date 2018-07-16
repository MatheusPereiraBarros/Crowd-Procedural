using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(BuildingsField))]

public class BuildingsFieldEditor : Editor {
    bool PrefabSaved = false;
    GUIStyle newStyle;
    void GenerateStyle()
    {
        newStyle = new GUIStyle();
        newStyle.alignment = TextAnchor.MiddleCenter;
        newStyle.fontStyle = FontStyle.BoldAndItalic;
    }
    public override void OnInspectorGUI()
    {
        GenerateStyle();
        GUI.color = Color.Lerp(Color.blue, Color.white, .8f);
        Texture myTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Crowd Procedural/Editor/logo.png", typeof(Texture));
        GUILayout.Label(myTexture , newStyle);
        BuildingsField field = (BuildingsField)target;
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Size ");
		field.size = EditorGUILayout.Slider (field.size, 0, 5);
		EditorGUILayout.EndHorizontal ();
        EditorGUI.BeginDisabledGroup(PrefabSaved);
        if (GUILayout.Button("Save as Prefab"))
        {
           field.SavePrefab();
           PrefabSaved = true;
        }
        EditorGUI.EndDisabledGroup();
		if (PrefabSaved) {
			EditorGUILayout.HelpBox ("Now you can check your prefab at CombinedBuildings => Prefabs" , MessageType.None);
		}
    }
}
