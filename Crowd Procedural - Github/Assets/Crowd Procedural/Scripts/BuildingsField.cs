using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class BuildingsField : MonoBehaviour {

	public float size = 0;
	void Update(){
		gameObject.transform.localScale = Vector3.one + new Vector3(size, size , size);
	}
       public void SavePrefab(){
			foreach (Transform child in transform)
			{
				if (child.gameObject.name == ("poinnt"))
				{
					Destroy(child.gameObject);
				}
			}
           if (!AssetDatabase.IsValidFolder("Assets/CombinedBuildings"))
           {
               AssetDatabase.CreateFolder("Assets", "CombinedBuildings");
           }
           if (!AssetDatabase.IsValidFolder("Assets/CombinedBuildings/Prefabs"))
           {
               AssetDatabase.CreateFolder("Assets/CombinedBuildings", "Prefabs");
           }
           GameObject prefab = PrefabUtility.CreatePrefab("Assets/CombinedBuildings/Prefabs/" + gameObject.name + " ID" + System.DateTime.Now.Year + System.DateTime.Now.Month + System.DateTime.Now.Day + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + System.DateTime.Now.Millisecond + ".prefab", gameObject) as GameObject;
           DestroyImmediate(prefab.GetComponent<BuildingsField>(), true);
        
       }
}