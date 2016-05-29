using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

	protected static UIController _instance;

	public GameObject topPanel;
	public GameObject bottomPanel;
	public Text streetLabel;
	public Skybox helperPanorama;
	public bool showDebugPanel;


	void Awake() {
		_instance = this;
	}

	public static UIController instance {
		get{ return _instance; }
	}


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {


	}


	//Set text on the horizontal line above the bottom panes
	public void setStreetLabel(string text) {
		streetLabel.text = text;
	}
		

	//Closes UI and goes fullscreen panorama mode
	public void OpenPanoramaMode(string name) {
		TogglePanel (topPanel, false);
		TogglePanel (bottomPanel, false);
		CameraController.instance.OpenPanoramaMode (name);
	}


	//Reopens UI and exists fullscreen panorama mode
	public void ClosePanoramaMode() {
		TogglePanel (topPanel, true);
		TogglePanel (bottomPanel, true);
		CameraController.instance.ClosePanoramaMode ();
	}


	//Opens or closes a panel
	public void TogglePanel(GameObject panel, bool state) {
		panel.SetActive (state);
	}


	//Loads panorama in the bottom right UI from Resources/Panos
	public bool SetHelperPanorama(string name) {
		Material mat = Resources.Load ("Panos/" + name) as Material;
		if(mat != null) {
			helperPanorama.material = mat;
			return true;
		}
		return false;
	}


	/** DEBUG PANEL TO BE ADDED **/
}
