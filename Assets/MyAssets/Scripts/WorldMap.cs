using UnityEngine;
using System.Collections;

public class WorldMap : MonoBehaviour {

	private GameObject map;
	private Renderer renderer;

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		map = GameObject.FindGameObjectWithTag ("GameController");
		renderer = GetComponent<Renderer> ();
		StartCoroutine (RefreshMap());
		#else
		Destroy(gameObject);
		#endif
	}
	
	IEnumerator RefreshMap () {
		while(true) {
			yield return new WaitForSeconds (3f);
			renderer.material = map.GetComponent<Renderer> ().material;
		}
	}
}
