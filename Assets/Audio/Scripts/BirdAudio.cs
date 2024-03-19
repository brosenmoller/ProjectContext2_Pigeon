using UnityEngine;

public class BirdAudio : MonoBehaviour
{
    [SerializeField] private AudioObject flapSound;

    public void PlayFlap()
    {
        flapSound.Play();
    }
}
