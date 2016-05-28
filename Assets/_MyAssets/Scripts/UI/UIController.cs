using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {

	public GameObject topPanel;
	public GameObject bottomPanel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if(Input.touchCount == 0) {
			return;
		}

		//Swipe
		if(Mathf.Abs(Input.touches[0].deltaPosition.y) > Screen.height / 3f) {
			if(Input.touches[0].deltaPosition.y > 0) bottomPanel.SetActive (false);
			else bottomPanel.SetActive (true);
		}

		*/
	}

	public void LoadPanorama(string name) {
		TogglePanel (topPanel, false);
		TogglePanel (bottomPanel, false);
		CameraController.instance.LoadPanorama (name);
	}

	public void ClosePanorama() {
		TogglePanel (topPanel, true);
		TogglePanel (bottomPanel, true);
		CameraController.instance.ClosePanorama ();
	}

	public void TogglePanel(GameObject panel, bool state) {
		panel.SetActive (state);
	}
}
