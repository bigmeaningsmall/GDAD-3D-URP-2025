using UnityEngine;
using Unity.Cinemachine;

public class SetFollowCamera : MonoBehaviour
{
    [Tooltip("Optional: assign directly. If left empty, will find the first CinemachineCamera in the scene.")]
    public CinemachineCamera cineCam;

    private void Start()
    {
        // Try assigned reference first
        if (cineCam == null)
            cineCam = FindObjectOfType<CinemachineCamera>();

        // Tag fallback (optional): tag your camera object as "VirtualCamera"
        if (cineCam == null)
        {
            GameObject tagged = GameObject.FindGameObjectWithTag("VirtualCamera");
            if (tagged != null)
                cineCam = tagged.GetComponent<CinemachineCamera>();
        }

        if (cineCam != null)
        {
            // CM3: set the tracking target directly
            cineCam.Follow = this.transform;
        }
        else
        {
            Debug.LogError("[SetFollowCamera] CinemachineCamera not found in the scene.");
        }
    }
}