using UnityEngine;
using System.Collections;

public class CameraPointer : MonoBehaviour {


	public CameraController.CameraType cameraType;

	protected Renderer renderer;

	void Start() {
		renderer = GetComponent<Renderer> ();
	}

	// Update is called once per frame
	void Update () {
		
		Vector3 pos = CameraController.instance.GetCameraPosition (cameraType);
		Quaternion rot = CameraController.instance.GetCameraRotation (cameraType);

		//Debug.Log (cameraTransform.position);
		transform.position = new Vector3 (pos.x / 100f, transform.position.y, pos.z / 100f);
		transform.rotation = Quaternion.Euler (transform.eulerAngles.x, rot.eulerAngles.y, transform.rotation.eulerAngles.z);
	}

	public void TogglePointer(bool state) {
		renderer.enabled = state;
	}
}
