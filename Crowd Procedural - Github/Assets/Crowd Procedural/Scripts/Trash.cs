using UnityEngine;
using System.Collections;

public class Trash : MonoBehaviour {
    bool triggered;
	void Start () {
        StartCoroutine("AbleCollider");
	}
    IEnumerator AbleCollider()
    {
        yield return new WaitForSeconds(0.05f);
        if (triggered == false)
        {
            gameObject.GetComponent<Collider>().isTrigger = false;
            gameObject.GetComponent<Collider>().enabled = true;
            DestroyImmediate(gameObject.GetComponent<Trash>(), true);
        }
        else
        {
            StartCoroutine("AbleCollider");
            StopCoroutine("AbleCollider");
        }
    }
    void OnTriggerEnter()
    {
        triggered = true;
    }
    void OnTriggerExit()
    {
        triggered = false;
    }
}
