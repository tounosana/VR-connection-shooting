using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CreatePracticeBalloons : NetworkBehaviour {
	
	public GameObject red_balloon;
	public GameObject blue_balloon;
	public GameObject [] positions;
	public bool [] practicecheck= new bool[6]; 

	public bool P1_on = false;
	public bool P2_on = false;

	public bool P1_repractice = false;
	public bool P2_repractice = false;

	public float time = 0.0f;
	public bool create = false;
	public bool check = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ((P1_on == true) && (P2_on == true)) {
			time += Time.deltaTime;
			if ((time > 2.0f) && (create == false)) {
				if (isServer) {
					CmdCreatePracticeBalloons ();
					GameObject.Find ("Broadcast").GetComponent<Allbroadcast> ().broadcast = true;
				}
				create = true;
			}
		}
		check = true;
		for (var i = 0; i < practicecheck.Length; i++) {
			if (practicecheck [i] == false) {
				check = false;
			}
		}
		if (check == true) {
			for (var i = 0; i < practicecheck.Length; i++) {
				practicecheck [i] = false;
			}
			Debug.Log ("Practice Over!");
			GameObject.Find ("SelectBalloonPoints").GetComponent<SelectBalloonCreate> ().readyballooncreate = true;
			GameObject.Find ("SelectBalloonPoints").GetComponent<SelectBalloonCreate> ().wordnumber = 6;
			GameObject.Find ("SelectBalloonPoints").GetComponent<SelectBalloonCreate> ().go = true;
			GameObject.Find ("Broadcast").GetComponent<Allbroadcast> ().Playnumber = 1;
			GameObject.Find ("Broadcast").GetComponent<Allbroadcast> ().broadcastphase = 1;
			GameObject.Find ("Broadcast").GetComponent<Allbroadcast> ().CanPlay = true;
			GameObject.Find ("Broadcast").GetComponent<Allbroadcast> ().broadcast = true;
		}
		if ((P1_repractice == true) && (P2_repractice == true)) {
			time = 0.0f;
			create = false;
			P1_repractice = false;
			P2_repractice = false;
			GameObject.Find("FireWorkSets").GetComponent<StartAllFire>().Fire = true;
			GameObject.Find ("Broadcast").GetComponent<Allbroadcast> ().broadcastphase = 0;
			GameObject.Find ("Broadcast").GetComponent<Allbroadcast> ().Playnumber = 0;
			GameObject.Find ("Broadcast").GetComponent<Allbroadcast> ().CanPlay = false;
			//GameObject.Find ("Broadcast").GetComponent<Allbroadcast> ().broadcast = true;
			Cmdreset ();
		}
	}
	[Command]
	void CmdCreatePracticeBalloons (){
		for (var i = 0; i < positions.Length; i++) {
			if (i < 3) {
				GameObject createballoon = Instantiate (red_balloon);
				createballoon.transform.position = positions [i].transform.position;
				NetworkServer.Spawn (createballoon);
				RpcPracticeBalloonCreate (i,createballoon);
			}
			if (i >= 3) {
				GameObject createballoon = Instantiate (blue_balloon);
				createballoon.transform.position = positions [i].transform.position;
				NetworkServer.Spawn (createballoon);
				RpcPracticeBalloonCreate (i,createballoon);
			}
		}	
	}
	[Command]
	void Cmdreset (){
		int Quitballoon = GameObject.Find ("SelectBalloonPoints").GetComponent<SelectBalloonCreate> ().quitballoonpoints.Length;
		for (int i = 0; i < Quitballoon; i++) {
			NetworkServer.Destroy (GameObject.Find ("SelectBalloonPoints").GetComponent<SelectBalloonCreate> ().quitballoonpoints[i].transform.GetChild(0).gameObject);
		}
	}
	[ClientRpc]
	void RpcPracticeBalloonCreate (int number ,GameObject createballoon){
		if (number < 3) {
			createballoon.transform.parent = GameObject.Find ("PracticeBalloonPoints").GetComponent<CreatePracticeBalloons>().positions [number].transform;
			createballoon.GetComponent<Hit> ().BalloonNumber = number;
			createballoon.GetComponent<Hit> ().BalloonColor = "red";
			createballoon.GetComponent<Hit> ().WordNumber = 5;
		}
		if (number >= 3) {
			createballoon.transform.parent = GameObject.Find ("PracticeBalloonPoints").GetComponent<CreatePracticeBalloons>().positions [number].transform;
			createballoon.GetComponent<Hit> ().BalloonNumber = number;
			createballoon.GetComponent<Hit> ().BalloonColor = "blue";
			createballoon.GetComponent<Hit> ().WordNumber = 5;
		}
	}
}
