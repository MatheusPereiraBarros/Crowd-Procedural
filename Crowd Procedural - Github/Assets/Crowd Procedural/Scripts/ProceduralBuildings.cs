using UnityEngine;
using System.Collections;

public enum BuildingFace {Left , Right};
public enum Direction {forward , backward , right , left , none};
public class ProceduralBuildings : MonoBehaviour
{
	public BuildingFace BuildingFace;
	public Direction Direction;
    private GameObject BuildingTools;
    private GameObject BuildingField;
    private GameObject StartPoint;
    private GameObject EndPoint;
    private GameObject point;
    //++++++++++++++++++++++++++++
    public GameObject[] Cornerbuildings;
    public GameObject[] Buildings;
    GameObject[] PointsGenrated;
    public bool placeable , BuildingsNotNull;
    public GameObject[] points;
    public GameObject[] BuildingsInstantiated;
    private GameObject[] CornerBuildingsInstantiated;
    private GameObject[] field;
    public bool BuilderInBuildingField = true;
    int RandomCompleteBuildings;
    int RandomCompletedCornerBuildings;
	int LastRandom;
    float distPoint;
    float BuldingSize;
    float WidthOfGonnaBuild;
    float LengthOfGonnaBuild;
    GameObject GonnaBuild;
    GameObject BuildingFieldCastingObject;
    Vector3 PosToBuild;
    //________________________________________
	public Transform PointTransform;
	public string CurveBuildingState = "Start Curve Building";
	public string BuildingState = "Start Building";
	public bool IsBuilding;
    public bool GenrateNewField = false;
    public bool ResetBuilderPosition;
    public bool StopBuilding;
    public bool StartBuilding;
    public bool PlaceCornerSideBuilding;
	public bool BuildingCurve;
	public float CurveBuildingAngle , StoredCurveBuilding;
    private bool forward;
    private bool backward;
    private bool right;
    private bool left;
    bool once = false;
	Vector3 LastPosition;
	Vector3 stored;
	Vector3[] GetBuildingVertices(GameObject Building) {
		Vector3[] vertices = new Vector3[8];
		Matrix4x4 matrix = Building.transform.localToWorldMatrix;
		Quaternion BuildingRotaion = Building.transform.rotation;
		Building.transform.rotation = Quaternion.identity;
		Vector3 extents = Building.GetComponent<BoxCollider> ().bounds.extents;
		Vector3 center = Building.GetComponent<BoxCollider> ().center;
		vertices [0] = matrix.MultiplyPoint3x4 (extents);
		vertices [1] = matrix.MultiplyPoint3x4(new Vector3(-extents.x, extents.y, extents.z) + center);
		vertices [2] = matrix.MultiplyPoint3x4(new Vector3(extents.x, extents.y, -extents.z) + center);
		vertices [3] = matrix.MultiplyPoint3x4(new Vector3(-extents.x, extents.y, -extents.z) + center);
		vertices [4] = matrix.MultiplyPoint3x4(new Vector3(extents.x, -extents.y, extents.z) + center);
		vertices [5] = matrix.MultiplyPoint3x4(new Vector3(-extents.x, -extents.y, extents.z) + center);
		vertices [6] = matrix.MultiplyPoint3x4(new Vector3(extents.x, -extents.y, -extents.z) + center);
		vertices [7] = matrix.MultiplyPoint3x4(new Vector3(-extents.x, -extents.y, -extents.z) + center);

		Building.transform.rotation = BuildingRotaion;
		return vertices;
	}
	public void GoTo(Transform point , string Direction){
		foreach (Transform child in point){
			if (child.tag == "PointGenrated" && Direction == "right") {
				if (Direction == "right")
					gameObject.transform.position = child.position;
			} else if (child.name == "NextSidePoint" && Direction == "left") {
				gameObject.transform.position = child.position;
			}
		}
	}
	public void PlaceCurveBuilding(float angle){
		if (BuildingCurve) {

			LastRandom = RandomCompleteBuildings;
			RandomCompleteBuildings = Random.Range(0, Buildings.Length);
			#region Prevent Repeating
			if (Buildings.Length > 1) {
				while (RandomCompleteBuildings == LastRandom) {
					RandomCompleteBuildings = Random.Range (0, Buildings.Length);
				}
			}
			#endregion
			StoredCurveBuilding += CurveBuildingAngle;
			angle = StoredCurveBuilding;
			if (right) {
				// right
				GonnaBuild = (GameObject)Instantiate(Buildings[RandomCompleteBuildings]);
				if (BuildingFace == BuildingFace.Right) {
					PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z - GonnaBuild.GetComponent<BoxCollider> ().center.z 
						                    , points [points.Length - 1].transform.position.y - GetBuildingVertices(GonnaBuild)[7].y
									        , points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x + GonnaBuild.GetComponent<BoxCollider> ().center.x);
					GonnaBuild.transform.position = PosToBuild;
					GonnaBuild.transform.localEulerAngles = new Vector3 (0, 90, 0);
				}else if (BuildingFace == BuildingFace.Left){
					PosToBuild = new Vector3(points[points.Length - 1].transform.position.x + GonnaBuild.GetComponent<BoxCollider>().bounds.extents.z + GonnaBuild.GetComponent<BoxCollider>().center.z
						                   , points[points.Length - 1].transform.position.y - GetBuildingVertices(GonnaBuild)[7].y
						                   , points[points.Length - 1].transform.position.z - GonnaBuild.GetComponent<BoxCollider>().bounds.extents.x - GonnaBuild.GetComponent<BoxCollider>().center.x);
					GonnaBuild.transform.position = PosToBuild;
					GonnaBuild.transform.localEulerAngles = new Vector3 (0, 270, 0);
				}
				GonnaBuild.tag = ("Building");
				for (int i = 0; i < BuildingsInstantiated.Length +2; i++)
				{
					GonnaBuild.name = ("Building" + i);
				}
				if (BuildingFace == BuildingFace.Right) {
					GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[4], Quaternion.identity);
					PointInstantiated.transform.parent = GonnaBuild.transform;
					PointInstantiated.name = "Point";
					GameObject NextSidePoint = new GameObject ("NextSidePoint");
					NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [6];
					NextSidePoint.transform.parent = GonnaBuild.transform;
					GonnaBuild.transform.RotateAround (GetBuildingVertices(GonnaBuild)[6], Vector3.up, angle);
				} else if (BuildingFace == BuildingFace.Left) {
					GameObject PointInstantiated = (GameObject)Instantiate(point, GetBuildingVertices(GonnaBuild)[6], Quaternion.identity);
					PointInstantiated.transform.parent = GonnaBuild.transform;
					PointInstantiated.name = "Point";
					GameObject NextSidePoint = new GameObject ("NextSidePoint");
					NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [4];
					NextSidePoint.transform.parent = GonnaBuild.transform;
					GonnaBuild.transform.RotateAround (GetBuildingVertices(GonnaBuild)[4], Vector3.up, angle);
				}
			} 
			else if (left) {
				// left
				GonnaBuild = (GameObject)Instantiate(Buildings[RandomCompleteBuildings]);
				if (BuildingFace == BuildingFace.Right) {
					PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z + GonnaBuild.GetComponent<BoxCollider> ().center.z
					                   , points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
									   , points [points.Length - 1].transform.position.z - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x - GonnaBuild.GetComponent<BoxCollider> ().center.x);
					GonnaBuild.transform.position = PosToBuild;
					GonnaBuild.transform.localEulerAngles = new Vector3 (0, 270, 0);
				}else if (BuildingFace == BuildingFace.Left){
					PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z + GonnaBuild.GetComponent<BoxCollider> ().center.z
						, points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
						, points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x + GonnaBuild.GetComponent<BoxCollider> ().center.x);
					GonnaBuild.transform.position = PosToBuild;
					GonnaBuild.transform.localEulerAngles = new Vector3 (0, 90, 0);
				}
				GonnaBuild.tag = ("Building");
				for (int i = 0; i < BuildingsInstantiated.Length +2; i++)
				{
					GonnaBuild.name = ("Building" + i);
				}
				if (BuildingFace == BuildingFace.Right) {
					GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices (GonnaBuild) [4], Quaternion.identity);
					PointInstantiated.transform.parent = GonnaBuild.transform;
					PointInstantiated.name = "Point";
					GameObject NextSidePoint = new GameObject ("NextSidePoint");
					NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [6];
					NextSidePoint.transform.parent = GonnaBuild.transform;
					GonnaBuild.transform.RotateAround (GetBuildingVertices (GonnaBuild) [6], Vector3.up, angle);
				}else if (BuildingFace == BuildingFace.Left) {
					GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices (GonnaBuild) [6], Quaternion.identity);
					PointInstantiated.transform.parent = GonnaBuild.transform;
					PointInstantiated.name = "Point";
					GameObject NextSidePoint = new GameObject ("NextSidePoint");
					NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [4];
					NextSidePoint.transform.parent = GonnaBuild.transform;
					GonnaBuild.transform.RotateAround (GetBuildingVertices (GonnaBuild) [4], Vector3.up, angle);
				}
			} 
			else if (backward) {
				// back
				GonnaBuild = (GameObject)Instantiate(Buildings[RandomCompleteBuildings]);
				if (BuildingFace == BuildingFace.Right) {
					PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x + GonnaBuild.GetComponent<BoxCollider> ().center.x 
					                          , points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
											  , points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().center.z - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z);
					GonnaBuild.transform.position = PosToBuild;
					GonnaBuild.transform.localRotation = Quaternion.Euler (0, 180, 0);
				} else if (BuildingFace == BuildingFace.Left){
					PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x - GonnaBuild.GetComponent<BoxCollider> ().center.x 
						                      , points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
						                      , points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().center.z - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z);
					GonnaBuild.transform.position = PosToBuild;
					GonnaBuild.transform.localRotation = Quaternion.Euler (0, 0, 0);
				}
				GonnaBuild.tag = ("Building");
				for (int i = 0; i < BuildingsInstantiated.Length +2; i++)
				{
					GonnaBuild.name = ("Building" + i);
				}
				if (BuildingFace == BuildingFace.Right) {
					GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[4], Quaternion.identity);
					PointInstantiated.transform.parent = GonnaBuild.transform;
					PointInstantiated.name = "Point";
					GameObject NextSidePoint = new GameObject ("NextSidePoint");
					NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [6];
					NextSidePoint.transform.parent = GonnaBuild.transform;
					GonnaBuild.transform.RotateAround (GetBuildingVertices(GonnaBuild)[6], Vector3.up, angle);
				}else if (BuildingFace == BuildingFace.Left){
					GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[6], Quaternion.identity);
					PointInstantiated.transform.parent = GonnaBuild.transform;
					PointInstantiated.name = "Point";
					GameObject NextSidePoint = new GameObject ("NextSidePoint");
					NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [4];
					NextSidePoint.transform.parent = GonnaBuild.transform;
					GonnaBuild.transform.RotateAround (GetBuildingVertices(GonnaBuild)[4], Vector3.up, angle);
				}
			} 
			else if (forward) {
				// forward
				GonnaBuild = (GameObject)Instantiate (Buildings [RandomCompleteBuildings]);
				if (BuildingFace == BuildingFace.Right) {
					PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x - GonnaBuild.GetComponent<BoxCollider> ().center.x
						                    , points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
										    , points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z - GonnaBuild.GetComponent<BoxCollider> ().center.z);
				}else if (BuildingFace == BuildingFace.Left){
					PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x + GonnaBuild.GetComponent<BoxCollider> ().center.x
						                    , points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
										    , points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z - GonnaBuild.GetComponent<BoxCollider> ().center.z);
				}   
				GonnaBuild.transform.position = PosToBuild;
				if (BuildingFace == BuildingFace.Left) {
					GonnaBuild.transform.localRotation = Quaternion.Euler(0, 180, 0);
				}
				GonnaBuild.tag = ("Building");
				for (int i = 0; i < BuildingsInstantiated.Length + 2; ++i) {
					GonnaBuild.name = ("Building" + i);
				}
				if (BuildingFace == BuildingFace.Right) {
					GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[4], Quaternion.identity);
					PointInstantiated.transform.parent = GonnaBuild.transform;
					PointInstantiated.name = "Point";
					GameObject NextSidePoint = new GameObject ("NextSidePoint");
					NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [6];
					NextSidePoint.transform.parent = GonnaBuild.transform;
					GonnaBuild.transform.RotateAround (GetBuildingVertices (GonnaBuild) [6], Vector3.up, angle);
				} else if (BuildingFace == BuildingFace.Left) {
					GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[6], Quaternion.identity);
					PointInstantiated.transform.parent = GonnaBuild.transform;
					PointInstantiated.name = "Point";
					GameObject NextSidePoint = new GameObject ("NextSidePoint");
					NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [4];
					NextSidePoint.transform.parent = GonnaBuild.transform;
					GonnaBuild.transform.RotateAround (GetBuildingVertices (GonnaBuild) [4], Vector3.up, angle);
				}
			}
		}
	}
	public void Reset(){
		StoredCurveBuilding = 0;
		if (StartPoint != null) {
			StartPoint.transform.position = points [points.Length - 1].transform.position;
			gameObject.transform.position = StartPoint.transform.position;
		}
		forward = false;
		backward = false;
		right = false;
		left = false;
	}
	void BlockMovment(){
		if (Direction == Direction.forward) {
			if (LastPosition.z <= gameObject.transform.position.z) {
				stored = gameObject.transform.position;
				if (stored.z >= gameObject.transform.position.z) {
					LastPosition = gameObject.transform.position;
				}
			}
			gameObject.transform.position = stored;
		} else if (Direction == Direction.backward) {
			if (LastPosition.z >= gameObject.transform.position.z) {
				stored = gameObject.transform.position;
				if (stored.z <= gameObject.transform.position.z) {
					LastPosition = gameObject.transform.position;
				}
			}
			gameObject.transform.position = stored;
		} else if (Direction == Direction.left) {
			if (LastPosition.x >= gameObject.transform.position.x) {
				stored = gameObject.transform.position;
				if (stored.x <= gameObject.transform.position.x) {
					LastPosition = gameObject.transform.position;
				}
			}
			gameObject.transform.position = stored;
		} else if (Direction == Direction.right) {
			if (LastPosition.x <= gameObject.transform.position.x) {
				stored = gameObject.transform.position;
				if (stored.x >= gameObject.transform.position.x) {
					LastPosition = gameObject.transform.position;
				}
			}
			gameObject.transform.position = stored;
		}
		stored = gameObject.transform.position;
		LastPosition = gameObject.transform.position;
	}
    void PlaceSidecornerBuilding()
    {
        // Place Corner Building Forward
		if (forward) {
			GonnaBuild = (GameObject)Instantiate (Cornerbuildings [RandomCompletedCornerBuildings]);
			if (BuildingFace == BuildingFace.Right) {
				PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z - GonnaBuild.GetComponent<BoxCollider> ().center.z ,
					                      points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y ,
                                          points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x + GonnaBuild.GetComponent<BoxCollider> ().center.x);
				GonnaBuild.transform.position = PosToBuild;
				GonnaBuild.transform.localRotation = Quaternion.Euler (0, 90, 0);
			} else if (BuildingFace == BuildingFace.Left) {
				PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z + GonnaBuild.GetComponent<BoxCollider> ().center.z ,
					                      points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y ,
									      points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x + GonnaBuild.GetComponent<BoxCollider> ().center.x);
				GonnaBuild.transform.position = PosToBuild;
				GonnaBuild.transform.localRotation = Quaternion.Euler (0, 0, 0);
			}
			GonnaBuild.tag = ("CornerSideBuilding");
			for (int i = 0; i < CornerBuildingsInstantiated.Length + 2; ++i) {
				GonnaBuild.name = ("Corner Side Building" + i);
			}
			if (BuildingFace == BuildingFace.Right) {
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[7] , new Quaternion (0, 0, 0, 0));
				PointInstantiated.transform.parent = GonnaBuild.transform;   
				PointInstantiated.name = "Point";
			} else if (BuildingFace == BuildingFace.Left) {
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[4] , new Quaternion (0, 0, 0, 0));
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";
			}
		}
        // Place Corner Building Backward
		else if (backward) {
			GonnaBuild = (GameObject)Instantiate (Cornerbuildings [RandomCompletedCornerBuildings]);
			if (BuildingFace == BuildingFace.Right) {

				PosToBuild = new Vector3(points [points.Length - 1].transform.position.x + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z - GonnaBuild.GetComponent<BoxCollider> ().center.z  ,
										 points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y ,
					                     points [points.Length - 1].transform.position.z - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x - GonnaBuild.GetComponent<BoxCollider> ().center.x);
				GonnaBuild.transform.position = PosToBuild;
				GonnaBuild.transform.localEulerAngles = new Vector3 (0, 270, 0);
			} else if (BuildingFace == BuildingFace.Left) {
				PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x + GonnaBuild.GetComponent<BoxCollider> ().center.x ,
					                      points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y ,
					                      points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().center.z - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z);
				GonnaBuild.transform.position = PosToBuild;
				GonnaBuild.transform.localEulerAngles = new Vector3 (0, 180, 0);
			}
			GonnaBuild.tag = ("CornerSideBuilding");
			for (int i = 0; i < CornerBuildingsInstantiated.Length + 2; ++i) {
				GonnaBuild.name = ("Corner Side Building" + i);
			}
			if (BuildingFace == BuildingFace.Right) {
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[7] , new Quaternion (0, 0, 0, 0));
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";
			}else if (BuildingFace == BuildingFace.Left){
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[4] , new Quaternion (0, 0, 0, 0));
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";
			}
		}
        // Place Corner Building Left
		else if (left) {
			GonnaBuild = (GameObject)Instantiate (Cornerbuildings [RandomCompletedCornerBuildings]);
			if (BuildingFace == BuildingFace.Right) {
				PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x - GonnaBuild.GetComponent<BoxCollider> ().center.x, 
					points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y,
					points [points.Length - 1].transform.position.z - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z - GonnaBuild.GetComponent<BoxCollider> ().center.z);
				GonnaBuild.transform.position = PosToBuild;
				GonnaBuild.transform.localEulerAngles = new Vector3 (0, 0, 0);
			}else if (BuildingFace == BuildingFace.Left){
				PosToBuild = new Vector3(points [points.Length - 1].transform.position.x - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z - GonnaBuild.GetComponent<BoxCollider> ().center.z  ,
					                     points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y ,
					                     points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x - GonnaBuild.GetComponent<BoxCollider> ().center.x);
				GonnaBuild.transform.position = PosToBuild;
				GonnaBuild.transform.localEulerAngles = new Vector3 (0, 270, 0);
			}
			GonnaBuild.tag = ("CornerSideBuilding");
			for (int i = 0; i < CornerBuildingsInstantiated.Length + 2; ++i) {
				GonnaBuild.name = ("Corner Side Building" + i);
			}
			if (BuildingFace == BuildingFace.Right) {
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[7], new Quaternion (0, 0, 0, 0));
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";
			}else if (BuildingFace == BuildingFace.Left){
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[4], new Quaternion (0, 0, 0, 0));
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";
			}
		}
        // Place Corner Building Right
		else if (right) {
			GonnaBuild = (GameObject)Instantiate (Cornerbuildings [RandomCompletedCornerBuildings]);
			if (BuildingFace == BuildingFace.Right) {
				PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x + GonnaBuild.GetComponent<BoxCollider> ().center.x
					                    , points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
                                        , points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z + GonnaBuild.GetComponent<BoxCollider> ().center.z);
				GonnaBuild.transform.position = PosToBuild;
				GonnaBuild.transform.localRotation = Quaternion.Euler (0, 180, 0);
			} else if (BuildingFace == BuildingFace.Left) {
				PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z - GonnaBuild.GetComponent<BoxCollider> ().center.z
					                    , points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
						                , points [points.Length - 1].transform.position.z - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x + GonnaBuild.GetComponent<BoxCollider> ().center.x);
				GonnaBuild.transform.position = PosToBuild;
				GonnaBuild.transform.localRotation = Quaternion.Euler (0, 90, 0);
			}
			GonnaBuild.tag = ("CornerSideBuilding");
			for (int i = 0; i < CornerBuildingsInstantiated.Length + 2; ++i) {
				GonnaBuild.name = ("Corner Side Building" + i);
			}
			if (BuildingFace == BuildingFace.Right) {
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[7], Quaternion.Euler (new Vector3 (0, 0, 0)));
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";
			} else if (BuildingFace == BuildingFace.Left) {
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[4], new Quaternion (0, 0, 0, 0));
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";

			}
		}

        once = true;
        GenrateNewField = true;
    }
    void NewBuildingField()
    {
        if (BuildingsInstantiated.Length > 0)
        {
			DirectionOfBuilding ();
            BuildingFieldCastingObject = new GameObject();
            StartPoint.transform.position = points[points.Length - 1].transform.position;
            gameObject.transform.position = StartPoint.transform.position;
            if (once)
            {
                BuildingFieldCastingObject.tag = ("Field");
                BuildingFieldCastingObject.AddComponent<BuildingsField>();
				if (right)
                {
					if (BuildingFace == BuildingFace.Right){
						BuildingFieldCastingObject.transform.position = GetBuildingVertices (BuildingsInstantiated [0]) [6];
					} else if (BuildingFace == BuildingFace.Left){
						BuildingFieldCastingObject.transform.position = GetBuildingVertices (BuildingsInstantiated [0]) [4];
					}
                }
				else if (left)
                {
					if (BuildingFace == BuildingFace.Right){
						BuildingFieldCastingObject.transform.position = GetBuildingVertices (BuildingsInstantiated [0]) [6];
					} else if (BuildingFace == BuildingFace.Left){
						BuildingFieldCastingObject.transform.position = GetBuildingVertices (BuildingsInstantiated [0]) [4];
					}
                }
				else if (backward)
                {
					if (BuildingFace == BuildingFace.Right){
						BuildingFieldCastingObject.transform.position = GetBuildingVertices (BuildingsInstantiated [0]) [6];
					} else if (BuildingFace == BuildingFace.Left){
						BuildingFieldCastingObject.transform.position = GetBuildingVertices (BuildingsInstantiated [0]) [4];
					}
                }
				else if (forward)
                {
					if (BuildingFace == BuildingFace.Right){
						BuildingFieldCastingObject.transform.position = GetBuildingVertices (BuildingsInstantiated [0]) [6];
					} else if (BuildingFace == BuildingFace.Left){
						BuildingFieldCastingObject.transform.position = GetBuildingVertices (BuildingsInstantiated [0]) [4];
					}
                }
                BuildingFieldCastingObject.name = ("Building Field " + (field.Length + 1));
                Destroy(point);
                point = Instantiate(point); point.transform.position = gameObject.transform.position; point.transform.parent = BuildingTools.transform; point.tag = ("point");
                once = false;
            }
            if (GameObject.Find("LastPoint") != null)
            {
                GameObject.Find("LastPoint").name = ("poinnt");
            }
            points[points.Length - 1].name = ("LastPoint");
            foreach (GameObject pont in points)
            {
                pont.tag = ("PointGenrated");
            }
            foreach (GameObject building in CornerBuildingsInstantiated)
            {
                if (building.tag == ("CornerSideBuilding"))
                {
                    building.tag = ("CornerSideBuildingGenrated");
                    building.transform.parent = BuildingFieldCastingObject.transform;
                    Destroy(building.GetComponent<BoxCollider>());
                }
            }
            foreach (GameObject bldInstantiated in BuildingsInstantiated)
            {
                if (bldInstantiated.tag == ("Building"))
                {
                    bldInstantiated.tag = ("BuildingGenrated");
                    bldInstantiated.transform.parent = BuildingFieldCastingObject.transform;
                    Destroy(bldInstantiated.GetComponent<BoxCollider>());
                }
            }



            forward = false;
            backward = false;
            right = false;
            left = false;
            if (forward || backward || left || right)
            {
                forward = false;
                backward = false;
                right = false;
                left = false;
            }

		}

    }
    void DirectionOfBuilding()
    {
        Vector3 StartPointPosition = StartPoint.transform.position - transform.transform.position;
		if (Vector3.Dot(transform.TransformDirection(Vector3.right), StartPointPosition) == 0 && Vector3.Dot(transform.TransformDirection(Vector3.forward), StartPointPosition) == 0  ){
			Direction = Direction.none;
		}
        if (Vector3.Dot(transform.TransformDirection(Vector3.right), StartPointPosition) > 0)
        {
            left = true;
            right = false;
			Direction = Direction.left;
        }
        if (Vector3.Dot(transform.TransformDirection(Vector3.right), StartPointPosition) < 0)
        {
            left = false;
            right = true;
			Direction = Direction.right;
        }

        if (Vector3.Dot(transform.TransformDirection(Vector3.forward), StartPointPosition) < 0)
        {
            forward = true;
            backward = false;
			Direction = Direction.forward;

        }
        if (Vector3.Dot(transform.TransformDirection(Vector3.forward), StartPointPosition) > 0)
        {
            forward = false;
            backward = true;
			Direction = Direction.backward;
        }

    }
    void BuildForward(bool acive)
    {
        if (acive)
        {
            GonnaBuild = (GameObject)Instantiate(Buildings[RandomCompleteBuildings]);
			if (BuildingFace == BuildingFace.Right) {
				PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x - GonnaBuild.GetComponent<BoxCollider> ().center.x
					               , points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
                                   , points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z - GonnaBuild.GetComponent<BoxCollider> ().center.z);
			}else if (BuildingFace == BuildingFace.Left) {
				PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x + GonnaBuild.GetComponent<BoxCollider> ().center.x
					               , points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
								   , points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z - GonnaBuild.GetComponent<BoxCollider> ().center.z);
			} 
            GonnaBuild.transform.position = PosToBuild;
			if (BuildingFace == BuildingFace.Left){
				GonnaBuild.transform.localRotation = Quaternion.Euler(0, 180, 0);
			}
            GonnaBuild.tag = ("Building");
            for (int i = 0; i < BuildingsInstantiated.Length+2 ; ++i)
            {
                GonnaBuild.name = ("Building" + i);
            }
			if (BuildingFace == BuildingFace.Right) {
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[4], Quaternion.identity);
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";
				GameObject NextSidePoint = new GameObject ("NextSidePoint");
				NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [6];
				NextSidePoint.transform.parent = GonnaBuild.transform;
			}else if (BuildingFace == BuildingFace.Left) {
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[6], Quaternion.identity);
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";
				GameObject NextSidePoint = new GameObject ("NextSidePoint");
				NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [4];
				NextSidePoint.transform.parent = GonnaBuild.transform;
			}
        }
    }
    void BuildBackward(bool active)
    {
        if (active)
        {
            GonnaBuild = (GameObject)Instantiate(Buildings[RandomCompleteBuildings]);
			if (BuildingFace == BuildingFace.Right) {
				PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x + GonnaBuild.GetComponent<BoxCollider> ().center.x 
					               , points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
                                   , points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().center.z - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z);
				GonnaBuild.transform.position = PosToBuild;
				GonnaBuild.transform.localRotation = Quaternion.Euler (0, 180, 0);
			} else if (BuildingFace == BuildingFace.Left){
				PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x - GonnaBuild.GetComponent<BoxCollider> ().center.x 
					                , points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
									, points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().center.z - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z);
				GonnaBuild.transform.position = PosToBuild;
				GonnaBuild.transform.localRotation = Quaternion.Euler (0, 0, 0);
			}
            GonnaBuild.tag = ("Building");
            for (int i = 0; i < BuildingsInstantiated.Length +2; i++)
            {
                GonnaBuild.name = ("Building" + i);
            }
			if (BuildingFace == BuildingFace.Right) {
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[4], Quaternion.identity);
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";
				GameObject NextSidePoint = new GameObject ("NextSidePoint");
				NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [6];
				NextSidePoint.transform.parent = GonnaBuild.transform;
			}else if (BuildingFace == BuildingFace.Left){
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices (GonnaBuild) [6], Quaternion.identity);
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";
				GameObject NextSidePoint = new GameObject ("NextSidePoint");
				NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [4];
				NextSidePoint.transform.parent = GonnaBuild.transform;
			}
        }
    }
    void BuildRight(bool active)
    {
        if (active)
        {
            GonnaBuild = (GameObject)Instantiate(Buildings[RandomCompleteBuildings]);
			if (BuildingFace == BuildingFace.Right) {
				PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z - GonnaBuild.GetComponent<BoxCollider> ().center.z 
					               , points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
                                   , points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x + GonnaBuild.GetComponent<BoxCollider> ().center.x);
				GonnaBuild.transform.position = PosToBuild;
				GonnaBuild.transform.localEulerAngles = new Vector3 (0, 90, 0);
			} else if (BuildingFace == BuildingFace.Left) {
				PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z - GonnaBuild.GetComponent<BoxCollider> ().center.z ,
					                 points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y ,
								     points [points.Length - 1].transform.position.z - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x - GonnaBuild.GetComponent<BoxCollider> ().center.x);
				GonnaBuild.transform.position = PosToBuild;
				GonnaBuild.transform.localEulerAngles = new Vector3 (0, 270, 0);
			}
            GonnaBuild.tag = ("Building");
            for (int i = 0; i < BuildingsInstantiated.Length +2; i++)
            {
                GonnaBuild.name = ("Building" + i);
            }
			if (BuildingFace == BuildingFace.Right) {
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[4], Quaternion.identity);
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";
				GameObject NextSidePoint = new GameObject ("NextSidePoint");
				NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [6];
				NextSidePoint.transform.parent = GonnaBuild.transform;
			} else if (BuildingFace == BuildingFace.Left) {
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices (GonnaBuild) [6], Quaternion.identity);
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";
				GameObject NextSidePoint = new GameObject ("NextSidePoint");
				NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [4];
				NextSidePoint.transform.parent = GonnaBuild.transform;
			}
        }
    }
    void BuildLeft(bool active)
    {
        if (active)
        {
            GonnaBuild = (GameObject)Instantiate(Buildings[RandomCompleteBuildings]);
			if (BuildingFace == BuildingFace.Right) {
				PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z + GonnaBuild.GetComponent<BoxCollider> ().center.z
					                    , points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
                                        , points [points.Length - 1].transform.position.z - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x - GonnaBuild.GetComponent<BoxCollider> ().center.x);
				GonnaBuild.transform.position = PosToBuild;
				GonnaBuild.transform.localEulerAngles = new Vector3 (0, 270, 0);
			}else if (BuildingFace == BuildingFace.Left){
				PosToBuild = new Vector3 (points [points.Length - 1].transform.position.x - GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.z - GonnaBuild.GetComponent<BoxCollider> ().center.z 
					                    , points [points.Length - 1].transform.position.y - GetBuildingVertices (GonnaBuild) [7].y
					                    , points [points.Length - 1].transform.position.z + GonnaBuild.GetComponent<BoxCollider> ().bounds.extents.x + GonnaBuild.GetComponent<BoxCollider> ().center.x);
				GonnaBuild.transform.position = PosToBuild;
				GonnaBuild.transform.localEulerAngles = new Vector3 (0, 90, 0);
			}
            GonnaBuild.tag = ("Building");
            for (int i = 0; i < BuildingsInstantiated.Length +2; i++)
            {
                GonnaBuild.name = ("Building" + i);
            }
			if (BuildingFace == BuildingFace.Right) {
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[4], Quaternion.identity);
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";
				GameObject NextSidePoint = new GameObject ("NextSidePoint");
				NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [6];
				NextSidePoint.transform.parent = GonnaBuild.transform;
			}else if (BuildingFace == BuildingFace.Left){
				GameObject PointInstantiated = (GameObject)Instantiate (point, GetBuildingVertices(GonnaBuild)[6], Quaternion.identity);
				PointInstantiated.transform.parent = GonnaBuild.transform;
				PointInstantiated.name = "Point";
				GameObject NextSidePoint = new GameObject ("NextSidePoint");
				NextSidePoint.transform.position = GetBuildingVertices (GonnaBuild) [4];
				NextSidePoint.transform.parent = GonnaBuild.transform;
			}
        }
    }
    void Start()
    {
        gameObject.tag = ("ProceduralBuildings");
        BuildingTools = new GameObject("Building Tools"); BuildingTools.transform.position = gameObject.transform.position; BuildingTools.name = ("Building Tools");
        StartPoint = new GameObject("Start Point"); StartPoint.transform.position = gameObject.transform.position ; StartPoint.transform.parent = BuildingTools.transform;
		EndPoint = new GameObject("End Point"); EndPoint.transform.parent = BuildingTools.transform;EndPoint.transform.position = gameObject.transform.position;
        point = new GameObject("Point"); point.transform.position = gameObject.transform.position; point.tag = ("point"); point.transform.parent = BuildingTools.transform;
		StartBuilding = false; StopBuilding = true;
    }

    void Update()
    {
        if (gameObject.tag != ("ProceduralBuildings"))
        {
            gameObject.tag = ("ProceduralBuildings");
        }
        PointsGenrated = GameObject.FindGameObjectsWithTag("PointGenrated");
        field = GameObject.FindGameObjectsWithTag("Field");
        DirectionOfBuilding();
		BlockMovment ();
        BuildingsInstantiated = GameObject.FindGameObjectsWithTag("Building");
        CornerBuildingsInstantiated = GameObject.FindGameObjectsWithTag("CornerSideBuilding");
        points = GameObject.FindGameObjectsWithTag("point");
        EndPoint.transform.position = gameObject.transform.position;

        RandomCompletedCornerBuildings = Random.Range(0, Cornerbuildings.Length);
        distPoint = Vector3.Distance(points[points.Length - 1].transform.position, EndPoint.transform.position);

		if (EndPoint.transform.position == StartPoint.transform.position) {
			placeable = false;
		}else {
			placeable = true;
		}
		if (BuildingsInstantiated.Length > 0){
			BuildingsNotNull = false;
        }
        else
        {
			BuildingsNotNull = true;
        }
        if (GenrateNewField)
        {
            once = true;
            NewBuildingField();
            GenrateNewField = false;
        }
        if (ResetBuilderPosition)
        {
            if (GameObject.Find("LastPoint") != null)
            {
                StartPoint.transform.position = PointsGenrated[PointsGenrated.Length - 1].transform.position;
                point.transform.position = StartPoint.transform.position;
                gameObject.transform.position = StartPoint.transform.position;
				stored = gameObject.transform.position;
				LastPosition = gameObject.transform.position;
                if (forward || backward || left || right)
                {
                    forward = false;
                    backward = false;
                    right = false;
                    left = false;
                }
                ResetBuilderPosition = false;
            }
            else
            {
                Debug.Log("There is no last position to reset to it ");
                ResetBuilderPosition = false;
            }
        }

        if (StopBuilding)
        {
            forward = false;
            backward = false;
            right = false;
            left = false;
            once = true;
            if (BuildingsInstantiated.Length > 0)
            {
                NewBuildingField();
            }
            StartPoint.transform.position = gameObject.transform.position;
            point.transform.position = gameObject.transform.position;
        }
        if (StartBuilding)
        {
            StopBuilding = false;
            StartBuilding = false;
        }
        if (PlaceCornerSideBuilding)
        {
            PlaceSidecornerBuilding();
            PlaceCornerSideBuilding = false;
        }

		if (forward || backward) {
			BuldingSize = Buildings [RandomCompleteBuildings].GetComponent<BoxCollider> ().bounds.size.z - Buildings [RandomCompleteBuildings].GetComponent<BoxCollider> ().center.z;
		}
		if (right || left){
			BuldingSize = Buildings [RandomCompleteBuildings].GetComponent<BoxCollider> ().bounds.size.z - Buildings [RandomCompleteBuildings].GetComponent<BoxCollider> ().center.x;
		}

		if (!BuildingCurve) {
			if (distPoint > BuldingSize && BuilderInBuildingField == true) {
				LastRandom = RandomCompleteBuildings;
				RandomCompleteBuildings = Random.Range(0, Buildings.Length);
				#region Prevent Repeating
				if (Buildings.Length > 1) {
					while (RandomCompleteBuildings == LastRandom) {
							RandomCompleteBuildings = Random.Range (0, Buildings.Length);
					}
				}
				#endregion
				if (forward) {
					BuildForward (true);
				}
				if (backward) {
					BuildBackward (true);
				}
				if (right) {
					BuildRight (true);
				}
				if (left) {
					BuildLeft (true);
				}
			}
		}
    }
}