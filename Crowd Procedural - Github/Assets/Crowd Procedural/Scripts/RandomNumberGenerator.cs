using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumberGenerator : MonoBehaviour {

	float a = 1103515245;
	float c = 12345;
	float m = Mathf.Pow(2 , 32);
	float seed = 1;
	void Start () {
	}
	float GetRandomNumber(){
		seed = Mathf.FloorToInt (((a * seed + c) / m) * 10);
		return seed;
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Debug.Log (GetRandomNumber());
		}
	}
}
