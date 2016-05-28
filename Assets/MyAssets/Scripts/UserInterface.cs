using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour {

	private Camera mapCamera;
	private Camera mainCamera;


	void Start() {
		mapCamera = GameObject.FindWithTag ("MapCamera").GetComponent<Camera>();
		mainCamera = GameObject.FindWithTag ("FinalCamera").GetComponent<Camera>();
		mapCamera.enabled = false;
	}

	public void ToggleCamera() {
		if (mapCamera.enabled) {
			mapCamera.enabled = false;
			mainCamera.enabled = true;
			#if UNITY_ANDROID
			//SceneManager.GetInstance().EnableHelperCameras(true);
			#endif
		} else {
			mapCamera.enabled = true;
			mainCamera.enabled = false;
			#if UNITY_ANDROID
			SceneManager.GetInstance().EnableHelperCameras(false);
			#endif
		}
	}


}
