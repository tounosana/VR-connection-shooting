using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DataUpdate : NetworkBehaviour {
	public GameObject UpdateTarget;
	public GameObject DataSource;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isServer) {
			if (UpdateTarget.name == "P1_Score") {
				CmdP1ScoreUpdate ();
			}
			if (UpdateTarget.name == "P2_Score") {
				CmdP2ScoreUpdate ();
			}
			if (UpdateTarget.name == "Minite") {
				CmdMiniteUpdate ();
			}
			if (UpdateTarget.name == "Second") {
				CmdPSecondUpdate ();
			}

			if (UpdateTarget.name == "P1word1") {
				CmdP1WordsUpdate (0);
			}
			if (UpdateTarget.name == "P1word2") {
				CmdP1WordsUpdate (1);
			}
			if (UpdateTarget.name == "P1word3") {
				CmdP1WordsUpdate (2);
			}
			if (UpdateTarget.name == "P1word4") {
				CmdP1WordsUpdate (3);
			}
			if (UpdateTarget.name == "P1word5") {
				CmdP1WordsUpdate (4);
			}

			if (UpdateTarget.name == "P2word1") {
				CmdP2WordsUpdate (0);
			}
			if (UpdateTarget.name == "P2word2") {
				CmdP2WordsUpdate (1);
			}
			if (UpdateTarget.name == "P2word3") {
				CmdP2WordsUpdate (2);
			}
			if (UpdateTarget.name == "P2word4") {
				CmdP2WordsUpdate (3);
			}
			if (UpdateTarget.name == "P2word5") {
				CmdP2WordsUpdate (4);
			}
		}
	}
	[Command]
	void CmdP1ScoreUpdate (){
		Rpc_P1ScoreUpdate (UpdateTarget,DataSource);
	}
	[Command]
	void CmdP2ScoreUpdate (){
		Rpc_P2ScoreUpdate (UpdateTarget,DataSource);
	}
	[Command]
	void CmdMiniteUpdate  (){
		Rpc_MiniteUpdate (UpdateTarget,DataSource);
	}
	[Command]
	void CmdPSecondUpdate (){
		Rpc_SecondUpdate (UpdateTarget,DataSource);
	}
	[Command]
	void CmdP1WordsUpdate (int WordNumber){
		Rpc_P1WordsUpdate(UpdateTarget, DataSource,WordNumber);
	}
	[Command]
	void CmdP2WordsUpdate (int WordNumber){
		Rpc_P2WordsUpdate(UpdateTarget, DataSource,WordNumber);
	}



	[ClientRpc]
	void Rpc_P1ScoreUpdate(GameObject UpdateTarget,GameObject DataSource){
		UpdateTarget.GetComponent<UnityEngine.UI.Text> ().text = DataSource.GetComponent<BalloonCreateSystem> ().P1_Score.ToString ();
	}

	[ClientRpc]
	void Rpc_P2ScoreUpdate(GameObject UpdateTarget,GameObject DataSource){
		UpdateTarget.GetComponent<UnityEngine.UI.Text> ().text = DataSource.GetComponent<BalloonCreateSystem> ().P2_Score.ToString ();
	}
	[ClientRpc]
	void Rpc_MiniteUpdate(GameObject UpdateTarget,GameObject DataSource){
		UpdateTarget.GetComponent<UnityEngine.UI.Text> ().text = DataSource.GetComponent<GetReady>().minite.ToString();
	}
	[ClientRpc]
	void Rpc_SecondUpdate(GameObject UpdateTarget,GameObject DataSource){
		if (DataSource.GetComponent<GetReady> ().second < 10) {
			UpdateTarget.GetComponent<UnityEngine.UI.Text> ().text = "0"+DataSource.GetComponent<GetReady> ().second.ToString ();
		} else {
			UpdateTarget.GetComponent<UnityEngine.UI.Text> ().text = DataSource.GetComponent<GetReady> ().second.ToString ();
		}
	}
	[ClientRpc]
	void Rpc_P1WordsUpdate (GameObject UpdateTarget,GameObject DataSource,int WordNumber){
		if (DataSource.GetComponent<BalloonCreateSystem> ().P1_WordGet [WordNumber] == true) {
			UpdateTarget.GetComponent<UnityEngine.UI.Text> ().color = new Color (1, 1, 1);
		} else {
			UpdateTarget.GetComponent<UnityEngine.UI.Text> ().color = GameObject.Find("P1WordsMarkColor").GetComponent<UnityEngine.UI.RawImage>().color;
		}
	}
	[ClientRpc]
	void Rpc_P2WordsUpdate (GameObject UpdateTarget,GameObject DataSource,int WordNumber){
		if (DataSource.GetComponent<BalloonCreateSystem> ().P2_WordGet [WordNumber] == true) {
			UpdateTarget.GetComponent<UnityEngine.UI.Text> ().color = new Color (1, 1, 1);
		} else {
			UpdateTarget.GetComponent<UnityEngine.UI.Text> ().color = GameObject.Find("P2WordsMarkColor").GetComponent<UnityEngine.UI.RawImage>().color;
		}
	}
}
