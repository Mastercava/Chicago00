using UnityEngine;
using System.IO;
using System.Collections;

public class IMUAnalyzer : MonoBehaviour {

	private string fileName;
	private StreamWriter stream;
	private bool isLogging;
	private float startTime;

	private Vector3 arStartPosition, sensorStartPosition;
	private Quaternion arStartRotation, sensorStartRotation;

	private Transform arTransform, sensorTransform;

	void Start() {
		arTransform = GameObject.FindGameObjectWithTag ("ARCamera").transform.parent;
		sensorTransform = GameObject.FindGameObjectWithTag ("SensorCamera").transform;
		#if UNITY_EDITOR
		fileName = "C:\\Users\\Mastercava\\Desktop\\IMULog.txt";
		#else
		fileName = Application.persistentDataPath + "/IMULog" + Time.time + ".txt";
		#endif
	}


	// Use this for initialization
	public void StartLogging() {
		stream = File.CreateText(fileName);
		isLogging = true;
		startTime = Time.time;
		arStartRotation = arTransform.rotation;
		arStartPosition = arTransform.position;
		sensorStartRotation = sensorTransform.rotation;
		sensorStartPosition = sensorTransform.position;
	}

	public void StopLogging() {
		isLogging = false;
		stream.Close();
	}

	// Update is called once per frame
	void LateUpdate () {
		if(isLogging) {
			Vector3 arPos = GetArDeltaPosition ();
			Vector3 arRot = GetArDeltaRotation ();
			Vector3 sensorPos = GetSensorDeltaPosition ();
			Vector3 sensorRot = GetSensorDeltaRotation ();
			stream.WriteLine ("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12}",
				Time.time - startTime,
				arPos.x, arPos.y, arPos.z, arRot.x, arRot.y, arRot.z,
				sensorPos.x, sensorPos.y, sensorPos.z, sensorRot.x, sensorRot.y, sensorRot.z
			);
		}
	}

	public Vector3 GetArDeltaRotation() {
		Vector3 a = arStartRotation.eulerAngles;
		Vector3 b = arTransform.rotation.eulerAngles;
		return new Vector3 (GetAngleDifference(a.x, b.x), GetAngleDifference(a.y, b.y), GetAngleDifference(a.z, b.z));
	}

	public Vector3 GetArDeltaPosition() {
		return arTransform.position - arStartPosition;
	}

	public Vector3 GetSensorDeltaRotation() {
		Vector3 a = sensorStartRotation.eulerAngles;
		Vector3 b = sensorTransform.rotation.eulerAngles;
		return new Vector3 (GetAngleDifference(a.x, b.x), GetAngleDifference(a.y, b.y), GetAngleDifference(a.z, b.z));
	}

	public Vector3 GetSensorDeltaPosition() {
		return sensorTransform.position - sensorStartPosition;
	}

	private float GetAngleDifference(float a, float b) {
		float diff = (b-a) % 360;
		if(diff >= 180) {
			diff = diff - 360;
		}
		else if(diff < -180) {
			diff = diff + 360;
		}
		return diff;
	}
}
