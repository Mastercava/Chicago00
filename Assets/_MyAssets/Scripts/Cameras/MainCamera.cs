using UnityEngine;
using System.Collections;

public class MainCamera : VirtualCamera {

	// Use this for initialization
	void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetBackgroundPlane(GameObject plane) {
		plane.transform.parent = transform;
	}
}
