using UnityEngine;

public class FXTestInput : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode shakeKey = KeyCode.Alpha1;
    public KeyCode chromaKey = KeyCode.Alpha2;

    [Header("Shake Settings")]
    public float shakeFrequency = 10f;
    public float shakeAmplitude = 4f;
    public float shakeDuration = 1f;

    [Header("Chromatic Aberration Settings")]
    public float chromaIntensity = 1f;
    public float chromaDuration = 1f;

    void Update()
    {
        // Press 1 to trigger Camera Shake
        if (Input.GetKeyDown(shakeKey))
        {
            Debug.Log("[FXTestInput] Triggered Camera Shake");
            FeedbackEventManager.ShakeCamera(shakeFrequency, shakeAmplitude, shakeDuration);
        }

        // Press 2 to trigger Chromatic Aberration
        if (Input.GetKeyDown(chromaKey))
        {
            Debug.Log("[FXTestInput] Triggered Chromatic Aberration");
            FeedbackEventManager.ChromaticAberrationLerp(chromaIntensity, chromaDuration);
        }
    }
}