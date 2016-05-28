using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneController : MonoBehaviour {

	protected static SceneController _instance;

	protected Dictionary<string, GameObject> virtualObjects = new Dictionary<string, GameObject> ();

	void Awake() {
		_instance = this;
	}

	public static SceneController instance {
		get{ return _instance; }
	}


	public GameObject GetObject(string name) {
		if (virtualObjects.ContainsKey (name)) {
			return virtualObjects["name"];
		}
		return null;
	}

	//TODO
	public GameObject[] GetNearbyObjects(float radius) {
		return null;
	}

	public GameObject GetCurrentObject() {
		return null;
	}

	protected void Start() {
		InitVirtualContent ();
	}

	protected void InitVirtualContent() {
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("VirtualContent");
		foreach(GameObject obj in objects) {
			virtualObjects.Add (obj.name, obj);
		}
	}


}
