using UnityEngine;

[CreateAssetMenu(fileName = "New Story", menuName = "Story")]
public class Story : ScriptableObject
{
    public string title;
    public string[] stories;
    public AudioClip[] narrationClips;
}
