using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ProceduralTrash))]
public class ProceduralTrashEditor : Editor {

    GUIStyle newStyle;
	bool IsGenerating;
	string TrashState = "Start Generate Trash";
    void OnSceneGUI()
    {
        ProceduralTrash Ptrash = (ProceduralTrash)target;
        float Distance = Ptrash.Distance;
        float radius = Ptrash.force / 10;
        Handles.color = Color.green;
        Handles.DrawWireDisc(Ptrash.transform.position, Vector3.up, Ptrash.InstantiateRange);
        Handles.DrawWireDisc(new Vector3(Ptrash.gameObject.transform.position.x, Ptrash.gameObject.transform.position.y - Distance, Ptrash.gameObject.transform.position.z), Vector3.up, Ptrash.TowardRange + radius);
        Handles.DrawLine(new Vector3(Ptrash.gameObject.transform.position.x + Ptrash.InstantiateRange, Ptrash.gameObject.transform.position.y, Ptrash.gameObject.transform.position.z), new Vector3(Ptrash.gameObject.transform.position.x + Ptrash.TowardRange + radius, Ptrash.gameObject.transform.position.y - Distance, Ptrash.gameObject.transform.position.z));
        Handles.DrawLine(new Vector3(Ptrash.gameObject.transform.position.x - Ptrash.InstantiateRange, Ptrash.gameObject.transform.position.y, Ptrash.gameObject.transform.position.z), new Vector3(Ptrash.gameObject.transform.position.x - Ptrash.TowardRange - radius, Ptrash.gameObject.transform.position.y - Distance, Ptrash.gameObject.transform.position.z));
        Handles.DrawLine(new Vector3(Ptrash.gameObject.transform.position.x, Ptrash.gameObject.transform.position.y, Ptrash.gameObject.transform.position.z + Ptrash.InstantiateRange), new Vector3(Ptrash.gameObject.transform.position.x, Ptrash.gameObject.transform.position.y - Distance, Ptrash.gameObject.transform.position.z + Ptrash.TowardRange + radius));
        Handles.DrawLine(new Vector3(Ptrash.gameObject.transform.position.x, Ptrash.gameObject.transform.position.y, Ptrash.gameObject.transform.position.z - Ptrash.InstantiateRange), new Vector3(Ptrash.gameObject.transform.position.x, Ptrash.gameObject.transform.position.y - Distance, Ptrash.gameObject.transform.position.z - Ptrash.TowardRange - radius));
    }
    void GenerateStyle()
    {
        newStyle = new GUIStyle();
        newStyle.alignment = TextAnchor.MiddleCenter;
        newStyle.fontStyle = FontStyle.BoldAndItalic;
    }
    public override void OnInspectorGUI()
    {
        GenerateStyle();
        Texture myTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Crowd Procedural/Editor/logo.png", typeof(Texture));
        GUILayout.Box(myTexture, newStyle);
        ProceduralTrash trash = (ProceduralTrash)target;
        GUI.color = Color.Lerp(Color.blue, Color.white, .8f);
        GUILayout.Label("~Generate trash using~", newStyle);
        GUILayout.Space(3);
        EditorGUILayout.BeginHorizontal();
        if (EditorGUILayout.ToggleLeft("Moving object", trash.MovingObject))
        {
            trash.MovingObject = true;
            trash.TimePass = false;
        }
        else
        {
            trash.TimePass = true;
            trash.MovingObject = false;
        }
        if (EditorGUILayout.ToggleLeft("Time pass", trash.TimePass))
        {
            trash.TimePass = true;
            trash.MovingObject = false;
        }
        else
        {
            trash.MovingObject = true;
            trash.TimePass = false;
        }
        EditorGUILayout.EndHorizontal();
        if (trash.TimePass)
        {
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Generate trash foreach");
            trash.Delay = EditorGUILayout.FloatField(trash.Delay);
            EditorGUILayout.EndHorizontal();
        }
        GUILayout.Space(5);
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("TrashModels"), true);
        serializedObject.ApplyModifiedProperties();
        trash.InstantiateRange = EditorGUILayout.Slider("Instantiate Range",trash.InstantiateRange ,0, trash.TowardRange);
        trash.TowardRange = EditorGUILayout.Slider("Toward Range", trash.TowardRange, 0, 10);
        trash.force = EditorGUILayout.Slider("Force", trash.force, 0, 150);
        EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button(TrashState)){
			IsGenerating = !IsGenerating;
			if (IsGenerating){
				TrashState = "Stop Generate Trash";
				trash.StartGenrateTrash = true;
			}else {
				TrashState = "Start Generate Trash";
				trash.StopGenrateTrash = true;
			}
		}
        EditorGUILayout.EndHorizontal();
        if (trash.StartGenrateTrash == false)
        {
            EditorGUILayout.HelpBox("Genrating is off !", MessageType.None);
        }
        else
        {
            EditorGUILayout.HelpBox("Genrating is on !", MessageType.None);
        }

    }
}
