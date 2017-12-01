using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TakeAim : NetworkBehaviour {

	public bool right_take_up = false;
	public bool right_take_down = false;
	public bool left_take_up = false;
	public bool left_take_down = false;
	public bool right_istake = false;
	public bool left_istake = false;

	public GameObject Aim;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			
			right_take_up = this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().gripButtonDown;
			left_take_up = this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().gripButtonDown;

			right_take_down = this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().gripButtonUp;
			left_take_down = this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().gripButtonDown;

			right_istake = this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().canTake;
			left_istake = this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().canTake;

			string ControllerName;
			if (right_istake == true) {
				ControllerName = "Controller (right)";
				CmdTake (ControllerName);
				this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().canTake = false;
			}
			if (left_istake == true) {
				ControllerName = "Controller (left)";
				CmdTake (ControllerName);
				this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().canTake = false;
			}
		}
	}

	[Command]
	void CmdTake(string ControllerName){
		
		GameObject instance = Instantiate (Aim);
		instance.transform.position = this.transform.FindChild (ControllerName).transform.position;
		instance.transform.rotation = this.transform.FindChild (ControllerName).transform.rotation;
		instance.transform.parent = this.transform.FindChild (ControllerName).transform;
		instance.GetComponent<BoxCollider> ().enabled = false;
		//instance.GetComponent<Rigidbody> ().isKinematic = true;
		instance.GetComponent<DeleteAim> ().enabled = false;
		instance.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		NetworkServer.Spawn (instance);

		//this.GetComponent<TakeAim> ().Rpc_Take (ControllerName);
		this.GetComponent<TakeAim> ().Rpc_CTake (ControllerName,instance);
	}
	/*
	[RPC]
	void Rpc_Take(string ControllerName){
	
		GameObject instance = Instantiate (Aim);


		instance.name = "Aim";
		instance.transform.position = this.transform.FindChild (ControllerName).transform.position;
		instance.transform.rotation = this.transform.FindChild (ControllerName).transform.rotation;
		instance.transform.parent = this.transform.FindChild (ControllerName).transform;
		NetworkServer.Spawn (instance);
		instance.GetComponent<BoxCollider> ().enabled = false;
		//instance.GetComponent<Rigidbody> ().isKinematic = true;
		instance.GetComponent<DeleteAim> ().enabled = false;
		instance.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		this.GetComponent<TakeAim> ().Rpc_CTake (ControllerName,instance);
	}
*/
	[ClientRpc]
	void Rpc_CTake(string ControllerName,GameObject instance){

			instance.name = "Aim";
			//instance.transform.position = this.transform.FindChild (ControllerName).transform.position;
			//instance.transform.rotation = this.transform.FindChild (ControllerName).transform.rotation;
			instance.transform.parent = this.transform.FindChild (ControllerName).transform;
			instance.GetComponent<BoxCollider> ().enabled = false;
			//instance.GetComponent<Rigidbody> ().isKinematic = true;
			instance.GetComponent<DeleteAim> ().enabled = false;
			instance.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;

	}
}
