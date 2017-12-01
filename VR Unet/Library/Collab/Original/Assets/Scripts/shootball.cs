using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class shootball : NetworkBehaviour {

	public bool rightshoot=false;
	public bool leftshoot=false;

	public float shootpower=800.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			rightshoot = this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().triggerButtonDown;
			leftshoot = this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().triggerButtonDown;

			if (rightshoot == true) {
				CmdShootAim_right ();
			}
			if (leftshoot == true) {
				CmdShootAim_left ();
			}
		}
		if (!isLocalPlayer) {
			this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().enabled = false;
			this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().enabled = false;
		}
	}

	[Command]
	void CmdShootAim_right(){
		this.GetComponent<shootball> ().RPC_right ();
	}
	void CmdShootAim_left(){
		this.GetComponent<shootball> ().RPC_left ();
	}

	[RPC]
	void RPC_right()
	{
		GameObject instance = Instantiate (Resources.Load ("ball"))as GameObject;

		instance.name = "Aim";
		instance.transform.position = this.transform.FindChild ("Controller (right)").transform.position;
		instance.GetComponent<Rigidbody> ().AddForce (this.transform.FindChild ("Controller (right)").transform.forward*shootpower);

		NetworkServer.Spawn (instance);
	}
	void RPC_left()
	{
		GameObject instance = Instantiate (Resources.Load ("ball"))as GameObject;

		instance.name = "Aim";
		instance.transform.position = this.transform.FindChild ("Controller (left)").transform.position;
		instance.GetComponent<Rigidbody> ().AddForce (this.transform.FindChild ("Controller (left)").transform.forward*shootpower);

		NetworkServer.Spawn (instance);
	}
}
