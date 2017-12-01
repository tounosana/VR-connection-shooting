using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GetReady : NetworkBehaviour {
	
	public GameObject [] positions;
	public Material [] word;

	public GameObject red_balloon;
	public GameObject blue_balloon;
	public bool Ready = false;
	public bool Go = false;

	public bool ReCreateballoon = false;

	public AudioClip battlemusic;
	public AudioClip basicmusic;
	public AudioClip balloonpop;
	public AudioClip gamecountdown;

	public bool Endgame = false;
	public float countdowntime = 124.0f;
	public float baseTime;

	public int minite = 0;
	public int second = 0;

	public bool countdown = false;

	// Use this for initialization
	void Start () {
		minite = (int)(countdowntime / 60);
		second =(int)(countdowntime % 60);

	}
	
	// Update is called once per frame
	void Update () {
		if ((Ready == true) && (Go == false)) {
			//countdowntime = baseTime;
			CmdBalloonCreate ();
			GameObject.Find ("Broadcast").GetComponent<Allbroadcast> ().broadcast = true;
		}
		if ((ReCreateballoon == true)&&(Endgame ==false)) {
			List<int> EmptyPositions = GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().EmptyPositions;
			int NewPosition = Random.Range (0, EmptyPositions.Count - 1);
			List<BalloonData> ReCreateBallooList = GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().ReCreateBallooList;
			CmdBalloonReCreate (EmptyPositions[NewPosition], ReCreateBallooList[0]);
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().EmptyPositions.RemoveAt (NewPosition);
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().ReCreateBallooList.RemoveAt(0);
			ReCreateballoon = false;
		}
		if (Go == true) {
			if (isServer) {
				CmdCountDown ();
			}
		}
		if ((countdowntime <= 0.0f)&&(Go == true)) {
		//	GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_Ready = false;
		//	GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_Ready = false;
			Ready = false;
			Go = false;
			ReCreateballoon = false;
			countdowntime = 0.0f;
			Endgame = true;

		}
		if (Endgame == true) {
			GameObject.Find ("Broadcast").GetComponent<Allbroadcast> ().CanPlay = false;
			GameObject.Find ("Broadcast").GetComponent<Allbroadcast> ().broadcastphase = 3;

			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().EmptyPositions.Clear();
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().ReCreateBallooList.Clear ();		
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().StartCountdown = false;
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().CreateTime = 0.0f;
			ReCreateballoon = false;
			CmdEndGame ();
			Endgame = false;
			countdown = false;
			///
			GameObject.Find ("SelectBalloonPoints").GetComponent<SelectBalloonCreate> ().readyballooncreate = true;
			GameObject.Find ("SelectBalloonPoints").GetComponent<SelectBalloonCreate> ().quitballooncreate = true;
			GameObject.Find ("SelectBalloonPoints").GetComponent<SelectBalloonCreate> ().wordnumber = 7;
			GameObject.Find ("SelectBalloonPoints").GetComponent<SelectBalloonCreate> ().go = true;
			///
		}
	}

	[Command]
	void CmdBalloonCreate (){
		Rpc_ResetAndGo ();
		for (var i = 0; i < positions.Length; i++) {
			if (i % 2 == 0) {
				GameObject createballoon = Instantiate (red_balloon);
				createballoon.transform.position = positions [i].transform.position;
				NetworkServer.Spawn (createballoon);
				Rpc_BalloonCreate (i,createballoon);
			}
			if (i % 2 == 1) {
				GameObject createballoon = Instantiate (blue_balloon);
				createballoon.transform.position = positions [i].transform.position;
				NetworkServer.Spawn (createballoon);
				Rpc_BalloonCreate (i,createballoon);
			}
		}	
	}

	[Command]
	void CmdBalloonReCreate (int EmptyPositions, BalloonData ReCreateBalloon){
		if (ReCreateBalloon.BalloonColor == "red") {
			GameObject createballoon = Instantiate (red_balloon);
			createballoon.transform.position = positions[EmptyPositions].transform.position;
			createballoon.transform.parent = positions[EmptyPositions].transform;
			createballoon.GetComponent<Hit> ().BalloonNumber = EmptyPositions;
			createballoon.GetComponent<Hit> ().BalloonColor = ReCreateBalloon.BalloonColor;
			createballoon.GetComponent<Hit> ().WordNumber = ReCreateBalloon.WordNumber;
			createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [ReCreateBalloon.WordNumber];
			createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			NetworkServer.Spawn (createballoon);
			this.GetComponent<GetReady> ().Rpc_BalloonReCreate (EmptyPositions,ReCreateBalloon,createballoon);
		} else {
			GameObject createballoon = Instantiate (blue_balloon);
			createballoon.transform.position = positions[EmptyPositions].transform.position;
			createballoon.transform.parent = positions[EmptyPositions].transform;
			createballoon.GetComponent<Hit> ().BalloonNumber = EmptyPositions;
			createballoon.GetComponent<Hit> ().BalloonColor = ReCreateBalloon.BalloonColor;
			createballoon.GetComponent<Hit> ().WordNumber = ReCreateBalloon.WordNumber;
			createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [ReCreateBalloon.WordNumber];
			createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			NetworkServer.Spawn (createballoon);
			this.GetComponent<GetReady> ().Rpc_BalloonReCreate (EmptyPositions,ReCreateBalloon,createballoon);
		}

	}

	[Command]
	void CmdEndGame (){
		for (var i = 0; i < positions.Length; i++) {
			if (positions [i].transform.childCount > 0) {
				GameObject createpaticle = Instantiate (positions [i].transform.GetChild (0).GetComponent<Hit>().paticles);
				createpaticle.transform.position = positions [i].transform.position;
				NetworkServer.Spawn (createpaticle);
				Rpc_EndGameDelete (i,createpaticle);
				NetworkServer.Destroy (positions [i].transform.GetChild (0).gameObject);
			}
		}
		GameObject.Find ("BalloonPop").GetComponent<AudioSource> ().Play ();
		///
		GameObject.Find ("BalloonPoints").GetComponent<AudioSource> ().Stop ();
		GameObject.Find ("BalloonPoints").GetComponent<AudioSource> ().clip = basicmusic;
		GameObject.Find ("BalloonPoints").GetComponent<AudioSource> ().Play ();
		///
		Rpc_EndGame ();
	}
	[Command]
	void CmdCountDown (){
		bool playaudio = false;
		if ((countdown == false)&&(countdowntime < 11.0f)) {
			GameObject.Find ("BoardAudioPlayer").GetComponent<AudioSource> ().clip = gamecountdown;
			GameObject.Find ("BoardAudioPlayer").GetComponent<AudioSource> ().Play ();
			countdown = true;
			playaudio = true;
		}
		countdowntime -= Time.fixedDeltaTime;
		minite = (int)(countdowntime / 60);
		second =(int)(countdowntime % 60);
		Rpc_CountDown (countdowntime,minite,second,playaudio);
	}
	[ClientRpc]
	void Rpc_ResetAndGo (){
		GameObject.Find ("Minite").GetComponent<UnityEngine.UI.Text> ().color = new Color (1, 1, 1);
		GameObject.Find ("point").GetComponent<UnityEngine.UI.Text> ().color = new Color (1, 1, 1);
		GameObject.Find ("Second").GetComponent<UnityEngine.UI.Text> ().color = new Color (1, 1, 1);
		GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().countdowntime = GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().baseTime;
		GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_Score = 0;
		GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_Score = 0;

		GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_WordGet = new bool[5]; 
		GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_WordGet = new bool[5];
		GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().EmptyPositions = new List<int> ();
		GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().ReCreateBallooList = new List<BalloonData> ();

		GameObject.Find ("BalloonPoints").GetComponent<AudioSource> ().Stop ();
		GameObject.Find ("BalloonPoints").GetComponent<AudioSource> ().clip = battlemusic;
		GameObject.Find ("BalloonPoints").GetComponent<AudioSource> ().Play ();

		GameObject.Find("BalloonPoints").GetComponent<GetReady> ().Go = true;
	}

	[ClientRpc]
	void Rpc_BalloonCreate(int number ,GameObject createballoon){
		int language = GameObject.Find ("NetworkManager").GetComponent<Language_Select> ().language_number*5;
		if (number % 2 == 0) {
			createballoon.transform.parent = GameObject.Find ("BalloonPoints").GetComponent<GetReady>().positions [number].transform;
			createballoon.GetComponent<Hit> ().BalloonNumber = number;
			createballoon.GetComponent<Hit> ().BalloonColor = "red";
			if (number == 0) {
				createballoon.GetComponent<Hit> ().WordNumber = 0;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(0+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 6) {
				createballoon.GetComponent<Hit> ().WordNumber = 1;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(1+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 2) {
				createballoon.GetComponent<Hit> ().WordNumber = 2;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(2+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 8) {
				createballoon.GetComponent<Hit> ().WordNumber = 3;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(3+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 4) {
				createballoon.GetComponent<Hit> ().WordNumber = 4;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(4+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 10) {
				createballoon.GetComponent<Hit> ().WordNumber = 0;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(0+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 16) {
				createballoon.GetComponent<Hit> ().WordNumber = 1;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(1+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 12) {
				createballoon.GetComponent<Hit> ().WordNumber = 2;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(2+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 18) {
				createballoon.GetComponent<Hit> ().WordNumber = 3;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(3+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 14) {
				createballoon.GetComponent<Hit> ().WordNumber = 4;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(4+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
		}
		if (number % 2 == 1) {
			createballoon.transform.parent =  GameObject.Find ("BalloonPoints").GetComponent<GetReady>().positions [number].transform;
			createballoon.GetComponent<Hit> ().BalloonNumber = number;
			createballoon.GetComponent<Hit> ().BalloonColor = "blue";
			if (number == 5) {
				createballoon.GetComponent<Hit> ().WordNumber = 0;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(0+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 1) {
				createballoon.GetComponent<Hit> ().WordNumber = 1;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(1+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 7) {
				createballoon.GetComponent<Hit> ().WordNumber = 2;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(2+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 3) {
				createballoon.GetComponent<Hit> ().WordNumber = 3;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(3+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 9) {
				createballoon.GetComponent<Hit> ().WordNumber = 4;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(4+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 15) {
				createballoon.GetComponent<Hit> ().WordNumber = 0;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(0+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 11) {
				createballoon.GetComponent<Hit> ().WordNumber = 1;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(1+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 17) {
				createballoon.GetComponent<Hit> ().WordNumber = 2;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(2+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 13) {
				createballoon.GetComponent<Hit> ().WordNumber = 3;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(3+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}
			if (number == 19) {
				createballoon.GetComponent<Hit> ().WordNumber = 4;
				createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = word [(4+language)];
				createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
			}

		}	
	}

	[ClientRpc]
	void Rpc_BalloonReCreate (int EmptyPositions,BalloonData ReCreateBalloon, GameObject createballoon){
		int language = GameObject.Find ("NetworkManager").GetComponent<Language_Select> ().language_number*5;
		createballoon.transform.position = GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().positions[EmptyPositions].transform.position;
		createballoon.transform.parent = GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().positions[EmptyPositions].transform;
		createballoon.GetComponent<Hit> ().BalloonNumber = EmptyPositions;
		createballoon.GetComponent<Hit> ().BalloonColor = ReCreateBalloon.BalloonColor;
		createballoon.GetComponent<Hit> ().WordNumber = ReCreateBalloon.WordNumber;
		createballoon.GetComponentInChildren<ParticleSystemRenderer> ().material = GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().word [(ReCreateBalloon.WordNumber + language)];
		createballoon.GetComponentInChildren<ParticleSystem> ().Play ();
	}
	[ClientRpc]
	void Rpc_EndGameDelete (int pos,GameObject createpaticle){
		//Debug.Log (pos);
		//createpaticle.transform.position = GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().positions[pos].transform.position;
		createpaticle.transform.parent = GameObject.Find ("BalloonPoints").GetComponent<GetReady>().positions [pos].transform;
		createpaticle.GetComponent<ParticleSystem> ().Play ();
		createpaticle.GetComponent<AudioSource> ().Stop();
	}
	[ClientRpc]
	void Rpc_EndGame(){
		GameObject.Find ("BalloonPop").GetComponent<AudioSource> ().Play ();
		///
		GameObject.Find ("BalloonPoints").GetComponent<AudioSource> ().Stop ();
		GameObject.Find ("BalloonPoints").GetComponent<AudioSource> ().clip = basicmusic;
		GameObject.Find ("BalloonPoints").GetComponent<AudioSource> ().Play ();
		///
		GameObject.Find ("Minite").GetComponent<UnityEngine.UI.Text> ().color = new Color (1, 1, 1);
		GameObject.Find ("point").GetComponent<UnityEngine.UI.Text> ().color = new Color (1, 1, 1);
		GameObject.Find ("Second").GetComponent<UnityEngine.UI.Text> ().color = new Color (1, 1, 1);
	}
	[ClientRpc]
	void Rpc_CountDown (float countdowntime,int minite,int second,bool play){
		if (play == true) {
			GameObject.Find ("BoardAudioPlayer").GetComponent<AudioSource> ().clip = gamecountdown;
			GameObject.Find ("BoardAudioPlayer").GetComponent<AudioSource> ().Play ();
		}
		if (countdowntime < 11.0f) {
			GameObject.Find ("Minite").GetComponent<UnityEngine.UI.Text> ().color = new Color (0.8f, 0, 0);
			GameObject.Find ("point").GetComponent<UnityEngine.UI.Text> ().color = new Color (0.8f, 0, 0);
			GameObject.Find ("Second").GetComponent<UnityEngine.UI.Text> ().color = new Color (0.8f, 0, 0);
		}
		GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().countdowntime = countdowntime;
		GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().minite = minite;
		GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().second = second;
	}
}
