using UnityEngine;

[CreateAssetMenu(fileName = "New Story", menuName = "Story")]
public class Story : ScriptableObject
{
    public string title;
    public string story;
    public AudioClip narrationClip;
}
