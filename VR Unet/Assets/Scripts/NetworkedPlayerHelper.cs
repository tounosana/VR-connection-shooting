using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

public class NetworkedPlayerHelper : NetworkBehaviour
{
    public GameObject Hmd;
    public GameObject Controller;

    /// <summary>
    /// Called when local player authority has been assigned to a network object
    /// </summary>
    public override void OnStartLocalPlayer()
    {
        //enable all cameras and listeners for the local player
        foreach (Camera cam in GetComponentsInChildren<Camera>())
        {
            cam.enabled = true;
        }
        GetComponentInChildren<AudioListener>().enabled = true;

        if (SteamVR.active)
        {
            //enable steam vr scripts for the local player
            List<Component> allComponents = GetComponents<Component>().ToList();
            allComponents.AddRange(GetComponentsInChildren<Component>());
            foreach (
                MonoBehaviour currentComponent in
                    allComponents.Where(component => component.ToString().Contains("Steam")).Cast<MonoBehaviour>())
            {
                currentComponent.enabled = true;
            }
        }
        else
        {
            transform.position = transform.position + Vector3.up * 1.75f;
        }
    }

    /// <summary>
    /// Used to check for non-local player as onstartclient is ambiguous
    /// </summary>
    void Start()
    {
        switch (isLocalPlayer)
        {
            case true:
                gameObject.name = gameObject.name.Replace("(Clone)", "") + "_localPlayer";
                break;
            case false:
                gameObject.name = gameObject.name.Replace("(Clone)", "") + "_clientPlayer";

                //Create HMD and Controller objects
                foreach (Transform childTransform in transform)
                {
                    CreateObjectFor(childTransform);
                }
                break;
        }
    }

    /// <summary>
    /// Create object model for transform
    /// </summary>
    /// <param name="tf"></param>
    private void CreateObjectFor(Transform tf)
    {
        GameObject newGameObject = null;
        if (tf.name.Contains("Controller"))
        {
            newGameObject = Instantiate(Controller);
        }
        else if (tf.name.Contains("head"))
        {
            newGameObject = Instantiate(Hmd);
        }

        if (newGameObject == null) return;
        newGameObject.transform.position = tf.position;
        newGameObject.transform.parent = tf;
        tf.gameObject.SetActive(true);
    }

    /// <summary>
    /// For testing only! - simulate orientation of non-vr player
    /// </summary>
    void Update()
    {
        if (isLocalPlayer && !SteamVR.active)
        {
            GameObject otherPlayer = GameObject.Find("OnlinePlayer_clientPlayer");
            if (otherPlayer != null)
            {
                Transform hmd = transform.FindChild("Camera (head)");
                hmd.LookAt(otherPlayer.transform.FindChild("Camera (head)"));
                transform.FindChild("Controller (right)").position = hmd.transform.position - Vector3.up * 0.5f + hmd.transform.forward * 0.25f + hmd.transform.right * 0.25f;
                transform.FindChild("Controller (right)").rotation = hmd.rotation;
                transform.FindChild("Controller (left)").position = hmd.transform.position - Vector3.up * 0.5f + hmd.transform.forward * 0.25f - hmd.transform.right * 0.25f;
                transform.FindChild("Controller (left)").rotation = hmd.rotation;
            }
        }
    }
}
