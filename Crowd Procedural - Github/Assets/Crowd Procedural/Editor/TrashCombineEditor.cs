using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(TrashCombine))]
public class TrashCombineEditor : Editor {

	public bool ShowHelpThree;
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
        TrashCombine trash = (TrashCombine)target;
        EditorGUI.BeginDisabledGroup(trash.Similar);
        if (GUILayout.Button("Combine Trash"))
        {
            trash.CombineSimilar = true;
            trash.Similar = true;
            trash.All = false;
			trash.StartCounting ();
        }
        EditorGUI.EndDisabledGroup();
		if (trash.ShowHelp) {
			EditorGUILayout.HelpBox ("Loading...", MessageType.Warning);
		}
		if (trash.ShowHelpTwo) {
			EditorGUILayout.HelpBox ("Now you can get your prefab after getting out the play mode at CombinedTrash => Prefabs and your trash mesh at CombinedTrash => Meshs", MessageType.None);
		}
		if (trash.vertLimit > 65535)
		{
			trash.Similar = true; trash.All = true;
			EditorGUILayout.HelpBox("Sorry you can't combine meshes because vertices's number is high , please delete this object and try again , Or you can just save trash as prefab without combine", MessageType.Error);
		}
		if (trash.vertLimit < 65535) {
			GUILayout.Label ("~ Or ~", newStyle); 
		}
		EditorGUI.BeginDisabledGroup(trash.saveObject);
		if (GUILayout.Button("Save Trash as prefab"))
		{
			trash.save = true;
			trash.saveObject = true;
			ShowHelpThree = true;
		}
		EditorGUI.EndDisabledGroup();
		if (ShowHelpThree) {
			EditorGUILayout.HelpBox ("Now you can get your prefab at CombinedTrash => Prefabs", MessageType.None);
		}
    }
}

 

