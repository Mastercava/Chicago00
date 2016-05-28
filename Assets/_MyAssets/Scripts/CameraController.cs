using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	public enum CameraType {ArCamera, SensorCamera, MainCamera, PanoCamera}

	protected static CameraController _instance;

	public ARCamera arCamera;
	public SensorCamera sensorCamera;
	public MainCamera mainCamera;
	public PanoCamera panoCamera;

	protected bool isInitialized = false;

	void Awake() {
		_instance = this;
	}

	public static CameraController instance {
		get{ return _instance; }
	}

	// Use this for initialization
	IEnumerator Start () {

		yield return new WaitForSeconds (1f);

		ApplyFieldOfView ();

		DisableHelperCameras ();

		SetBackground ();

		isInitialized = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(isInitialized) {

			/*** MODIFY HERE FOR THE BEHAVIOR OF THE CAMERAS ***/

			mainCamera.transform.position = sensorCamera.GetPosition ();
			mainCamera.transform.rotation = sensorCamera.GetRotation ();

			/*** END OF THE MODIFIABLE AREA ***/
		}
	}

	/* PUBLIC - USE THESE IN YOUR SCRIPTS */

	public Vector3 GetCameraPosition(CameraType type) {
		if(type == CameraType.ArCamera) return arCamera.GetPosition();
		if(type == CameraType.SensorCamera) return sensorCamera.GetPosition();
		return  mainCamera.GetPosition();
	}

	public Quaternion GetCameraRotation(CameraType type) {
		if(type == CameraType.ArCamera) return arCamera.GetRotation();
		if(type == CameraType.SensorCamera) return sensorCamera.GetRotation();
		return  mainCamera.GetRotation();
	}

	public bool InitializeGps(float accuracy) {
		return sensorCamera.InitializeGps (accuracy);
	}

	public float GetGeolocationAccuracy() {
		return sensorCamera.GetGeolocationAccuracy ();
	}

	public Vector2 GetGpsPosition() {
		return sensorCamera.GetRawGeolocation ();
	}

	public LocationServiceStatus GetGpsStatus() {
		return sensorCamera.GetGpsStatus ();
	}

	public float DistanceToLocation(double lat, double lng) {

		if (GetGpsStatus () != LocationServiceStatus.Running) {
			return Single.MaxValue;
		}

		double radius = 6371;
		double dLat = DegreesToRadians(lat - GetGpsPosition().x);
		double dLon = DegreesToRadians(lng - GetGpsPosition().y);
		double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
			Math.Cos(DegreesToRadians(GetGpsPosition().x)) * Math.Cos(DegreesToRadians(lat)) *
			Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
		double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
		double d = radius * c;
		return (float) d;
	}

	public bool IsWithinArea(double lat, double lng, float radius) {
		return DistanceToLocation (lat, lng) < radius;
	}

	public List<GameObject> GetFiducials() {
		return arCamera.GetFiducials ();
	}

	public bool IsFiducialAvailable() {
		return arCamera.GetFiducials ().Count > 0;
	}

	public String GetCurrentFiducial() {
		if(!IsFiducialAvailable()) {
			return "";
		}
		return GetFiducials()[0].name;
	}

	public void ClosePanorama() {
		panoCamera.ToggleCamera (false);
	}

	public void LoadPanorama(string panoName) {
		panoCamera.ToggleCamera (true);
	}

	/* PROTECTED */

	protected void ApplyFieldOfView() {
		float fov = arCamera.GetFieldOfView ();
		sensorCamera.SetFieldOfView (fov);
		mainCamera.SetFieldOfView (fov);
	}

	protected void DisableHelperCameras() {
		//arCamera.ToggleCamera (false);
		sensorCamera.ToggleCamera (false);
		mainCamera.GetComponent<Camera> ().depth = 2;
	}

	protected void SetBackground () {
		mainCamera.SetBackgroundPlane (arCamera.GetBackgroundPlane());
	}
		
	// Convert to Radians.
	private double DegreesToRadians(double val) {
		return (Math.PI / 180) * val;
	}

}
