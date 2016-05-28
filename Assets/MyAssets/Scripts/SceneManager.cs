using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class SceneManager : MonoBehaviour {



	/*** PRIVATE ATTRIBUTES ***/

	private static SceneManager instance;

	private List<GameObject> markers = new List<GameObject>();

	private Transform arTransform;
	private Camera vuforiaCamera, arCamera;
	private Transform sensorTransform;
	private Camera sensorCamera;
	private Camera mapCamera;
	private Camera mainCamera;
	private Transform mainTransform;

	private Camera currentCamera;

	private Transform backgroundPlane;
	private Vector3 backgroundPosition;
	private Vector3 backgroundScale;
	private Quaternion backgroundRotation;

	private Quaternion deltaRotation = Quaternion.Inverse(Quaternion.identity);

	private bool isGPSInitialized = false;
	private Transform userTransform;

	private Cardboard cardboard;

	/** SMOOTHING **/
	private Vector3 lastArPosition;
	private Quaternion lastArRotation;

	/** AR STABILITY **/
	private bool isArReliable;
	private Vector3 lastArReliablePosition;
	private Quaternion lastArReliableRotation;

	/** EDITING **/
	private Vector3 dragOrigin;

	/*** RUNTIME METHOS ***/

	private SceneManager() {}


	public static SceneManager GetInstance() {
		if (instance == null) {
			instance = new SceneManager ();
		}
		return instance;
	}


	void Awake () {
		if (instance == null) {
			instance = this;
			LinkGameobjects ();
		}
	}

	void LateUpdate() {

		float smoothingFactor = 4f;
		Vector3 rawArPosition = arTransform.position;
		arTransform.position = Vector3.Lerp (lastArPosition, arTransform.position, Time.deltaTime * smoothingFactor);
		arTransform.rotation = Quaternion.Slerp (lastArRotation, arTransform.rotation, Time.deltaTime * smoothingFactor);
		lastArPosition = arTransform.position;
		lastArRotation = arTransform.rotation;
		if (isArReliable && Vector3.Distance (rawArPosition, lastArReliablePosition) < 0.1f) {
			Debug.Log ("Updating reliable position");
			lastArReliablePosition = lastArPosition;
			lastArReliableRotation = lastArRotation;
		}

		#if UNITY_EDITOR
		//Sensor camera height
		if (Input.GetMouseButtonDown(0))
		{
			dragOrigin = Input.mousePosition;
			return;
		}

		if (!Input.GetMouseButton(0)) return;

		sensorTransform.Rotate(new Vector3(Vector3.Normalize(Input.mousePosition - dragOrigin).y / 5f, 0f, 0f));
		#endif

		if(markers.Count > 0) {
			mainTransform.rotation = lastArReliableRotation; //arTransform.rotation;
			mainTransform.position = lastArReliablePosition;//arTransform.position;
		}
		else {
			mainTransform.rotation = sensorTransform.rotation * deltaRotation;
			mainTransform.position = lastArReliablePosition;//(isArReliable ? arTransform.position : lastArPosition);
		}
	}


	/*** HELPERS ***/

	private void LinkGameobjects() {
		//Get GameObjects
		sensorTransform = GameObject.FindGameObjectWithTag ("SensorCamera").transform;
		sensorCamera = sensorTransform.gameObject.GetComponent<Camera> ();
		mainTransform = GameObject.FindGameObjectWithTag ("FinalCamera").transform;
		mainCamera = mainTransform.gameObject.GetComponent<Camera> ();
		arCamera = GameObject.FindGameObjectWithTag ("ARCamera").GetComponent<Camera> ();
		arTransform = arCamera.gameObject.transform.parent;
		backgroundPlane = GameObject.FindWithTag ("BackgroundPlane").transform;

		//MapCamera
		mapCamera = GameObject.FindWithTag ("MapCamera").GetComponent<Camera>();

		cardboard = GameObject.FindGameObjectWithTag ("Cardboard").GetComponent<Cardboard>();

		StartCoroutine (InitializeCameras());

		//GPS from map pointer
		userTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}


	private IEnumerator InitializeCameras() {

		yield return new WaitForSeconds (2f);

		while(! Vuforia.VuforiaManager.Instance.Initialized) {
			yield return null;
		}

		float fov = GetFieldOfView (arCamera);
		Debug.Log ("Field of view: " + fov);

		//Set field of view
		sensorCamera.fieldOfView = fov;
		mainCamera.fieldOfView = fov;

		/*
		//Replace Vuforia camera with Unity one
		arCamera.enabled = false;
		sensorCamera.enabled = false;
		mapCamera.enabled = false;
		*/

		StoreBackgroundParameters ();
		backgroundPlane.parent = mainTransform;
		ResetBackgroundParameters ();

		#if UNITY_EDITOR
		sensorCamera.enabled = false;
		arCamera.enabled = false;
		#endif

		Debug.Log ("Cameras correctly initialized");

	}


	public void MarkerFound(GameObject marker) {
		if (!markers.Contains (marker)) {
			markers.Add (marker);
			StartCoroutine (SetArReliable());
		}
	}

	public IEnumerator SetArReliable() {
		yield return new WaitForSeconds (0.5f);
		isArReliable = true;
		lastArReliablePosition = arTransform.position;
	}

	public void MarkerLost(GameObject marker) {
		markers.Remove (marker);
		if (markers.Count == 0) {
			AlignCameras ();
		}
	}


	private float GetFieldOfView(Camera cam) {
		Matrix4x4 mat = cam.projectionMatrix;

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
		


	private void StoreBackgroundParameters() {
		//Copy background parameters
		backgroundPosition = new Vector3(backgroundPlane.localPosition.x, backgroundPlane.localPosition.y, backgroundPlane.localPosition.z);
		backgroundScale = new Vector3 (backgroundPlane.localScale.x, backgroundPlane.localScale.y, backgroundPlane.localScale.z);
		backgroundRotation = new Quaternion(backgroundPlane.rotation.x, backgroundPlane.rotation.y, backgroundPlane.rotation.z, backgroundPlane.rotation.w);
		Debug.Log (backgroundRotation);
	}


	private void ResetBackgroundParameters() {
	backgroundPlane.localPosition = backgroundPosition;
	backgroundPlane.localRotation = backgroundRotation;
	backgroundPlane.localScale = backgroundScale;
	}


	public void AlignCameras() {
		//RecenterSensorCamera ();
		deltaRotation = Quaternion.Inverse (sensorTransform.rotation) * lastArReliableRotation;//arTransform.rotation;
		Debug.Log ("Cameras aligned: " + deltaRotation);
		isArReliable = false;
	}

	public void ForceCamerasAlignment() {
		AlignCameras ();
		markers.Clear ();
	}

	public void RecenterSensorCamera() {
		cardboard.Recenter ();
	}

	public Transform GetSensorTransform() {
		return sensorTransform;
	}

	public Transform GetArTransform() {
		return arTransform;
	}

	public Transform GetMainTransform() {
		return mainTransform;
	}

	public void EnableHelperCameras(bool val) {
		sensorCamera.enabled = val;
		arCamera.enabled = val;
	}

}
