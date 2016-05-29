using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {

	protected static MapController _instance;

	public bool enableMap = false;
	public bool showMap = false;
	public bool showWorldMap = false;

	protected Camera mapCamera;
	protected MapNav mapNav;
	protected WorldMap worldMap;


	// Use this for initialization
	void Awake () {
		_instance = this;
		mapCamera = GetComponentInChildren<Camera> ();
		mapNav = GetComponentInChildren<MapNav> ();
		worldMap = GetComponentInChildren<WorldMap> ();
		ToggleMap (false);
		EnableMap (false);
	}

	public static MapController instance {
		get{ return _instance; }
	}

	/* INITIALIZATION */

	public void LoadMap() {
		enableMap = true;
	}

	/* SHOW AND HIDE MAP */

	public void ShowMap() {
		showMap = true;
	}

	public void HideMap () {
		showMap = false;
	}

	public void ToggleMap() {
		showMap = !showMap;
	}

	/* VISIBILITY */

	public bool IsEnabled() {
		return mapNav.gameObject.activeSelf;
	}

	public bool IsVisible() {
		return mapCamera.enabled;
	}


	/* PROTECTED */

	protected void ToggleMap(bool state) {
		mapCamera.enabled = state;
	}

	protected void EnableMap(bool state) {
		foreach (Transform child in transform) {
			child.gameObject.SetActive (state);
		}
	}

	void Update() {

		//Enable map
		if (enableMap && !IsEnabled ()) {
			EnableMap (true);
		} else if(!enableMap && IsEnabled()) {
			EnableMap (false);

		//I map active and enabled
		} else if (IsEnabled ()) {

			//Show map
			if (showMap && !IsVisible ()) {
				ToggleMap (true);
			} else if (!showMap && IsVisible ()) {
				ToggleMap (false);
			}

			//Show World Map
			if (showWorldMap && !worldMap.IsVIsible ()) {
				worldMap.ToggleMap (true);
			} else if (!showWorldMap && worldMap.IsVIsible ()) {
				worldMap.ToggleMap (false);
			}
		}

		//Show pointers

	}
}
