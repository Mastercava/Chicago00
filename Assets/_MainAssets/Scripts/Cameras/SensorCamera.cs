using UnityEngine;
using System.Collections;

public class SensorCamera : VirtualCamera {

	// Use this for initialization
	void Start () {
		base.Start ();
	}
	
	protected void Update() {
		UpdateCameraPose ();
	}

	public bool InitializeGps(float accuracy) {
		//GPS not enabled on mobile phone
		if (!Input.location.isEnabledByUser) {
			return false;
		}

		Input.location.Start(accuracy, accuracy);
		return true;
	}

	public LocationServiceStatus GetGpsStatus() {
		return Input.location.status;
	}

	public Vector2 GetRawGeolocation() {
		return new Vector2 (Input.location.lastData.latitude, Input.location.lastData.longitude);
	}

	public float GetGeolocationAccuracy() {
		return Input.location.lastData.horizontalAccuracy;
	}


}
