using UnityEngine;
using System.Collections;

public class RiverwalkSample : MonoBehaviour {

	private CameraController cameraController;
	private UIController uiController;
	private SceneController sceneController;
	private MapController mapController;

	// Use this for initialization
	void Start () {
		cameraController = CameraController.instance;
		uiController = UIController.instance;
		sceneController = SceneController.instance;
		mapController = MapController.instance;

		//Require 3m accuracy GPS
		cameraController.InitializeGps (3.0f);
		cameraController.setDefaultCameraBehavior ();

		//Full screen panorama mode (from Resources/Panos)
		uiController.OpenPanoramaMode("DarkSkies");

		//Set Helper panorama
		uiController.SetHelperPanorama("Darkskies");

		//Load and show map
		mapController.LoadMap();
		//mapController.ShowMap();
	}
	
	// Update is called once per frame
	void Update () {

		//Wait for GPS signal to activate
		if (cameraController.GetGpsStatus() != LocationServiceStatus.Running) {
			return;
		}

		//User is in the right place (e.g. 200m of radius)
		if (cameraController.IsWithinArea (41.887471d, -87.631765d, 200f)) {

		} 
		//User in the wrong place, tell him to go to the riverwalk
		else {
			uiController.setStreetLabel ("You're still far from our destination! Try to get closer to Chicago RiverWalk");
		}
	
	}
}
