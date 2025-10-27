using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraShaker : MonoBehaviour
{
    [Tooltip("Assign your CinemachineCamera. If left empty, it will auto-find the first one in the scene.")]
    public CinemachineCamera cineCam;

    private CinemachineBasicMultiChannelPerlin noise;
    private float initialFrequency;
    private float initialAmplitude;
    private Coroutine shakeCoroutine;

    private void OnEnable()
    {
        FeedbackEventManager.ShakeCamera += ShakeCamera;
    }

    private void OnDisable()
    {
        FeedbackEventManager.ShakeCamera -= ShakeCamera;
    }

    private void Awake()
    {
        if (cineCam == null)
            cineCam = FindObjectOfType<CinemachineCamera>();
    }

    private void Start()
    {
        if (cineCam == null)
        {
            Debug.LogError("[CameraShaker] No CinemachineCamera found.");
            return;
        }

        // Get the new CM3 noise component
        noise = cineCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null)
        {
            Debug.LogError("[CameraShaker] No CinemachineBasicMultiChannelPerlin found on the camera.");
            return;
        }

        // Cache initial values (note: CM3 uses properties, not 'm_' fields)
        initialFrequency = noise.FrequencyGain;
        initialAmplitude = noise.AmplitudeGain;
    }

    public void ShakeCamera(float targetFrequency, float targetAmplitude, float duration)
    {
        if (noise == null) return;

        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(targetFrequency, targetAmplitude, duration));
    }

    private IEnumerator ShakeRoutine(float targetFrequency, float targetAmplitude, float duration)
    {
        float half = duration * 0.5f;
        float t = 0f;

        // Ramp up
        while (t < half)
        {
            t += Time.deltaTime;
            float u = t / half;
            noise.FrequencyGain = Mathf.Lerp(initialFrequency, targetFrequency, u);
            noise.AmplitudeGain = Mathf.Lerp(initialAmplitude, targetAmplitude, u);
            yield return null;
        }

        // Ramp down
        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float u = t / half;
            noise.FrequencyGain = Mathf.Lerp(targetFrequency, initialFrequency, u);
            noise.AmplitudeGain = Mathf.Lerp(targetAmplitude, initialAmplitude, u);
            yield return null;
        }

        // Reset
        noise.FrequencyGain = initialFrequency;
        noise.AmplitudeGain = initialAmplitude;
    }
}
