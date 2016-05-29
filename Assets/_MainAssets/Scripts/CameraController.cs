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
	protected bool useDefaultCameraBehavior = false;

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

			if (useDefaultCameraBehavior) {

				ApplyDefaultCameraBehavior ();

			} else {

				/*** MODIFY HERE FOR THE BEHAVIOR OF THE CAMERAS ***/

				//mainCamera.transform.position = sensorCamera.GetPosition ();
				//mainCamera.transform.rotation = sensorCamera.GetRotation ();

				/*** END OF THE MODIFIABLE AREA ***/

			}
		}
	}


	/* PUBLIC - USE THESE IN YOUR SCRIPTS */


	//Returns the position of a specified camera
	public Vector3 GetCameraPosition(CameraType type) {
		if(type == CameraType.ArCamera) return arCamera.GetPosition();
		if(type == CameraType.SensorCamera) return sensorCamera.GetPosition();
		return  mainCamera.GetPosition();
	}


	//Returns the orientation of a specified camera
	public Quaternion GetCameraRotation(CameraType type) {
		if(type == CameraType.ArCamera) return arCamera.GetRotation();
		if(type == CameraType.SensorCamera) return sensorCamera.GetRotation();
		return  mainCamera.GetRotation();
	}


	//Requires the GPS to activate, requiring a specific accuracy in meters
	public bool InitializeGps(float accuracy) {
		return sensorCamera.InitializeGps (accuracy);
	}


	//Returns current accuracy of the GPS in meters
	public float GetGeolocationAccuracy() {
		return sensorCamera.GetGeolocationAccuracy ();
	}


	//Returns lat and lng obtainerd through the GPS
	public Vector2 GetGpsPosition() {
		return sensorCamera.GetRawGeolocation ();
	}


	//Returns LocationServiceStatus.Running if GPS is working
	public LocationServiceStatus GetGpsStatus() {
		return sensorCamera.GetGpsStatus ();
	}


	//Returns distance in meters of the user from a specified location
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


	//Returns true if user is within an area by a certain radius
	public bool IsWithinArea(double lat, double lng, float radius) {
		return DistanceToLocation (lat, lng) < radius;
	}


	//Tells app to use th standard Main Camera behavior
	public void setDefaultCameraBehavior() {
		useDefaultCameraBehavior = true;
	}


	//Returns the list of currently tracked fiducials
	public List<GameObject> GetFiducials() {
		return arCamera.GetFiducials ();
	}


	//Returns if at least one fiducial is available
	public bool IsFiducialAvailable() {
		return arCamera.GetFiducials ().Count > 0;
	}


	//Returns current fiducial, if available
	public GameObject GetCurrentFiducial() {
		if(!IsFiducialAvailable()) {
			return null;
		}
		return GetFiducials()[0];
	}


	//Closes full screen panorama mode
	public void ClosePanoramaMode() {
		panoCamera.ToggleCamera (false);
	}


	//Load from Resources/Panos/ a panorama t be displayed in panorama mode
	public void OpenPanoramaMode(string panoName) {
		panoCamera.loadPanorama (panoName);
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

	//Defautl main camera behavior
	protected void ApplyDefaultCameraBehavior () {
		mainCamera.transform.position = sensorCamera.GetPosition ();
		mainCamera.transform.rotation = sensorCamera.GetRotation ();
	}

}
