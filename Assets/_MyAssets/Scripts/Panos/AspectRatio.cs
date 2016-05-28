using UnityEngine;
using System.Collections;

public class AspectRatio : MonoBehaviour {

	public float ratio = 4.052f;

	// Use this for initialization
	void Start () {
		Debug.Log (GetComponent<Camera> ().aspect);	
		GetComponent<Camera> ().aspect = ratio;
		Debug.Log (GetComponent<Camera> ().aspect);
	}

}
