using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Vuforia;

public class ARCamera : VirtualCamera {

	protected List<GameObject> fiducials = new List<GameObject>();

	// Use this for initialization
	protected void Start () {
		base.Start ();
	}

	protected void Update() {
		UpdateCameraPose ();
	}


	public float GetFieldOfView() {
		Matrix4x4 mat = camera.projectionMatrix;

		float a = mat[0];
		float b = mat[5];
		float c = mat[10];
		float d = mat[14];

		float aspect_ratio = b / a;

		float k = (c - 1.0f) / (c + 1.0f);
		float clip_min = (d * (1.0f - k)) / (2.0f * k);
		float clip_max = k * clip_min;

		float RAD2DEG = 180.0f / 3.14159265358979323846f;
		return RAD2DEG * (2.0f * (float) Math.Atan (1.0f / b));
	}


	public GameObject GetBackgroundPlane() {
		return GetComponentInChildren<BackgroundPlaneBehaviour> ().gameObject;
	}

	public void MarkerFound(GameObject fiducial) {
		if (!fiducials.Contains (fiducial)) {
			fiducials.Add (fiducial);
		}
	}

	public void MarkerLost(GameObject fiducial) {
		fiducials.Remove (fiducial);
	}

	public List<GameObject> GetFiducials() {
		return fiducials;
	}

}
