using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Hit : NetworkBehaviour {
	public int BalloonNumber;
	public string BalloonColor;
	public int WordNumber;
	public GameObject paticles;
	public GameObject pointshow;
	public Material [] pointwords;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter(Collision collision){
		if (BalloonColor == "green") {
			if (collision.gameObject.tag == this.tag) {
				CmdHit (collision.gameObject, BalloonColor, WordNumber);
			}
		}else if(BalloonColor == "orange"){
			if (collision.gameObject.tag == this.tag) {
				CmdHit (collision.gameObject, BalloonColor, WordNumber);
			}
		}
		else {
			if (isServer) {
				CmdHit (collision.gameObject, BalloonColor, WordNumber);
			}
		}
	
	}
	[Command]
	void CmdHit(GameObject Obj,string Color,int WordNumber){
		//GameObject BalloonPoints = GameObject.Find ("BalloonPoints").gameObject;
		//GameObject PracticeBalloonPoints = GameObject.Find ("PracticeBalloonPoints").gameObject;

		GameObject PointShow = Instantiate (pointshow);
		int pointword = 0;
		if (Obj.tag == "P1") {
			if (Color == "red") {
				GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_Score += 1;
				PointShow.GetComponentInChildren<ParticleSystemRenderer> ().material = pointwords [0];
				pointword = 0;
				if (WordNumber < 5) {
					GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_WordGet [WordNumber] = true;
					GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_HitPosition = this.transform.position;
					Rpc_ScoreChange (1);
				} else {
					GameObject.Find ("PracticeBalloonPoints").GetComponent<CreatePracticeBalloons> ().practicecheck [BalloonNumber] = true;
					Rpc_Practicehit (1);
				}
			} 
			else if (Color == "blue") {
				GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_Score -= 1;
				PointShow.GetComponentInChildren<ParticleSystemRenderer> ().material = pointwords [1];
				pointword = 1;
				if (WordNumber < 5) {
					for (var i = 0; i < GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_WordGet.Length; i++) {
						GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_WordGet [i] = false;
					}
					Rpc_ScoreChange (2);
				} else {
					GameObject.Find ("PracticeBalloonPoints").GetComponent<CreatePracticeBalloons> ().practicecheck [BalloonNumber] = true;
					Rpc_Practicehit (2);
				}
			} 
			else if(Color == "green"){
				pointword = 2;
				if (WordNumber == 6) {
					GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_Ready = true;
				} else {
					GameObject.Find ("PracticeBalloonPoints").GetComponent<CreatePracticeBalloons> ().P1_repractice = true;
				}
			}
			else{
				pointword = 2;
				if (isServer) {
					CmdQuit ();
				} else {
					Application.Quit ();
				}
			}
		}
		if (Obj.tag == "P2") {
			if (Color == "blue") {
				GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_Score += 1;
				PointShow.GetComponentInChildren<ParticleSystemRenderer> ().material = pointwords [0];
				pointword = 0;
				if (WordNumber < 5) {
					GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_WordGet [WordNumber] = true;
					GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_HitPosition = this.transform.position;
					Rpc_ScoreChange (3);
				} else {
					GameObject.Find ("PracticeBalloonPoints").GetComponent<CreatePracticeBalloons> ().practicecheck [BalloonNumber] = true;
					Rpc_Practicehit (3);
				}
			} 
			else if(Color == "red"){
				GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_Score -= 1;
				PointShow.GetComponentInChildren<ParticleSystemRenderer> ().material = pointwords [1];
				pointword = 1;
				if (WordNumber < 5) {
					for (var i = 0; i < GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_WordGet.Length; i++) {
						GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_WordGet[i]=false;
					}
					Rpc_ScoreChange (4);
				} else {
					GameObject.Find ("PracticeBalloonPoints").GetComponent<CreatePracticeBalloons> ().practicecheck [BalloonNumber] = true;
					Rpc_Practicehit (4);
				}
			}
			else if(Color == "green"){
				pointword = 2;
				if (WordNumber == 6) {
					GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_Ready = true;
				} else {
					GameObject.Find ("PracticeBalloonPoints").GetComponent<CreatePracticeBalloons> ().P2_repractice = true;
				}
			}
			else{
				pointword = 2;
				if (isServer) {
					CmdQuit ();
				} else {
					Application.Quit ();
				}
			}
		}
		if (WordNumber < 5) {
			if ((((GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().ReCreateBallooList.Count+1) * 3.0f) 
				< (GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().countdowntime - 1.0f))) {
				GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().EmptyPositions.Add (BalloonNumber);
				BalloonData data = new BalloonData ();
				data.BalloonColor = Color;
				data.WordNumber = WordNumber;
				GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().ReCreateBallooList.Add (data);
			}
		}
		GameObject createpaticle = Instantiate (paticles);
		createpaticle.transform.position = this.transform.position;
		PointShow.transform.position = this.transform.position + new Vector3(0.0f,0.5f,0.0f);
		NetworkServer.Spawn (createpaticle);
		NetworkServer.Spawn (PointShow);
		createpaticle.GetComponent<ParticleSystem> ().Play ();
		if (pointword < 2) {
			PointShow.GetComponent<ParticleSystem> ().Play ();
		}
		createpaticle.GetComponent<AudioSource> ().Play ();

		Rpc_Createpaticle (createpaticle,PointShow,pointword, this.gameObject);

		NetworkServer.Destroy (Obj);
		NetworkServer.Destroy (this.gameObject);
	}
	[Command]
	void CmdQuit(){
		Rpc_Quit ();
		Application.Quit ();
	}
	[ClientRpc]
	void Rpc_ScoreChange (int nomber){
		//GameObject BalloonPoints = GameObject.Find ("BalloonPoints").gameObject;
		if (nomber == 1) {
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_Score += 1;
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_HitPosition = this.transform.position;
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_WordGet [WordNumber] = true;
		}
		if (nomber == 2){
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_Score -= 1;
			for (var i = 0; i < GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_WordGet.Length; i++) {
				GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_WordGet[i]=false;
			}
		}
		if (nomber == 3){
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_Score += 1;
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_HitPosition = this.transform.position;
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_WordGet [WordNumber] = true;
		}
		if (nomber == 4){
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_Score -= 1;
			for (var i = 0; i < GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_WordGet.Length; i++) {
				GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_WordGet[i]=false;
			}
		}
	}
	[ClientRpc]
	void Rpc_Practicehit (int nomber){
		//GameObject BalloonPoints = GameObject.Find ("BalloonPoints").gameObject;
		if (nomber == 1) {
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_Score += 1;
		}
		if (nomber == 2){
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_Score -= 1;
		}
		if (nomber == 3){
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_Score += 1;
		}
		if (nomber == 4){
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_Score -= 1;
		}
	}

	[ClientRpc]
	void Rpc_Createpaticle (GameObject paticle,GameObject pointpaticle,int pointword, GameObject balloon){
		paticle.transform.SetParent(balloon.transform.parent.transform);
		paticle.GetComponent<ParticleSystem> ().Play ();
		if(pointword <2){
			pointpaticle.GetComponentInChildren<ParticleSystemRenderer> ().material = pointwords [pointword];
			pointpaticle.GetComponent<ParticleSystem> ().Play ();
		}
		paticle.GetComponent<AudioSource> ().Play ();
	}
	[ClientRpc]
	void Rpc_Quit(){
		Application.Quit ();
	}
}
