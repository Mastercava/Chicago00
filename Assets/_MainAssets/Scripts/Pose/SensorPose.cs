using UnityEngine;
using System.Collections;

public class SensorPose : MonoBehaviour {

	public Transform emulatedPlayer;
	public float emulatedHeight = 1.7f;

	// Use this for initialization
	void Start () {
		Input.gyro.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		//Simulate movements in editor
		#if UNITY_EDITOR
		if(emulatedPlayer) {
			transform.position = new Vector3 (emulatedPlayer.position.x * 100f, emulatedHeight, emulatedPlayer.position.z * 100f);
			transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles.x, emulatedPlayer.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
		}
		//Get data from mobile device
		#else
		Quaternion gyro = Input.gyro.attitude;
		transform.rotation = new Quaternion(gyro.x, gyro.y, -gyro.z, -gyro.w);
		transform.Rotate (90, 0, 0, Space.World);
		#endif
	}
}
