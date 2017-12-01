using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class StartAllFire : NetworkBehaviour {
	public int FireSteps = 0;
	public bool Fire = false;
	public float passtime = 0.0f;
	public float waittime = 5.0f;
	public bool AudioPlay = false;
	public GameObject[] FireWorkSets = new GameObject[3]; 
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (Fire == true){
			if (FireSteps == 0) {
				Fire = false;
				FireSteps = 1;
				for (int i = 0; i < FireWorkSets.Length; i++) {
					FireWorkSets [i].GetComponent<FireWorkPlayer> ().StartFire = true;
					FireWorkSets [i].GetComponent<FireWorkPlayer> ().playtimes += 1;
				}
				AudioPlay = true;
			}else if(FireSteps == 1){
				Fire = false;
				FireSteps = 0;
				CmdPlayFireAudio (false);
				for (int i = 0; i < FireWorkSets.Length; i++) {
					FireWorkSets [i].GetComponent<FireWorkPlayer> ().StartFire = false;
				}
				for (int i = 0; i < FireWorkSets.Length; i++) {
					FireWorkSets[i].GetComponent<FireWorkPlayer>().CmdDeleteFireWork ();
				}
			}
		} 
		if (AudioPlay == true) {
			passtime += Time.fixedDeltaTime;
		}
		if (passtime >= waittime) {
			CmdPlayFireAudio (true);
			AudioPlay = false;
			passtime = 0.0f;
		}
	}
	[Command]
	void CmdPlayFireAudio (bool go){
		if (go == true) {
			this.GetComponent<AudioSource> ().Play ();
		} else {
			this.GetComponent<AudioSource> ().Stop ();
		}
		Rpc_PlayFireAudio (this.gameObject,go);
	}
	[ClientRpc]
	void Rpc_PlayFireAudio (GameObject Player,bool go){
		if (go == true) {
			Player.GetComponent<AudioSource> ().Play ();
		} else {
			Player.GetComponent<AudioSource> ().Stop ();
		}
	}
}
