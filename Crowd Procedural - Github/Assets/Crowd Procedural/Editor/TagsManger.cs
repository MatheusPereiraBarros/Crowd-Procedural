using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
public class TagsManger : Editor
{

    static void SaveTag(string tag)
    {
        // Open tag manager
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        // Adding a Tag
        string s = tag;

        // First check if it is not already present
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(s)) { found = true; break; }
        }

        // if not found, add it
        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
            n.stringValue = s;
        }

        // and to save the changes
        tagManager.ApplyModifiedProperties();
    }
    [MenuItem("Tools/Import Crowd Tags" , false , 1)]
    static void SaveTags()
    {
        SaveTag("point");
        SaveTag("Building");
        SaveTag("Field");
        SaveTag("PointGenrated");
        SaveTag("BuildingGenrated");
        SaveTag("ProceduralBuildings");
        SaveTag("ProceduralTrash");
        SaveTag("CornerSideBuilding");
        SaveTag("CornerSideBuildingGenrated");
    }
	[MenuItem("Tools/Procedural Buildings")]
	static void CreateProceduralBuildingObject(){
		if (!GameObject.FindObjectOfType <ProceduralBuildings>()) {
			GameObject ProceduralBuildingCast = new GameObject ("Procedural Buildings");
			ProceduralBuildingCast.AddComponent<ProceduralBuildings> ();
		}else {
			Debug.LogWarningFormat ("Procedural Buildings is already exist");
		}
	}
	[MenuItem("Tools/Procedural Trash")]
	static void CreatProceduralTrashObject(){
		if (!GameObject.FindObjectOfType <ProceduralTrash> ()) {
			GameObject ProceduralTrashCast = new GameObject ("Procedural Trash");
			ProceduralTrashCast.AddComponent<ProceduralTrash> ();
		}else {
			Debug.LogWarningFormat ("Procedural Trash is already exist");
		}
	}
}
