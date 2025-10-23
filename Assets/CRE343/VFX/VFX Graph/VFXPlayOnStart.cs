using System;
using UnityEngine;
using UnityEngine.VFX;

using UnityEngine;
using UnityEngine.VFX;

public class VFXPlayOnStart : MonoBehaviour
{
    private VisualEffect vfx;

    void Start()
    {
        // Get the VisualEffect component on this GameObject
        vfx = GetComponent<VisualEffect>();

        if (vfx != null)
        {
            // Trigger the "OnPlay" event in the VFX Graph
            vfx.SendEvent("OnPlay");
            Debug.Log("OnPlay event sent to VFX Graph.");
        }
        else
        {
            Debug.LogWarning("No VisualEffect component found on this GameObject.");
        }
    }

    private void Update(){
        if (Input.GetKeyDown((KeyCode.Alpha0))){
            vfx.SendEvent("OnPlay");
        }
    }
}

