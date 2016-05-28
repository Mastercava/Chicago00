using UnityEngine;
using System.Collections;

public class VirtualCamera : MonoBehaviour {

	public Transform cameraTransform;
	public bool useSmoothing = false;
	public float smoothingFactor = 10f;

	protected Camera camera;

	protected Vector3 position;
	protected Quaternion rotation;

	protected Vector3 lastPosition;
	protected Quaternion lastRotation;

	protected void Start() {
		camera = GetComponent<Camera> ();
		if (!camera) {
			camera = GetComponentInChildren<Camera> ();
		}
	}

	protected void UpdateCameraPose() {
		if (useSmoothing) {
			position = Vector3.Lerp (lastPosition, cameraTransform.position, Time.deltaTime * smoothingFactor);
			rotation = Quaternion.Slerp (lastRotation, cameraTransform.rotation, Time.deltaTime * smoothingFactor);
		} else {
			position = cameraTransform.position;
			rotation = cameraTransform.rotation;
		}
			
		lastPosition = position;
		lastRotation = rotation;

	}

	public void SetFieldOfView(float fov) {
		camera.fieldOfView = fov;
	}

	public void ToggleCamera(bool state) {
		camera.enabled = state;
	}

	public Vector3 GetPosition() {
		return position;
	}

	public Quaternion GetRotation() {
		return rotation;
	}

}
