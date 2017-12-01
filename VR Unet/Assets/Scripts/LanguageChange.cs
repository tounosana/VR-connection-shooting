using UnityEngine;
using System.Collections;

public class LanguageChange : MonoBehaviour {
	public GameObject[] target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ((this.gameObject.name == "P1WordBoard") || (this.gameObject.name == "P2WordBoard")) {
			if (GameObject.Find ("NetworkManager").GetComponent<Language_Select> ().language_number == 1) {
				target [0].GetComponent<UnityEngine.UI.Text> ().text = "M";
				target [1].GetComponent<UnityEngine.UI.Text> ().text = "C";
				target [2].GetComponent<UnityEngine.UI.Text> ().text = "U";
				target [3].GetComponent<UnityEngine.UI.Text> ().text = "6";
				target [4].GetComponent<UnityEngine.UI.Text> ().text = "0";
			} else if (GameObject.Find ("NetworkManager").GetComponent<Language_Select> ().language_number == 2) {
				target [0].GetComponent<UnityEngine.UI.Text> ().text = "祝";
				target [1].GetComponent<UnityEngine.UI.Text> ().text = "銘";
				target [2].GetComponent<UnityEngine.UI.Text> ().text = "伝";
				target [3].GetComponent<UnityEngine.UI.Text> ().text = "六";
				target [4].GetComponent<UnityEngine.UI.Text> ().text = "十";
			}else {
				target [0].GetComponent<UnityEngine.UI.Text> ().text = "資";
				target [1].GetComponent<UnityEngine.UI.Text> ().text = "傳";
				target [2].GetComponent<UnityEngine.UI.Text> ().text = "歡";
				target [3].GetComponent<UnityEngine.UI.Text> ().text = "迎";
				target [4].GetComponent<UnityEngine.UI.Text> ().text = "您";
			}
		}
	}
}
