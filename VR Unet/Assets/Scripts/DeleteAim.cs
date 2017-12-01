using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DeleteAim : NetworkBehaviour {

	private float age;
	public float lifetime=2.0f;


	// Use this for initialization
	void Start () {
		age = 0.0f;
	}
	
	// Update is called once per frame
	//[ServerCallback]
	void Update () {
		age += Time.deltaTime;
		if (age > lifetime) {
			//this.transform.parent.GetComponent<WandController> ().TakeThings = false;
			NetworkServer.Destroy (gameObject);
		}
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Ground") {
			Debug.Log ("on");
			this.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		}
	}
}
