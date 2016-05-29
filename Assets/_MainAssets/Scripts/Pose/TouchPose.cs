using UnityEngine;
using System.Collections;

public class TouchPose : MonoBehaviour {

	
	private Vector3 firstpoint; //change type on Vector3
	private Vector3 secondpoint;
	private float xAngle = 0.0f; //angle for axes x for rotation
	private float yAngle= 0.0f;
	private float xAngTemp = 0.0f; //temp variable for angle
	private float yAngTemp = 0.0f;
	private bool movementStarted = false;

	void Start() {
		//Initialization our angles of camera
		xAngle = 0.0f;
		yAngle = 0.0f;
		this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
	}

	void  Update() {
		//Check count touches
		if(Input.touchCount > 0) {
			//Only if in correct area
			//Touch began, save position
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				if (Input.GetTouch (0).position.y < Screen.height / 3f && Input.GetTouch (0).position.x > Screen.width / 5f) {
					firstpoint = Input.GetTouch (0).position;
					xAngTemp = xAngle;
					yAngTemp = yAngle;
					movementStarted = true;
				}
			}
			//Move finger by screen
			else if (movementStarted && Input.GetTouch (0).phase == TouchPhase.Moved) {
				secondpoint = Input.GetTouch (0).position;
				//Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
				xAngle = xAngTemp + (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
				yAngle = yAngTemp - (secondpoint.y - firstpoint.y) * 90.0f / Screen.height;
				//Rotate camera
				this.transform.rotation = Quaternion.Euler (-yAngle, -xAngle, 0.0f);
			//End movement
			} else if (Input.GetTouch (0).phase == TouchPhase.Ended || Input.GetTouch (0).phase == TouchPhase.Canceled) {
				movementStarted = false;
			}
		}
	}
}
