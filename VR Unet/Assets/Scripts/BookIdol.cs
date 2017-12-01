using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BookIdol : NetworkBehaviour {
	float x = 0;
	float y = 0;
	float z = 0;
	float my = 0;
	public float waverange = 1;
	// Use this for initialization
	void Start () {
		x = this.transform.position.x;
		my = this.transform.position.y;
		z = this.transform.position.z;
	}

	// Update is called once per frame
	void Update () {
		if (isServer) {
			CmdWave ();
		}
	}
	[Command]
	void CmdWave (){
		y = my + waverange *Mathf.Cos (Mathf.PI * Time.time);
		this.transform.position = new Vector3 (x,y,z);
		this.transform.Rotate (0.0f, 1.0f, 0.0f);
	}
}
