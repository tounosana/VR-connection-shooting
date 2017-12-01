using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Allbroadcast :NetworkBehaviour {
	public AudioClip[] broadcastlist = new AudioClip[17];
	public int Playnumber = 0;
	public int broadcastphase = 0;
	//public float[] tempwait = new float[11];
	public float wait = 0.0f;
	public bool broadcast = false;
	public bool CanPlay = true;
	public List<int> wordfinishplay = new List<int>();
	/*phase = 0 => phase Practice
	 *phase = 1 => phase Ready 
	 *phase = 2 => phase GamePlay
	 *Phase = 3 => phase GameEnd
	 */
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (broadcast == false) {
			return;
		}
		if ((broadcastphase == 0) && (CanPlay == true)) {
			Cmdbroadcast ();
			Playnumber += 1;
			if (Playnumber > 0) {
				broadcastphase = 1;
				broadcast = false;
			}
		} else if ((broadcastphase == 1) && (CanPlay == true)) {
			Cmdbroadcast ();
			Playnumber += 1;
			if (Playnumber > 2) {
				broadcastphase = 2;
				broadcast = false;
			}
		} else if ((broadcastphase == 2) && (CanPlay == true)) {
			if (wordfinishplay.Count > 0) {
				Playnumber = wordfinishplay [0];
				Cmdbroadcast ();
				wordfinishplay.RemoveAt (0);
			}
		} else if ((broadcastphase == 3)&& (CanPlay == true)) {
			int P1Sc = GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P1_Score;
			int P2Sc = GameObject.Find ("BalloonPoints").GetComponent<BalloonCreateSystem> ().P2_Score;
			if (P1Sc > P2Sc) {
				Playnumber = 6;
				Cmdbroadcast ();
			} else if (P1Sc < P2Sc) {
				Playnumber = 5;
				Cmdbroadcast ();
			} else {
				Playnumber = 7;
				Cmdbroadcast ();
			}
		}
		if ((broadcastphase == 0)&&(this.GetComponent<AudioSource> ().isPlaying == false)&&(CanPlay == false)) {
			wait += Time.fixedDeltaTime;
			if (wait >= 0.1f) {
				CanPlay = true;
				wait = 0.0f;
			}
		}
		if ((broadcastphase == 1) && (this.GetComponent<AudioSource> ().isPlaying == false) && (CanPlay == false)) {
			wait += Time.fixedDeltaTime;
			if (wait >= 0.1f) {
				CanPlay = true;
				wait = 0.0f;
			}
		}
		if ((broadcastphase == 2) && (this.GetComponent<AudioSource> ().isPlaying == false) && (CanPlay == false)) {
			CanPlay = true;
		}
		if ((broadcastphase == 3) && (this.GetComponent<AudioSource> ().isPlaying == false) && (CanPlay == false)) {
			wait += Time.fixedDeltaTime;
			if ((wait >= 1.0f)&&(wait<1.5f)) {
				GameObject.Find ("FireWorkSets").GetComponent<StartAllFire> ().Fire = true;
				wait = 1.5f;
			}
			if (wait >= 2.0f) {
				CanPlay = true;
				wait = 0.0f;
			}
		}
	}
	[Command]
	void Cmdbroadcast (){
		//this.GetComponent<AudioSource> ().clip = broadcastlist [Playnumber];
		//this.GetComponent<AudioSource> ().Play ();
		if(broadcastphase == 3){
			broadcast = false;
		}
		Rpc_broadcast (this.gameObject,Playnumber);
		//CanPlay = false;
	}
	[ClientRpc]
	public void Rpc_broadcast (GameObject broadcaster,int Playnumber){
		if (GameObject.Find ("NetworkManager").GetComponent<Language_Select> ().language_number == 1) {
			broadcaster.GetComponent<AudioSource> ().clip = broadcastlist [Playnumber+8];
			broadcaster.GetComponent<AudioSource> ().Play ();
			broadcaster.GetComponent<Allbroadcast> ().CanPlay = false;
		} 
		else if(GameObject.Find ("NetworkManager").GetComponent<Language_Select> ().language_number == 2){
			broadcaster.GetComponent<AudioSource> ().clip = broadcastlist [Playnumber+16];
			broadcaster.GetComponent<AudioSource> ().Play ();
			broadcaster.GetComponent<Allbroadcast> ().CanPlay = false;
		}else {
			broadcaster.GetComponent<AudioSource> ().clip = broadcastlist [Playnumber];
			broadcaster.GetComponent<AudioSource> ().Play ();
			broadcaster.GetComponent<Allbroadcast> ().CanPlay = false;
		}
	}
}
