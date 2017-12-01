using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class shootball : NetworkBehaviour {
	public string PlayCode;
	public GameObject Aim;

	public GameObject r_ShootAimOnBow=null;
	public GameObject l_ShootAimOnBow=null;

	public bool rightshoot=false;
	public bool leftshoot=false;

	public float shootpower=2000.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			rightshoot = this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().triggerButtonDown;
			leftshoot = this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().triggerButtonDown;

			if (this.gameObject.GetComponent<TakeAim> ().r_isLoadAim == true) {
				r_ShootAimOnBow = this.gameObject.GetComponent<TakeAim> ().AimOnBow;
			}
			if (this.gameObject.GetComponent<TakeAim> ().l_isLoadAim == true) {
				l_ShootAimOnBow = this.gameObject.GetComponent<TakeAim> ().AimOnBow;
			}
			string ControllerName;
			if (rightshoot == true && r_ShootAimOnBow != null) {
				ControllerName = "Controller (right)";
				this.transform.FindChild (ControllerName).GetComponent<WandController> ().TakeThings = false;
				CmdShootAim (ControllerName, PlayCode,r_ShootAimOnBow);
				this.gameObject.GetComponent<TakeAim> ().r_isLoadAim = false;
				this.gameObject.GetComponent<TakeAim> ().r_bowChange = true;

				r_ShootAimOnBow = null;
			}
			if (leftshoot == true && l_ShootAimOnBow != null) {
				ControllerName = "Controller (left)";


				CmdShootAim (ControllerName, PlayCode,l_ShootAimOnBow);
				this.gameObject.GetComponent<TakeAim> ().l_isLoadAim = false;
				this.gameObject.GetComponent<TakeAim> ().l_bowChange = true;
				l_ShootAimOnBow = null;
			}
			if (Input.GetKeyDown (KeyCode.Space)) {
				CmdReady (PlayCode);
			}
		}
		if (!isLocalPlayer) {
			this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().TakeThings = false;
			this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().enabled = false;
			this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().TakeThings = false;
			this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().enabled = false;
		}
	}

	[Command]
	void CmdShootAim(string ControllerName,string PlayCode,GameObject Aim){

		//GameObject instance = null;
		/*
		if (ControllerName == "Controller (left)") {
			instance = l_ShootAimOnBow;

		}
		if (ControllerName == "Controller (right)") {
			instance = r_ShootAimOnBow;

		}*/
		//GameObject instance = ShootAimOnBow;
		/*GameObject instance = Instantiate (Aim,this.transform.FindChild (ControllerName).transform.position +
			this.transform.FindChild (ControllerName).transform.forward*0.1f,
			this.transform.FindChild (ControllerName).transform.rotation,null) as GameObject;
			this.transform.FindChild (ControllerName).transform.Rotate (new Vector3 (45f, 0f, 0f));*/


		//instance.name = "Aim";
		//this.gameObject.GetComponent<TakeAim>().ControllerName=ControllerName;

		//instance.transform.parent = null;

		this.GetComponent<shootball> ().Rpc_rigid (ControllerName,Aim);

		//instance.gameObject.AddComponent<Rigidbody> ();
		//instance.GetComponent<Rigidbody> ().mass = 2f;
		//instance.gameObject.GetComponent<NetworkTransform> ().transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
		//instance.gameObject.GetComponent<NetworkTransform> ().enabled =true;
		//Aim.GetComponent<Rigidbody> ().AddForce (this.transform.FindChild (ControllerName).transform.FindChild("Bow").transform.forward*shootpower);
		Aim.GetComponent<ArrowDown> ().enabled = true;
		Aim.GetComponent<DeleteAim> ().enabled = true;

		Aim.tag = PlayCode;
		if(PlayCode == "P1"){
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_ShootCount += 1;
		}
		if(PlayCode == "P2"){
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_ShootCount += 1;
		}
		//NetworkServer.Spawn (instance);
		this.GetComponent<shootball> ().Rpc_Shoot (ControllerName,PlayCode,Aim);


	}
	[Command]
	void CmdReady (string Code){
		if (Code == "P1") {
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_Ready = true;
		}
		if (Code == "P2") {
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_Ready = true;
		}
	}

	[ClientRpc]
	void Rpc_Shoot(string ControllerName,string PlayCode,GameObject Aim)
	{
		/*
		//GameObject instance = ShootAimOnBow;

		//instance.name = "shootAim";
		instance.transform.parent = null;
		//instance.transform.position = this.transform.FindChild (ControllerName).transform.position;
		//instance.transform.rotation = this.transform.FindChild (ControllerName).transform.rotation;
		instance.GetComponent<Rigidbody> ().mass = 2f;
		instance.GetComponent<Rigidbody> ().AddForce (this.transform.FindChild (ControllerName).transform.FindChild("Bow").transform.forward*shootpower);
		instance.GetComponent<ArrowDown> ().enabled = true;
		instance.GetComponent<DeleteAim> ().enabled = true;
		instance.gameObject.GetComponent<NetworkTransform> ().transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
		*/
		Aim.tag = PlayCode;
		if(PlayCode == "P1"){
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_ShootCount += 1;
		}
		if(PlayCode == "P2"){
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_ShootCount += 1;
		}


	}

	[ClientRpc]
	void Rpc_rigid(string ControllerName,GameObject Aim){
		//GameObject instance = ShootAimOnBow;

		Debug.Log (Aim);
		//instance.name = "shootAim";

		Aim.transform.parent = null;
		//instance.transform.position = this.transform.FindChild (ControllerName).transform.position;
		//instance.transform.rotation = this.transform.FindChild (ControllerName).transform.rotation;
		//instance.gameObject.AddComponent<Rigidbody> ();
		//Aim.GetComponent<NetworkTransform> ().enabled =true;
		Aim.GetComponent<Rigidbody> ().mass = 0.5f;
		Aim.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		Aim.GetComponent<Rigidbody>().isKinematic = false;
		Aim.GetComponent<Rigidbody> ().AddForce (this.transform.FindChild (ControllerName).transform.FindChild("Bow").transform.forward*shootpower);
		Aim.GetComponent<Collider> ().isTrigger = false;
		Aim.GetComponent<ArrowDown> ().enabled = true;
		Aim.GetComponent<DeleteAim> ().enabled = true;

		//Aim.gameObject.GetComponent<NetworkTransform> ().transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
	}
		
}
