using UnityEngine;
using System.Collections;

public class PanoCamera : VirtualCamera {

	protected Skybox skybox;

	// Use this for initialization
	void Awake () {
		base.Start ();
		skybox = GetComponent<Skybox> ();
		Debug.Log (skybox);
	}


	public bool loadPanorama(string name) {
		Material mat = Resources.Load ("Panos/" + name) as Material;
		if(mat != null) {
			skybox.material = mat;
			return true;
		}
		return false;
	}

}