using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TrashCombine : MonoBehaviour {
    public bool CombineSimilar;
    public bool CombineAll;
    public bool savetrash;
    //--------------------
	public bool ShowHelp;
	public bool ShowHelpTwo;
    public bool All = true;
    public bool Similar = false;
    public bool saveObject;
    //-------------------------
    public int vertLimit = 0;
    public bool save;
    List<Material> mat = new List<Material>();
    List<Material> mmm = new List<Material>();
    private List<string> MaterialNames = new List<string>();
    private List<MeshFilter> list = new List<MeshFilter>();
    private List<GameObject> child = new List<GameObject>();
    ProceduralTrash trash;
    public void CombineMesh()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            MaterialNames.Add(child.gameObject.GetComponent<MeshRenderer>().material.name);
        }
        MaterialNames = MaterialNames.Distinct().ToList();
        for (int n = 0; n < MaterialNames.Count; n++)
        {
            foreach (Transform child in this.gameObject.transform)
            {
                if (child.GetComponent<MeshRenderer>().material.name == MaterialNames[n])
                {
                    list.Add(child.gameObject.GetComponent<MeshFilter>());
                    gameObject.GetComponent<MeshRenderer>().material = child.GetComponent<MeshRenderer>().material;
                }
            }
            Quaternion oldRot = transform.rotation;
            Vector3 oldPos = transform.position;
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.tag == "trash")
                {
                    transform.rotation = Quaternion.identity;
                    transform.position = Vector3.zero;
                }
                else
                {
                    transform.rotation = gameObject.transform.rotation;
                    transform.position = gameObject.transform.position;
                }
            }
            MeshFilter[] filter = list.ToArray();
            Mesh finalMesh = new Mesh();
            CombineInstance[] combiners = new CombineInstance[filter.Length];
            for (int i = 0; i < filter.Length; i++)
            {
                if (filter[i].transform == transform)
                    continue;
                combiners[i].subMeshIndex = 0;
                combiners[i].mesh = filter[i].mesh;
                combiners[i].transform = filter[i].transform.localToWorldMatrix;
            }
            finalMesh.CombineMeshes(combiners, true);
            GetComponent<MeshFilter>().mesh = finalMesh;
            transform.rotation = oldRot;
            transform.position = oldPos;
            foreach (Transform child in this.gameObject.transform)
            {
                if (child.GetComponent<MeshRenderer>().material.name == MaterialNames[n])
                {
                    Destroy(child.gameObject);
                }
            }

            GameObject TrashList = new GameObject(); TrashList.AddComponent<MeshFilter>(); TrashList.AddComponent<MeshRenderer>(); TrashList.GetComponent<MeshFilter>().mesh = gameObject.GetComponent<MeshFilter>().mesh; TrashList.GetComponent<MeshRenderer>().material = gameObject.GetComponent<MeshRenderer>().material; TrashList.transform.parent = gameObject.transform;
            list.Clear();
            mat.Clear();
        }
    }
    public void CombineTwo()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.GetComponent<MeshRenderer>() != null)
            {
                mat.Add(child.gameObject.GetComponent<MeshRenderer>().material);
            }
        }
        gameObject.GetComponent<MeshRenderer>().materials = mat.ToArray();
        Quaternion oldRott = transform.rotation;
        Vector3 oldPoss = transform.position;
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;
        MeshFilter[] filterr = GetComponentsInChildren<MeshFilter>();
        Mesh finalMeshh = new Mesh();
        CombineInstance[] combinerss = new CombineInstance[filterr.Length];
        for (int i = 0; i < filterr.Length; i++)
        {
            if (filterr[i].transform == transform)
                continue;
            combinerss[i].subMeshIndex = 0;
            combinerss[i].mesh = filterr[i].mesh;
            combinerss[i].transform = filterr[i].transform.localToWorldMatrix;
        }
        finalMeshh.CombineMeshes(combinerss, false);
        GetComponent<MeshFilter>().mesh = finalMeshh;
        transform.rotation = oldRott;
        transform.position = oldPoss;
        foreach (Transform child in this.gameObject.transform)
        {
            Destroy(child.gameObject);
        }

    }
    public void SaveObject()
    {
        GameObject savedTrash = (GameObject)PrefabUtility.CreatePrefab("Assets/CombinedTrash/Prefabs/" + gameObject.name + " " + System.DateTime.Now.Year + System.DateTime.Now.Month + System.DateTime.Now.Day + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".prefab", gameObject);
        foreach (Transform childo in savedTrash.transform)
        {
            child.Add(childo.gameObject);
            if (childo.gameObject.GetComponent<Collider>() != null && childo.gameObject.GetComponent<Rigidbody>() != null)
            {
                DestroyImmediate(childo.gameObject.GetComponent<Collider>(),true);
                DestroyImmediate(childo.gameObject.GetComponent<Rigidbody>(),true);
            }
        }
        for (int a = 0; a < trash.mesh.Count; a++)
        {
            child[a].gameObject.GetComponent<MeshFilter>().mesh = trash.mesh[a];
        }

        trash.mesh.Clear();
        DestroyImmediate(savedTrash.GetComponent<TrashCombine>(), true);
    }
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("ProceduralTrash") != null)
        trash = GameObject.FindGameObjectWithTag("ProceduralTrash").GetComponent<ProceduralTrash>();
        foreach (Transform child in transform)
        {
            vertLimit += child.GetComponent<MeshFilter>().mesh.vertexCount;
        }
    }
    void Update()
    {
        if (save)
        {
            if (!AssetDatabase.IsValidFolder("Assets/CombinedTrash"))
            {
                AssetDatabase.CreateFolder("Assets", "CombinedTrash");
            }
            if (!AssetDatabase.IsValidFolder("Assets/CombinedTrash/Materials"))
            {
                AssetDatabase.CreateFolder("Assets/CombinedTrash", "Materials");
            }
            if (!AssetDatabase.IsValidFolder("Assets/CombinedTrash/Meshs"))
            {
                AssetDatabase.CreateFolder("Assets/CombinedTrash", "Meshs");
            }
            if (!AssetDatabase.IsValidFolder("Assets/CombinedTrash/Prefabs"))
            {
                AssetDatabase.CreateFolder("Assets/CombinedTrash", "Prefabs");
            }
            SaveObject();
            save = false;
        }
        if (CombineSimilar)
        {
            CombineMesh();
            CombineSimilar = false;
        }
        if (CombineAll)
        {
            CombineTwo();
            if (!AssetDatabase.IsValidFolder("Assets/CombinedTrash"))
            {
                AssetDatabase.CreateFolder("Assets", "CombinedTrash");
            }
            if (!AssetDatabase.IsValidFolder("Assets/CombinedTrash/Materials"))
            {
                AssetDatabase.CreateFolder("Assets/CombinedTrash", "Materials");
            }
            if (!AssetDatabase.IsValidFolder("Assets/CombinedTrash/Meshs"))
            {
                AssetDatabase.CreateFolder("Assets/CombinedTrash", "Meshs");
            }
            if (!AssetDatabase.IsValidFolder("Assets/CombinedTrash/Prefabs"))
            {
                AssetDatabase.CreateFolder("Assets/CombinedTrash", "Prefabs");
            }
            foreach (Material materia in gameObject.GetComponent<MeshRenderer>().materials)
            {
                if (AssetDatabase.LoadAssetAtPath("Assets/CombinedTrash/Materials/" + materia.name + "Material" + ".mat", typeof(Material)) == null)
                {
                    AssetDatabase.CreateAsset(materia, "Assets/CombinedTrash/Materials/" + materia.name + "Material" + ".mat");
                    mmm.Add(materia);

                }
                else if (AssetDatabase.LoadAssetAtPath("Assets/CombinedTrash/Materials/" + materia.name + "Material" + ".mat", typeof(Material)) != null)
                {
                    mmm.Add(AssetDatabase.LoadAssetAtPath("Assets/CombinedTrash/Materials/" + materia.name + "Material" + ".mat", typeof(Material)) as Material);
                    gameObject.GetComponent<MeshRenderer>().materials = mmm.ToArray();
                }
            }
            AssetDatabase.CreateAsset(gameObject.GetComponent<MeshFilter>().mesh, "Assets/CombinedTrash/Meshs/" + gameObject.name + " " + System.DateTime.Now.Year + System.DateTime.Now.Month + System.DateTime.Now.Day + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + System.DateTime.Now.Millisecond + "Mesh.obj");
            CombineAll = false;
            }
        }
    void OnApplicationQuit()
    {
        if (All && Similar && AssetDatabase.IsValidFolder("Assets/CombinedTrash/Prefabs"))
        {
            GameObject prefab = PrefabUtility.CreatePrefab("Assets/CombinedTrash/Prefabs/" + gameObject.name + " " + System.DateTime.Now.Year + System.DateTime.Now.Month + System.DateTime.Now.Day + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + System.DateTime.Now.Millisecond + ".prefab", gameObject) as GameObject;
            DestroyImmediate(prefab.GetComponent<TrashCombine>(), true);
        }
    }
	public void StartCounting(){
		ShowHelp = true;
		StartCoroutine (SecondCombine ());
	}
	IEnumerator SecondCombine(){
		yield return new WaitForSeconds (0.5f);
		CombineAll = true;
		All = true;
		ShowHelp = false;
		ShowHelpTwo = true;
	}

}
