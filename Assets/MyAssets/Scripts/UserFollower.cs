using UnityEngine;
using System.Collections;

public class UserFollower : MonoBehaviour {

	private Transform playerTransform;

	// Use this for initialization
	void Start () {
		playerTransform = GameObject.FindWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (playerTransform.position.x * 100f, transform.position.y, playerTransform.position.z * 100f);
		transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles.x, playerTransform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
	}
}
