using UnityEngine;
using System.Collections;

public class CameraPointer : MonoBehaviour {

	public enum CameraType {ArCamera, SensorCamera, MainCamera}

	public CameraType cameraType;
	private Transform cameraTransform;



	// Use this for initialization
	void Start () {
		cameraTransform = (cameraType == CameraType.ArCamera ? SceneManager.GetInstance().GetArTransform() : (cameraType == CameraType.SensorCamera ? SceneManager.GetInstance().GetSensorTransform() : SceneManager.GetInstance().GetMainTransform()));
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (cameraTransform.position);
		transform.position = new Vector3 (cameraTransform.position.x / 100f, transform.position.y, cameraTransform.position.z / 100f);
		transform.rotation = Quaternion.Euler (transform.eulerAngles.x, cameraTransform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
	}
}
