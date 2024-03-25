using UnityEngine;

public class WindAudio : MonoBehaviour
{
    [SerializeField] private BirdMovement movement;
    [SerializeField] private float minVelocityBound;
    [SerializeField] private float maxVelocityBound;
    [SerializeField, Range(0, 1)] private float tunnelVolume;
    [SerializeField] private float tunnelFadeDuration;

    private AudioSource audioSource;
    private TunnelScript tunnelScript;

    private bool wasInsideTunnel = false;

    private Coroutine fadeRoutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        tunnelScript = movement.GetComponent<TunnelScript>();

        GameManager.Instance.OnCityLevelChange += TurnOffAudio;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnCityLevelChange -= TurnOffAudio;
    }

    private void TurnOffAudio(int level)
    {
        if (level == 3) 
        {
            audioSource.volume = 0;
            enabled = false;
        }
    }

    private void Update()
    {
        if (tunnelScript.insideTunnel && !wasInsideTunnel)
        {
            if (fadeRoutine != null) { StopCoroutine(fadeRoutine); }
            fadeRoutine = StartCoroutine(
                GameManager.AudioManager.FadeAudio(audioSource, tunnelFadeDuration, 
                tunnelVolume)
            );
        }
        else if (wasInsideTunnel && !tunnelScript.insideTunnel)
        {
            if (fadeRoutine != null) { StopCoroutine(fadeRoutine); }
            fadeRoutine = StartCoroutine(
                GameManager.AudioManager.FadeAudio(audioSource, tunnelFadeDuration, 
                Mathf.InverseLerp(minVelocityBound, maxVelocityBound, movement.velocity))
            );
        }
        else if (!tunnelScript.insideTunnel)
        {
            audioSource.volume = Mathf.InverseLerp(minVelocityBound, maxVelocityBound, movement.velocity);
        }

        wasInsideTunnel = tunnelScript.insideTunnel;
    }
}
