using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class BalloonCreateSystem :NetworkBehaviour {
	public bool P1_Ready = false;
	public bool P2_Ready = false;
	public bool startgame = false;
	public bool countdown = false;

	public int P1_Score = 0;
	public int P2_Score = 0;
	public int P1_ShootCount = 0;
	public int P2_ShootCount = 0;

	public bool[] P1_WordGet = new bool[5]; 
	public bool[] P2_WordGet = new bool[5]; 
	public GameObject WordFinishShow;

	public Vector3 P1_HitPosition = Vector3.zero;
	public Vector3 P2_HitPosition = Vector3.zero;

	public List<int> EmptyPositions = new List<int> ();
	public List<BalloonData> ReCreateBallooList = new List<BalloonData> ();

	public float CreateTime = 0.0f;
	public bool StartCountdown = false;

	public AudioClip sec_countdown;
	public bool countdownaudioplay = false;
	//public float oneseccheck = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ((P1_Ready == true) && (P2_Ready == true)) {
			//startgame = true;
			P1_Ready = false;
			P2_Ready = false;
			countdown = true;
			GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().countdowntime = 3.9f;

			//GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().Ready = true;
		}
		if (countdown == true) {
			CmdStratCountDown ();
		}
		if ((countdown == true) && (GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().countdowntime <= 0.0f)) {
			countdown = false;
			countdownaudioplay = false;
			startgame = true;
		} 
		if (startgame == true) {
			startgame = false;
			//GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().countdowntime = GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().baseTime;
			GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().Ready = true;

		}
		if((P1_WordGet[0]==true)&&(P1_WordGet[1]==true)&&(P1_WordGet[2]==true)
			&&(P1_WordGet[3]==true)&&(P1_WordGet[4]==true)){
			CmdGotWords (1);

		}
		if((P2_WordGet[0]==true)&&(P2_WordGet[1]==true)&&(P2_WordGet[2]==true)
			&&(P2_WordGet[3]==true)&&(P2_WordGet[4]==true)){
			CmdGotWords (2);  
		}
		if ((ReCreateBallooList.Count > 0 )&&(GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().ReCreateballoon == false)) {
			StartCountdown = true;
		} 
		if (StartCountdown == true) {
			CreateTime += Time.fixedDeltaTime;
		} 
		if (CreateTime >= 3.0f) {
			GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().ReCreateballoon = true;
			CreateTime = 0.0f;
			StartCountdown = false;
		}
	}
	[Command]
	void CmdGotWords(int who){
		GameObject WordFinish = Instantiate (WordFinishShow);
		if(who == 1){
			WordFinish.transform.position = P1_HitPosition + new Vector3 (1.0f,0.5f,0.0f);
			GameObject.Find ("Broadcast").GetComponent<Allbroadcast> ().wordfinishplay.Add(4);
		}
		else if(who == 2){
			WordFinish.transform.position = P2_HitPosition + new Vector3 (1.0f,0.5f,0.0f);
			GameObject.Find ("Broadcast").GetComponent<Allbroadcast> ().wordfinishplay.Add(3);
		}
		NetworkServer.Spawn (WordFinish);
		WordFinish.GetComponent<ParticleSystem> ().Play ();
		Rpc_GotWords (who,WordFinish);
	}
	[Command]
	void CmdStratCountDown (){
		bool playaudio = false;
		GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().countdowntime -= Time.deltaTime;
		GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().minite = (int)(GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().countdowntime / 60);
		GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().second =(int)(GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().countdowntime % 60);
		if (countdownaudioplay == false) {
			GameObject.Find ("BoardAudioPlayer").GetComponent<AudioSource> ().clip = sec_countdown;
			GameObject.Find ("BoardAudioPlayer").GetComponent<AudioSource> ().Play ();
			countdownaudioplay = true;
			playaudio = true;
		}
		Rpc_StratCountDown (GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().countdowntime,
			GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().minite,
			GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().second,playaudio);
	}
	[ClientRpc]
	void Rpc_GotWords(int who,GameObject WordFinish){
		if (who == 1) {
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_Score+= 20;
			for (var i = 0; i < GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_WordGet.Length; i++) {
				GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_WordGet[i] = false;
			}
		}
		if (who == 2) {
			GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_Score+= 20;
			for (var i = 0; i < GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_WordGet.Length; i++) {
				GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_WordGet[i] = false;
			}
		}
		WordFinish.GetComponent<ParticleSystem> ().Play ();
	}
	[ClientRpc]
	void Rpc_StratCountDown(float countdowntime,int minite,int second,bool play){
		GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().countdowntime = countdowntime;
		GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().minite = minite;
		GameObject.Find ("BalloonPoints").GetComponent<GetReady> ().second = second;
		if (play ==true) {
			GameObject.Find ("BoardAudioPlayer").GetComponent<AudioSource> ().clip = sec_countdown;
			GameObject.Find ("BoardAudioPlayer").GetComponent<AudioSource> ().Play ();
		}
		GameObject.Find ("Minite").GetComponent<UnityEngine.UI.Text> ().color = new Color (1, 1, 0);
		GameObject.Find ("point").GetComponent<UnityEngine.UI.Text> ().color = new Color (1, 1, 0);
		GameObject.Find ("Second").GetComponent<UnityEngine.UI.Text> ().color = new Color (1, 1, 0);
	}
}

