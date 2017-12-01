using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SelectBalloonCreate : NetworkBehaviour {
	public bool go = false;
	public bool readyballooncreate = false;
	public bool quitballooncreate = false;
	public Material [] word;

	public int wordnumber = 6;

	public GameObject[] readyballoonpoints;
	public GameObject[] quitballoonpoints;

	public GameObject readyballoon;
	public GameObject quitballoon;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (go == true) {
			CmdSelectBalloonCreate ();
			go = false;
			readyballooncreate = false;
			quitballooncreate = false;
		}
	}
	[Command]
	void CmdSelectBalloonCreate (){
		if (readyballooncreate == true) {
			for (var i = 0; i < readyballoonpoints.Length; i++) {
				GameObject createballoon = Instantiate (readyballoon);
				createballoon.transform.position = readyballoonpoints [i].transform.position;
				if (wordnumber == 6) {
					createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [0];
				} else{
					createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [1];
				}
				NetworkServer.Spawn (createballoon);
				RpcReadyBalloonCreate (i, createballoon,wordnumber);
			}
		}
		if (quitballooncreate == true) {
			for (var i = 0; i < quitballoonpoints.Length; i++) {
				GameObject createballoon = Instantiate (quitballoon);
				createballoon.transform.position = quitballoonpoints [i].transform.position;
				NetworkServer.Spawn (createballoon);
				RpcQuitBalloonCreate (i, createballoon);
			}
		}
	}
	[ClientRpc]
	void RpcReadyBalloonCreate (int number,GameObject balloon,int wordnumber){
		if (wordnumber == 6) {
			balloon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [0];
		} else{
			balloon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [1];
		}
		balloon.transform.parent = GameObject.Find ("SelectBalloonPoints").GetComponent<SelectBalloonCreate>().readyballoonpoints[number].transform;
		balloon.GetComponent<Hit> ().BalloonColor = "green";
		balloon.GetComponent<Hit> ().WordNumber = wordnumber;
		balloon.GetComponentInChildren<ParticleSystem> ().Play ();
	}
	[ClientRpc]
	void RpcQuitBalloonCreate (int number,GameObject balloon){
		balloon.transform.parent = GameObject.Find ("SelectBalloonPoints").GetComponent<SelectBalloonCreate>().quitballoonpoints[number].transform;
		balloon.GetComponent<Hit> ().BalloonColor = "orange";
		balloon.GetComponent<Hit> ().WordNumber = GameObject.Find ("SelectBalloonPoints").GetComponent<SelectBalloonCreate>().wordnumber;
		balloon.GetComponentInChildren<ParticleSystem> ().Play ();
	}
}
