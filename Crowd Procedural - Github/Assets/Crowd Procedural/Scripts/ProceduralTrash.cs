using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralTrash : MonoBehaviour
{
    public float TowardRange;
    public float InstantiateRange;
    [HideInInspector]
    public float force = 100;
    private float Rate = 50;
    [HideInInspector]
    public bool StopGenrateTrash;
    [HideInInspector]
    public bool StartGenrateTrash;
    public GameObject[] TrashModels;
    int RandomTrash;
    Vector3 LastPos;
    Vector3 rot;
    GameObject Trash;
    int i = 0;
    List<GameObject> instantiated = new List<GameObject>();
    [HideInInspector]
    public List<Mesh> mesh = new List<Mesh>();
    public float Distance;
    public bool MovingObject = true, TimePass;
    public float Delay;

    void Start()
    {
        gameObject.tag = "ProceduralTrash";
        foreach (GameObject trash in TrashModels)
        {
            if (trash.GetComponent<Collider>() != null)
            {
                Destroy(trash.GetComponent<Collider>());
                trash.AddComponent<BoxCollider>();
            }
            if (trash.GetComponent<BoxCollider>() == null)
            {
                trash.AddComponent<BoxCollider>();
            }
            if (trash.GetComponent<Rigidbody>() == null)
            {
                trash.AddComponent<Rigidbody>();
                trash.GetComponent<Rigidbody>().mass = 10;
                trash.GetComponent<Rigidbody>().drag = 0;
                trash.GetComponent<Rigidbody>().angularDrag = 5;
				trash.GetComponent<Rigidbody> ().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
        }
        LastPos = gameObject.transform.position;
    }
    void Update ()
    {
        Distance = force / 10 + 3;
        if (StartGenrateTrash && MovingObject)
        {
             if (gameObject.transform.position != LastPos)
             {
                 InvokeRepeating("Genrate", .1f, Rate);
             }
             else
             {
                 CancelInvoke("Genrate");
             }
             LastPos = gameObject.transform.position;
        }
        else if (StartGenrateTrash && TimePass)
        {
             StartCoroutine("GenerateTrash");
        }
        if (StopGenrateTrash)
        {
                if (TimePass)
                {
                    StopCoroutine("GenerateTrash");
                    if (instantiated.Count > 0)
                    {
                        i++;
                        Trash = new GameObject(); Trash.AddComponent<TrashCombine>(); Trash.name = ("Trash" + i);
                        Trash.transform.position = instantiated[0].gameObject.transform.position;
                        foreach (GameObject trash in instantiated)
                        {
                            mesh.Add(trash.GetComponent<MeshFilter>().sharedMesh);
                            trash.transform.parent = Trash.transform;
                        }
                        instantiated.Clear();
                    }
                    StartGenrateTrash = false;
                    StopGenrateTrash = false;
                }
                else if (!TimePass)
                {
                    if (instantiated.Count > 0)
                    {
                        i++;
                        Trash = new GameObject(); 
                        Trash.AddComponent<TrashCombine>();
                        Trash.name = ("Trash" + i);
                        if (Trash != null && instantiated[0] != null)
                        Trash.transform.position = instantiated[0].gameObject.transform.position;
                        foreach (GameObject trash in instantiated)
                        {
                            mesh.Add(trash.GetComponent<MeshFilter>().sharedMesh);
                            trash.transform.parent = Trash.transform;
                        }
                        instantiated.Clear();
                    }
                    StartGenrateTrash = false;
                    StopGenrateTrash = false;
                }
        }       
        
    }
    void Genrate()
     {
             RandomTrash = Random.Range(0, TrashModels.Length);
             rot = new Vector3(Random.Range(-TowardRange, TowardRange), gameObject.transform.position.y - Distance,Random.Range(-TowardRange, TowardRange));
             if (TrashModels.Length > 0 && TrashModels[RandomTrash] != null)
             {
                 GameObject TrashCast = (GameObject)Instantiate(TrashModels[RandomTrash], new Vector3(gameObject.transform.position.x + Random.Range(-InstantiateRange, InstantiateRange), gameObject.transform.position.y - 1, gameObject.transform.position.z + Random.Range(-InstantiateRange, InstantiateRange)), Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0));
                 TrashCast.GetComponent<Collider>().enabled = false;
                 TrashCast.GetComponent<Collider>().isTrigger = true;
                 TrashCast.AddComponent<Trash>();
                 instantiated.Add(TrashCast);
                 TrashCast.GetComponent<Rigidbody>().AddRelativeForce(rot * -force, ForceMode.Impulse);
             }
            
     }
    IEnumerator GenerateTrash()
    {
        yield return new WaitForSeconds(Delay);
        RandomTrash = Random.Range(0, TrashModels.Length);
        rot = new Vector3(Random.Range(-TowardRange, TowardRange), gameObject.transform.position.y - Distance, Random.Range(-TowardRange, TowardRange));
        if (TrashModels.Length > 0 && TrashModels[RandomTrash] != null)
        {
            GameObject TrashCast = (GameObject)Instantiate(TrashModels[RandomTrash], new Vector3(gameObject.transform.position.x + Random.Range(-InstantiateRange, InstantiateRange), gameObject.transform.position.y - 1, gameObject.transform.position.z + Random.Range(-InstantiateRange, InstantiateRange)), Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0));
            TrashCast.GetComponent<Collider>().enabled = false;
            TrashCast.GetComponent<Collider>().isTrigger = true;
            TrashCast.AddComponent<Trash>();
            instantiated.Add(TrashCast);
            TrashCast.GetComponent<Rigidbody>().AddRelativeForce(rot * -force , ForceMode.Impulse);
        }
        StartCoroutine("GenerateTrash");
        StopCoroutine("GenerateTrash");
    }
}