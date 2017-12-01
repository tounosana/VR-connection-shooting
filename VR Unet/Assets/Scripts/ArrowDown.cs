using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ArrowDown : NetworkBehaviour {

	private Rigidbody rigid;

	// Use this for initialization
	void Start () {
		rigid = this.GetComponent<Rigidbody> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.LookAt (transform.position + rigid.velocity);
	
	}


}
