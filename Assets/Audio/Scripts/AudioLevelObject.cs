using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioLevelObject : MonoBehaviour
{
    [SerializeField] private float[] volumeLevels;
    [SerializeField] private float fadeDuration;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameManager.Instance.OnCityLevelChange += SetLevel;
        audioSource.volume = volumeLevels[0];
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCityLevelChange -= SetLevel;
    }

    private void SetLevel(int level)
    {
        if (level >= volumeLevels.Length)
        {
            GameManager.AudioManager.FadeAudio(audioSource, fadeDuration, 0);
            return;
        }

        GameManager.AudioManager.FadeAudio(audioSource, fadeDuration, volumeLevels[level]);
    }
}
