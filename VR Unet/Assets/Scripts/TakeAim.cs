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
	public bool right_take_stay = false;
	public bool left_take_stay = false;

	public bool r_canTakeThings = false;
	public bool l_canTakeThings = false;
	public bool canTakeBow = false;
	public bool holdingArrow = false;
	public bool r_holdingArrow = false;
	public bool l_holdingArrow = false;
	public bool holdingBow_r = false;
	public bool BowOnRight = false;
	public bool holdingBow_l = false;
	public bool BowOnLeft = false;

	public bool putAimOn = false;
	public bool trans = false;

	public bool r_isLoadAim = false;
	public bool l_isLoadAim = false;
	public bool rnl_isLoadAim = false;

	public bool r_bowChange = false;
	public bool l_bowChange = false;

	public GameObject Detect = null;
	//public bool rigid_is=false;
	public GameObject AimOnBow = null;


	public Vector3 holdpos;

	public float rotateAngle;

	//public GameObject Aim;

	public Vector3 rotateBow = new Vector3 (20f,0f,0f);


	public AudioClip LoadAimSound;
	public AudioClip ShootAimSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {

			r_canTakeThings = (this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().TakeThings);
			l_canTakeThings = (this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().TakeThings);

			canTakeBow = this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().TakeBow || (this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().TakeBow);

			right_take_up = this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().gripButtonDown;
			left_take_up = this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().gripButtonDown;

			right_take_down = this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().gripButtonUp;
			left_take_down = this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().gripButtonUp;

			right_take_stay = this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().gripButtonPressed;
			left_take_stay = this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().gripButtonPressed;

			right_istake = this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().canTake;
			left_istake = this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().canTake;
			//Debug.Log ("r  " + r_holdingArrow);
			//Debug.Log ("l  " + l_holdingArrow);

			if (l_isLoadAim || r_isLoadAim) {
				rnl_isLoadAim = true;
			} else {
				rnl_isLoadAim = false;
			}

			string ControllerName;


			if (right_istake == true && l_isLoadAim == false && holdingBow_r == false) {
				ControllerName = "Controller (right)";
				CmdTake (ControllerName);

				r_holdingArrow = true;

				this.transform.FindChild ("Controller (right)").GetComponent<WandController> ().canTake = false;



			} else if (right_take_down && this.transform.FindChild ("Controller (right)").transform.FindChild("Aim")!=null  && holdingBow_r == false) {
				ControllerName = "Controller (right)";
				CmdPutDown (ControllerName);

				r_holdingArrow = false;
				//Debug.Log ("r");
			}
			if (left_istake == true && r_isLoadAim == false && holdingBow_l == false) {
				ControllerName = "Controller (left)";
				CmdTake (ControllerName);

				l_holdingArrow = true;

				this.transform.FindChild ("Controller (left)").GetComponent<WandController> ().canTake = false;
			} else if (left_take_down && this.transform.FindChild ("Controller (left)").transform.FindChild("Aim")!=null && holdingBow_l == false) {
				ControllerName = "Controller (left)";
				CmdPutDown (ControllerName);
				l_holdingArrow = false;
				//Debug.Log ("l");
			}


			if (right_take_up == true && r_canTakeThings ==true && holdingBow_r == false && l_isLoadAim ==false ) {
				ControllerName = "Controller (right)";
				CmdTakeThings (ControllerName);
				r_holdingArrow = true;
			}
			if (left_take_up == true && l_canTakeThings ==true && holdingBow_l == false && r_isLoadAim == false ) {
				ControllerName = "Controller (left)";
				CmdTakeThings (ControllerName);
				l_holdingArrow = true;
			}


			if (right_take_up == true && canTakeBow == true && holdingBow_r ==false && l_isLoadAim ==false ) {
				ControllerName = "Controller (right)";
				CmdTakeBow (ControllerName);
				holdingBow_r = true;
				holdingBow_l = false;
				//Debug.Log ("here");
			}
			if (left_take_up == true && canTakeBow == true && holdingBow_l == false && r_isLoadAim ==false ) {
				ControllerName = "Controller (left)";
				CmdTakeBow (ControllerName);
				holdingBow_l = true;
				holdingBow_r = false;
			}

			if (holdingBow_r == true) {
				
				//holdpos = this.transform.FindChild ("Controller (right)").transform.FindChild ("Bow").transform.FindChild("Handle").transform.position;
				holdingBow_l = false;
				//BowOnRight = true;
				//BowOnLeft = false;

			
			}
			if (holdingBow_l == true) {
				//holdpos = this.transform.FindChild ("Controller (left)").transform.FindChild ("Bow").transform.FindChild("Handle").transform.position;
				holdingBow_r = false;
				//BowOnLeft = true;
				//BowOnRight = false;
			}


			string transName;

			if (holdingBow_r == true && l_holdingArrow == true && Detect != null && left_take_stay == true) {
				Debug.Log ("yes");
				ControllerName = "Controller (right)";
				transName = "Controller (left)";
				//if (this.transform.FindChild ("Controller (left)").transform.FindChild ("Aim").GetComponent<ArrowOnBow> ().is_on_Bow == true)
				//if (Detect != null) {
					CmdPutAimOnBow (ControllerName, transName, Detect);
					Detect = null;
				//}

				this.transform.FindChild (ControllerName).transform.FindChild ("Bow").GetComponent<AudioSource> ().Stop ();
				this.transform.FindChild (ControllerName).transform.FindChild ("Bow").GetComponent<AudioSource> ().clip = LoadAimSound;
				this.transform.FindChild (ControllerName).transform.FindChild ("Bow").GetComponent<AudioSource> ().Play ();

				this.transform.FindChild(ControllerName).GetComponent<WandController>().isShake=true;



				holdingBow_l = false;
				l_holdingArrow = false;
				//Debug.Log("one");
				trans=true;
			}
			if (holdingBow_l == true && r_holdingArrow == true && Detect != null && right_take_stay == true) {
				Debug.Log ("yes");
				ControllerName = "Controller (left)";
				transName = "Controller (right)";
				//if (Detect != null) {
					CmdPutAimOnBow (ControllerName, transName, Detect);
					Detect = null;
				//}

				this.transform.FindChild (ControllerName).transform.FindChild ("Bow").GetComponent<AudioSource> ().Stop ();
				this.transform.FindChild (ControllerName).transform.FindChild ("Bow").GetComponent<AudioSource> ().clip = LoadAimSound;
				this.transform.FindChild (ControllerName).transform.FindChild ("Bow").GetComponent<AudioSource> ().Play ();

				this.transform.FindChild(ControllerName).GetComponent<WandController>().isShake=true;



				holdingBow_r = false;
				r_holdingArrow = false;
				//Debug.Log("one");
				trans=true;
			}

			if (r_bowChange == true && r_isLoadAim == false && trans ==true) {
				ControllerName = "Controller (right)";
				//GameObject up = this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.FindChild ("Bow").transform.FindChild ("UpperLimb").transform.FindChild ("StringAttachPoint").gameObject;
				//GameObject lower=this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.FindChild ("Bow").transform.FindChild ("LowerLimb").transform.FindChild ("StringAttachPoint").gameObject;

				this.transform.FindChild (ControllerName).transform.FindChild ("Bow").GetComponent<AudioSource> ().Stop ();
				this.transform.FindChild (ControllerName).transform.FindChild ("Bow").GetComponent<AudioSource> ().clip = ShootAimSound;
				this.transform.FindChild (ControllerName).transform.FindChild ("Bow").GetComponent<AudioSource> ().Play ();

				CmdChageBowOrig (ControllerName);
				this.transform.FindChild(ControllerName).GetComponent<WandController>().isShake=true;


				//ControllerName = null;
				//r_bowChange = false;
				trans=false;
				Debug.Log("r");

			}
			if (l_bowChange == true && l_isLoadAim == false && trans ==true) {
				//Debug.Log ("y");
				ControllerName = "Controller (left)";
				//GameObject up = this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.FindChild ("Bow").transform.FindChild ("UpperLimb").transform.FindChild ("StringAttachPoint").gameObject;
				//GameObject lower=this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.FindChild ("Bow").transform.FindChild ("LowerLimb").transform.FindChild ("StringAttachPoint").gameObject;

				this.transform.FindChild (ControllerName).transform.FindChild ("Bow").GetComponent<AudioSource> ().Stop ();
				this.transform.FindChild (ControllerName).transform.FindChild ("Bow").GetComponent<AudioSource> ().clip = ShootAimSound;
				this.transform.FindChild (ControllerName).transform.FindChild ("Bow").GetComponent<AudioSource> ().Play ();
				//ControllerName = null;

				CmdChageBowOrig (ControllerName);
				this.transform.FindChild(ControllerName).GetComponent<WandController>().isShake=true;


				//l_bowChange = false;
				trans=false;
				Debug.Log("l");
			}

			//if (BowOnRight == true) {
				
			//	rotateAngle = this.transform.FindChild ("Controller (right)").transform.FindChild("Bow").transform.FindChild ("Aim").GetComponent<ArrowOnBow> ().angle;
			//	this.transform.FindChild ("Controller (right)").transform.FindChild ("Bow").transform.Rotate (new Vector3 (rotateAngle, 0f, 0f));
			//}

		}



	}

	[Command]
	void CmdTake(string ControllerName){
		
		GameObject instance = Instantiate (Resources.Load("Arrow")as GameObject);
		instance.transform.position = this.transform.FindChild (ControllerName).transform.position;
		instance.transform.rotation = this.transform.FindChild (ControllerName).transform.rotation;
		instance.transform.parent = this.transform.FindChild (ControllerName).transform;
		//instance.GetComponent<BoxCollider> ().enabled = false;
		//instance.GetComponent<Rigidbody> ().isKinematic = true;
		instance.GetComponent<DeleteAim> ().enabled = false;
		instance.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		//instance.GetComponent<Rigidbody> ().isKinematic = true;
		NetworkServer.Spawn (instance);

		//this.GetComponent<TakeAim> ().Rpc_Take (ControllerName);
		this.GetComponent<TakeAim> ().Rpc_CTake (ControllerName,instance);
	}

	[Command]
	void CmdPutDown(string ControllerName){
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		//this.transform.FindChild (ControllerName).transform.FindChild ("Aim").position = this.transform.FindChild (ControllerName).transform.position;
		//this.transform.FindChild (ControllerName).transform.FindChild ("Aim").rotation = this.transform.FindChild (ControllerName).transform.rotation;
		//this.transform.FindChild (ControllerName).transform.FindChild ("Aim").GetComponent<Rigidbody> ().isKinematic = false;
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").GetComponent<ArrowDown>().enabled = false;
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").GetComponent<BoxCollider> ().enabled = true;
		//this.transform.FindChild (ControllerName).transform.FindChild ("Aim").transform.parent=null;
		//holdingArrow = false;
		this.GetComponent<TakeAim> ().Rpc_CPutDown (ControllerName);
	}

	[Command]
	void CmdTakeThings(string ControllerName){
		
		this.transform.FindChild (ControllerName).GetComponent<WandController> ().takeObj.transform.parent = this.transform.FindChild (ControllerName).transform;
		//this.transform.FindChild (ControllerName).transform.FindChild ("Aim").GetComponent<BoxCollider> ().enabled = false;
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").transform.position = this.transform.FindChild (ControllerName).transform.position;
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").transform.rotation = this.transform.FindChild (ControllerName).transform.rotation;

		//holdingArrow = true;

		this.GetComponent<TakeAim> ().Rpc_CTakeThings (ControllerName);
	}

	[Command]
	void CmdTakeBow(string ControllerName){
		


		this.transform.FindChild (ControllerName).GetComponent<WandController> ().takeBowObj.transform.name="Bow";
		this.transform.FindChild (ControllerName).GetComponent<WandController> ().takeBowObj.transform.parent = this.transform.FindChild (ControllerName).transform;
		this.transform.FindChild (ControllerName).transform.FindChild ("Bow").GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.position = this.transform.FindChild (ControllerName).transform.position;
		this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.Translate (new Vector3 (0f, 0f, 0.2f));
		this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.rotation = this.transform.FindChild (ControllerName).transform.rotation;
		this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.Rotate(new Vector3 (45f,0f,90f));

		//holdpos = this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.position;
		this.GetComponent<TakeAim> ().Rpc_CTakeBow (ControllerName);
	}

	[Command]
	void CmdPutAimOnBow(string ControllerName,string transName,GameObject Detect){

		GameObject Arrow = Detect;

		this.GetComponent<TakeAim>().Rpc_CPutAimOnBow(ControllerName, transName, Arrow);

		Arrow.gameObject.transform.parent = this.transform.FindChild (ControllerName);
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").transform.position = 
			this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.FindChild ("Hold").transform.position;
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").transform.rotation = 
			this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.FindChild ("Hold").transform.rotation;

		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").transform.Rotate (new Vector3(0f,-45f,0f));
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").transform.Translate (new Vector3 (0.082f, 0f, -0.085f));


	}

	[Command]
	void CmdChageBowOrig(string Name){


		//GameObject up=this.transform.FindChild (Name).transform.FindChild ("Bow").transform.FindChild ("Bow").transform.FindChild ("UpperLimb").transform.FindChild ("StringAttachPoint").gameObject;
		//GameObject lower=this.transform.FindChild (Name).transform.FindChild ("Bow").transform.FindChild ("Bow").transform.FindChild ("LowerLimb").transform.FindChild ("StringAttachPoint").gameObject;

		this.gameObject.GetComponent<TakeAim> ().Rpc_CChageBowOrig (Name);
		/*
		up.transform.Rotate(new Vector3(-30f,0f,0f));
		up.transform.localScale -= new Vector3 (0f, 0.15f, 0f);
		lower.transform.Rotate(new Vector3(30f,0f,0f));
		lower.transform.localScale -= new Vector3 (0f, 0.15f, 0f);
*/
		AimOnBow = null;
		//bowChange = false;
		if (Name == "Controller (left)") {
			l_bowChange = false;

		}
		if (Name == "Controller (right)") {

			r_bowChange = false;
		}
		//this.gameObject.GetComponent<TakeAim> ().Rpc_CChageBowOrig (Name);
	}

	[ClientRpc]
	void Rpc_CTake(string ControllerName,GameObject instance){

		instance.name = "Aim";
		instance.transform.position = this.transform.FindChild (ControllerName).transform.position;
		instance.transform.rotation = this.transform.FindChild (ControllerName).transform.rotation;
		instance.transform.parent = this.transform.FindChild (ControllerName).transform;
		//instance.GetComponent<BoxCollider> ().enabled = false;

		instance.GetComponent<DeleteAim> ().enabled = false;
		instance.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;

		//instance.GetComponent<Rigidbody> ().isKinematic = true;

	}

	[ClientRpc]
	void Rpc_CPutDown(string ControllerName){
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		//this.transform.FindChild (ControllerName).transform.FindChild ("Aim").position = this.transform.FindChild (ControllerName).transform.position;
		//this.transform.FindChild (ControllerName).transform.FindChild ("Aim").rotation = this.transform.FindChild (ControllerName).transform.rotation;
		//this.transform.FindChild (ControllerName).transform.FindChild ("Aim").GetComponent<Rigidbody> ().isKinematic = false;
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").GetComponent<ArrowDown>().enabled = false;
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").GetComponent<BoxCollider> ().enabled = true;
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").transform.parent=null;
		//holdingArrow = false;
	}

	[ClientRpc]
	void Rpc_CTakeThings(string ControllerName){
		this.transform.FindChild (ControllerName).GetComponent<WandController> ().takeObj.transform.parent = this.transform.FindChild (ControllerName).transform;
		//this.transform.FindChild (ControllerName).transform.FindChild ("Aim").GetComponent<BoxCollider> ().enabled = false;
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").transform.position = this.transform.FindChild (ControllerName).transform.position;
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").transform.rotation = this.transform.FindChild (ControllerName).transform.rotation;
		//holdingArrow = true;
	}

	[ClientRpc]
	void Rpc_CTakeBow(string ControllerName){
		this.transform.FindChild (ControllerName).GetComponent<WandController> ().takeBowObj.transform.name="Bow";
		this.transform.FindChild (ControllerName).GetComponent<WandController> ().takeBowObj.transform.parent = this.transform.FindChild (ControllerName).transform;
		this.transform.FindChild (ControllerName).transform.FindChild ("Bow").GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.position = this.transform.FindChild (ControllerName).transform.position;
		this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.Translate (new Vector3 (0f, 0f, 0.2f));
		this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.rotation = this.transform.FindChild (ControllerName).transform.rotation;
		this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.Rotate(new Vector3 (45f,0f,90f));
		//holdpos = this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.position;
	}

	[ClientRpc]
	void Rpc_CPutAimOnBow(string ControllerName,string transName, GameObject Arrow){
		
	
		Arrow.gameObject.transform.parent = this.transform.FindChild (ControllerName);

		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").transform.position = 
			this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.FindChild ("Hold").transform.position;
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").transform.rotation = 
			this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.FindChild ("Hold").transform.rotation;
		
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").transform.Rotate (new Vector3(0f,-45f,0f));
		this.transform.FindChild (ControllerName).transform.FindChild ("Aim").transform.Translate (new Vector3 (0.082f, 0f, -0.085f));

		//Destroy( this.transform.FindChild (ControllerName).transform.FindChild ("Aim").GetComponent<Rigidbody> ());

		GameObject up=this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.FindChild ("Bow").transform.FindChild ("UpperLimb").transform.FindChild ("StringAttachPoint").gameObject;
		GameObject lower=this.transform.FindChild (ControllerName).transform.FindChild ("Bow").transform.FindChild ("Bow").transform.FindChild ("LowerLimb").transform.FindChild ("StringAttachPoint").gameObject;

		up.transform.Rotate(new Vector3(30f,0f,0f));
		up.transform.localScale += new Vector3 (0f, 0.15f, 0f);
		lower.transform.Rotate(new Vector3(-30f,0f,0f));
		lower.transform.localScale += new Vector3 (0f, 0.15f, 0f);
		
		Arrow.gameObject.GetComponent<ArrowOnBow> ().enabled = false;

		AimOnBow = this.transform.FindChild (ControllerName).transform.FindChild ("Aim").gameObject;
		if (ControllerName == "Controller (left)") {
			l_isLoadAim = true;
			r_isLoadAim = false;
			l_bowChange = true;
			r_bowChange = false;

			r_holdingArrow = false;
		}
		if (ControllerName == "Controller (right)") {
			r_isLoadAim = true;
			l_isLoadAim = false;
			r_bowChange = true;
			l_bowChange = false;

			l_holdingArrow = false;
		}
		//bowChange = true;
		//}
	}

	[ClientRpc]
	void Rpc_CChageBowOrig(string Name){
		
		GameObject up=this.transform.FindChild (Name).transform.FindChild ("Bow").transform.FindChild ("Bow").transform.FindChild ("UpperLimb").transform.FindChild ("StringAttachPoint").gameObject;
		GameObject lower=this.transform.FindChild (Name).transform.FindChild ("Bow").transform.FindChild ("Bow").transform.FindChild ("LowerLimb").transform.FindChild ("StringAttachPoint").gameObject;

		up.transform.Rotate(new Vector3(-30f,0f,0f));
		up.transform.localScale -= new Vector3 (0f, 0.15f, 0f);
		lower.transform.Rotate(new Vector3(30f,0f,0f));
		lower.transform.localScale -= new Vector3 (0f, 0.15f, 0f);
		Debug.Log ("2");

		AimOnBow = null;

		if (Name == "Controller (left)") {
			l_bowChange = false;

		}
		if (Name == "Controller (right)") {

			r_bowChange = false;
		}
	}
}
