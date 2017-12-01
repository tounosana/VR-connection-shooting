using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Create_Table : NetworkBehaviour {
	public GameObject table;
	public float time = 0.0f;
	public bool create = false;
	// Use this for initialization
	void Awake(){

	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isServer) {
			time += Time.deltaTime;
			if ((time > 2.0f) && (create == false)) {
				CmdCreateTable (table, this.transform.gameObject);
				create = true;
			}
		}
	}
	[Command]
	void CmdCreateTable (GameObject table,GameObject pos){
		GameObject seat = Instantiate (table);
		seat.transform.position = pos.transform.position;
		NetworkServer.Spawn (seat);
	}
}
