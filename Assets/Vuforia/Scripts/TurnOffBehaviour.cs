/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using System.Collections;


namespace Vuforia
{
    /// <summary>
    /// A utility behaviour to disable rendering of a game object at run time.
    /// </summary>
    public class TurnOffBehaviour : TurnOffAbstractBehaviour
    {

		public bool showImage = true;

        #region UNITY_MONOBEHAVIOUR_METHODS

        void Awake()
        {
            if (VuforiaRuntimeUtilities.IsVuforiaEnabled())
            {
                // We remove the mesh components at run-time only, but keep them for
                // visualization when running in the editor:
                MeshRenderer targetMeshRenderer = this.GetComponent<MeshRenderer>();
                //Destroy(targetMeshRenderer);
                MeshFilter targetMesh = this.GetComponent<MeshFilter>();
                //Destroy(targetMesh);
				if (showImage) {
					StartCoroutine (ShowImage ());
				} else {
					this.GetComponent<MeshRenderer> ().enabled = false;
				}
            }
        }

        #endregion // UNITY_MONOBEHAVIOUR_METHODS

		IEnumerator ShowImage() {
			yield return new WaitForSeconds (0.3f);
			this.GetComponent<MeshRenderer> ().enabled = true;
		}

    }
}
