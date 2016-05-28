using UnityEngine;
using System.Collections;

public class MapDuplicate : MonoBehaviour {

	private Transform objectTransform;

	// Use this for initialization
	void Start () {
	
	}

	public void SetObjectTransform(Transform t) {
		objectTransform = t;
	}

	public void UpdateTransform() {
		transform.position = objectTransform.position / 100f + Vector3.up * 100f;
		transform.localScale = objectTransform.localScale / 100f;
		transform.rotation = objectTransform.rotation;
	}

}
