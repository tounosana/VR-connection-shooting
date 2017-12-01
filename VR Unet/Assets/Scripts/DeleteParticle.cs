using UnityEngine;
using System.Collections;

public class DeleteParticle : MonoBehaviour {

	public GameObject particle;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (particle.GetComponent<ParticleSystem> ().isStopped == true) {
			Destroy (gameObject);
		}
	}
}
