using UnityEngine;
using System.Collections;

public class TestRotations : MonoBehaviour {

	public Transform[] cameras;

	private Quaternion deltaRotation = Quaternion.identity;

	// Use this for initialization
	void Start () {
		deltaRotation = Quaternion.Inverse (cameras [1].localRotation) * cameras [0].localRotation;
		Debug.Log (deltaRotation.eulerAngles.y);
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3  dir = cameras[1].InverseTransformDirection(Vector3.forward);
		//cameras [2].localRotation = cameras [1].localRotation 
		//cameras[2].Rotate(, Space.Self);
		cameras[2].localRotation = cameras [1].localRotation * deltaRotation;
		//* Quaternion.Euler (Vector3.up * deltaRotation.eulerAngles.y); 
	}
}
