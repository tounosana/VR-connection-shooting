using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Follow :NetworkBehaviour {
	public GameObject Object;
	public GameObject target;
	public Vector3 reposition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			Object.transform.position = target.transform.position;
			Object.transform.localPosition += reposition;

			Object.transform.rotation = target.transform.rotation;
			Object.transform.FindChild ("ghost04").gameObject.SetActive (false);

		} else {
			Object.transform.position = target.transform.position;
			Object.transform.localPosition += reposition;

			Object.transform.rotation = target.transform.rotation;
		}

	}

}
