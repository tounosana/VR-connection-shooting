using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FireWorkPlayer : NetworkBehaviour {
	public bool StartFire = false;
	public GameObject[] Playpositions = new GameObject[3];
	public GameObject[] Fireworks = new GameObject[3];
	public float timepass = 0.0f;
	public float firstplaywait = 0.0f;
	public float basetimewait = 0.5f;
	public bool play = false;
	public int number = 0;
	public int fireworkkind = 0;
	public int playtimes = 0;
	// Use this for initialization
	void Start () {
		float go = Random.Range(0,1) + Random.Range(0,7)*0.1f;
		firstplaywait = go;
	}
	
	// Update is called once per frame
	void Update () {
		if (StartFire == true) {
			timepass += Time.fixedDeltaTime;
			if ((play == false) && (timepass >= firstplaywait)) {
				play = true;
				timepass = 0.0f;
			}
			if ((play == true) && (timepass >= basetimewait) && (number < 3)) {
				fireworkkind = Random.Range (0, 2);
				CmdCreateFireWork ();
				number += 1;
				timepass = 0.0f;
			}
		} 
		else{			
			timepass = 0.0f;
			play = false;
			number = 0;
		}
	}
	[Command]
	void CmdCreateFireWork (){
		GameObject createfirework = Instantiate (Fireworks[fireworkkind]);
		createfirework.transform.position = Playpositions[number].transform.position;
		NetworkServer.Spawn (createfirework);
		createfirework.GetComponent<ParticleSystem> ().Play ();
		Rpc_CreateFireWork (createfirework,Playpositions[number]);
	}
	[Command]
	public void CmdDeleteFireWork (){
		for (int i = 0; i < Playpositions.Length; i++) {
			NetworkServer.Destroy (Playpositions [i].transform.GetChild (0).gameObject);
		}

	}
	[ClientRpc]
	void Rpc_CreateFireWork (GameObject firework,GameObject Playposition){
		firework.transform.SetParent (Playposition.transform);
		firework.GetComponent<ParticleSystem> ().Play ();
	}
}
