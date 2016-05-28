//MAPNAV Navigation ToolKit v.1.3.4
//Attention: This script uses a custom editor inspector: MAPNAV/Editor/SetGeoInspector.cs

using UnityEngine;
using System.Collections;

using Vuforia;


public class GeoLocation : MonoBehaviour
{
    public float lat;
    public float lon;
    public float height;
    public float orientation;
    public float scaleX = 1f;
    public float scaleY = 1f;
    public float scaleZ = 1f;
    private float initX;
    private float initZ;
    private MapNav gps;
    private bool gpsFix;
    private float fixLat;
    private float fixLon;

	private MapDuplicate mapDuplicate;

    void Awake()
    {
        //Reference to the MapNav.js script and gpsFix variable. gpsFix will be true when a valid location data has been set.
        gps = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapNav>();
        gpsFix = gps.gpsFix;
    }

    IEnumerator Start()
    {
        //Wait until the gps sensor provides a valid location.
        while (!gpsFix)
        {
            gpsFix = gps.gpsFix;
            yield return null;
        }
        //Read initial position (used as a reference system)
        initX = gps.iniRef.x;
        initZ = gps.iniRef.z;
        //Set object geo-location
		if (lat == 0f || lon == 0f) {
			UpdateParameters ();
		} else {
			GeoLocate ();
		}
		Duplicate ();
    }
		

    void GeoLocate()
    {
        //Translate the geographical coordinate system used by gps mobile devices(WGS84), into Unity's Vector2 Cartesian coordinates(x,z) and set height(1:100 scale).
        transform.position = 100f * new Vector3(((lon * 20037508.34f) / 18000) - initX, height / 100, ((Mathf.Log(Mathf.Tan((90 + lat) * Mathf.PI / 360)) / (Mathf.PI / 180)) * 1113.19490777778f) - initZ);

        //Set object orientation
        Vector3 tmp = transform.eulerAngles;
        tmp.y = orientation;
        transform.eulerAngles = tmp;

		//Set local object scale
		transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

	void Duplicate() {
		GameObject mapCopy = GameObject.Instantiate (gameObject);
		Destroy(mapCopy.GetComponent<GeoLocation>());
		if(mapCopy.GetComponent<ImageTargetBehaviour>()) {
			Destroy (mapCopy.GetComponent<ImageTargetBehaviour>());
			Destroy (mapCopy.GetComponent<TurnOffBehaviour>());
			Destroy(mapCopy.GetComponent<DefaultTrackableEventHandler> ());
		}
		mapDuplicate = mapCopy.AddComponent<MapDuplicate> ();
		mapDuplicate.SetObjectTransform (transform);
		mapDuplicate.UpdateTransform ();
		mapDuplicate.transform.parent = GameObject.FindGameObjectWithTag ("MapDuplicates").transform;
	}

	void Update() {
		if(transform.hasChanged && mapDuplicate != null) {
			mapDuplicate.UpdateTransform ();
			UpdateParameters ();
		}
	}

	private void UpdateGeolocation() {
		float posX = transform.position.x / 100f;
		float posZ = transform.position.z / 100f;
		lat = ((360 / Mathf.PI) * Mathf.Atan(Mathf.Exp(0.00001567855943f * (posZ + initZ)))) - 90;
		lon = (18000 * (posX + initX)) / 20037508.34f;
	}

	private void UpdateParameters() {

		//Update in editor
		height = transform.position.y;
		scaleX = transform.localScale.x;
		scaleY = transform.localScale.y;
		scaleZ = transform.localScale.z;
		orientation = transform.rotation.eulerAngles.y;
		UpdateGeolocation ();
	}

    //This function is similar to GeoLocation() but is to be used by SetGeoInspector.cs
    public void EditorGeoLocation()
    {
        gps = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapNav>();
        fixLat = gps.fixLat;
        fixLon = gps.fixLon;

        initX = fixLon * 20037508.34f / 18000;
        initZ = (float) (System.Math.Log(System.Math.Tan((90 + fixLat) * System.Math.PI / 360)) / (System.Math.PI / 180));
        initZ = initZ * 20037508.34f / 18000;

        //Translate the geographical coordinate system used by gps mobile devices(WGS84), into Unity's Vector2 Cartesian coordinates(x,z) and set height(1:100 scale).
        transform.position = new Vector3(((lon * 20037508.34f) / 18000) - initX, height / 100, ((Mathf.Log(Mathf.Tan((90 + lat) * Mathf.PI / 360)) / (Mathf.PI / 180)) * 1113.19490777778f) - initZ);
       
        //Set object orientation
        Vector3 tmp = transform.eulerAngles;
        tmp.y = orientation;
        transform.eulerAngles = tmp;
       
		//Set local object scale
		if(transform.localScale != Vector3.zero)
			transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }
}