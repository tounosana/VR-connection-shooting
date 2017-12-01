using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WandController : NetworkBehaviour {
	
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    public bool gripButtonDown = false;
    public bool gripButtonUp = false;
    public bool gripButtonPressed = false;
	

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    public bool triggerButtonDown = false;
    public bool triggerButtonUp = false;
    public bool triggerButtonPressed = false;



	private SteamVR_Controller.Device controller;

    private SteamVR_TrackedObject trackedObj;


	public GameObject takeObj;
	public GameObject takeBowObj;


	public bool canTake=false;
	public bool TakeThings=false;
	public bool TakeBow=false;


	public bool isShake = false;
	bool isShock = false;



	private float shootpower;

	// Use this for initialization
	void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();

		shootpower = 800.0f;


	}
	
	// Update is called once per frame
	void Update () {
		controller = SteamVR_Controller.Input ((int)trackedObj.index);



		triggerButtonDown = controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger);
		triggerButtonUp = controller.GetPressUp (SteamVR_Controller.ButtonMask.Trigger);
		triggerButtonPressed = controller.GetPress (SteamVR_Controller.ButtonMask.Trigger);

		gripButtonDown = controller.GetPressDown (SteamVR_Controller.ButtonMask.Grip);
		gripButtonUp = controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip);
		gripButtonPressed = controller.GetPress(SteamVR_Controller.ButtonMask.Grip);

		if (controller == null) {
			Debug.Log ("Controller not initialized");
			return;
		}


		if (gripButtonDown && takeObj != null) {

			TakeThings = true;
			/*
			takeObj.transform.parent = this.transform;
			takeObj.GetComponent<Rigidbody> ().isKinematic = true;*/

		}
		if (gripButtonUp && takeObj != null) {

			TakeThings = false;

			/*
			takeObj.transform.parent = null;
			takeObj.GetComponent<Rigidbody> ().isKinematic = false;*/

		}


		if (triggerButtonDown==true) {
			
			//CmdShootAim ();
			Debug.Log ("Trigger Button was just pressed");
		}


		if (isShake == true) {

			isShock = false;  //每次按下，IsShock为false,才能保证手柄震动
			StartCoroutine("Shock",0.2f); //开启协程Shock(),第二个参数0.5f 即为协程Shock()的形参

			//controller.TriggerHapticPulse (1000);
			//controller.TriggerHapticPulse (1000);
			isShake = false;
		}

			
	}

	void OnTriggerStay(Collider other)
	{
		//takeObj = other.gameObject;

		if (other.tag == "Spawn" && gripButtonDown == true) {
			//Debug.Log ("take");
			canTake = true;
			//canSpawn = true;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "ArrowTip") {
			TakeThings = true;

			takeObj = other.gameObject;
		}
		if (other.tag == "Bow") {
			TakeBow = true;

			takeBowObj= other.gameObject.transform.parent.gameObject;

			//Debug.Log("bow");
		}
			
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "ArrowTip") {
			TakeThings = false;

			takeObj = null;
		}

		if (other.tag == "Bow") {
			TakeBow = false;

			takeBowObj = null;
		}

		if (other.tag == "Spawn") {
			canTake = false;
		}
	}

	IEnumerator Shock(float durationTime) 

	{

		//Invoke函数，表示durationTime秒后，执行StopShock函数；
		Invoke("StopShock", durationTime);

		//协程一直使得手柄产生震动，直到布尔型变量IsShock为false;
		while (!isShock)
		{
			controller.TriggerHapticPulse(1000);
			controller.TriggerHapticPulse(2000);
			yield return new WaitForEndOfFrame();

		}


	}

	void StopShock()
	{
		isShock = true; //关闭手柄的震动
	}

}
