using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Language_Select : MonoBehaviour {
	public NetworkManager manager;
	public int language_number = 0;
	public bool ShowGui = true;
	public bool EnglishOn = false;
	public bool JapaneseOn = false;
	public bool ChineseOn = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnGUI(){
		if (!this.ShowGui) {
			return;
		}
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("", GUILayout.Width (50.0f));
		GUILayout.BeginVertical();
		GUILayout.Label ("", GUILayout.Height (50.0f));
		GUILayout.BeginHorizontal ();
		GUILayout.BeginVertical(GUI.skin.box);
		if (GUILayout.Button ("Host")) {
			this.manager.StartHost ();
			this.ShowGui = false;
		}
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Client",GUILayout.Width(80.0f))) {
			this.manager.StartClient ();
			this.ShowGui = false;
		}
		this.manager.networkAddress = GUILayout.TextField (this.manager.networkAddress,GUILayout.Width(100.0f));
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		EnglishOn = GUILayout.Toggle (EnglishOn, "English");
		JapaneseOn = GUILayout.Toggle (JapaneseOn, "日本語");
		ChineseOn = GUILayout.Toggle (ChineseOn, "中文");
		GUILayout.EndVertical();
		GUILayout.EndHorizontal ();
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal ();

		if ((EnglishOn ==true)&&(language_number != 1)) {
			ChineseOn = false;
			JapaneseOn = false;
			language_number = 1;
		} 
		else if((JapaneseOn == true)&&(language_number !=2)){
			ChineseOn = false;
			EnglishOn = false;
			language_number = 2;
		}else if((ChineseOn == true)&&(language_number !=0)){
			JapaneseOn = false;
			EnglishOn = false;
			language_number = 0;
		}
		
	}
}
