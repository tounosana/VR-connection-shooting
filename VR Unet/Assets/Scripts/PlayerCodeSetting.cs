using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCodeSetting : NetworkBehaviour {
	public GameObject P1Point;
	public GameObject P2Point;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		P1Point = GameObject.Find("P1StartPoint");
		P2Point = GameObject.Find("P2StartPoint");
		Vector3 VecTortoP1 = this.transform.position - P1Point.transform.position;
		Vector3 VecTortoP2 = this.transform.position - P2Point.transform.position;
		float LengthToP1 = Mathf.Pow (VecTortoP1.x, 2) + Mathf.Pow (VecTortoP1.y, 2) + Mathf.Pow (VecTortoP1.z, 2);
		float LengthToP2 = Mathf.Pow (VecTortoP2.x, 2) + Mathf.Pow (VecTortoP2.y, 2) + Mathf.Pow (VecTortoP2.z, 2);

		if (LengthToP1 < LengthToP2) {
			this.gameObject.GetComponent<shootball> ().PlayCode = "P1";
			GameObject.Find ("PracticeBalloonPoints").GetComponent<CreatePracticeBalloons> ().P1_on = true;
		} else {
			this.gameObject.GetComponent<shootball> ().PlayCode = "P2";
			GameObject.Find ("PracticeBalloonPoints").GetComponent<CreatePracticeBalloons> ().P2_on = true;
		}
	}

}
