using UnityEngine;
using System.Collections;

public class WorldMap : MonoBehaviour {

	private Renderer mapRenderer;
	private Renderer renderer;

	// Use this for initialization
	void Start () {
		mapRenderer = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Renderer>();
		renderer = GetComponent<Renderer> ();
		renderer.enabled = false;
		StartCoroutine (RefreshMap());
	}
	
	IEnumerator RefreshMap () {
		while(true) {
			yield return new WaitForSeconds (3f);
			if(renderer.enabled) {
				UpdateMaterial ();
			}
		}
	}

	public void ToggleMap(bool state) {
		renderer.enabled = state;
		if(IsVIsible()) {
			UpdateMaterial ();
		}
	}

	public bool IsVIsible() {
		return renderer.enabled;
	}

	public void UpdateMaterial() {
		renderer.material = mapRenderer.GetComponent<Renderer> ().material;
	}

}
