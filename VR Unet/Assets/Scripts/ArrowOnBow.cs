using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ArrowOnBow : NetworkBehaviour {

	public GameObject HandContorller = null;


	public bool is_Hold_Arrow = false;
	public bool is_calcu = false;

	public bool is_on_Bow = false;

	public Vector3 lastVec;
	public Vector3 newVec;

	public Vector3 holdpoint;
	public Vector3 snappoint;

	public Vector3 arrowpoint;



	public float angle;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//arrowpoint = this.transform.FindChild ("Arrow").transform.FindChild ("ArrowObject").transform.FindChild ("Tip").transform.position;

		//lastVec = arrowpoint - 

		if (HandContorller != null) {
			//holdpoint = HandContorller.transform.parent.GetComponent<TakeAim> ().holdpos;
			if (HandContorller.name == "Controller (right)") {
				is_Hold_Arrow = HandContorller.transform.parent.GetComponent<TakeAim> ().l_holdingArrow;
			}
			if (HandContorller.name == "Controller (left)") {
				is_Hold_Arrow = HandContorller.transform.parent.GetComponent<TakeAim> ().r_holdingArrow;
			}

			//is_Hold_Arrow = HandContorller.transform.parent.GetComponent<TakeAim> ().holdingArrow;

			//lastVec =  holdpoint-arrowpoint;
		}
		/*
		if (is_calcu == true && HandContorller != null) {
			holdpoint = HandContorller.transform.parent.GetComponent<TakeAim> ().holdpos;
			newVec = HandContorller.transform.position-holdpoint;
			angle = angle_360 (lastVec.normalized, newVec.normalized);
			//Debug.Log (angle);
			is_on_Bow = true;

		}*/
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Hand") {
			//Debug.Log("Y");
			HandContorller = other.gameObject;

		}

		if (other.tag == "Bow" && is_Hold_Arrow) {
			if (other.gameObject.transform.parent.transform.parent.transform.parent.GetComponent<TakeAim> ().rnl_isLoadAim == false) {
				other.gameObject.transform.parent.transform.parent.transform.parent.GetComponent<TakeAim> ().Detect = this.gameObject;
				is_on_Bow = true;
			}

			//Destroy (this.gameObject.GetComponent<Rigidbody> ());
		}	
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Hand") {
			//Debug.Log("n");
			//HandContorller = other.gameObject;

		}
		/*
		if (other.tag == "Bow" && is_Hold_Arrow) {
			other.gameObject.transform.parent.transform.parent.transform.parent.GetComponent<TakeAim> ().Detect = this.gameObject;
			is_on_Bow = true;
			//Destroy (this.gameObject.GetComponent<Rigidbody> ());
		}	*/
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Bow") {
			//other.gameObject.transform.parent.transform.parent.transform.parent.GetComponent<TakeAim> ().Detect = null;
		}
	}
	/*
	float angle_360(Vector3 before ,Vector3 after )
	{
		Vector3 angle = Vector3.Cross (before, after);
		if (angle.z > 0)
			return Vector3.Angle (before, after);
		else
			return 0 - Vector3.Angle (before, after);
	}*/
}
