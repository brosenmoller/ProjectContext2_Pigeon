using System;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New Audio Object", menuName = "AudioObject")]
public class AudioObject : ScriptableObject
{
    public AudioClip[] clips;
    public bool randomiseClip;
    public AudioMixerGroup audioMixerGroup;

    public bool loop;
    public bool PlayOnAwake;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(.1f, 3f)] public float pitch = 1f;

    private AudioSource _source;
    private int audioClipIndex = 0;

    [HideInInspector] public AudioSource Source 
    { 
        get
        {
            if (_source == null) { AddAudioSourceToAudioManager(); }
            return _source;
        }
        private set { _source = value; }
    }

    public bool IsPlaying { get { return Source.isPlaying; } }

    public void Play()
    {
        if (randomiseClip)
        {
            audioClipIndex = UnityEngine.Random.Range(0, clips.Length);
        }
        else if (clips.Length > 1)
        {
            audioClipIndex++;
            if (audioClipIndex >= clips.Length)
            {
                audioClipIndex = 0;
            }
        }

        Source.clip = clips[audioClipIndex];
        Source.Play();
    }

    public void Stop() => Source.Stop();
    public void Pause() => Source.Pause();
    public void UnPause() => Source.UnPause();

    public void PlayFadeIn(float fadeDuration, float startVolume = 0f, float targetVolume = 1f)
    {
        Source.volume = startVolume;
        Source.Play();

        GameManager.Instance.StartCoroutine(
            GameManager.AudioManager.FadeAudio(Source, fadeDuration, targetVolume)
        );
    }

    public void StopFadeOut(float fadeDuration, float startVolume = 1f, float targetVolume = 0f)
    {
        Source.volume = startVolume;

        GameManager.Instance.StartCoroutine(
            GameManager.AudioManager.FadeAudio(Source, fadeDuration, targetVolume, Stop)
        );
    }

    private void AddAudioSourceToAudioManager()
    {
        Source = FindObjectOfType<BirdMovement>().gameObject.AddComponent<AudioSource>();
        Source.clip = clips[audioClipIndex];
        Source.outputAudioMixerGroup = audioMixerGroup;

        Source.loop = loop;
        Source.playOnAwake = PlayOnAwake;

        Source.pitch = pitch;
        Source.volume = volume;
    }
}
